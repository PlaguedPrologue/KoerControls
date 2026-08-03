Imports System.ComponentModel

'Stops a StaticColumnsDataGridView ColumnList from sorting. This keeps them sorted by DisplayIndex
Friend Class StaticDataGridViewColumnPropertyDescriptorCollection
    Inherits PropertyDescriptorCollection

    Public Sub New(properties() As PropertyDescriptor)
        MyBase.New(properties, False)
    End Sub

    Public Overrides Function Sort() As PropertyDescriptorCollection
        Return Me
    End Function
    Public Overrides Function Sort(comparer As IComparer) As PropertyDescriptorCollection
        Return Me
    End Function
    Public Overrides Function Sort(names() As String) As PropertyDescriptorCollection
        Return Me
    End Function
    Public Overrides Function Sort(names() As String, comparer As IComparer) As PropertyDescriptorCollection
        Return Me
    End Function

End Class
