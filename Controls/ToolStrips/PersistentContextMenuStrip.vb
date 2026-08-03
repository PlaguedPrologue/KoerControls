Imports System.ComponentModel

Public Class PersistentContextMenuStrip
    Inherits ContextMenuStrip

#Region "Private Items"

    Private _lastItemClicked As ToolStripMenuItem

#End Region

#Region "Properties"

    '''<summary>Determines if the control closes when the checked status of a <see cref="ToolStripMenuItem"/> changes.</summary>
    '''<returns><see cref="Boolean">true</see> if the control closes when the checked status of a <see cref="ToolStripMenuItem"/> changes; otherwise <see cref="Boolean">false</see>. The default value is <see cref="Boolean">false</see>.</returns>
    <Category("Behavior")>
    <DefaultValue(False)>
    <Description("Determines if the control closes when the check status of an item changes.")>
    Public Property CloseOnCheckChanged As Boolean = False

#End Region

#Region "Functions"

    Protected Overrides Sub OnClosing(e As ToolStripDropDownClosingEventArgs)
        If DesignMode OrElse CloseOnCheckChanged OrElse e.CloseReason <> ToolStripDropDownCloseReason.ItemClicked OrElse Not _lastItemClicked.CheckOnClick Then MyBase.OnClosing(e)
    End Sub
    Protected Overrides Sub OnItemClicked(e As ToolStripItemClickedEventArgs)
        If Not DesignMode Then _lastItemClicked = TryCast(e.ClickedItem, ToolStripMenuItem)
        MyBase.OnItemClicked(e)
    End Sub

#End Region

End Class
