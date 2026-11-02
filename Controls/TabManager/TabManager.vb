Imports System.ComponentModel
Imports System.ComponentModel.Design

'''<summary>A component that can allow TabPages to be reoredred, detached from a TabControl, and attached to another TabControl with the same within the same specified TabGroup.</summary>
<Designer(GetType(SingleInstanceComponentDesigner))>
<ProvideProperty("AllowReordering", GetType(TabControl))>
<ProvideProperty("TabGroup", GetType(TabControl))>
<ToolboxItem(True)>
Public Class TabManager
    Inherits Component
    Implements IExtenderProvider

#Region "Private Items"

    Private ReadOnly _tabControlTabGroup As New WeakKeyDictionary(Of TabControl, TabGroup) 'Represents TabGroups from this TabManager instance assigned to Controls
    Private _name As String
    Private ReadOnly _tabControlsAllowedReordering As New WeakCollection(Of TabControl) 'Represents all Controls that can be have their tab pages reordered
    Private ReadOnly _tabDragging As New WeakReference(Of TabPage)(Nothing) 'Represents the current tab page being draged through any TabManager
    Private _tabDraggingLastTabRect As Rectangle 'Gets the TabRect of the current tab being dragged
    Private ReadOnly _tabDraggingSwitched As New WeakReference(Of TabPage)(Nothing) 'Represents the tab that will be switched with the currently dragging tab
    Private ReadOnly _tabGroupManagedTabControls As New WeakCollection(Of TabControl) 'Represents all Controls with at least one TabGroup from this TabManager instance assigned to it
    Private ReadOnly _tabGroups As New TabGroupCollection 'Repersents the TabGroups in this TabManager
    Private ReadOnly _tabPageOriginalTabControl As New WeakKeyWeakValueDictionary(Of TabPage, TabControl) 'Gets each TabControl that all TabPages are originally assigned to before detaching

    Private Sub AddHandlers(tabControl As TabControl)
        AddHandler tabControl.ControlAdded, AddressOf TabControl_ControlAdded
        AddHandler tabControl.MouseDown, AddressOf TabControl_MouseDown
        AddHandler tabControl.MouseMove, AddressOf TabControl_MouseMove
        AddHandler tabControl.MouseUp, AddressOf TabControl_MouseUp
    End Sub
    Private Function CanExtend(extendee As Object) As Boolean Implements IExtenderProvider.CanExtend
        Return TypeOf extendee Is TabControl
    End Function
    Private Sub NotifyPropertyChanged(control As Control, propertyName As String, oldValue As Object, newValue As Object)
        Dim ccsChangeService As IComponentChangeService = TryCast(GetService(GetType(IComponentChangeService)), IComponentChangeService)
        If ccsChangeService Is Nothing Then Return
        Dim pdDescriptor As PropertyDescriptor = TypeDescriptor.GetProperties(control)(propertyName)
        If pdDescriptor Is Nothing Then Return
        ccsChangeService.OnComponentChanged(control, pdDescriptor, oldValue, newValue)
        TypeDescriptor.Refresh(control)
    End Sub
    Private Sub RemoveHandlers(tabControl As TabControl)
        RemoveHandler tabControl.ControlAdded, AddressOf TabControl_ControlAdded
        RemoveHandler tabControl.MouseDown, AddressOf TabControl_MouseDown
        RemoveHandler tabControl.MouseMove, AddressOf TabControl_MouseMove
        RemoveHandler tabControl.MouseUp, AddressOf TabControl_MouseUp
    End Sub
    Private Sub TabControl_ControlAdded(sender As Object, e As ControlEventArgs)
        Dim tcTabControl As TabControl = TryCast(sender, TabControl)
        If tcTabControl Is Nothing Then Return
        Dim tpTabPage As TabPage = TryCast(e.Control, TabPage)
        If tpTabPage Is Nothing OrElse _tabPageOriginalTabControl.TryGetValue(tpTabPage, Nothing) Then Return
        _tabPageOriginalTabControl.Add(tpTabPage, tcTabControl)
    End Sub
    Private Sub TabControl_MouseDown(sender As Object, e As MouseEventArgs)
        Dim tcTabControl As TabControl = TryCast(sender, TabControl)
        If tcTabControl Is Nothing Then Return
        For iTabPageIndex As Integer = 0 To tcTabControl.TabCount - 1 'Go through each TabPage in the TabControl
            Dim rTabBounds As Rectangle = tcTabControl.GetTabRect(iTabPageIndex) 'Get the TabRect of the TabPage
            If rTabBounds.Contains(e.Location) Then 'Determine if the cursor is in the TabRect
                _tabDragging.SetTarget(tcTabControl.TabPages(iTabPageIndex)) 'Set the TabPage being dragged
                Return
            End If
        Next
    End Sub
    Private Sub TabControl_MouseMove(sender As Object, e As MouseEventArgs)
        Dim tcTabControl As TabControl = TryCast(sender, TabControl)
        If tcTabControl Is Nothing Then Return
        Dim tpTabDragging As TabPage = Nothing
        If Not _tabDragging.TryGetTarget(tpTabDragging) OrElse tpTabDragging Is Nothing Then Return

        'Determine if changing position in the current tab control
        Dim pMouseLocation As Point = tcTabControl.PointToClient(Cursor.Position)
        For Each tpTabPage As TabPage In tcTabControl.TabPages 'Go through each TabPage in the TabControl
            If Not tpTabPage.GetTabRect.Contains(pMouseLocation) Then Continue For 'Go to the next TabPage if its TabRect does not contain the cursor
            Dim tpTabDraggingSwitched As TabPage = Nothing
            _tabDraggingSwitched.TryGetTarget(tpTabDraggingSwitched)
            If ReferenceEquals(tpTabPage, tpTabDraggingSwitched) AndAlso Not _tabDraggingLastTabRect.Contains(pMouseLocation) Then Return 'Mouse is in the TabPage the dragging TabPage last switched with and the cursor is not in the location the dragging TabPage TabRect was last in, do nothing
            If ReferenceEquals(tpTabPage, tpTabDragging) Then 'Mouse is in the dragging tab page. Set previous switch to nothing and then do nothing
                _tabDraggingLastTabRect = Nothing
                _tabDraggingSwitched.SetTarget(Nothing)
                Return
            End If
            _tabDraggingSwitched.SetTarget(tpTabPage) 'Set previous TabPage switch
            _tabDraggingLastTabRect = tpTabDragging.GetTabRect 'Set the previous TabRect of the dragging TabPage
            Using vbBuffer As VisualBuffer = VisualBuffer.Show(tcTabControl) 'Create visual buffor to make it more visually appealing
                Dim iTabPageIndex As Integer = tcTabControl.TabPages.IndexOf(tpTabPage) 'Get the current index of the tab page switching
                tcTabControl.TabPages.Remove(tpTabDragging) 'Remove the dragging page
                tcTabControl.TabPages.Insert(iTabPageIndex, tpTabDragging) 'Add the dragging page back in at the switching index
                tcTabControl.SelectedTab = tpTabDragging 'Make sure the tab bieng dragged is still selected
            End Using
            Exit Sub
        Next

        'Tab page is being moved to another tab control
        If DesignMode OrElse LicenseManager.UsageMode = LicenseUsageMode.Designtime OrElse Not _tabControlTabGroup.TryGetValue(tcTabControl, Nothing) Then Return 'If in the designer, pages should never detach. This could easily cause lost data. If the TabControl does not have any TabGroups, then don't move
        Dim frmCurrentForm As Form = tcTabControl.FindForm 'Get the current Form of the TabControl
        If frmCurrentForm Is Nothing Then Return 'The current Form is nothing, do nothing
        Dim dtpNewFrom As New DetachedTabControl(tpTabDragging, Me) 'Create a new DetachedTabPage to detach to
        dtpNewFrom.Show() 'Show the new DetachedTabPage
        ReleaseCapture() 'Removes focus form Form
        SendMessage(dtpNewFrom.Handle, &HA1, &H2, 0) '&HA1 represents the mouse down event, &H2 represents the new forms title bar. The DetachedTabPage is initialized with the mouse down on the title bar
        _tabDragging.SetTarget(Nothing) 'There is no more TabPage being dragged, the new DetachedTabPage is now being dragged
    End Sub
    Private Sub TabControl_MouseUp(sender As Object, e As MouseEventArgs)
        _tabDragging.SetTarget(Nothing)
    End Sub

#End Region

#Region "Properties"

    '''<summary>Determines if each <see cref="TabPage"/> in a <see cref="TabControl"/> can be reordered.</summary>
    '''<returns><see langword="True"/>if each <see cref="TabPage"/> in a <see cref="TabControl"/> can be reordered; otherwise <see langword="False"/>.</returns>
    '''<param name="tabControl">The <see cref="TabControl"/> to determine the reorder value of.</param>
    <Category("Behavior")>
    <DefaultValue(False)>
    <Description("Determines if each TabPage in the TabControl can be reordered.")>
    Public Function GetAllowReordering(tabControl As TabControl) As Boolean
        Return _tabControlsAllowedReordering.Contains(tabControl)
    End Function
    '''<summary>Determines if each <see cref="TabPage"/> in a <see cref="TabControl"/> can be reordered.</summary>
    '''<param name="tabControl">The <see cref="TabControl"/> to determine the reorder value of.</param>
    '''<param name="value"><see langword="True"/>if each <see cref="TabPage"/> in a <see cref="TabControl"/> can be reordered; otherwise <see langword="False"/>.</param>
    <Category("Behavior")>
    <DefaultValue(False)>
    <Description("Determines if each TabPage in the TabControl can be reordered.")>
    Public Sub SetAllowReordering(tabControl As TabControl, value As Boolean)
        If _tabControlsAllowedReordering.Contains(tabControl) = value Then Return
        If value Then
            _tabControlsAllowedReordering.Add(tabControl)
            AddHandlers(tabControl)
        Else
            _tabControlsAllowedReordering.Remove(tabControl)

            Dim tgTabGroup As TabGroup = Nothing
            If _tabControlTabGroup.TryGetValue(tabControl, tgTabGroup) Then 'Control that has 'AllowReordering = False' requires it to have 'TabGroup = (none)'
                _tabControlTabGroup.Remove(tabControl)
                _tabGroupManagedTabControls.Remove(tabControl)
                RemoveHandlers(tabControl)
                NotifyPropertyChanged(tabControl, "TabGroup", tgTabGroup, Nothing)
            End If
        End If
    End Sub
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
    '''<summary>Gets the TabGroup assigned to the specified <see cref="TabControl"/>.</summary>
    '''<returns>The TabGroup assigned to the specified <see cref="TabControl"/>.</returns>
    '''<param name="tabControl">The <see cref="TabControl"/> to get the TabGroup of.</param>
    <Category("Behavior")>
    <Description("The TabGroup assigned to the TabControl for rearranging tabs while the program is running. Setting a TabGroup allows each TabPage to be detached from the TabControl.")>
    <TypeConverter(GetType(TabGroupConverter))>
    Public Function GetTabGroup(tabControl As TabControl) As TabGroup
        Dim tgTabGroup As TabGroup = Nothing
        _tabControlTabGroup.TryGetValue(tabControl, tgTabGroup)
        Return tgTabGroup
    End Function
    '''<summary>Gets the tab group assigned to a tab page.</summary>
    '''<returns>The tab group assigned to the <see cref="TabPage"/>.</returns>
    '''<param name="tabPage">The <see cref="TabPage"/> to get the tab group from.</param>
    <Category("Behavior")>
    <Description("The TabGroupCollection assigned to the TabControl for rearranging tabs while the program is running. Setting a TabGroup allows each TabPage to be detached from the TabControl.")>
    Public Function GetTabGroup(tabPage As TabPage) As TabGroup
        Dim tcTabControl As TabControl = GetOriginalTabControl(tabPage)
        If tcTabControl Is Nothing Then Return Nothing
        Dim tgTabGroup As TabGroup = Nothing
        _tabControlTabGroup.TryGetValue(tcTabControl, tgTabGroup)
        Return tgTabGroup
    End Function
    '''<summary>Assigns the specified TabGroup to the specified <see cref="TabControl"/>.</summary>
    '''<param name="tabControl">The <see cref="TabControl"/> to set the TabGroup of.</param>
    '''<param name="value">The TabGroup to assign to the specified <see cref="TabControl"/>.</param>
    <Category("Behavior")>
    <Description("The TabGroup assigned to the TabControl for rearranging tabs while the program is running.")>
    <TypeConverter(GetType(TabGroupConverter))>
    Public Sub SetTabGroup(tabControl As TabControl, value As TabGroup)
        Dim tgLinkingGroup As TabGroup = Nothing
        Dim bControlHasTabGroup As Boolean = _tabControlTabGroup.TryGetValue(tabControl, tgLinkingGroup) 'Get the TabGroup from the current TabGroupCollection assigned to the Control
        If bControlHasTabGroup AndAlso ReferenceEquals(tgLinkingGroup, value) Then Return 'Control has a TabGroup from the current TabGroupCollection assigned and TabGroup does not change for the Control, exit

        Dim bNewValueIsNull As Boolean = value Is Nothing 'Determine if new value is null
        If Not bControlHasTabGroup AndAlso bNewValueIsNull Then Return 'TabGroup was nothing and remains nothing, exit

        If bControlHasTabGroup Then 'Determine if the current TabManager contains a TabGroup assigned to the Control
            If bNewValueIsNull Then 'Removing TabGroup
                RemoveHandlers(tabControl) 'Remove Control handlers, as TabControl cannot reorder TabPages
                _tabControlTabGroup.Remove(tabControl) 'Remove Control from tracked TabGroups
                _tabGroupManagedTabControls.Remove(tabControl) 'Remove Control from tracked Controls

            Else 'Changing TabGroups=
                _tabControlTabGroup.Remove(tabControl) 'Remove control from tracked LinkingGroups
                _tabControlTabGroup.Add(tabControl, value) 'Add new Control/TabGroup pair to tracked TabGroups
            End If

        Else 'Adding LinkingGroup
            _tabControlTabGroup.Add(tabControl, value) 'Add new Control/TabGroup pair to tracked TabGroups
            _tabGroupManagedTabControls.Add(tabControl) 'Force TabControl to allow reordering TabPages

            If Not _tabControlsAllowedReordering.Contains(tabControl) Then 'Control that has 'TabGroup <> (none)' requires it to have 'AllowReordering = True'
                _tabControlsAllowedReordering.Add(tabControl) 'TabControls with a TabGroup assigned must be allowed to reorder
                AddHandlers(tabControl) 'Add Control handlers if TabControl
                NotifyPropertyChanged(tabControl, "AllowReordering", False, True)
            End If
        End If
    End Sub
    Friend Sub ResetTabGroup(tabControl As TabControl)
        RemoveHandlers(tabControl) 'Remove Control handlers
        _tabControlTabGroup.Remove(tabControl) 'Remove the LinkingGroup from the Control
        _tabGroupManagedTabControls.Remove(tabControl) 'Remove the LinkingGroup from the managed Controls
    End Sub
    Friend Function ShouldSerializeTabGroup(tabControl As TabControl) As Boolean
        Return _tabGroupManagedTabControls.Contains(tabControl)
    End Function
    '''<summary>Gets or sets each <see cref="TabGroup"/> assigned to the <see cref="TabManager"/></summary>
    '''<returns>Each <see cref="TabGroup"/> assigned to the <see cref="TabManager"/>.</returns>
    <Category("Data")>
    <Description("The tab groups available for the owning form.")>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)>
    Public ReadOnly Property TabGroups As TabGroupCollection
        Get
            Return _tabGroups
        End Get
    End Property
    Friend Sub ResetTabGroups()
        For Each tcTabControl As TabControl In _tabGroupManagedTabControls 'Go through each Control the TabManager has TabGroups assigned to
            _tabControlTabGroup.Remove(tcTabControl) 'Remove the TabGroup from the Control
        Next
        _tabGroupManagedTabControls.Clear() 'Clear all managed Controls
        _tabGroups.Clear() 'Clear out each TabGroup in the TabGroupCollection
    End Sub
    Friend Function ShouldSerializeTabGroups() As Boolean
        Return _tabGroups.Any
    End Function

