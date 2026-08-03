Imports System.ComponentModel
Imports System.Globalization
Imports System.Reflection

Friend Class StaticColumnsDataGridViewToolStripItemCollectionConverter
    Inherits ExpandableObjectConverter

    'Gets each ToolStripItem as a field in the class that implements the StaticColumnsDataGridView
    Private Iterator Function GetToolStripItemFields(dataGridView As StaticColumnsDataGridView) As IEnumerable(Of FieldInfo)
        Dim tType As Type = dataGridView.GetType
        While tType IsNot Nothing AndAlso tType IsNot GetType(DataGridView) AndAlso tType IsNot GetType(Component)
            For Each fiField As FieldInfo In tType.GetFields(BindingFlags.Instance Or BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.DeclaredOnly)
                If GetType(ToolStripItem).IsAssignableFrom(fiField.FieldType) Then Yield fiField
            Next
            tType = tType.BaseType
        End While
    End Function

    Public Overrides Function ConvertTo(context As ITypeDescriptorContext, culture As CultureInfo, value As Object, destinationType As Type) As Object
        If destinationType = GetType(String) AndAlso TypeOf value Is StaticColumnsDataGridViewToolStripItemCollection Then
            If DirectCast(value, StaticColumnsDataGridViewToolStripItemCollection).DataGridView.ContextMenuStrip Is Nothing Then Return "N/A"
            Return $"{DirectCast(value, StaticColumnsDataGridViewToolStripItemCollection).Count} Items"
        End If
        Return MyBase.ConvertTo(context, culture, value, destinationType)
    End Function
    Public Overrides Function GetPropertiesSupported(context As ITypeDescriptorContext) As Boolean
        Return True
    End Function
    'Adds each ToolStripItem as an expandable property in the StaticColumnsDataGridView's ToolStripItemList Property
    Public Overrides Function GetProperties(context As ITypeDescriptorContext, value As Object, attributes() As Attribute) As PropertyDescriptorCollection
        Dim scdgvtsicToolStripItems As StaticColumnsDataGridViewToolStripItemCollection = TryCast(value, StaticColumnsDataGridViewToolStripItemCollection)
        If scdgvtsicToolStripItems Is Nothing Then Return PropertyDescriptorCollection.Empty
        Dim pdProperties As New List(Of PropertyDescriptor)
        For Each fiField As FieldInfo In GetToolStripItemFields(scdgvtsicToolStripItems.DataGridView)
            pdProperties.Add(New StaticColumnsDataGridViewToolStripItemPropertyDescriptor(fiField, scdgvtsicToolStripItems.DataGridView))
        Next
        Return New StaticColumnsDataGridViewToolStripItemPropertyDescriptorCollection(pdProperties.ToArray)
    End Function
    Public Overrides Function GetStandardValuesSupported(context As ITypeDescriptorContext) As Boolean
        Return False
    End Function

End Class
