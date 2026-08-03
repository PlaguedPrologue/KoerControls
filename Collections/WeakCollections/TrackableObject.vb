Imports System.Collections.Concurrent
Imports System.Threading

'''<summary>Represents an <see cref="Object"/> that can be retreived while alive and not disposed.</summary>
'''<typeparam name="T">The <see cref="Type"/> of the <see cref="Object"/> to track.</typeparam>
Public MustInherit Class TrackableObject(Of T As TrackableObject(Of T))
    Implements IDisposable

#Region "Private Items"

    Private _disposed As Integer = 0
    Private Shared ReadOnly _instances As New ConcurrentDictionary(Of WeakReference(Of T), Byte)(ReferenceEqualityComparer.Instance)

    Protected Overridable Sub Dispose(disposing As Boolean)
        If disposing AndAlso Interlocked.Exchange(_disposed, 1) = 1 Then
        End If
    End Sub

#End Region

#Region "Initializers"

    '''<summary>Creates a new instance of <see cref="TrackableObject(Of T)"/> and adds it to the list of tracked instances.</summary>
    Public Sub New()
        _instances.TryAdd(New WeakReference(Of T)(Me), 0)
    End Sub

#End Region

#Region "Properties"

    '''<summary>Determines if the <see cref="Object"/> as been disposed.</summary>
    '''<returns><see langword="True"/> if the <see cref="Object"/> has been disposed; otherwise, <see langword="False"/>.</returns>
    Public ReadOnly Property Disposed As Boolean
        Get
            Return _disposed = 1
        End Get
    End Property
    '''<summary>Gets all active instances of the <see cref="Type"/> <typeparamref name="T"/>.</summary>
    '''<returns>All active instances of the <see cref="Type"/> <typeparamref name="T"/>.</returns>
    Public ReadOnly Iterator Property Items As IEnumerable(Of T)
        Get
            For Each kvpItem As KeyValuePair(Of WeakReference(Of T), Byte) In _instances
                Dim tItem As T = Nothing
                If kvpItem.Key.TryGetTarget(tItem) AndAlso tItem IsNot Nothing AndAlso Not tItem.Disposed Then
                    Yield tItem
                Else
                    _instances.TryRemove(kvpItem.Key, Nothing)
                End If
            Next
        End Get
    End Property

#End Region

#Region "Functions"

    Public Overridable Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
    '''<summary>Gets the first <typeparamref name="T"/> that fulfills the provided <see cref="Func(Of T, TResult)"/>.</summary>
    '''<returns>The first <typeparamref name="T"/> that fulfills the provided <see cref="Func(Of T, TResult)"/> if it exists; otherwise, <see langword="Nothing"/>.</returns>
    '''<param name="predicate">The <see cref="Func(Of T, Boolean)"/> to apply to the current tracked instances.</param>
    Public Shared Function FirstOrDefault(predicate As Func(Of T, Boolean)) As T
        For Each kvpItem As KeyValuePair(Of WeakReference(Of T), Byte) In _instances
            Dim tItem As T = Nothing
            If kvpItem.Key.TryGetTarget(tItem) AndAlso tItem IsNot Nothing AndAlso Not tItem.IsDisposed Then
                If predicate(tItem) Then Return tItem
            Else
                _instances.TryRemove(kvpItem.Key, Nothing)
            End If
        Next
        Return Nothing
    End Function
    '''<summary>Gets each <typeparamref name="T"/> that fulfills the provided <see cref="Func(Of T, TResult)"/>.</summary>
    '''<returns>Each <typeparamref name="T"/> that fulfills the provided <see cref="Func(Of T, TResult)"/> if it exists; otherwise, <see langword="Nothing"/></returns>
    '''<param name="predicate">The <see cref="Func(Of T, TResult)"/> to apply to the current tracked instances.</param>
    Public Shared Iterator Function Where(predicate As Func(Of T, Boolean)) As IEnumerable(Of T)
        For Each kvpItem As KeyValuePair(Of WeakReference(Of T), Byte) In _instances
            Dim tItem As T = Nothing
            If kvpItem.Key.TryGetTarget(tItem) AndAlso tItem IsNot Nothing AndAlso Not tItem.IsDisposed Then
                If predicate(tItem) Then Yield tItem
            Else
                _instances.TryRemove(kvpItem.Key, Nothing)
            End If
        Next
    End Function

#End Region

End Class
