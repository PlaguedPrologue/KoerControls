Imports System.Collections.Specialized

'''<summary>Provides data for the <see cref="INotifyHashSetChanged.CollectionChanged"/> event.</summary>
Public Class NotifyHashSetChangedEventArgs
    Inherits EventArgs

#Region "Initializers"

    '''<summary>Initializes a new instance of the <see cref="NotifyHashSetChangedEventArgs"/> class that describes a <see cref="NotifyCollectionChangedAction.Reset"/> change.</summary>
    '''<param name="action">The action that caused the event. This must be set to <see cref="NotifyCollectionChangedAction.Reset"/>.</param>
    Public Sub New(action As NotifyCollectionChangedAction)
        Me.Action = action
        NewItems = Nothing
        OldItems = Nothing
    End Sub
    '''<summary>Initializes a new instance of the <see cref="NotifyHashSetChangedEventArgs"/> class that describes a <see cref="NotifyCollectionChangedAction.Reset"/> change.</summary>
    '''<param name="action">The action that caused the event. This must be set to <see cref="NotifyCollectionChangedAction.Reset"/>, <see cref="NotifyCollectionChangedAction.Add"/>, or <see cref="NotifyCollectionChangedAction.Remove"/>.</param>
    '''<param name="changedItem">The item that is affected by the change.</param>
    '''<exception cref="ArgumentException">The action is not Reset, Add, or Remove - OR - the action is Reset and changedItem is not null.</exception>
    Public Sub New(action As NotifyCollectionChangedAction, changedItem As Object)
        Me.Action = action
        Select Case action
            Case NotifyCollectionChangedAction.Add
                NewItems = New Array() {changedItem}
                OldItems = Nothing
            Case NotifyCollectionChangedAction.Remove
                NewItems = Nothing
                OldItems = New Array() {changedItem}
            Case NotifyCollectionChangedAction.Reset
                If changedItem IsNot Nothing Then Throw New ArgumentException("Reset action cannot have items.")
                NewItems = Nothing
                OldItems = Nothing
            Case Else
                Throw New ArgumentException("Invalid action.")
        End Select
    End Sub
    '''<summary>Initializes a new instance of the <see cref="NotifyHashSetChangedEventArgs"/> class that describes a <see cref="NotifyCollectionChangedAction.Reset"/> change.</summary>
    '''<param name="action">The action that caused the event. This must be set to <see cref="NotifyCollectionChangedAction.Reset"/>, <see cref="NotifyCollectionChangedAction.Add"/>, or <see cref="NotifyCollectionChangedAction.Remove"/>.</param>
    '''<param name="changedItems">The items that are affected by the change.</param>
    '''<exception cref="ArgumentException">The action is not Reset, Add, or Remove - OR - the action is Reset and changedItem is not null.</exception>
    Public Sub New(action As NotifyCollectionChangedAction, changedItems As IEnumerable)
        Me.Action = action
        Select Case action
            Case NotifyCollectionChangedAction.Add
                NewItems = changedItems
                OldItems = Nothing
            Case NotifyCollectionChangedAction.Remove
                NewItems = Nothing
                OldItems = changedItems
            Case NotifyCollectionChangedAction.Reset
                If changedItems IsNot Nothing Then Throw New ArgumentException("Reset action cannot have items.")
                NewItems = Nothing
                OldItems = Nothing
            Case Else
                Throw New ArgumentException("Invalid action.")
        End Select
    End Sub


#End Region

#Region "Properties"

    '''<summary>Gets the action that caused the event.</summary>
    '''<value>A <see cref="NotifyCollectionChangedAction"/> value that describes the action that caused the event.</value>
    Public ReadOnly Property Action As NotifyCollectionChangedAction
    '''<summary>Gets the list of new items involved in the change.</summary>
    '''<value>THe list of new items involved in the change.</value>
    Public ReadOnly Property NewItems As IEnumerable
    '''<summary>Gets the list of items affected by a <see cref="NotifyCollectionChangedAction.Replace"/> or <see cref="NotifyCollectionChangedAction.Remove"/> action.</summary>
    '''<value>The list of items affected by a <see cref="NotifyCollectionChangedAction.Replace"/> or <see cref="NotifyCollectionChangedAction.Remove"/> action.</value>
    Public ReadOnly Property OldItems As IEnumerable

#End Region

End Class
