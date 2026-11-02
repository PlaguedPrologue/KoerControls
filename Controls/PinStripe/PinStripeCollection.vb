Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Drawing.Design

'''<summary>Repersents a collection of pin stripes.</summary>
<Editor(GetType(PinStripeCollectionEditor), GetType(UITypeEditor))>
Public Class PinStripeCollection
    Inherits Collection(Of PinStripe)

    '''<summary>Initializes a new instance of the <see cref="PinStripeCollection"/> class that is empty.</summary>
    '''<param name="owner">The owing <see cref="PinStripeList"/> of the <see cref="Collection(Of T)"/>.</param>
    Public Sub New(owner As PinStripeList)
        MyBase.New
        Me.Owner = owner
    End Sub

    '''<summary>The owning <see cref="PinStripeList"/> of the <see cref="Collection(Of T)"/>.</summary>
    Public ReadOnly Property Owner As PinStripeList

    '''<summary>Adds an object to the end of the <see cref="Collection(Of T)"/>.</summary>
    '''<param name="item">The object to be added to the end of the <see cref="Collection(Of T)"/>. The value can be null for reference types.</param>
    Public Overloads Sub Add(item As PinStripe)
        MyBase.Add(item)
        If Not ReferenceEquals(item.PinStripeList, Owner) Then item.PinStripeList = Owner
    End Sub

End Class
