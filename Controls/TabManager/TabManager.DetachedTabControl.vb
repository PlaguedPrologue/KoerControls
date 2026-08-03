Imports System.ComponentModel

Partial Public Class TabManager

    Private Class DetachedTabControl
        Inherits Form

#Region "Private Items"

        Private WithEvents MyTabControl As New TabControl

        Private _dropTabControl As TabControl
        Private _dropTabIndex As Integer = -1
        Private ReadOnly _tabControlHighlighter As New TabControlHighlighter
        Private ReadOnly _tabGroup As TabGroup
        Private ReadOnly _tabManager As TabManager
        Private ReadOnly _tabPageInsertHighlighter As New TabPageInsertHighlighter

        Private Sub MyTabControl_ControlRemoved(sender As Object, e As ControlEventArgs) Handles MyTabControl.ControlRemoved
            'ControlRemoved is called before the TabPage is really removed. Check if there is one TabPage (the one being removed) and close if that is the case
            If MyTabControl.TabCount = 1 Then Dispose()
        End Sub

#End Region

#Region "Initializers"

        '''<summary>Creates a new instance of the <see cref="DetachedTabControl"/> class.</summary>
        '''<param name="tabPage">The <see cref="TabPage"/> being used to create the <see cref="DetachedTabControl"/>.</param>
        Public Sub New(tabPage As TabPage, tabManager As TabManager)
            _tabGroup = TabManager.GetTabGroup(tabPage)
            _tabManager = tabManager

            Dim tcTabControl As TabControl = TabManager.GetOriginalTabControl(tabPage)
            Dim fSourceForm As Form = tcTabControl.FindForm

            TabManager._tabControlTabGroup.TryGetValue(tcTabControl, _tabGroup)
            tabManager.SetTabGroup(tcTabControl, _tabGroup)

            SuspendLayout()
            Controls.Add(MyTabControl)
            Icon = fSourceForm?.Icon
            Location = Cursor.Position - New Point(15, 20)
            MyTabControl.Dock = DockStyle.Fill
            MyTabControl.TabPages.Add(tabPage)
            ShowInTaskbar = False
            Size = tcTabControl.Size + New Size(16, 39)
            StartPosition = FormStartPosition.Manual
            Text = $"{fSourceForm?.Text} - ({_tabGroup})"
            ResumeLayout(False)
        End Sub

#End Region

