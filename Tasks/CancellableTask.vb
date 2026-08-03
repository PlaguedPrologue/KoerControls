Imports System.Threading

'''<summary>Represents an object that can run asynchronous operations, cancelling or waiting for the previous action to finish before running a new asynchronous operation.</summary>
Public Class CancellableTask

#Region "Private Items"

    Private ReadOnly _cancelTokensLock As New Object
    Private ReadOnly _cancelTokenSources As New List(Of CancellationTokenSource)
    Private _isRunning As Boolean = False
    Private _lastTaskRan As Long

    Private Async Function CancelAsync(currentTask As Long) As Task(Of Boolean)
        Await CancelAsync()
        Return currentTask = _lastTaskRan
    End Function

#End Region

#Region "Properties"

    '''<summary>Determines if cancellation has been requested for this <see cref="CancellableTask"/>.</summary>
    '''<returns><see langword="true"/> if cancellation has been requested for this <see cref="CancellableTask"/>; otherwise, <see langword="false"/>.</returns>
    Public ReadOnly Property IsCancellationRequested
        Get
            Return _cancelTokenSources.Any(Function(cts) cts.IsCancellationRequested)
        End Get
    End Property
    '''<summary>Determines if a <see cref="Task"/> is currently running for this <see cref="CancellableTask"/>.</summary>
    '''<returns><see langword="true"/> if a <see cref="Task"/> is currently running for this <see cref="CancellableTask"/>; otherwise, <see langword="false"/>.</returns>
    Public ReadOnly Property IsRunning As Boolean
        Get
            Return _isRunning
        End Get
    End Property
    '''<summary>Gets the <see cref="CancellationToken"/> associated with this <see cref="CancellableTask"/>.</summary>
    '''<returns>The <see cref="CancellationToken"/> associated with this <see cref="CancellableTask"/>.</returns>
    '''<exception cref="ObjectDisposedException">The token source has been disposed.</exception>
    Public ReadOnly Property Token As CancellationToken
        Get
            If _cancelTokenSources.Count = 0 Then Throw New ObjectDisposedException(NameOf(CancellableTask))
            Return _cancelTokenSources(_cancelTokenSources.Count - 1).Token
        End Get
    End Property

#End Region

