Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Drawing.Design

'''<summary>Represents a collection of date scroller items.</summary>
<Editor(GetType(DateScrollerItemCollectionEditor), GetType(UITypeEditor))>
Public Class DateScrollerItemCollection
    Inherits Collection(Of DateScrollerItem)

    '''<summary>Initializes a new instance of the <see cref="DateScrollerItemCollection"/> class that is empty.</summary>
    '''<param name="owner">The owing <see cref="DateScroller"/> of the <see cref="Collection(Of T)"/>.</param>
    '''<exception cref="ArgumentNullException">owner is null.</exception>
    Public Sub New(owner As DateScroller)
        MyBase.New
        If owner Is Nothing Then Throw New ArgumentNullException(NameOf(owner))
        Me.Owner = owner
    End Sub

    '''<summary>Gets the owning <see cref="DateScroller"/> of the <see cref="Collection(Of T)"/>.</summary>
    '''<returns>The owning <see cref="DateScroller"/> of the <see cref="Collection(Of T)"/>.</returns>
    Public ReadOnly Property Owner As DateScroller

    Protected Overrides Sub ClearItems()
        For Each dsiItem As DateScrollerItem In ToArray
            If ReferenceEquals(dsiItem.Parent, Owner) Then Owner.Controls.Remove(dsiItem)
        Next
        MyBase.ClearItems()
    End Sub
    Protected Overrides Sub InsertItem(index As Integer, item As DateScrollerItem)
        If Not Contains(item) Then MyBase.InsertItem(index, item)
        If Not ReferenceEquals(item.Parent, Owner) Then Owner.Controls.Add(item)
    End Sub
    Protected Overrides Sub RemoveItem(index As Integer)
        If index >= Count Then Throw New ArgumentOutOfRangeException(NameOf(index))
        Dim dsiItem As DateScrollerItem = Me(index)
        MyBase.RemoveItem(index)
        If ReferenceEquals(dsiItem.Parent, Owner) Then Owner.Controls.Remove(dsiItem)
    End Sub
    Protected Overrides Sub SetItem(index As Integer, item As DateScrollerItem)
        If index >= Count Then Throw New ArgumentOutOfRangeException(NameOf(index))
        Dim dsiItem As DateScrollerItem = Me(index)
        If Not Contains(item) OrElse Not ReferenceEquals(item, dsiItem) Then MyBase.SetItem(index, item)
        If Owner IsNot Nothing AndAlso Not Owner.Controls.Contains(item) Then
            Owner.Controls.Remove(dsiItem)
            Owner.Controls.Add(item)
        End If
    End Sub

End Class
