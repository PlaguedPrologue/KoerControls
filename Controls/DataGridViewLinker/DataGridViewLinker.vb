Imports System.ComponentModel
Imports System.Drawing.Design

<Designer(GetType(SingleInstanceComponentDesigner))>
<ProvideProperty("LinkingGroup", GetType(DataGridView))>
<ToolboxItem(True)>
Public Class DataGridViewLinker
    Inherits Component
    Implements IExtenderProvider

#Region "Private Items"

    Private ReadOnly _dataGridViewLinkingGroup As New WeakKeyDictionary(Of DataGridView, DataGridViewLinkingGroup) 'Represents LinkingGroups from this Linker instance assigned to Controls
    Private ReadOnly _linkingGroupManagedDataGridViews As New WeakCollection(Of DataGridView) 'Represents all Controls with at least one LinkingGroup from this Linker instance assigned to it
    Private ReadOnly _linkingGroups As New DataGridViewLinkingGroupCollection 'Represents the LinkingGroups in this Linker
    Private _name As String

    Private Sub AddHandlers(dataGridView As DataGridView)
        AddHandler dataGridView.CurrentCellChanged, AddressOf DataGridView_CurrentCellChanged
    End Sub
    Private Function CanExtend(extendee As Object) As Boolean Implements IExtenderProvider.CanExtend
        Return TypeOf extendee Is DataGridView AndAlso Not IsDisposed
    End Function
    Private Sub DataGridView_CurrentCellChanged(sender As Object, e As EventArgs)
        Dim dgvSender As DataGridView = TryCast(sender, DataGridView)
        If dgvSender Is Nothing OrElse dgvSender.CurrentCell Is Nothing Then Return
        Dim dgvlgLinkingGroup As DataGridViewLinkingGroup = Nothing
        If Not _dataGridViewLinkingGroup.TryGetValue(dgvSender, dgvlgLinkingGroup) Then Return
        For Each kvpDataGridViewLinkingGroup As KeyValuePair(Of DataGridView, DataGridViewLinkingGroup) In _dataGridViewLinkingGroup
            If dgvlgLinkingGroup.Value = kvpDataGridViewLinkingGroup.Value.Value Then
                Dim dgvDataGridView As DataGridView = kvpDataGridViewLinkingGroup.Key
                If Not ReferenceEquals(dgvSender, dgvDataGridView) Then dgvDataGridView.CurrentCell = Nothing
            End If
        Next
    End Sub
    Private Sub RemoveHandlers(dataGridView As DataGridView)
        RemoveHandler dataGridView.CurrentCellChanged, AddressOf DataGridView_CurrentCellChanged
    End Sub

#End Region

