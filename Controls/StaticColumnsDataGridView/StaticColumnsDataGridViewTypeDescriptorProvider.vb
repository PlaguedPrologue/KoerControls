Imports System.ComponentModel

'Privides the StaticColumnsDataGridViewTypeDescriptor for a class that implements a StaticColumnsDataGridView
Public Class StaticColumnsDataGridViewTypeDescriptorProvider
    Inherits TypeDescriptionProvider

    Public Sub New()
        MyBase.New(TypeDescriptor.GetProvider(GetType(DataGridView)))
    End Sub

    'Privides the StaticColumnsDataGridViewTypeDescriptor for a class that implements a StaticColumnsDataGridView
    Public Overrides Function GetTypeDescriptor(objectType As Type, instance As Object) As ICustomTypeDescriptor
        Dim ctdDescriptor As ICustomTypeDescriptor = MyBase.GetTypeDescriptor(objectType, instance)
        Dim scdgvDataGridView As StaticColumnsDataGridView = TryCast(instance, StaticColumnsDataGridView)
        If scdgvDataGridView Is Nothing Then Return ctdDescriptor
        Return New StaticColumnsDataGridViewTypeDescriptor(ctdDescriptor, scdgvDataGridView)
    End Function

End Class
