Imports System.ComponentModel
Imports System.Globalization
Imports System.Reflection

Friend Class StaticDataGridViewColumnCollectionConverter
    Inherits ExpandableObjectConverter

    'Gets each DataGridViewColumn as a field in the class that implements the StaticColumnsDataGridView
    Private Iterator Function GetColumnFields(dataGridView As StaticColumnsDataGridView) As IEnumerable(Of FieldInfo)
        Dim tType As Type = dataGridView.GetType
        While tType IsNot Nothing AndAlso tType IsNot GetType(DataGridView) AndAlso tType IsNot GetType(Component)
            For Each fiField As FieldInfo In tType.GetFields(BindingFlags.Instance Or BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.DeclaredOnly)
                If GetType(DataGridViewColumn).IsAssignableFrom(fiField.FieldType) Then Yield fiField
            Next
            tType = tType.BaseType
        End While
    End Function

    Public Overrides Function ConvertTo(context As ITypeDescriptorContext, culture As CultureInfo, value As Object, destinationType As Type) As Object
        If destinationType = GetType(String) AndAlso TypeOf value Is StaticDataGridViewColumnCollection Then Return $"{DirectCast(value, StaticDataGridViewColumnCollection).Count} Columns"
        Return MyBase.ConvertTo(context, culture, value, destinationType)
    End Function
    Public Overrides Function GetPropertiesSupported(context As ITypeDescriptorContext) As Boolean
        Return True
    End Function
    'Adds each DataGridViewColumn as an expandable property in the StaticColumnsDataGridView's ColumnList Property
    Public Overrides Function GetProperties(context As ITypeDescriptorContext, value As Object, attributes() As Attribute) As PropertyDescriptorCollection
        Dim sdgvccColumns As StaticDataGridViewColumnCollection = TryCast(value, StaticDataGridViewColumnCollection)
        If sdgvccColumns Is Nothing Then Return PropertyDescriptorCollection.Empty
        Dim pdProperties As New List(Of PropertyDescriptor)
        For Each fiField As FieldInfo In GetColumnFields(sdgvccColumns.DataGridView)
            pdProperties.Add(New StaticDataGridViewColumnPropertyDescriptor(fiField, sdgvccColumns.DataGridView))
        Next
        Return New StaticDataGridViewColumnPropertyDescriptorCollection(pdProperties.ToArray)
    End Function
    Public Overrides Function GetStandardValuesSupported(context As ITypeDescriptorContext) As Boolean
        Return False
    End Function

End Class
