Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Reflection

'''<summary>Represents a <see cref="DataGridView"/> with pre-defined columns craeted during initialization.</summary>
'''<remarks>This class must be inherited to use correctly. To create a new StaticColumnsDataGridView control, do the following:
'''<para/>Add a new item to the desired file location. Select 'User Control (Windows Forms)'
'''<para/>Open the designer file (expand the file object and double click 'InitializeComponent()') and replace the 'Inherits System.Windows.Forms.UserControl' line at the top with 'Inherits LincolnControls.StaticColumnsDataGridView'.
'''<para/>In the main class file, set &lt;ToolBoxItem(True)&gt; above the class declaration.
'''<para/>To add/edit columns, go to the designer and add through the 'Properties' window.
'''<para/>When finished, rebuild your project and the new control will be available to drop into any Form.</remarks>
<Designer(GetType(StaticColumnsDataGridViewDesigner))>
<ToolboxItem(False)>
<TypeDescriptionProvider(GetType(StaticColumnsDataGridViewTypeDescriptorProvider))>
Public Class StaticColumnsDataGridView
    Inherits DataGridView

#Region "Private Items"

    Private ReadOnly _columnList As New StaticDataGridViewColumnCollection(Me)
    Private _columnsInitialized As Boolean = False
    Private _contextMenuStripInitialized As Boolean = False
    Private Shared ReadOnly _preDefinedContextMenuStrips As New Dictionary(Of Type, ContextMenuStrip)
    Private ReadOnly _contextMenuStripItems As New StaticColumnsDataGridViewToolStripItemCollection(Me)
    Private Shared _typeDescriptorProvidersAdded As New HashSet(Of Type)

    Private Sub EnsureColumns()
        If _columnsInitialized Then Return
        _columnsInitialized = True
        Try
            For Each fiField As FieldInfo In [GetType].GetFields(BindingFlags.Instance Or BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.DeclaredOnly)
                If GetType(DataGridViewColumn).IsAssignableFrom(fiField.FieldType) Then
                    Dim dgvcColumn As DataGridViewColumn = fiField.GetValue(Me)
                    If dgvcColumn IsNot Nothing AndAlso dgvcColumn.DataGridView Is Nothing Then MyBase.Columns.Add(dgvcColumn)
                End If
            Next
        Catch
        End Try
    End Sub
    Private Sub EnsureContextMenuStrip()
        If _contextMenuStripInitialized Then Return
        _contextMenuStripInitialized = True
        Dim cmsContextMenuStrip As ContextMenuStrip = Nothing
        _preDefinedContextMenuStrips.TryGetValue([GetType], cmsContextMenuStrip)
        If cmsContextMenuStrip IsNot Nothing Then ContextMenuStrip = cmsContextMenuStrip
    End Sub
    Private Function GetDesignerType() As Type
        Dim host = TryCast(Site?.GetService(GetType(IDesignerHost)), IDesignerHost)
        If host Is Nothing OrElse host.RootComponent Is Nothing Then Return MyBase.GetType
        Dim typeName = host.RootComponent.Site?.Name
        If String.IsNullOrEmpty(typeName) Then Return host.RootComponent.GetType
        Dim asm = host.RootComponent.GetType.Assembly
        Return If(asm.GetTypes.FirstOrDefault(Function(t) t.Name = typeName), host.RootComponent.GetType)
    End Function
    Private Function IsRootComponent() As Boolean
        If Site Is Nothing Then Return False
        Dim dhHost As IDesignerHost = TryCast(Site.GetService(GetType(IDesignerHost)), IDesignerHost)
        Return dhHost IsNot Nothing AndAlso ReferenceEquals(dhHost.RootComponent, Me)
    End Function

#End Region

#Region "Initializers"

    Public Sub New()
        MyBase.New

        Dim tType As Type = GetDesignerType()
        If _typeDescriptorProvidersAdded.Add(tType) Then TypeDescriptor.AddProvider(New StaticColumnsDataGridViewTypeDescriptorProvider, tType)
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
    '''<summary>Gets a collection that contains all the tool strip items in the context menu strip assigned to the control.</summary>
    '''<returns>The <see cref="StaticColumnsDataGridViewToolStripItemCollection"/> that contains each <see cref="ToolStripItem"/> in the <see cref="ContextMenuStrip"/> assigned to the <see cref="DataGridView"/> control</returns>
    <Category("Behavior")>
    <Description("The items in the shortcut menu to display when the user right-clicks the control.")>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)>
    Public ReadOnly Property ContextMenuStripItems As StaticColumnsDataGridViewToolStripItemCollection
        Get
            Return _contextMenuStripItems
        End Get
    End Property
    '''<summary>Gets the <see cref="Windows.Forms.ContextMenuStrip"/> the control will be foreced to use. If no pre defined context menu strip is set, the control may use any context menu strip.</summary>
    '''<returns>The <see cref="Windows.Forms.ContextMenuStrip"/> the control will be forced to use.</returns>
    <Browsable(False)>
    Public ReadOnly Property PreDefinedCotextMenuStrip As ContextMenuStrip
        Get
            EnsureContextMenuStrip()
            Dim cmsContextMenuStrip As ContextMenuStrip = Nothing
            _preDefinedContextMenuStrips.TryGetValue([GetType], cmsContextMenuStrip)
            Return cmsContextMenuStrip
        End Get
    End Property

#End Region

#Region "Functions"

    '''<summary>Gets the static column with the specified name.</summary>
    '''<returns>The static column with the specified name.</returns>
    '''<param name="columnName">The name of the column to get.</param>
    Public Function GetStaticColumn(columnName As String) As DataGridViewColumn
        EnsureColumns()
        Return ColumnList(columnName)
    End Function
    '''<summary>Gets the tool strip item in the context menu strip with the specified name.</summary>
    '''<returns>The tool strip item with the specified name.</returns>
    '''<param name="toolStripItemName">The name of the tool strip item to get.</param>
    Public Function GetContextMenuStripItem(toolStripItemName As String) As ToolStripItem
        EnsureContextMenuStrip()
        If ContextMenuStrip Is Nothing Then Return Nothing
        Return ContextMenuStrip.Items(toolStripItemName)
    End Function
    Protected Overrides Sub InitLayout()
        MyBase.InitLayout()
        EnsureColumns()
        EnsureContextMenuStrip()
    End Sub
    Protected Overrides Sub OnContextMenuStripChanged(e As EventArgs)
        Dim tType As Type = GetDesignerType()
        If DesignMode AndAlso IsRootComponent() Then 'Determine if designing a StaticColumnsDataGridView class
            If ContextMenuStrip Is Nothing Then
                If _preDefinedContextMenuStrips.ContainsKey(tType) Then _preDefinedContextMenuStrips.Remove(tType)
            Else
                _preDefinedContextMenuStrips(tType) = ContextMenuStrip
            End If
        ElseIf Not IsRootComponent() Then 'Changing property value of a StaticColumnsDataGridView object
            If ContextMenuStrip Is Nothing Then
                Dim cmsPreDefinedContextMenuStrip As ContextMenuStrip = Nothing
                _preDefinedContextMenuStrips.TryGetValue(tType, cmsPreDefinedContextMenuStrip)
                If cmsPreDefinedContextMenuStrip IsNot Nothing Then
                    ContextMenuStrip = cmsPreDefinedContextMenuStrip
                    Return 'Exit, as setting the ContextMenuStrip again will trigger the OnContextMenuStrip again
                End If
            End If
        End If
        MyBase.OnContextMenuStripChanged(e)
    End Sub
    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        EnsureColumns()
        EnsureContextMenuStrip()
    End Sub

#End Region

End Class
