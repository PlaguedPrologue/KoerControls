Imports System.Runtime.Serialization
Imports System.Security

Public Class ObservableWeakReference(Of T As Class)
    Implements ISerializable

#Region "Private Items"

    Private _currentTargetIsNull As Boolean
    Private ReadOnly _source As WeakReference(Of T)

#End Region

#Region "Events"

    '''<summary>Occurs when the target  has been changed.</summary>
    Public Event TargetChanged As EventHandler

#End Region

#Region "Initializers"

    '''<summary>Initializes a new instance of the <see cref="ObservableWeakReference(Of T)"/> class that references the specified object.</summary>
    Public Sub New()
        _currentTargetIsNull = True
        _source = New WeakReference(Of T)(Nothing)
    End Sub
    '''<summary>Initializes a new instance of the <see cref="ObservableWeakReference(Of T)"/> class that references the specified object.</summary>
    '''<param name="target">The object to reference, or null.</param>
    Public Sub New(target As T)
        _currentTargetIsNull = target Is Nothing
        _source = New WeakReference(Of T)(target)
    End Sub
    '''<summary>Initializes a new instance of the <see cref="ObservableWeakReference(Of T)"/> class that references the specified object and uses the specified resurrection tracking.</summary>
    '''<param name="target">The object to reference, or null.</param>
    '''<param name="trackResurrection"><see langword="True"/> to track the object after finalization; <see langword="False"/> to track the object only until finalization.</param>
    Public Sub New(target As T, trackResurrection As Boolean)
        _currentTargetIsNull = target Is Nothing
        _source = New WeakReference(Of T)(target, trackResurrection)
    End Sub
    '''<summary>Initializes a new instance of the <see cref="ObservableWeakReference(Of T)"/> class that references the specified object and uses the specified resurrection tracking.</summary>
    '''<param name="source">The source <see cref="WeakReference"/> to use as the <see cref="ObservableWeakReference"/>.</param>
    Public Sub New(source As WeakReference(Of T))
        Dim tTarget As T = Nothing
        _currentTargetIsNull = source.TryGetTarget(tTarget) AndAlso tTarget Is Nothing
        _source = source
    End Sub
    '''<summary>Initializes a new instance of the <see cref="ObservableWeakReference(Of T)"/> class that references the specified object and uses the specified resurrection tracking.</summary>
    '''<param name="source">The source <see cref="ObservableWeakReference"/> to use as the <see cref="ObservableWeakReference"/>.</param>
    Public Sub New(source As ObservableWeakReference(Of T))
        Dim tTarget As T = Nothing
        _currentTargetIsNull = source.TryGetTarget(tTarget) AndAlso tTarget Is Nothing
        _source = source._source
    End Sub

#End Region

#Region "Functions"

    '''<summary>Populates a <see cref="SerializationInfo"/> object with all the data necessary to serialize the current <see cref="ObservableWeakReference"/> object.</summary>
    '''<param name="info">An object that holds all the data necessary to serialize or deserialize the current <see cref="ObservableWeakReference"/>.</param>
    '''<param name="context">The location where serialized data is stored and retrieved.</param>
    '''<exception cref="ArgumentNullException"><paramref name="info"/> is null.</exception>
    <SecurityCritical>
    Public Sub GetObjectData(info As SerializationInfo, context As StreamingContext) Implements ISerializable.GetObjectData
        _source.GetObjectData(info, context)
    End Sub
    '''<summary>Raises the <see cref="TargetChanged"/> event.</summary>
    '''<param name="e">The <see cref="EventArgs"/> that contains the event data.</param>
    Protected Overridable Sub OnTargetChanged(e As EventArgs)
        RaiseEvent TargetChanged(Me, EventArgs.Empty)
    End Sub
    '''<summary>Sets the target object that is referenced by the <see cref="ObservableWeakReference"/>.</summary>
    '''<param name="target">The new target object.</param>
    Public Sub SetTarget(target As T)
        Dim tCurrentTarget As T = Nothing
        _source.TryGetTarget(tCurrentTarget)
        Dim bTargetIsNull As Boolean = target Is Nothing

        If bTargetIsNull AndAlso tCurrentTarget Is Nothing AndAlso Not _currentTargetIsNull Then 'Target appears to stay null
            If Not _currentTargetIsNull Then 'Target appears to stay null, but the last set target was not null. I.e., original target disposed without being set and new target is set to null
                _currentTargetIsNull = True 'State that target is null
                OnTargetChanged(EventArgs.Empty) 'Raise TargetChanged event
            End If
            Return 'All work done
        End If
        If ReferenceEquals(target, tCurrentTarget) Then Return 'Target does not change

        _currentTargetIsNull = bTargetIsNull 'State whether the target is null or not
        _source.SetTarget(target) 'Set the target
        OnTargetChanged(EventArgs.Empty) 'Raise TargetChanged event
    End Sub
    '''<summary>Tries to retrieve the target object that is referenced by the current <see cref="ObservableWeakReference"/>.</summary>
    '''<returns><see langword="True"/> if the target was retrieved; otherwise <see langword="False"/>.</returns>
    '''<param name="target">When this method returns, contains the target object, if it is available. This parameter is treated as uninitialized.</param>
    Public Function TryGetTarget(ByRef target As T) As Boolean
        If _source.TryGetTarget(target) Then Return True
        If Not _currentTargetIsNull Then
            _currentTargetIsNull = True
            OnTargetChanged(EventArgs.Empty)
        End If
        Return False
    End Function

#End Region

End Class
