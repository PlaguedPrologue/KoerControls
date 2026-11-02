Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Reflection

'''<summary>Represents a <see cref="DataGridView"/> with pre-defined columns craeted during initialization.</summary>
<Designer(GetType(StaticColumnsDataGridViewDesigner))>
<ToolboxItem(False)>
Public Class StaticColumnsDataGridView
    Inherits DataGridView

#Region "Private Items"

    Private ReadOnly _columnList As New StaticDataGridViewColumnCollection(Me)
    Private _columnsInitialized As Boolean = False
    Private _defaultContextMenuStripItems As New StaticColumnsDataGridViewDefaultContextMenuStripItemCollection(DefaultContextMenuStrip)
    Private Shared ReadOnly _defaultContextMenuStrips As New Dictionary(Of Type, ContextMenuStrip)
    Private Shared ReadOnly _typeDescriptorProvidersAdded As New HashSet(Of Type)

    Private Sub EnsureColumns()
        If _columnsInitialized Then Return
        _columnsInitialized = True
        For Each fiField As FieldInfo In [GetType].GetFields(BindingFlags.Instance Or BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.DeclaredOnly)
            If GetType(DataGridViewColumn).IsAssignableFrom(fiField.FieldType) Then
                Dim dgvcColumn As DataGridViewColumn = fiField.GetValue(Me)
                If dgvcColumn IsNot Nothing AndAlso dgvcColumn.DataGridView Is Nothing Then Columns.Add(dgvcColumn)
            End If
        Next
    End Sub
    Private Function GetDesignerType() As Type
        Dim host = TryCast(Site?.GetService(GetType(IDesignerHost)), IDesignerHost)
        If host Is Nothing OrElse host.RootComponent Is Nothing Then Return MyBase.GetType
        Dim typeName = host.RootComponent.Site?.Name
        If String.IsNullOrEmpty(typeName) Then Return host.RootComponent.GetType
        Dim asm = host.RootComponent.GetType.Assembly
        Return If(asm.GetTypes.FirstOrDefault(Function(t) t.Name = typeName), host.RootComponent.GetType)
    End Function

#End Region

#Region "Events"

    '''<summary>Occurs when the value of the <see cref="DefaultContextMenuStrip"/> property changes.</summary>
    Public Event DefaultContextMenuStripChanged As EventHandler

#End Region

#Region "Initializers"

    Public Sub New()
        MyBase.New

        Dim tType As Type = GetDesignerType()
        If _typeDescriptorProvidersAdded.Add(tType) Then TypeDescriptor.AddProvider(New StaticColumnsDataGridViewTypeDescriptorProvider, tType)

        ContextMenuStrip = DefaultContextMenuStrip
    End Sub

#End Region

#Region "Properties"

    '''<summary>Gets a collection that contains all the columns in the control.</summary>
    '''<returns>The <see cref="StaticDataGridViewColumnCollection"/> that contains all the columns in the <see cref="DataGridView"/> control.</returns>
    <Category("Data")>
    <Description("Columns")>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)>
    Public ReadOnly Property ColumnList As StaticDataGridViewColumnCollection
        Get
            Return _columnList
        End Get
    End Property
    '''<summary>Gets a collection that contains all the tool strip items in the default context menu strip assigned to the control.</summary>
    '''<returns>The <see cref="StaticColumnsDataGridViewDefaultContextMenuStripItemCollection"/> that contains each <see cref="ToolStripItem"/> in the <see cref="DefaultContextMenuStrip"/> assigned to the <see cref="DataGridView"/> control</returns>
    <Category("Behavior")>
    <Description("The default items in the shortcut menu to display when the user right-clicks the control.")>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)>
    Public ReadOnly Property DefaultContextMenuStripItems As StaticColumnsDataGridViewDefaultContextMenuStripItemCollection
        Get
            Return _defaultContextMenuStripItems
        End Get
    End Property
    '''<summary>Gets the default <see cref="Windows.Forms.ContextMenuStrip"/> the control will use.</summary>
    '''<returns>The default <see cref="Windows.Forms.ContextMenuStrip"/> the control will use.</returns>
    <Category("Behavior")>
    <DefaultValue(GetType(ContextMenuStrip), Nothing)>
    <Description("The default shortcut menu to display when the user right-clicks the control.")>
    Public Property DefaultContextMenuStrip As ContextMenuStrip
        Get
            Dim cmsDefaultContextMenuStrip As ContextMenuStrip = Nothing
            _defaultContextMenuStrips.TryGetValue([GetType], cmsDefaultContextMenuStrip)
            Return cmsDefaultContextMenuStrip
        End Get
        Set(value As ContextMenuStrip)
            Dim cmsDefaultContextMenuStrip As ContextMenuStrip = Nothing
            _defaultContextMenuStrips.TryGetValue([GetType], cmsDefaultContextMenuStrip)
            If ReferenceEquals(cmsDefaultContextMenuStrip, value) Then Return

            If value Is Nothing Then
                _defaultContextMenuStrips.Remove([GetType])
            Else
                _defaultContextMenuStrips([GetType]) = value
            End If
            ContextMenuStrip = value
            _defaultContextMenuStripItems = New StaticColumnsDataGridViewDefaultContextMenuStripItemCollection(value)

            OnDefaultContextMenuStripChanged(EventArgs.Empty)
        End Set
    End Property

#End Region

#Region "Functions"

    '''<summary>Gets the tool strip item in the context menu strip with the specified name.</summary>
    '''<returns>The tool strip item with the specified name.</returns>
    '''<param name="toolStripItemName">The name of the tool strip item to get.</param>
    Public Function GetDefaultContextMenuStripItem(toolStripItemName As String) As ToolStripItem
        Return DefaultContextMenuStripItems(toolStripItemName)
    End Function
    '''<summary>Gets the static column with the specified name.</summary>
    '''<returns>The static column with the specified name.</returns>
    '''<param name="columnName">The name of the column to get.</param>
    Public Function GetStaticColumn(columnName As String) As DataGridViewColumn
        EnsureColumns()
        Return ColumnList(columnName)
    End Function
    Protected Overrides Sub InitLayout()
        MyBase.InitLayout()
        EnsureColumns()
    End Sub
    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        EnsureColumns()
    End Sub
    '''<summary>Raises the <see cref="StaticColumnsDataGridView.DefaultContextMenuStripChanged"/> event.</summary>
    '''<param name="e">The <see cref="EventArgs"/> taht contains the event data.</param>
    Protected Overridable Sub OnDefaultContextMenuStripChanged(e As EventArgs)
        RaiseEvent DefaultContextMenuStripChanged(Me, e)
    End Sub

#End Region

End Class
