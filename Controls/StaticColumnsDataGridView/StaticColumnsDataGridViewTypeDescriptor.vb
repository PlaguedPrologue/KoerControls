Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.ComponentModel.Design.Serialization
Imports System.Reflection

'Gets additional properties and discription for a StaticColumnsDataGridView
Friend Class StaticColumnsDataGridViewTypeDescriptor
    Inherits CustomTypeDescriptor
    Implements ICustomTypeDescriptor

    Private ReadOnly _owner As StaticColumnsDataGridView

    'Gets each column as a field in the class that implements the StaticColumnsDataGridView
    Private Iterator Function GetColumnFields() As IEnumerable(Of FieldInfo)
        Dim tType As Type = _owner.GetType
        While tType IsNot Nothing AndAlso tType IsNot GetType(DataGridView) AndAlso tType IsNot GetType(Component)
            For Each fiField As FieldInfo In tType.GetFields(BindingFlags.Instance Or BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.DeclaredOnly)
                If GetType(DataGridViewColumn).IsAssignableFrom(fiField.FieldType) Then Yield fiField
            Next
            tType = tType.BaseType
        End While
    End Function
    'Gets each tool strip item in the context menu strip as a field in the class that implements the StaticColumnsDataGridView
    Private Iterator Function GetToolStripItemFields() As IEnumerable(Of FieldInfo)
        Dim tType As Type = _owner.GetType
        While tType IsNot Nothing AndAlso tType IsNot GetType(DataGridView) AndAlso tType IsNot GetType(Component)
            For Each fiField As FieldInfo In tType.GetFields(BindingFlags.Instance Or BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.DeclaredOnly)
                If GetType(ToolStripItem).IsAssignableFrom(fiField.FieldType) Then Yield fiField
            Next
            tType = tType.BaseType
        End While
    End Function
    'Determines if editing StaticColumnsDataGridView control class, or editing an instance of the StaticColumnsDataGridView in a Form or another Control
    Private Function IsRootComponent() As Boolean
        If _owner?.Site Is Nothing Then Return False
        Dim dhHost As IDesignerHost = TryCast(_owner.Site.GetService(GetType(IDesignerHost)), IDesignerHost)
        Return dhHost IsNot Nothing AndAlso ReferenceEquals(dhHost.RootComponent, _owner)
    End Function

    Public Sub New(parent As ICustomTypeDescriptor, owner As StaticColumnsDataGridView)
        MyBase.New(parent)
        _owner = owner
    End Sub

    'Gets the base attributes AND assigns the StaticColumnsDataGridViewCodeDomSerializer to the class that implements the StaticColumnsDataGridView.
    Public Overrides Function GetAttributes() As AttributeCollection
        Dim aAttributes As List(Of Attribute) = MyBase.GetAttributes.Cast(Of Attribute).ToList
        aAttributes.Add(New DesignerSerializerAttribute(GetType(StaticColumnsDataGridViewCodeDomSerializer), GetType(CodeDomSerializer)))
        Return New AttributeCollection(aAttributes.ToArray)
    End Function
    'Gets the base properties AND each DataGridViewColumn in the class that implements the StaticColumnsDataGridView
    Public Overrides Function GetProperties() As PropertyDescriptorCollection
        Return GetProperties(Nothing)
    End Function
    'Gets the base properties AND each DataGridViewColumn in the class that implements the StaticColumnsDataGridView
    Public Overrides Function GetProperties(attributes() As Attribute) As PropertyDescriptorCollection
        If _owner Is Nothing Then Return MyBase.GetProperties(attributes)
        Dim pdProperties As List(Of PropertyDescriptor) = MyBase.GetProperties(attributes).Cast(Of PropertyDescriptor).ToList
        If IsRootComponent() Then pdProperties.RemoveAll(Function(pd) pd.Name = NameOf(StaticColumnsDataGridView.ColumnList) OrElse pd.Name = NameOf(StaticColumnsDataGridView.DefaultContextMenuStripItems))
        For Each fiField As FieldInfo In GetColumnFields()
            pdProperties.Add(New StaticDataGridViewColumnSerializationDescriptor(fiField, _owner))
        Next
        For Each fiField As FieldInfo In GetToolStripItemFields()
            pdProperties.Add(New StaticColumnsDataGridViewDefaultContextMenuStripItemSerializationDescriptor(fiField, _owner))
        Next
        Return New PropertyDescriptorCollection(pdProperties.ToArray)
    End Function

End Class
