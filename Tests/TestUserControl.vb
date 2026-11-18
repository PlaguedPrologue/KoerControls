Public Class TestUserControl
    Private Sub Overlay1_Paint(sender As Object, e As PaintEventArgs) Handles Overlay1.Paint
        Using bBrush As New SolidBrush(Color.Red)
            e.Graphics.FillRectangle(bBrush, New Rectangle(e.ClipRectangle.Location.X, e.ClipRectangle.Location.Y, e.ClipRectangle.Size.Width / 2, e.ClipRectangle.Size.Height / 2))
        End Using
    End Sub
End Class
