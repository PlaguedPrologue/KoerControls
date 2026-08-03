Partial Public Class Highlighter

    Friend Class HighlightableColorHandler

        Public Property Color As HighlightableColor
        Public Property SettingBackgroundColor As Boolean

        Public Sub New(color As HighlightableColor)
            Me.Color = color
        End Sub

    End Class

End Class
