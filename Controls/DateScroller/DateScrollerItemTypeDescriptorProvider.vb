Imports System.ComponentModel

'''<summary>Privides the <see cref="DateScrollerItemTypeDescriptor"/> for a class that implements a <see cref="DateScrollerItem"/>.</summary>
Public Class DateScrollerItemTypeDescriptorProvider
    Inherits TypeDescriptionProvider

    Public Sub New()
        MyBase.New(TypeDescriptor.GetProvider(GetType(Panel)))
    End Sub

    'Privides the StaticColumnsDataGridViewTypeDescriptor for a class that implements a StaticColumnsDataGridView
    Public Overrides Function GetTypeDescriptor(objectType As Type, instance As Object) As ICustomTypeDescriptor
        Dim ctdDescriptor As ICustomTypeDescriptor = MyBase.GetTypeDescriptor(objectType, instance)
        Dim dsiDateScrollerItem As DateScrollerItem = TryCast(instance, DateScrollerItem)
        If dsiDateScrollerItem Is Nothing Then Return ctdDescriptor
        Return New DateScrollerItemTypeDescriptor(ctdDescriptor, dsiDateScrollerItem)
    End Function

End Class
