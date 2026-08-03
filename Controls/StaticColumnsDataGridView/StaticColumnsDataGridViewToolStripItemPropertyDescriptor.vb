Imports System.ComponentModel
Imports System.Reflection

'Provides property descriptions for each ToolStripItem  in a StaticColumnsDataGridView, while removing the ability for the Form designer to create these items
'Shows ToolStripItems to have their properties to be edited from a ToolStripItemList in the Properties window
Friend Class StaticColumnsDataGridViewToolStripItemPropertyDescriptor
    Inherits PropertyDescriptor

    Private ReadOnly _field As FieldInfo
    Private ReadOnly _owner As StaticColumnsDataGridView

    Public Sub New(field As FieldInfo, owner As StaticColumnsDataGridView)
        MyBase.New(field.Name.TrimStart("_"c), Nothing)
        _field = field
        _owner = owner
    End Sub

    'Stops the Form designer from defining the ToolStripItems in a StaticColumnsDataGridView, but still allows them to be accessed for setting item properties
    Public Overrides ReadOnly Property Attributes As AttributeCollection
        Get
            Return New AttributeCollection(BrowsableAttribute.Yes, DesignerSerializationVisibilityAttribute.Content)
        End Get
    End Property
    'Sets the component type to StaticColumnsDataGridView
    Public Overrides ReadOnly Property ComponentType As Type
        Get
            Return _field.DeclaringType
        End Get
    End Property
    Public Overrides ReadOnly Property DisplayName As String
        Get
            Dim tsiItem As ToolStripItem = _field.GetValue(_owner)
            If Not String.IsNullOrWhiteSpace(tsiItem?.Text) Then Return tsiItem?.Text
            Return $"{{{tsiItem?.Name.TrimStart("_"c)}}}"
        End Get
    End Property
    'Allows setting properties of the ToolStripItem
    Public Overrides ReadOnly Property IsReadOnly As Boolean
        Get
            Return False
        End Get
    End Property
    'Gets the type of of the ToolStripItem being described
    Public Overrides ReadOnly Property PropertyType As Type
        Get
            Return _field.FieldType
        End Get
    End Property

    'A ToolStripItem cannot be reset in a StaticColumnsDataGridView
    Public Overrides Function CanResetValue(component As Object) As Boolean
        Return False
    End Function
    'Gets the TollStripItem of the field in the StaticDataGridView
    Public Overrides Function GetValue(component As Object) As Object
        If TypeOf component Is StaticColumnsDataGridView Then Return _field.GetValue(component)
        If TypeOf component Is StaticColumnsDataGridViewToolStripItemCollection Then Return _field.GetValue(DirectCast(component, StaticColumnsDataGridViewToolStripItemCollection).DataGridView)
        Return Nothing
    End Function
    'A ToolStripItem cannot be reset in a StaticColumnsDataGridView
    Public Overrides Sub ResetValue(component As Object)
    End Sub
    'Sets the ToolStripItem of the field in the StaticDataGridView
    Public Overrides Sub SetValue(component As Object, value As Object)
        _field.SetValue(component, value)
        OnValueChanged(component, EventArgs.Empty)
    End Sub
    'Allows the Form designer to specify property values of the ToolStripItem through serialization rather than creation
    Public Overrides Function ShouldSerializeValue(component As Object) As Boolean
        Return True
    End Function

End Class
