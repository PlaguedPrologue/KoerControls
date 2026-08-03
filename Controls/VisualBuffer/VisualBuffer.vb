Public Class VisualBuffer
    Inherits PictureBox
    Implements IDisposable

#Region "Initializers"

    '''<summary>Creates a new instance of the <see cref="VisualBuffer"/> class.</summary>
    '''<param name="owner">The owning control of the <see cref="VisualBuffer"/>.</param>
    Public Sub New(owner As Control)
        Me.Owner = owner
    End Sub

#End Region

#Region "Properties"

    '''<summary>Gets the owning <see cref="Control"/> of the <see cref="VisualBuffer"/>.</summary>
    '''<returns>The owning <see cref="Control"/> of the <see cref="VisualBuffer"/>.</returns>
    Public ReadOnly Property Owner As Control

#End Region

#Region "Functions"

    '''<summary>Displays the <see cref="VisualBuffer"/> to the user.</summary>
    Public Overloads Sub Show()
        Size = Owner.Size
        Using bmImage As New Bitmap(Size.Width, Size.Height)
            Using gGraphic As Graphics = Graphics.FromImage(bmImage)
                gGraphic.CopyFromScreen(Owner.PointToScreen(Point.Empty), Point.Empty, Size)
            End Using
            Image = bmImage.Clone
        End Using
        If TypeOf Owner Is Panel OrElse TypeOf Owner Is TabPage Then
            Owner.Controls.Add(Me)
        Else
            Owner.Parent.Controls.Add(Me)
        End If
    End Sub
    '''<summary>Displays the <see cref="VisualBuffer"/> to the user.</summary>
    '''<param name="owner">The owning control of the <see cref="VisualBuffer"/>.</param>
    Public Overloads Shared Function Show(owner As Control) As VisualBuffer
        Dim vbBuffer As New VisualBuffer(owner)
        vbBuffer.Show()
        Return vbBuffer
    End Function

#End Region

End Class