#Region "Functions"

    '''<summary>Cancels the currently running <see cref="Task"/>.</summary>
    Public Sub Cancel()
        SyncLock _cancelTokensLock
            For Each ctsCancel As CancellationTokenSource In _cancelTokenSources.ToArray
                ctsCancel.Cancel()
            Next
        End SyncLock
    End Sub
    '''<summary>Cancels the currently running <see cref="Task"/> and waits for each <see cref="Task"/> to finish.</summary>
    '''<returns>A <see cref="Task"/> used to wait for each <see cref="Task"/> to finish.</returns>
    Public Async Function CancelAsync() As Task
        Dim ctsCancels() As CancellationTokenSource
        SyncLock _cancelTokensLock
            ctsCancels = _cancelTokenSources.ToArray
        End SyncLock
        For Each ctsCancel As CancellationTokenSource In ctsCancels
            ctsCancel.Cancel()
        Next
        While True
            SyncLock _cancelTokensLock
                If _cancelTokenSources.Count = 0 Then Exit While
            End SyncLock
            Await Task.Delay(10)
        End While
    End Function
    '''<summary>Queues the specified work to run on the thread pool and returns a proxy for the task returned by function.</summary>
    '''<returns>A task that represents a proxy for the task returned by action.</returns>
    '''<param name="action">The work to execute asynchronously.</param>
    '''<exception cref="ArgumentNullException">The action parameter was null.</exception>
    Public Function Run(action As Action(Of CancellationToken)) As Task
        If action Is Nothing Then Throw New ArgumentNullException(NameOf(action))
        Return Task.Run(Async Function()
                            Dim lCurrentTask As Long = Interlocked.Increment(_lastTaskRan)
                            If Not Await CancelAsync(lCurrentTask) Then Return
                            Dim ctsCancel As New CancellationTokenSource
                            SyncLock _cancelTokensLock
                                If lCurrentTask = _lastTaskRan Then
                                    _cancelTokenSources.Add(ctsCancel)
                                Else
                                    ctsCancel.Dispose()
                                    Return
                                End If
                                Try
                                    _isRunning = True
                                    action.Invoke(ctsCancel.Token)
                                Catch
                                    Throw
                                Finally
                                    SyncLock _cancelTokensLock
                                        _cancelTokenSources.Remove(ctsCancel)
                                    End SyncLock
                                    ctsCancel.Dispose()
                                    _isRunning = False
                                End Try
                            End SyncLock
                        End Function)
    End Function
    '''<summary>Queues the specified work to run on the thread pool And returns a proxy for the task returned by function.</summary>
    '''<returns>A task that represents a proxy for the task returned by function.</returns>
    '''<param name="function">The work to execute asynchronously.</param>
    '''<exception cref="ArgumentNullException">The function parameter was null.</exception>
    Public Function Run([function] As Func(Of CancellationToken, Task)) As Task
        If [function] Is Nothing Then Throw New ArgumentNullException(NameOf([function]))
        Return Task.Run(Async Function()
                            Dim lCurrentTask As Long = Interlocked.Increment(_lastTaskRan)
                            If Not Await CancelAsync(lCurrentTask) Then Return
                            Dim ctsCancel As New CancellationTokenSource
                            SyncLock _cancelTokensLock
                                If lCurrentTask = _lastTaskRan Then
                                    _cancelTokenSources.Add(ctsCancel)
                                Else
                                    ctsCancel.Dispose()
                                    Return
                                End If
                            End SyncLock
                            Try
                                _isRunning = True
                                Await [function].Invoke(ctsCancel.Token)
                            Catch
                                Throw
                            Finally
                                SyncLock _cancelTokensLock
                                    _cancelTokenSources.Remove(ctsCancel)
                                End SyncLock
                                ctsCancel.Dispose()
                                _isRunning = False
                            End Try
                        End Function)
    End Function
    '''<summary>Queues the specified work to run on the thread pool And returns a proxy for the task returned by function And waits for completion.</summary>
    '''<returns>A task that represents a proxy for the task returned by function.</returns>
    '''<param name="function">The work to execute asynchronously.</param>
    '''<exception cref="ArgumentNullException">The function parameter was null.</exception>
    Public Async Function RunAsync([function] As Func(Of CancellationToken, Task)) As Task
        If [function] Is Nothing Then Throw New ArgumentNullException(NameOf([function]))
        Dim lCurrentTask As Long = Interlocked.Increment(_lastTaskRan)
        If Not Await CancelAsync(lCurrentTask) Then Return
        Dim ctsCancel As New CancellationTokenSource
        SyncLock _cancelTokensLock
            If lCurrentTask = _lastTaskRan Then
                _cancelTokenSources.Add(ctsCancel)
            Else
                ctsCancel.Dispose()
                Return
            End If
        End SyncLock
        Try
            _isRunning = True
            Await [function].Invoke(ctsCancel.Token)
        Catch
            Throw
        Finally
            SyncLock _cancelTokensLock
                _cancelTokenSources.Remove(ctsCancel)
            End SyncLock
            ctsCancel.Dispose()
            _isRunning = False
        End Try
    End Function
    '''<summary>Queues the specified work to run on the thread pool And returns a proxy for the task returned by function And waits for completion.</summary>
    '''<returns>A task that represents a proxy for the task returned by function.</returns>
    '''<typeparam name="T">The returning type of the task.</typeparam>
    '''<param name="function">The work to execute asynchronously.</param>
    '''<exception cref="ArgumentNullException">The function parameter was null.</exception>
    Public Async Function RunAsync(Of T)([function] As Func(Of CancellationToken, Task(Of T))) As Task(Of T)
        If [function] Is Nothing Then Throw New ArgumentNullException(NameOf([function]))
        Dim lCurrentTask As Long = Interlocked.Increment(_lastTaskRan)
        If Not Await CancelAsync(lCurrentTask) Then Return Activator.CreateInstance(GetType(T))
        Dim ctsCancel As New CancellationTokenSource
        SyncLock _cancelTokensLock
            If lCurrentTask = _lastTaskRan Then
                _cancelTokenSources.Add(ctsCancel)
            Else
                ctsCancel.Dispose()
                Return Activator.CreateInstance(GetType(T))
            End If
        End SyncLock
        Dim tReturn As T = Activator.CreateInstance(GetType(T))
        Try
            _isRunning = True
            tReturn = Await [function].Invoke(ctsCancel.Token)
        Catch
            Throw
        Finally
            SyncLock _cancelTokensLock
                _cancelTokenSources.Remove(ctsCancel)
            End SyncLock
            ctsCancel.Dispose()
            _isRunning = False
        End Try
        Return tReturn
    End Function

#End Region

End Class
