'''<summary>Represents a color with a highlighted value and unhighlighted value, and a <see cref="Boolean"/> to determine if the color is highlighted.</summary>
Public Structure HighlightableColor

#Region "Initializers"

    Private Sub New(baseColor As Color, Optional highlighted As Boolean = False)
        If highlighted Then
            HighlightedColor = baseColor
            Dim hslColor As HSLColor = HSLColor.FromColor(baseColor)
            If hslColor.Lightness < 0.5 Then
                UnhighlightedColor = HSLColor.FromAHSL(hslColor.Alpha, hslColor.Hue, hslColor.Saturation / 0.9, (hslColor.Lightness - 0.324) / 0.55).ToARGB
            Else
                UnhighlightedColor = HSLColor.FromAHSL(hslColor.Alpha, hslColor.Hue, hslColor.Saturation / 0.9, (hslColor.Lightness - 0.126) / 0.55).ToARGB
            End If
        Else
            UnhighlightedColor = baseColor
            Dim hslColor As HSLColor = HSLColor.FromColor(baseColor)
            If hslColor.Lightness < 0.5 Then
                'HighlightedColor = HSLColor.FromAHSL(hslColor.Alpha, hslColor.Hue, hslColor.Saturation * 0.9, hslColor.Lightness + (0.72 - hslColor.Lightness) * 0.45).ToARGB
                HighlightedColor = HSLColor.FromAHSL(hslColor.Alpha, hslColor.Hue, hslColor.Saturation * 0.9, 0.55 * hslColor.Lightness + 0.324).ToARGB 'Equal to the previous statement
            Else
                'HighlightedColor = HSLColor.FromAHSL(hslColor.Alpha, hslColor.Hue, hslColor.Saturation * 0.9, hslColor.Lightness + (0.28 - hslColor.Lightness) * 0.45).ToARGB
                HighlightedColor = HSLColor.FromAHSL(hslColor.Alpha, hslColor.Hue, hslColor.Saturation * 0.9, 0.55 * hslColor.Lightness + 0.126).ToARGB 'Equal to the previous statement
            End If
        End If
    End Sub

    '''<summary>Initializes a new instance of <see cref="HighlightableColor"/> class from the specified <see cref="Color"/>.</summary>
    '''<returns>The <see cref="HighlightableColor"/> structure that this method creates.</returns>
    '''<param name="baseColor">The <see cref="System.Drawing.Color"/> from which to create the new <see cref="HighlightableColor"/></param>
    Public Shared Function FromARGB(baseColor As Color) As HighlightableColor
        Return New HighlightableColor(baseColor)
    End Function
    '''<summary>Initializes a new instance of <see cref="HSLColor"/> class from the specified <see cref="Color"/>.</summary>
    '''<returns>The <see cref="HighlightableColor"/> structure that this method creates.</returns>
    '''<param name="alpha">The <see cref="Integer"/>, from 0 to 255, that represents the alpha of the <see cref="HighlightableColor"/>.</param>
    '''<param name="baseColor">The <see cref="System.Drawing.Color"/> from which to create the new <see cref="HighlightableColor"/></param>
    Public Shared Function FromARGB(alpha As Integer, baseColor As Color) As HighlightableColor
        Return New HighlightableColor(Color.FromArgb(alpha, baseColor))
    End Function
    '''<summary>Initializes a new instance of <see cref="HSLColor"/> class from the specified <see cref="Color"/>.</summary>
    '''<returns>The <see cref="HighlightableColor"/> structure that this method creates.</returns>
    '''<param name="argb">A value specifying the 32-bit ARGB value.</param>
    Public Shared Function FromARGB(argb As Integer) As HighlightableColor
        Return New HighlightableColor(Color.FromArgb(argb))
    End Function
    '''<summary>Initializes a new instance of <see cref="HSLColor"/> class from the specified <see cref="Color"/>.</summary>
    '''<returns>The <see cref="HighlightableColor"/> structure that this method creates.</returns>
    '''<param name="red">The <see cref="Integer"/>, from 0 to 255, that represents the red of the <see cref="HighlightableColor"/>.</param>
    '''<param name="green">The <see cref="Integer"/>, from 0 to 255, that represents the green of the <see cref="HighlightableColor"/>.</param>
    '''<param name="blue">The <see cref="Integer"/>, from 0 to 255, that represents the blue of the <see cref="HighlightableColor"/>.</param>
    Public Shared Function FromARGB(red As Integer, green As Integer, blue As Integer) As HighlightableColor
        Return New HighlightableColor(Color.FromArgb(red, green, blue))
    End Function
    '''<summary>Initializes a new instance of <see cref="HSLColor"/> class from the specified <see cref="Color"/>.</summary>
    '''<returns>The <see cref="HighlightableColor"/> structure that this method creates.</returns>
    '''<param name="alpha">The <see cref="Integer"/>, from 0 to 255, that represents the alpha of the <see cref="HighlightableColor"/>.</param>
    '''<param name="red">The <see cref="Integer"/>, from 0 to 255, that represents the red of the <see cref="HighlightableColor"/>.</param>
    '''<param name="green">The <see cref="Integer"/>, from 0 to 255, that represents the green of the <see cref="HighlightableColor"/>.</param>
    '''<param name="blue">The <see cref="Integer"/>, from 0 to 255, that represents the blue of the <see cref="HighlightableColor"/>.</param>
    Public Shared Function FromARGB(alpha As Integer, red As Integer, green As Integer, blue As Integer) As HighlightableColor
        Return New HighlightableColor(Color.FromArgb(alpha, red, blue, green))
    End Function
    '''<summary>Initializes a new instance of <see cref="HSLColor"/> class from the specified <see cref="Control"/>.</summary>
    '''<returns>The <see cref="HighlightableColor"/> structure that this method creates.</returns>
    '''<param name="control">The control to get the <see cref="HighlightableColor"/> from based on its current <see cref="Control.BackColor"/>.</param>
    Public Shared Function FromControl(control As Control) As HighlightableColor
        Return New HighlightableColor(control.GetDisplayedBackColor)
    End Function
    '''<summary>Initializes a new instance of <see cref="HSLColor"/> class from the specified <see cref="Control"/>.</summary>
    '''<returns>The <see cref="HighlightableColor"/> structure that this method creates.</returns>
    '''<param name="control">The control to get the <see cref="HighlightableColor"/> from based on its current <see cref="Control.BackColor"/>.</param>
    '''<param name="highlighted"><see langword="True"/> if the <see cref="Control"/> is highlighted; otherwise <see langword="False"/>.</param>
    Public Shared Function FromControl(control As Control, highlighted As Boolean) As HighlightableColor
        Return New HighlightableColor(control.GetDisplayedBackColor, highlighted)
    End Function

#End Region

#Region "Properties"

    '''<summary>Gets the color when the <see cref="HighlightableColor"/> is highlighted.</summary>
    '''<returns>The color when the <see cref="HighlightableColor"/> is highlighted.</returns>
    Public ReadOnly Property HighlightedColor As Color
    '''<summary>Gets a system-defined color.</summary>
    '''<returns>A <see cref="HighlightableColor"/> representing a system-defined color.</returns>
    Public Shared ReadOnly Property Transparent As New HighlightableColor(Color.Transparent)
    '''<summary>Gets the color when the <see cref="HighlightableColor"/> is not highlighted.</summary>
    '''<returns>The color when the <see cref="HighlightableColor"/> is not highlighted.</returns>
    Public ReadOnly Property UnhighlightedColor As Color

#End Region

#Region "Functions"

    '''<summary>Gets the <see cref="Color"/> based on the privided highlighted status.</summary>
    '''<returns>The <see cref="Color"/> based on the provided hightlighted status.</returns>
    '''<param name="highlighted"><see langword="True"/> if the <see cref="HighlightableColor"/> is highlighted; otherwise <see langword="False"/>.</param>
    Public Function GetColor(highlighted As Boolean) As Color
        If highlighted Then
            Return HighlightedColor
        Else
            Return UnhighlightedColor
        End If
    End Function
    Public Overrides Function ToString() As String
        Return $"{UnhighlightedColor}, {HighlightedColor}"
    End Function

#End Region

End Structure
