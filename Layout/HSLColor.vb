'''<summary>Represents an HSL (hue, saturation, lightness) color.</summary>
Public Structure HSLColor

#Region "Private Items"

    '''<summary>Converts a specified hue, the light value, and dark value of a color and returns the respective red, green, or blue value.</summary>
    '''<returns>A value for red, green, or blue.</returns>
    '''<param name="minimalLight">The amount of black in the color.</param>
    '''<param name="minimalDark">The amount of white in the color.</param>
    '''<param name="hue">The base hue value.</param>
    Private Function HueToRGB(minimalLight As Double, minimalDark As Double, hue As Double) As Double
        'Normalize hue
        Select Case hue
            Case < 0
                hue += 1
            Case > 1
                hue -= 1
        End Select

        'Hue is split between 4 quadrants. Can be split more, but 4 is deemed 'acceptable' and much quicker to calculate
        Select Case hue
            Case < 1.0 / 6.0 'Color falls between red and yellow (0 <= hue < 1/6 for 1/6 range)
                Return minimalLight + (minimalDark - minimalLight) * hue * 6
            Case < 1.0 / 2.0 'Color falls between yellow and cyan (1/6 <= hue < 3/6 for 2/6 range)
                Return minimalDark
            Case < 2.0 / 3.0 'Color falls between cyan and magenta (3/6 <= hue < 4/6 for 1/6 range
                Return minimalLight + (minimalDark - minimalLight) * (2.0 / 3.0 - hue) * 6
            Case Else 'Color falls between magenta and red (4/6 < hue < 6/6 for 2/6 range)
                Return minimalLight
        End Select
    End Function

#End Region

