Imports System.ComponentModel

'''<summary>Represents a pin stripe that can be painted onto the background of a control.</summary>
<DesignTimeVisible(False)>
<ToolboxItem(False)>
Public Class PinStripe

#Region "Private Items"

    Private _color As Color = Color.Transparent
    Private _highlightColor As HighlightableColor
    Private Const _defaultSpacing As Integer = 50
    Private Const _defaultThickness As Integer = 50
    Private _direction As Direction = Direction.None
    Private _spacing As Integer = _defaultSpacing
    Private _thickness As Integer = _defaultThickness
    Private _type As PinStripeType = PinStripeType.None

#End Region

#Region "Initializers"

    '''<summary>Create a new empty instance of the <see cref="PinStripe"/> clas.</summary>
    Public Sub New()
    End Sub
    '''<summary>Create a new instance of the <see cref="PinStripe"/> clas.</summary>
    '''<param name="container">The <see cref="IContainer"/> that will contain the <see cref="PinStripe"/>.</param>
    Public Sub New(container As IComponent)
        Owner = TryCast(container, PinStripeList)
    End Sub

#End Region

#Region "Events"

    ''' <summary>Occurs when the color has changed.</summary>
    Public Event ColorChanged As EventHandler
    ''' <summary>Occurs when the direction has changed.</summary>
    Public Event DirectionChanged As EventHandler
    ''' <summary>Occurs when the spacing has changed.</summary>
    Public Event SpacingChanged As EventHandler
    ''' <summary>Occurs when the thickness has changed.</summary>
    Public Event ThicknessChanged As EventHandler
    ''' <summary>Occurs when the type has changed.</summary>
    Public Event TypeChanged As EventHandler

#End Region

#Region "Properties"

    '''<summary>Gets or sets the color of the <see cref="PinStripe"/>.</summary>
    '''<returns>The color of the <see cref="PinStripe"/>.</returns>
    <Category("Appearance")>
    <DefaultValue(GetType(Color), "Transparent")>
    <Description("The color of each stripe. Default value is Transparent.")>
    Public Overridable Property Color As Color
        Get
            Return _color
        End Get
        Set(value As Color)
            If value = _color Then Return
            _color = value
            _highlightColor = HighlightableColor.FromARGB(value)
            OnColorChanged(EventArgs.Empty)
        End Set
    End Property
    '''<summary>Gets or sets the direction of 'travel' for the <see cref="PinStripe"/>.</summary>
    '''<returns>The direction of 'travel' for the <see cref="PinStripe"/>.</returns>
    <Category("Appearance")>
    <DefaultValue(Direction.None)>
    <Description("The direction of 'travel' for the pin stripe. Default value is none.")>
    <RefreshProperties(RefreshProperties.Repaint)>
    Public Property Direction As Direction
        Get
            Return _direction
        End Get
        Set(value As Direction)
            If value = _direction Then Return
            Dim bDirectionChanged As Boolean = (_direction = Direction.Up OrElse _direction = Direction.Down) <> Direction.Up OrElse value = Direction.Down OrElse (_direction = Direction.Left OrElse _direction = Direction.Right) <> Direction.Left OrElse value = Direction.Right
            _direction = value
            If bDirectionChanged Then OnDirectionChanged(EventArgs.Empty)
        End Set
    End Property
    '''<summary>Represents an empty pin stripe with default values.</summary>
    '''<returns>An empty pin stripe with default values.</returns>
    Public Shared ReadOnly Property Empty As PinStripe
        Get
            Return New PinStripe
        End Get
    End Property
    '''<summary>Gets the owning <see cref="PinStripeList"/> of the <see cref="PinStripe"/>.</summary>
    '''<returns>The owning <see cref="PinStripeList"/> of the <see cref="PinStripe"/>.</returns>
    <Browsable(False)>
    Public ReadOnly Property Owner As PinStripeList
    '''<summary>Gets or sets the the spacing between each stripes in the <see cref="PinStripe"/>.</summary>
    '''<returns>The spacing between each stripe in the <see cref="PinStripe"/>.</returns>
    <Category("Appearance")>
    <DefaultValue(_defaultSpacing)>
    <Description("The diagonal spacing between each stripe. Default value is 0.")>
    Public Property Spacing As Integer
        Get
            Return _spacing
        End Get
        Set(value As Integer)
            If value = _spacing Then Return
            _spacing = value
            OnSpacingChanged(EventArgs.Empty)
        End Set
    End Property
    '''<summary>Gets or sets the the thickness of each stripes in the <see cref="PinStripe"/>.</summary>
    '''<returns>The thickness of each stripe in the <see cref="PinStripe"/>.</returns>
    <Category("Appearance")>
    <DefaultValue(_defaultThickness)>
    <Description("The diagonal thickness of each stripe. Default value is 0.")>
    Public Property Thickness As Integer
        Get
            Return _thickness
        End Get
        Set(value As Integer)
            If value = _thickness Then Return
            _thickness = value
            OnThicknessChanged(EventArgs.Empty)
        End Set
    End Property
    '''<summary>Gets or sets the <see cref="PinStripe"/> of the <see cref="PinStripe"/>.</summary>
    '''<returns>The <see cref="PinStripeType"/> of the <see cref="PinStripe"/>.</returns>
    <Category("Appearance")>
    <DefaultValue(PinStripeType.None)>
    <Description("The displayed type of pin stripe. Default value is diagonal.")>
    <RefreshProperties(RefreshProperties.Repaint)>
    Public Property Type As PinStripeType
        Get
            Return _type
        End Get
        Set(value As PinStripeType)
            If value = _type Then Return
            _type = value
            OnTypeChanged(EventArgs.Empty)
        End Set
    End Property

#End Region

#Region "Functions"

    '''<summary>Determines if the current <see cref="PinStripe"/> is the default <see cref="PinStripe"/>.</summary>
    '''<returns><see langword="True"/> if the current <see cref="PinStripe"/> is the default <see cref="PinStripe"/>; otherwise, <see langword="False"/>.</returns>
    Public Overridable Function IsDefault() As Boolean
        Return _color = Color.Transparent AndAlso _direction = Direction.None AndAlso _spacing = _defaultSpacing AndAlso _thickness = _defaultThickness AndAlso _type = PinStripeType.None
    End Function
    '''<summary>Raises the <see cref="ColorChanged"/> event.</summary>
    '''<param name="e">The <see cref="EventArgs"/> containing the event data.</param>
    Protected Overridable Sub OnColorChanged(e As EventArgs)
        RaiseEvent ColorChanged(Me, e)
    End Sub
    '''<summary>Raises the <see cref="DirectionChanged"/> event.</summary>
    '''<param name="e">The <see cref="EventArgs"/> containing the event data.</param>
    Protected Overridable Sub OnDirectionChanged(e As EventArgs)
        RaiseEvent DirectionChanged(Me, e)
    End Sub
    '''<summary>Raises the <see cref="SpacingChanged"/> event.</summary>
    '''<param name="e">The <see cref="EventArgs"/> containing the event data.</param>
    Protected Overridable Sub OnSpacingChanged(e As EventArgs)
        RaiseEvent SpacingChanged(Me, e)
    End Sub
    '''<summary>Raises the <see cref="ThicknessChanged"/> event.</summary>
    '''<param name="e">The <see cref="EventArgs"/> containing the event data.</param>
    Protected Overridable Sub OnThicknessChanged(e As EventArgs)
        RaiseEvent ThicknessChanged(Me, e)
    End Sub
    '''<summary>Raises the <see cref="TypeChanged"/> event.</summary>
    '''<param name="e">The <see cref="EventArgs"/> containing the event data.</param>
    Protected Overridable Sub OnTypeChanged(e As EventArgs)
        RaiseEvent TypeChanged(Me, e)
    End Sub
    Public Overrides Function ToString() As String
        Dim sValue As String
        Select Case _type
            Case PinStripeType.Chevron
                sValue = "Chevron"
            Case PinStripeType.Diagonal
                sValue = "Diagonal"
            Case Else
                sValue = "No Type"
        End Select
        Select Case _direction
            Case Direction.Left
                sValue = $"{sValue} Left"
            Case Direction.Right
                sValue = $"{sValue} Right"
            Case Direction.Up
                sValue = $"{sValue} Up"
            Case Direction.Down
                sValue = $"{sValue} Down"
            Case Else
                sValue = $"{sValue} No Direction"
        End Select
        If _color.IsNamedColor Then
            sValue = $"{sValue}, {_color.Name}"
        ElseIf _color.A = 255 Then
            sValue = $"{sValue}, ({_color.R}, {_color.G}, {_color.B})"
        Else
            sValue = $"{sValue}, ({_color.A}, {_color.R}, {_color.G}, {_color.B})"
        End If
        Return sValue
    End Function

#End Region

End Class
