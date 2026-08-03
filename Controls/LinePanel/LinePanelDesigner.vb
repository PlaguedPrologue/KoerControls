Imports System.Windows.Forms.Design

'''<summary>Extends the design mode behavior of a <see cref="LinePanel"/></summary>
Public Class LinePanelDesigner
    Inherits ControlDesigner

    Public Overrides ReadOnly Property SelectionRules As SelectionRules
        Get
            Dim lpLine As LinePanel = TryCast(Control, LinePanel)
            If Not lpLine.Docked Then Return MyBase.SelectionRules
            If lpLine.Orientation = Orientation.Horizontal Then Return MyBase.SelectionRules Or SelectionRules.BottomSizeable Or SelectionRules.Moveable Or SelectionRules.TopSizeable Or SelectionRules.Visible
            Return MyBase.SelectionRules Or SelectionRules.LeftSizeable Or SelectionRules.Moveable Or SelectionRules.RightSizeable Or SelectionRules.Visible
        End Get
    End Property

    Protected Overrides Sub OnPaintAdornments(pe As PaintEventArgs)
        Dim pLine As Panel = TryCast(Control, Panel)
        If pLine Is Nothing Then ControlPaint.DrawFocusRectangle(pe.Graphics, Control.ClientRectangle)
        If pLine.BorderStyle = BorderStyle.None Then ControlPaint.DrawFocusRectangle(pe.Graphics, Control.ClientRectangle)
    End Sub

End Class
