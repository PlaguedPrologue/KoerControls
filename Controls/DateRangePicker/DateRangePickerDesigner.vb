Imports System.Windows.Forms.Design

'''<summary>Extends the design mode behavior of a <see cref="DateRangePicker"/></summary>
Public Class DateRangePickerDesigner
    Inherits ControlDesigner

    Public Overrides ReadOnly Property SelectionRules As SelectionRules
        Get
            Return MyBase.SelectionRules And Not SelectionRules.BottomSizeable And Not SelectionRules.TopSizeable
        End Get
    End Property

    Protected Overrides Sub OnPaintAdornments(pe As PaintEventArgs)
        ControlPaint.DrawFocusRectangle(pe.Graphics, Control.ClientRectangle)
    End Sub

End Class