#Region "Properties"

    '''<summary>Gets or sets each <see cref="DataGridViewLinkingGroup"/> assigned to the <see cref="DataGridViewLinker"/></summary>
    '''<returns>Each <see cref="DataGridViewLinkingGroup"/> assigned to the <see cref="DataGridViewLinker"/>.</returns>
    <Category("Data")>
    <Description("The linking groups available for the owning form.")>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)>
    <Editor(GetType(DataGridViewLinkingGroupCollectionEditor), GetType(UITypeEditor))>
    Public ReadOnly Property LinkingGroups As DataGridViewLinkingGroupCollection
        Get
            Return _linkingGroups
        End Get
    End Property
    Friend Sub ResetLinkingGroups()
        For Each dgvControl As DataGridView In _linkingGroupManagedDataGridViews 'Go through each Control the DataGridViewLinker has LinkingGroups assigned to
            _dataGridViewLinkingGroup.Remove(dgvControl) 'Remove the LinkingGroup from the Control
        Next
        _linkingGroupManagedDataGridViews.Clear() 'Clear all managed Controls
        _linkingGroups.Clear() 'Clear out each LinkingGroup in the LinkingGroupCollection
    End Sub
    Friend Function ShouldSerializeLinkingGroups() As Boolean
        Return _linkingGroups.Count > 0
    End Function
    '''<summary>Gets the LinkingGroup assigned to the specified <see cref="DataGridView"/>.</summary>
    '''<returns>The LinkingGroup assigned to the specified <see cref="DataGridView"/>.</returns>
    '''<param name="dataGridView">The <see cref="DataGridView"/> to get the LinkingGroup of.</param>
    <Category("Behavior")>
    <Description("The LinkingGroup assigned to the DataGridView for selecting cells.")>
    <TypeConverter(GetType(DataGridViewLinkingGroupConverter))>
    Public Function GetLinkingGroup(dataGridView As DataGridView) As DataGridViewLinkingGroup
        Dim dgvlgLinkingGroup As DataGridViewLinkingGroup = Nothing
        _dataGridViewLinkingGroup.TryGetValue(dataGridView, dgvlgLinkingGroup)
        Return dgvlgLinkingGroup
    End Function
    '''<summary>Gets the LinkingGroup assigned to the specified <see cref="DataGridView"/>.</summary>
    '''<param name="dataGridView">The <see cref="DataGridView"/> to get the LinkingGroup of.</param>
    '''<param name="value">The LinkingGroup assigned to the specified <see cref="DataGridView"/>.</param>
    <Category("Behavior")>
    <Description("The LinkingGroup assigned to the DataGridView for selecting cells.")>
    <TypeConverter(GetType(DataGridViewLinkingGroupConverter))>
    Public Sub SetLinkingGroup(dataGridView As DataGridView, value As DataGridViewLinkingGroup)
        Dim dgvlgLinkingGroup As DataGridViewLinkingGroup = Nothing
        Dim bControlHasLinkingGroup As Boolean = _dataGridViewLinkingGroup.TryGetValue(dataGridView, dgvlgLinkingGroup) 'Get the LinkingGroup from the current LinkingGroupCollection assigned to the Control
        If bControlHasLinkingGroup AndAlso ReferenceEquals(dgvlgLinkingGroup, value) Then Return 'Control has a LinkingGroup from the current LinkingGroupCollection assigned and LinkingGroup does not change for the Control, exit

        Dim bNewValueIsNull As Boolean = value Is Nothing 'Determine if new value is null
        If Not bControlHasLinkingGroup AndAlso bNewValueIsNull Then Return 'LinkingGroup was nothing and remains nothing, exit.

        If bControlHasLinkingGroup Then 'Determine if the current Linker contains a LinkingGroup assigned to the Control
            If bNewValueIsNull Then 'Removing LinkingGroup
                RemoveHandlers(dataGridView) 'Remove Control handlers
                _dataGridViewLinkingGroup.Remove(dataGridView) 'Remove Control from tracked LinkingGroups
                _linkingGroupManagedDataGridViews.Remove(dataGridView) 'Remove Control from tracked Controls

            Else 'Changing LinkingGroup
                _dataGridViewLinkingGroup.Remove(dataGridView) 'Remove control from tracked LinkingGroups
                _dataGridViewLinkingGroup.Add(dataGridView, value) 'Add new Control/LinkingGroup pair to tracked LinkingGroups
            End If

        Else 'Adding LinkingGroup
            AddHandlers(dataGridView) 'Add Control handlers
            _dataGridViewLinkingGroup.Add(dataGridView, value) 'Add new Control/LinkingGroup pair to tracked LinkingGroups
            _linkingGroupManagedDataGridViews.Add(dataGridView)
        End If
    End Sub
    Friend Sub ResetLinkingGroup(dataGridView As DataGridView)
        RemoveHandlers(dataGridView) 'Remove Control handlers
        _dataGridViewLinkingGroup.Remove(dataGridView) 'Remove the LinkingGroup from the Control
        _linkingGroupManagedDataGridViews.Remove(dataGridView) 'Remove the LinkingGroup from the managed Controls
    End Sub
    Friend Function ShouldSerializeLinkingGroup(dataGridView As DataGridView) As Boolean
        Return _linkingGroupManagedDataGridViews.Contains(dataGridView)
    End Function
    '''<summary>Gets the name of the component.</summary>
    '''<returns>The name of the component.</returns>
    <Browsable(False)>
    Public Property Name As String
        Get
            If Site IsNot Nothing AndAlso Not String.IsNullOrEmpty(Site.Name) Then _name = Site.Name
            Return _name
        End Get
        Set(value As String)
            If String.IsNullOrEmpty(value) Then
                _name = String.Empty
            Else
                _name = value
            End If
        End Set
    End Property

#End Region

#Region "Functions"

    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposing Then
            For Each dgvControl As DataGridView In _linkingGroupManagedDataGridViews 'Go through each DataGridView the DataGridViewLinker has DataGridViewLinkingGroups assigned to
                RemoveHandlers(dgvControl) 'Remove Control handlers
            Next
        End If
        MyBase.Dispose(disposing)
    End Sub

#End Region

End Class
