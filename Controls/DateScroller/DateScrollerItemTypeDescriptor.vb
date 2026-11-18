Imports System.ComponentModel

Friend Class DateScrollerItemTypeDescriptor
    Inherits CustomTypeDescriptor
    Implements ICustomTypeDescriptor

    Private ReadOnly _owner As DateScrollerItem

    Public Sub New(parent As ICustomTypeDescriptor, owner As DateScrollerItem)
        MyBase.New(parent)
        _owner = owner
    End Sub

    Public Overrides Function GetProperties() As PropertyDescriptorCollection
        Return GetProperties(Nothing)
    End Function
    Public Overrides Function GetProperties(attributes() As Attribute) As PropertyDescriptorCollection
        If _owner Is Nothing Then Return MyBase.GetProperties(attributes)
        Dim pdProperties As List(Of PropertyDescriptor) = MyBase.GetProperties(attributes).Cast(Of PropertyDescriptor).ToList
        For i As Integer = 0 To pdProperties.Count - 1
            Dim pdProperty As PropertyDescriptor = pdProperties(i)
            If pdProperty.Name = NameOf(DateScrollerItem.ColumnIndex) Then
                pdProperties(i) = New ConditionalReadonlyPropertyDescriptor(pdProperty, Function() _owner?.DateScroller IsNot Nothing AndAlso _owner.DateScroller.ItemPlacement <> DateScrollerItemPlacement.UserDefined)
                Exit For
            End If
        Next
        Return New PropertyDescriptorCollection(pdProperties.ToArray, True)
    End Function

End Class
