Partial Public Class TabManager

    Private Class TabPageInsertHighlighter
        Inherits Form

        Private WithEvents MyTabControl As TabControl

        Private _dropIndex As Integer = -1
        Private _sizeSet As Boolean = False

        Private Sub MyTabControl_ControlAdded(sender As Object, e As ControlEventArgs) Handles MyTabControl.ControlAdded
            If _sizeSet OrElse MyTabControl.TabPages.Count = 0 Then Return
            Height = MyTabControl.GetTabRect(0).Height + 3
            _sizeSet = True
        End Sub

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
            Width = 100
            ResumeLayout(False)
        End Sub

        Public Property DropIndex As Integer
            Get
                Return _dropIndex
            End Get
            Set(value As Integer)
                If value = _dropIndex OrElse MyTabControl Is Nothing Then Return
                _dropIndex = value
                Visible = value >= 0
                RefreshLocation()
            End Set
        End Property
        Public Property TabControl As TabControl
            Get
                Return MyTabControl
            End Get
            Set(value As TabControl)
                If ReferenceEquals(value, MyTabControl) Then Return
                MyTabControl = value

                _dropIndex = -1
                Visible = False

                If MyTabControl Is Nothing Then Return
                If value.TabPages.Count = 0 Then
                    Height = value.ItemSize.Height + 3
                    _sizeSet = False
                Else
                    Height = value.GetTabRect(0).Height + 3
                    _sizeSet = True
                End If

                RefreshLocation()
            End Set
        End Property

        Protected Overrides Sub OnVisibleChanged(e As EventArgs)
            MyBase.OnVisibleChanged(e)
            If Not Visible Then MyTabControl = Nothing
        End Sub
        Public Sub RefreshLocation()
            If Not Visible Then Return

            Dim pGlobalTabControlLocation As Point = MyTabControl.PointToScreen(Point.Empty)
            Dim iGlobalTabControlLocationX As Integer = pGlobalTabControlLocation.X
            Dim iGlobalTabControlLocationY As Integer = pGlobalTabControlLocation.Y

            Dim iLocationX As Integer
            Dim iLocationY As Integer
            If MyTabControl.TabPages.Count = 0 Then
                iLocationX = iGlobalTabControlLocationX + 4
                iLocationY = iGlobalTabControlLocationY + 2
            Else
                Dim iTabPageCount As Integer = MyTabControl.TabPages.Count
                Dim rDropTabPageRect As Rectangle
                If _dropIndex >= iTabPageCount Then
                    rDropTabPageRect = MyTabControl.GetTabRect(iTabPageCount - 1)
                    iLocationX = rDropTabPageRect.X + rDropTabPageRect.Width + iGlobalTabControlLocationX
                Else
                    rDropTabPageRect = MyTabControl.GetTabRect(_dropIndex)
                    iLocationX = rDropTabPageRect.X + iGlobalTabControlLocationX
                End If
                iLocationY = rDropTabPageRect.Y + iGlobalTabControlLocationY
            End If
            Location = New Point(iLocationX + 2, iLocationY)
        End Sub

    End Class

End Class