#Region "Functions"

        Protected Overrides Sub Dispose(disposing As Boolean)
            If disposing Then
                _tabControlHighlighter?.Dispose()
                _tabPageInsertHighlighter?.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub
        Protected Overrides Sub OnClosing(e As CancelEventArgs)
            MyBase.OnClosing(e)
            Dim tpSelectedTabPage As TabPage = MyTabControl.SelectedTab 'Get current selected tab
            If _dropTabControl Is Nothing Then 'Not dropping, add each TabPage back to their original TabControl
                For Each tpTabPage As TabPage In MyTabControl.TabPages 'Go through each TabPage in the DetachedTabPage
                    Dim tcOriginalTabControl As TabControl = _tabManager.GetOriginalTabControl(tpTabPage) 'Get original tab control
                    If tcOriginalTabControl Is Nothing Then Throw New NullReferenceException($"{tpTabPage.Name} has no original TabControl to return to. Cancelling form closing.") 'No original tab control found, throw
                    tcOriginalTabControl.TabPages.Add(tpTabPage) 'Add tab page back to original tab control
                Next
            Else 'Dropping, add to dropping TabControl
                For Each tpTabPage As TabPage In MyTabControl.TabPages 'Go through each TabPage in the DetachedTabPage
                    _dropTabControl.TabPages.Insert(_dropTabIndex, tpTabPage) 'Add TabPage to the specified index in the dropping TabControl
                Next
                Dim dtcDropTabForm As DetachedTabControl = TryCast(_dropTabControl.FindForm, DetachedTabControl)
            End If

            Dim tcSelectedTabControl As TabControl = TryCast(tpSelectedTabPage?.Parent, TabControl) 'Get the new TabControl of the selected TabPage
            If tcSelectedTabControl IsNot Nothing Then 'The TabPage was added back to a TabControl
                tcSelectedTabControl.SelectedTab = tpSelectedTabPage 'Reselect TabPage
                tcSelectedTabControl.Focus() 'Set focus to new TabControl
            End If
        End Sub
        Protected Overrides Sub OnLocationChanged(e As EventArgs)
            MyBase.OnLocationChanged(e)

            Dim pMouseLocation As Point = Cursor.Position
            'Get first TabControl that is visible, not the DetachedTabPage's TabControl, is the same TabGroup as the DetachedTabPage's TabGroup, and bounds contains the cursor
            Dim tgTabGroup As TabGroup = Nothing
            Dim tcDropTabControl As TabControl = _tabManager._tabGroupManagedTabControls.FirstOrDefault(Function(tc) tc.Visible AndAlso Not ReferenceEquals(tc, MyTabControl) AndAlso _tabManager._tabControlTabGroup.TryGetValue(tc, tgTabGroup) AndAlso ReferenceEquals(_tabGroup, tgTabGroup) AndAlso tc.ClientRectangle.Contains(tc.PointToClient(pMouseLocation)))

            If tcDropTabControl IsNot Nothing Then 'Determine if dropping into a TabControl

                Dim iDropTabIndex As Integer = tcDropTabControl.TabCount 'Set the initial index to drop to at the end of the list of the drop TabControl's TabPages
                For Each tpTabPage As TabPage In tcDropTabControl.TabPages 'Go through each TabPage in the drop TabControl
                    If tpTabPage.GetTabRect.Contains(tcDropTabControl.PointToClient(pMouseLocation)) Then 'Determine if the cursor is in the TabRect of the TabPage
                        iDropTabIndex = tcDropTabControl.TabPages.IndexOf(tpTabPage) 'Set the drop index to be at the TabPage whose TabRect contains the cursor
                        Exit For
                    End If
                Next

                Dim bChangingDropTabControl As Boolean = Not ReferenceEquals(tcDropTabControl, _dropTabControl) 'Determine if the drop TabControl is changing
                Dim bChangingDropTabIndex As Boolean = iDropTabIndex <> _dropTabIndex 'Determine if the drop index is changing

                If bChangingDropTabControl OrElse bChangingDropTabIndex Then 'Determine if changing drop TabControl or changing drop index
                    _tabControlHighlighter.TabControl = tcDropTabControl
                    _tabPageInsertHighlighter.TabControl = tcDropTabControl
                    _tabPageInsertHighlighter.DropIndex = iDropTabIndex

                    _dropTabControl = tcDropTabControl
                    _dropTabIndex = iDropTabIndex
                    Opacity = 0.85
                End If

            Else 'Not dropping into a tab control
                _tabControlHighlighter.Hide()
                _tabPageInsertHighlighter.Hide()
                _dropTabControl = Nothing
                _dropTabIndex = -1
                Opacity = 1
            End If
        End Sub
        Protected Overrides Sub WndProc(ByRef m As Message)
            MyBase.WndProc(m)
            If m.WParam <> &H2 Then Return ' Object being interacted needs to be the form header
            StartPosition = FormStartPosition.Manual
            If m.Msg <> &HA1 Then Return 'Action must be the mouse button released
            If _dropTabControl IsNot Nothing Then 'Dropping into another tab control
                _tabControlHighlighter?.Dispose()
                _tabPageInsertHighlighter?.Dispose()
                Close()
            ElseIf Not ShowInTaskbar Then 'Detached tab was just created. Need to drop into active, tabbable forms
                Location -= New Point(5, 30)
                FormBorderStyle = FormBorderStyle.Sizable
                ShowInTaskbar = True
            End If
        End Sub

#End Region

    End Class

End Class