#End Region

#Region "Functions"

    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposing Then
            For Each tcTabControl As TabControl In _tabGroupManagedTabControls 'Go through each TabControl the TabManager has TabGroups assigned to
                RemoveHandlers(tcTabControl) 'Remove Control handlers
            Next
            For Each tgTabGroup As TabGroup In _tabGroups
                tgTabGroup.Dispose()
            Next
        End If
        MyBase.Dispose(disposing)
    End Sub
    '''<summary>Gets the original tab control assigned to the tab page.</summary>
    '''<returns>The original <see cref="TabControl"/> assigned to the <see cref="TabPage"/>.</returns>
    '''<param name="tabPage">The <see cref="TabPage"/> to get the original <see cref="TabControl"/> from.</param>
    Public Function GetOriginalTabControl(tabPage As TabPage) As TabControl
        If tabPage Is Nothing Then Return Nothing
        Dim tcTabControl As TabControl = Nothing
        If _tabPageOriginalTabControl.TryGetValue(tabPage, tcTabControl) Then Return tcTabControl 'TabPage already has a set original TabControl, return the found original TabControl
        tcTabControl = TryCast(tabPage.Parent, TabControl) 'Get the current parent TabControl of the TabPage
        If tcTabControl Is Nothing Then Return Nothing 'TabPage does not have a parent TabControl, return Nothing
        _tabPageOriginalTabControl.Add(tabPage, tcTabControl) 'Set original TabControl of TabPage. No original already set, and the TabPage has a parent TabControl
        Return tcTabControl
    End Function

#End Region

End Class