#Region "Initializers"

    Private Sub New(color As Color)
        'Set the alpha
        Alpha = color.A / 255.0

        'Get RGB values
        Dim dRed As Double = color.R / 255.0
        Dim dGreen As Double = color.G / 255.0
        Dim dBlue As Double = color.B / 255.0
        Dim dMaxChannelValue As Double = Math.Max(dRed, Math.Max(dGreen, dBlue)) 'Represents the highest color value
        Dim dMinChannelValue As Double = Math.Min(dRed, Math.Min(dGreen, dBlue)) 'Represents the lowest color value
        Dim dChannelDelta As Double = dMaxChannelValue - dMinChannelValue 'Gets the diference between the highest and lowest color values

        'Set lightness
        Lightness = (dMaxChannelValue + dMinChannelValue) / 2 'Lightness is the average of the highest and lowest value colors.

        If dChannelDelta = 0 Then 'Color is grayscale
            Saturation = 0
            Hue = 0
        Else 'Color is not grayscale
            'Set saturation
            If Lightness > 0.5 Then 'Color is considered bright
                Saturation = dChannelDelta / (2.0 - dMaxChannelValue - dMinChannelValue) 'Brighter colors need less saturation for correct color.
            Else 'Color is considered dark
                Saturation = dChannelDelta / (dMaxChannelValue + dMinChannelValue) 'Darker needs more saturation for correct color.
            End If

            'Set hue
            Select Case dMaxChannelValue
                Case dRed 'Color is more red
                    Hue = ((dGreen - dBlue) / dChannelDelta + If(dGreen > dBlue, 0, 6)) / 6
                    'dGreen - dBlue : How shifted to green or blue the (predominantly) red color is.
                    '/ dDeltaChanel : Devide by how strong of a red the color is.
                    '+ 6 or + 0 : Add in pure red to the green/blue shift. (Green's red-shift is towards 0, Blue's red-shift is towards 1)
                    '/ 6 : Converts to a value that ranges from 0 to 1.
                    'The calculation divides the color wheel into 6 sectors (primary and secondary colors) for the major color shifts.
                Case dGreen 'Color is more green
                    Hue = ((dBlue - dRed) / dChannelDelta + 2) / 6
                Case Else 'Color is more blue
                    Hue = ((dRed - dGreen) / dChannelDelta + 4) / 6
            End Select
        End If
    End Sub
    Private Sub New(alpha As Double, color As Color)
        'Set the alpha
        Me.Alpha = alpha

        'Get RGB values
        Dim dRed As Double = color.R / 255.0
        Dim dGreen As Double = color.G / 255.0
        Dim dBlue As Double = color.B / 255.0
        Dim dMaxChannelValue As Double = Math.Max(dRed, Math.Max(dGreen, dBlue)) 'Represents the highest color value
        Dim dMinChannelValue As Double = Math.Min(dRed, Math.Min(dGreen, dBlue)) 'Represents the lowest color value
        Dim dChannelDelta As Double = dMaxChannelValue - dMinChannelValue 'Gets the diference between the highest and lowest color values

        'Set lightness
        Lightness = (dMaxChannelValue + dMinChannelValue) / 2 'Lightness is the average of the highest and lowest value colors.

        If dChannelDelta = 0 Then 'Color is grayscale
            Saturation = 0
            Hue = 0
        Else 'Color is not grayscale
            'Set saturation
            If Lightness > 0.5 Then 'Color is considered bright
                Saturation = dChannelDelta / (2.0 - dMaxChannelValue - dMinChannelValue) 'Brighter colors need less saturation for correct color.
            Else 'Color is considered dark
                Saturation = dChannelDelta / (dMaxChannelValue + dMinChannelValue) 'Darker needs more saturation for correct color.
            End If

            'Set hue
            Select Case dMaxChannelValue
                Case dRed 'Color is more red
                    Hue = ((dGreen - dBlue) / dChannelDelta + If(dGreen > dBlue, 0, 6)) / 6
                    'dGreen - dBlue : How shifted to green or blue the (predominantly) red color is.
                    '/ dDeltaChanel : Devide by how strong of a red the color is.
                    '+ 6 or + 0 : Add in pure red to the green/blue shift. (Green's red-shift is towards 0, Blue's red-shift is towards 1)
                    '/ 6 : Converts to a value that ranges from 0 to 1.
                    'The calculation divides the color wheel into 6 sectors (primary and secondary colors) for the major color shifts.
                Case dGreen 'Color is more green
                    Hue = ((dBlue - dRed) / dChannelDelta + 2) / 6
                Case Else 'Color is more blue
                    Hue = ((dRed - dGreen) / dChannelDelta + 4) / 6
            End Select
        End If
    End Sub
    Private Sub New(color As HSLColor)
        Alpha = color.Alpha
        Hue = color.Hue
        Saturation = color.Saturation
        Lightness = color.Lightness
    End Sub
    Private Sub New(alpha As Double, hue As Double, saturation As Double, lightness As Double)
        Me.Alpha = alpha
        Me.Hue = hue
        Me.Lightness = lightness
        Me.Saturation = saturation
    End Sub

    '''<summary>Initializes a new instance of <see cref="HSLColor"/> class from the specified <see cref="Color"/>.</summary>
    '''<remarks>WARNING : Be careful doing many conversions back and forth with a single color. Errors occur (as this is a piecewise function), and colors can drift. Consider having a base color to return to.
    '''<para>Lighter colors and primary colors take as few as 10-15 round-trip conversions before any noticeable drift.
    '''</para>Darker and grayscale colors can take as many as 100 round-trip conversions before any noticeable drift.</remarks>
    '''<returns>The <see cref="HSLColor"/> structure that this method creates.</returns>
    '''<param name="baseColor">The <see cref="Color"/> from which to create the new <see cref="HSLColor"/></param>
    Public Shared Function FromColor(baseColor As Color) As HSLColor
        Return New HSLColor(baseColor)
    End Function
    '''<summary>Initializes a new instance of <see cref="HSLColor"/> class from the specified <see cref="Color"/>.</summary>
    '''<remarks>WARNING : Be careful doing many conversions back and forth with a single color. Errors occur (as this is a piecewise function), and colors can drift. Consider having a base color to return to.
    '''<para>Lighter colors and primary colors take as few as 10-15 round-trip conversions before any noticeable drift.
    '''</para>Darker and grayscale colors can take as many as 100 round-trip conversions before any noticeable drift.</remarks>
    '''<returns>The <see cref="HSLColor"/> structure that this method creates.</returns>
    '''<param name="alpha">The <see cref="Double"/>, from 0 to 1, that represents the alpha of the <see cref="HSLColor"/>.</param>
    '''<param name="baseColor">The <see cref="Color"/> from which to create the new <see cref="HSLColor"/></param>
    Public Shared Function FromColor(alpha As Double, baseColor As Color) As HSLColor
        Return New HSLColor(alpha, baseColor)
    End Function
    '''<summary>Initializes a new instance of <see cref="HSLColor"/> class with the specified alpha, hue, staturation, and lightness.</summary>
    '''<returns>The <see cref="HSLColor"/> structure that this method creates.</returns>
    '''<param name="baseColor">The <see cref="HSLColor"/> from which to create the new <see cref="HSLColor"/></param>
    Public Shared Function FromAHSL(baseColor As HSLColor) As HSLColor
        Return New HSLColor(baseColor)
    End Function
    '''<summary>Initializes a new instance of <see cref="HSLColor"/> class with the specified alpha, hue, staturation, and lightness.</summary>
    '''<returns>The <see cref="HSLColor"/> structure that this method creates.</returns>
    '''<param name="alpha">The <see cref="Double"/>, from 0 to 1, that represents the alpha of the <see cref="HSLColor"/>.</param>
    '''<param name="baseColor">The <see cref="HSLColor"/> from which to create the new <see cref="HSLColor"/></param>
    Public Shared Function FromAHSL(alpha As Double, baseColor As HSLColor) As HSLColor
        Return New HSLColor(alpha, baseColor.Hue, baseColor.Saturation, baseColor.Lightness)
    End Function
    '''<summary>Initializes a new instance of <see cref="HSLColor"/> class with the specified alpha, hue, staturation, and lightness.</summary>
    '''<returns>The <see cref="HSLColor"/> structure that this method creates.</returns>
    '''<param name="hue">The <see cref="Double"/>, from 0 to 1, that represents the hue of the <see cref="HSLColor"/>.</param>
    '''<param name="saturation">The <see cref="Double"/>, from 0 to 1, that represents the saturation of the <see cref="HSLColor"/>.</param>
    '''<param name="lightness">The <see cref="Double"/>, from 0 to 1, that represents the lightness of the <see cref="HSLColor"/>.</param>
    Public Shared Function FromAHSL(hue As Double, saturation As Double, lightness As Double) As HSLColor
        Return New HSLColor(1, hue, saturation, lightness)
    End Function
    '''<summary>Initializes a new instance of <see cref="HSLColor"/> class with the specified alpha, hue, staturation, and lightness.</summary>
    '''<returns>The <see cref="HSLColor"/> structure that this method creates.</returns>
    '''<param name="alpha">The <see cref="Double"/>, from 0 to 1, that represents the alpha of the <see cref="HSLColor"/>.</param>
    '''<param name="hue">The <see cref="Double"/>, from 0 to 1, that represents the hue of the <see cref="HSLColor"/>.</param>
    '''<param name="saturation">The <see cref="Double"/>, from 0 to 1, that represents the saturation of the <see cref="HSLColor"/>.</param>
    '''<param name="lightness">The <see cref="Double"/>, from 0 to 1, that represents the lightness of the <see cref="HSLColor"/>.</param>
    Public Shared Function FromAHSL(alpha As Double, hue As Double, saturation As Double, lightness As Double) As HSLColor
        Return New HSLColor(alpha, hue, saturation, lightness)
    End Function

#End Region

#Region "Properties"

    '''<summary>Gets or sets the double between 0 and 1 that represents the alpha of the color.</summary>
    '''<returns>The <see cref="Double"/> that represents the alpha of the color.</returns>
    Public ReadOnly Property Alpha As Double
    '''<summary>Gets or sets the double between 0 and 1 that represents the hue of the color.</summary>
    '''<returns>The <see cref="Double"/> that represents the hue of the color.</returns>
    Public ReadOnly Property Hue As Double
    '''<summary>Gets or sets the double between 0 and 1 that represents the saturation of the color.</summary>
    '''<returns>The <see cref="Double"/> that represents the saturation of the color.</returns>
    Public ReadOnly Property Saturation As Double
    '''<summary>Gets or sets the double between 0 and 1 that represents the lightness of the color.</summary>
    '''<returns>The <see cref="Double"/> that represents the saturation of the color.</returns>
    Public ReadOnly Property Lightness As Double

#End Region

#Region "Functions"

    '''<summary>Sets the alpha of the <see cref="HSLColor"/>.</summary>
    '''<returns>The new <see cref="HSLColor"/> after the adjustment.</returns>
    '''<param name="alpha">The new alpha of the <see cref="HSLColor"/>.</param>
    Public Function SetAlpha(alpha As Double) As HSLColor
        Return New HSLColor(alpha, Hue, Saturation, Lightness)
    End Function
    '''<summary>Sets the alpha of the <see cref="HSLColor"/>.</summary>
    '''<returns>The new <see cref="HSLColor"/> after the adjustment.</returns>
    '''<param name="func">The function (Function(h) f(h)) to apply to the alpha of the <see cref="HSLColor"/>.</param>
    Public Function SetAlpha(func As Func(Of Double, Double)) As HSLColor
        Return New HSLColor(func(Alpha), Hue, Saturation, Lightness)
    End Function
    '''<summary>Sets the hue of the <see cref="HSLColor"/>.</summary>
    '''<returns>The new <see cref="HSLColor"/> after the adjustment.</returns>
    '''<param name="hue">The new hue of the <see cref="HSLColor"/>.</param>
    Public Function SetHue(hue As Double) As HSLColor
        Return New HSLColor(Alpha, hue, Saturation, Lightness)
    End Function
    '''<summary>Sets the hue of the <see cref="HSLColor"/>.</summary>
    '''<returns>The new <see cref="HSLColor"/> after the adjustment.</returns>
    '''<param name="func">The function (Function(h) f(h)) to apply to the hue of the <see cref="HSLColor"/>.</param>
    Public Function SetHue(func As Func(Of Double, Double)) As HSLColor
        Return New HSLColor(Alpha, func(Hue), Saturation, Lightness)
    End Function
    '''<summary>Sets the lightness of the <see cref="HSLColor"/>.</summary>
    '''<returns>The new <see cref="HSLColor"/> aft=ter the adjustment.</returns>
    '''<param name="lightness">The new lightness of the <see cref="HSLColor"/>.</param>
    Public Function SetLightness(lightness As Double) As HSLColor
        Return New HSLColor(Alpha, Hue, Saturation, lightness)
    End Function
    '''<summary>Sets the lightness of the <see cref="HSLColor"/>.</summary>
    '''<returns>The new <see cref="HSLColor"/> after the adjustment.</returns>
    '''<param name="func">The function (Function(l) f(l)) to apply to the lightness of the <see cref="HSLColor"/>.</param>
    Public Function SetLightness(func As Func(Of Double, Double)) As HSLColor
        Return New HSLColor(Alpha, Hue, Saturation, func(Lightness))
        Return Me
    End Function
    '''<summary>Sets the saturation of the <see cref="HSLColor"/>.</summary>
    '''<returns>The new <see cref="HSLColor"/> after the adjustment.</returns>
    '''<param name="saturation">The new saturation of the <see cref="HSLColor"/>.</param>
    Public Function SetSaturation(saturation As Double) As HSLColor
        Return New HSLColor(Alpha, Hue, saturation, Lightness)
    End Function
    '''<summary>Sets the saturation of the <see cref="HSLColor"/>.</summary>
    '''<returns>The new <see cref="HSLColor"/> after the adjustment.</returns>
    '''<param name="func">The function (Function(s) f(s)) to apply to the saturation of the <see cref="HSLColor"/>.</param>
    Public Function SetSaturation(func As Func(Of Double, Double)) As HSLColor
        Return New HSLColor(Alpha, Hue, func(Saturation), Lightness)
    End Function
    '''<summary>Converts the <see cref="HSLColor"/> to an RGB <see cref="Color"/>.</summary>
    '''<remarks>WARNING : Be careful doing many conversions back and forth with a single color. Errors occur (as this is a piecewise function), and colors can drift. Consider having a base color to return to.
    '''<para>Lighter colors and primary colors take as few as 10-15 round-trip conversions before any noticeable drift.
    '''</para>Darker and gray colors can take as many as 100 round-trip conversions before any noticeable drift.</remarks>
    Public Function ToARGB() As Color
        If Saturation = 0 Then 'Color is grayscale
            Return Color.FromArgb(Math.Min(255, Alpha * 255), Math.Min(255, Lightness * 255), Math.Min(255, Lightness * 255), Math.Min(255, Lightness * 255))
        Else 'Color is not grayscale
            Dim dMinimalDark As Double = If(Lightness < 0.5, Lightness * (1 + Saturation), Lightness + Saturation - Lightness * Saturation) 'The amount of light in the color
            Dim dMinimalLight As Double = 2 * Lightness - dMinimalDark 'The amount of black in the color
            Dim dRed As Double = HueToRGB(dMinimalLight, dMinimalDark, Hue + 1.0 / 3.0) 'Get red component
            Dim dGreen As Double = HueToRGB(dMinimalLight, dMinimalDark, Hue) 'Get green component
            Dim dBlue As Double = HueToRGB(dMinimalLight, dMinimalDark, Hue - 1.0 / 3.0) 'Get blue component
            Return Color.FromArgb(Math.Min(255, dRed * 255), Math.Min(255, dGreen * 255), Math.Min(255, dBlue * 255))
        End If
    End Function

#End Region

End Structure
