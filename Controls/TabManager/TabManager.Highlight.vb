Partial Public Class TabManager

    Private Class Highlight
        Inherits Form

        Public Sub New()
            SuspendLayout()
            AutoScaleMode = AutoScaleMode.Font
            BackColor = Color.FromArgb(0, 63, 255)
            FormBorderStyle = FormBorderStyle.None
            Opacity = 0.25R
            ShowInTaskbar = False
            StartPosition = FormStartPosition.Manual
            TopMost = True
            ResumeLayout(False)
        End Sub

        Public Shadows Sub Show(x As Integer, y As Integer, width As Integer, height As Integer)
            Bounds = New Rectangle(x, y, width, height)
            MyBase.Show()
        End Sub

    End Class

End Class
