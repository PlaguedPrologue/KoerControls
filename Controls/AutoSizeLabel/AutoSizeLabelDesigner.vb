Imports System.Windows.Forms.Design

'''<summary>Extends the design mode behavior of a <see cref="AutoSizeLabel"/></summary>
Public Class AutoSizeLabelDesigner
    Inherits ControlDesigner

    Public Overrides ReadOnly Property SelectionRules As SelectionRules
        Get
            If CType(Control, AutoSizeLabel).AutoSize Then
                Select Case Control.Dock
                    Case DockStyle.Top, DockStyle.Bottom
                        Return (MyBase.SelectionRules Or SelectionRules.Moveable Or SelectionRules.Visible) And Not SelectionRules.BottomSizeable And Not SelectionRules.TopSizeable
                    Case Else
                        Return MyBase.SelectionRules Or SelectionRules.BottomSizeable Or SelectionRules.Moveable Or SelectionRules.TopSizeable Or SelectionRules.Visible
                End Select
            End If
            Return MyBase.SelectionRules
        End Get
    End Property

    Protected Overrides Sub OnPaintAdornments(pe As PaintEventArgs)
        ControlPaint.DrawFocusRectangle(pe.Graphics, Control.ClientRectangle)
    End Sub

End Class
