Imports System.ComponentModel
Imports System.Reflection

'Provides property descriptions for each DataGridViewColumn in a StaticColumnsDataGridView, while removing the ability for the Form designer to create these columns
'Shows DataGridViewColumns to have their properties to be edited from a ColumnList in the Properties window
Friend Class StaticDataGridViewColumnPropertyDescriptor
    Inherits PropertyDescriptor

    Private ReadOnly _field As FieldInfo
    Private ReadOnly _owner As StaticColumnsDataGridView

    Public Sub New(field As FieldInfo, owner As StaticColumnsDataGridView)
        MyBase.New(field.Name.TrimStart("_"c), Nothing)
        _field = field
        _owner = owner
    End Sub

    'Stops the Form designer from defining the DataGridViewColumns in a StaticColumnsDataGridView, but still allows them to be accessed for setting column properties
    Public Overrides ReadOnly Property Attributes As AttributeCollection
        Get
            'Return New AttributeCollection(BrowsableAttribute.Yes, DesignerSerializationVisibilityAttribute.Content)
            Return New AttributeCollection(BrowsableAttribute.Yes)
        End Get
    End Property
    'Sets the component type to StaticColumnsDataGridView
    Public Overrides ReadOnly Property ComponentType As Type
        Get
            Return _field.DeclaringType
        End Get
    End Property
    Public Overrides ReadOnly Property Converter As TypeConverter
        Get
            Dim dgvcColumn As DataGridViewColumn = TryCast(GetValue(_owner), DataGridViewColumn)
            If dgvcColumn IsNot Nothing Then Return TypeDescriptor.GetConverter(dgvcColumn)
            Return MyBase.Converter
        End Get
    End Property
    'Provides a name ot display in the properties window
    Public Overrides ReadOnly Property DisplayName As String
        Get
            Dim dgvcColumn As DataGridViewColumn = _field.GetValue(_owner)
            If Not String.IsNullOrWhiteSpace(dgvcColumn?.HeaderText) Then Return dgvcColumn?.HeaderText
            Return $"{{{dgvcColumn?.Name.TrimStart("_"c)}}}"
        End Get
    End Property
    'Allows setting properties of the DataGridViewColumn
    Public Overrides ReadOnly Property IsReadOnly As Boolean
        Get
            Return False
        End Get
    End Property
    'Gets the type of of the DataGridViewColumn being described
    Public Overrides ReadOnly Property PropertyType As Type
        Get
            Return _field.FieldType
        End Get
    End Property

    'A DataGridViewColumn cannot be reset in a StaticColumnsDataGridView
    Public Overrides Function CanResetValue(component As Object) As Boolean
        Return False
    End Function
    Public Overrides Function GetEditor(editorBaseType As Type) As Object
        Dim dgvcColumn As DataGridViewColumn = TryCast(GetValue(_owner), DataGridViewColumn)
        If dgvcColumn IsNot Nothing Then Return TypeDescriptor.GetEditor(dgvcColumn, editorBaseType)
        Return MyBase.GetEditor(editorBaseType)
    End Function
    'Gets the DataGridViewColumn of the field in the StaticDataGridView
    Public Overrides Function GetValue(component As Object) As Object
        If TypeOf component Is StaticColumnsDataGridView Then Return _field.GetValue(component)
        If TypeOf component Is StaticDataGridViewColumnCollection Then Return _field.GetValue(DirectCast(component, StaticDataGridViewColumnCollection).DataGridView)
        Return Nothing
    End Function
    'A DataGridViewColumn cannot be reset in a StaticColumnsDataGridView
    Public Overrides Sub ResetValue(component As Object)
    End Sub
    'Sets the DataGridViewColumn of the field in the StaticDataGridView
    Public Overrides Sub SetValue(component As Object, value As Object)
        _field.SetValue(component, value)
        OnValueChanged(component, EventArgs.Empty)
    End Sub
    'Allows the Form designer to specify property values of the DataGridViewColumn through serialization rather than creation
    Public Overrides Function ShouldSerializeValue(component As Object) As Boolean
        Return True
    End Function

End Class
