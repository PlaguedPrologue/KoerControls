Partial Public Class TabManager

    Private Class TabControlHighlighter
        Inherits Form

        Private _tabControl As TabControl

        Public Sub New()
            SuspendLayout()
            AutoScaleMode = AutoScaleMode.Font
            BackColor = Color.FromArgb(0, 63, 255)
            FormBorderStyle = FormBorderStyle.None
            MinimumSize = New Size(1, 1)
            Opacity = 0.25R
            ShowInTaskbar = False
            StartPosition = FormStartPosition.Manual
            TopMost = True
            Visible = False
            ResumeLayout()
        End Sub

        Public Property TabControl As TabControl
            Get
                Return _tabControl
            End Get
            Set(value As TabControl)
                If ReferenceEquals(value, _tabControl) Then Return
                _tabControl = value

                Dim bValueIsNull As Boolean = value Is Nothing
                Visible = Not bValueIsNull

                If bValueIsNull Then Return
                Size = New Size(_tabControl.Size.Width - 8, _tabControl.Size.Height - 27)

                RefreshLocation()
            End Set
        End Property

        Protected Overrides Sub OnVisibleChanged(e As EventArgs)
            MyBase.OnVisibleChanged(e)
            If Not Visible Then _tabControl = Nothing
        End Sub
        Public Sub RefreshLocation()
            If Not Visible Then Return
            Dim pGlobalTabControlLocation As Point = _tabControl.PointToScreen(Point.Empty)
            Location = New Point(pGlobalTabControlLocation.X + 4, pGlobalTabControlLocation.Y + 23)
        End Sub

    End Class

End Class
