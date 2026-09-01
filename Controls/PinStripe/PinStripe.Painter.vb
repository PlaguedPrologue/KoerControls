Imports System.Collections.Specialized
Imports System.Reflection

Partial Public Class PinStripe

    Friend Class Painter

#Region "Private Items"

        Private WithEvents MyBrokenPinStripe As BrokenPinStripe
        Private WithEvents MyOwner As Control
        Private WithEvents MyPinStripe As PinStripe

        Private _borderStyle As BorderStyle
        Private _borderThickness As Integer
        Private ReadOnly _ownerBorderStyleProperty As PropertyInfo
        Private _thickness As Integer
        Private _x1 As Integer
        Private _x2 As Integer
        Private _x3 As Integer
        Private _x4 As Integer
        Private _y1 As Integer
        Private _y2 As Integer
        Private _y3 As Integer
        Private _y4 As Integer

        Private Sub MyBrokenPinStripe_PinStripeLocationsChanged(sender As Object, e As NotifyCollectionChangedEventArgs) Handles MyBrokenPinStripe.PinStripeLocationsChanged
            RefreshValues()
            MyOwner.Invalidate()
        End Sub
        Private Sub MyOwner_Layout(sender As Object, e As LayoutEventArgs) Handles MyOwner.Layout
            If _ownerBorderStyleProperty Is Nothing Then Return
            Dim bsBorderStyle As BorderStyle = _ownerBorderStyleProperty.GetValue(MyOwner)
            If bsBorderStyle = _borderStyle Then Return
            _borderStyle = bsBorderStyle
            Select Case bsBorderStyle
                Case BorderStyle.Fixed3D
                    _borderThickness = 2
                Case BorderStyle.FixedSingle
                    _borderThickness = 1
                Case BorderStyle.None
                    _borderThickness = 0
            End Select
            RefreshValues()
            MyOwner.Invalidate()
        End Sub
        Private Sub MyOwner_StyleChanged(sender As Object, e As EventArgs) Handles MyOwner.StyleChanged
            If _ownerBorderStyleProperty Is Nothing Then Return
            Dim bsBorderStyle As BorderStyle = _ownerBorderStyleProperty.GetValue(MyOwner)
            If bsBorderStyle = _borderStyle Then Return
            _borderStyle = bsBorderStyle
            Select Case bsBorderStyle
                Case BorderStyle.Fixed3D
                    _borderThickness = 2
                Case BorderStyle.FixedSingle
                    _borderThickness = 1
                Case BorderStyle.None
                    _borderThickness = 0
            End Select
            RefreshValues()
            MyOwner.Invalidate()
        End Sub
        Private Sub MyPinStripe_ColorChanged(sender As Object, e As EventArgs) Handles MyPinStripe.ColorChanged
            MyOwner.Invalidate()
        End Sub
        Private Sub MyPinStripe_DirectionChanged(sender As Object, e As EventArgs) Handles MyPinStripe.DirectionChanged
            Select Case MyPinStripe._direction
                Case Direction.Left, Direction.Right
                    _thickness = MyOwner.Height
                Case Direction.Up, Direction.Down
                    _thickness = MyOwner.Width
                Case Else
                    _thickness = 0
            End Select
            RefreshValues()
            MyOwner.Invalidate()
        End Sub
        Private Sub MyPinStripe_SpacingChanged(sender As Object, e As EventArgs) Handles MyPinStripe.SpacingChanged
            RefreshValues()
            MyOwner.Invalidate()
        End Sub
        Private Sub MyPinStripe_ThicknessChanged(sender As Object, e As EventArgs) Handles MyPinStripe.ThicknessChanged
            RefreshValues()
            MyOwner.Invalidate()
        End Sub
        Private Sub MyPinStripe_TypeChanged(sender As Object, e As EventArgs) Handles MyPinStripe.TypeChanged
            RefreshValues()
            MyOwner.Invalidate()
        End Sub

#End Region

#Region "Initializers"

        Public Sub New(owner As Control, pinStripe As PinStripe)
            MyBrokenPinStripe = TryCast(pinStripe, BrokenPinStripe)
            MyOwner = owner
            MyPinStripe = pinStripe

            Dim piBorderStyle As PropertyInfo = owner.GetType.GetProperty("BorderStyle", BindingFlags.Instance Or BindingFlags.Public) 'Get BorderStyle property for owner
            If piBorderStyle?.PropertyType Is GetType(BorderStyle) Then 'Determine if owner has a BorderStyle Property
                _ownerBorderStyleProperty = piBorderStyle 'Cache the BorderStyle property for quicker access later
                _borderStyle = piBorderStyle.GetValue(owner) 'Get the initial BorderStyle of the owner
                Select Case _borderStyle 'Determine the BorderStyle of the owner to get the initial BorderThickness of the owner
                    Case BorderStyle.Fixed3D
                        _borderThickness = 2
                    Case BorderStyle.FixedSingle
                        _borderThickness = 1
                    Case BorderStyle.None
                        _borderThickness = 0
                End Select
            End If

            Select Case MyPinStripe._direction
                Case Direction.Left, Direction.Right
                    _thickness = MyOwner.Height
                Case Direction.Up, Direction.Down
                    _thickness = MyOwner.Width
                Case Else
                    _thickness = 0
            End Select

            RefreshValues()
        End Sub

#End Region

#Region "Properties"

        '''<summary>Represents the <see cref="Windows.Forms.BorderStyle"/> of the <see cref="Control"/> the <see cref="Painter"/> is assigned to.</summary>
        '''<returns>The <see cref="Windows.Forms.BorderStyle"/> of the <see cref="Control"/> the <see cref="Painter"/> is assigned to.</returns>
        Public ReadOnly Property BorderStyle As BorderStyle
            Get
                Return _borderStyle
            End Get
        End Property
        '''<summary>Gets the <see cref="KoerControls.PinStripe"/> the <see cref="Painter"/> will paint.</summary>
        '''<returns>The <see cref="KoerControls.PinStripe"/> the <see cref="Painter"/> will paint.</returns>
        Public ReadOnly Property PinStripe As PinStripe
            Get
                Return MyPinStripe
            End Get
        End Property
        '''<summary>Represents the thickness of the <see cref="Control"/> according to the direction of the <see cref="KoerControls.PinStripe"/>.</summary>
        '''<returns>The thickness of the <see cref="Control"/> according to the direction of the <see cref="KoerControls.PinStripe"/>.</returns>
        Public Property Thickness As Integer
            Get
                Return _thickness
            End Get
            Friend Set(value As Integer)
                If value = _thickness Then Return
                _thickness = value
                RefreshValues()
            End Set
        End Property
        '''<summary>Represents one of the x-values in the <see cref="KoerControls.PinStripe"/>.</summary>
        '''<remarks>When Type = Chevron Horizontal : The x-value of the back of the trail of the <see cref="KoerControls.PinStripe"/>.
        '''<para/>When Type = Chevron Vertical : The x-value of the left of the <see cref="KoerControls.PinStripe"/>.
        '''<para/>When Type = Diagonal : The x-value of the top-left of the <see cref="KoerControls.PinStripe"/>.</remarks>
        '''<returns>One of the x-values in the <see cref="KoerControls.PinStripe"/>.</returns>
        Public Overridable Property X1 As Integer
            Get
                Return _x1
            End Get
            Protected Set(value As Integer)
                _x1 = value
            End Set
        End Property
        '''<summary>Represents one of the x-values in the <see cref="KoerControls.PinStripe"/>.</summary>
        '''<remarks>When Type = Chevron Horizontal : The x-value of the lead of the trail of the <see cref="KoerControls.PinStripe"/>.
        '''<para/>When Type = Chevorn Vertical : The x-value of the lead of trail for the left line of the <see cref="KoerControls.PinStripe"/>.
        '''<para/>When Type = Diagonal : The x-value of the bottom-right of the <see cref="KoerControls.PinStripe"/>.</remarks>
        '''<returns>One of the x-values in the <see cref="KoerControls.PinStripe"/>.</returns>
        Public Overridable Property X2 As Integer
            Get
                Return _x2
            End Get
            Protected Set(value As Integer)
                _x2 = value
            End Set
        End Property
        '''<summary>Represents one of the x-values in the <see cref="KoerControls.PinStripe"/>.</summary>.
        '''<remarks>When Type = Chevron Horizontal : Not used.
        '''<para/>When Type = Chevron Vertical : The x-vale of the lead of the trail for the right line of the <see cref="KoerControls.PinStripe"/>.
        '''<para/>When Type = Diagonal : Not used.</remarks>
        '''<returns>One of the x-values in the <see cref="KoerControls.PinStripe"/>.</returns>
        Public Overridable Property X3 As Integer
            Get
                Return _x3
            End Get
            Protected Set(value As Integer)
                _x3 = value
            End Set
        End Property
        '''<summary>Represents one of the x-values in the <see cref="KoerControls.PinStripe"/>.</summary>
        '''<remarks>When Type = Chevron Horizontal : Not used.
        '''<para/>When Type = Chevron Vertical : The x-vale of the right side of the <see cref="KoerControls.PinStripe"/>.
        '''<para/>When Type = Diagonal : Not used.</remarks>
        '''<returns>One of the x-values in the <see cref="KoerControls.PinStripe"/>.</returns>
        Public Overridable Property X4 As Integer
            Get
                Return _x4
            End Get
            Protected Set(value As Integer)
                _x4 = value
            End Set
        End Property
        '''<summary>Represents one of the y-values in the <see cref="KoerControls.PinStripe"/>.</summary>
        '''<remarks>When Type = Chevron Horizontal : The y-value of the top of the <see cref="KoerControls.PinStripe"/>.
        '''<para/>When Type = Chevron Vertical : The y-value of the back of the trail of the <see cref="KoerControls.PinStripe"/>.
        '''<para/>When Type = Diagonal : The x-value of the top-left of the <see cref="KoerControls.PinStripe"/>.</remarks>
        '''<returns>One of the x-values in the <see cref="KoerControls.PinStripe"/>.</returns>
        Public Overridable Property Y1 As Integer
            Get
                Return _y1
            End Get
            Protected Set(value As Integer)
                _y1 = value
            End Set
        End Property
        '''<summary>Represents one of the x-values in the <see cref="KoerControls.PinStripe"/>.</summary>
        '''<remarks>When Type = Chevron Horizontal : The y-value of the lead of trail for the top line of the <see cref="KoerControls.PinStripe"/>.
        '''<para/>When Type = Chevorn Vertical : The y-value of the lead of the trail of the <see cref="KoerControls.PinStripe"/>.
        '''<para/>When Type = Diagonal : The y-value of the bottom-right of the <see cref="KoerControls.PinStripe"/>.</remarks>
        '''<returns>One of the x-values in the <see cref="KoerControls.PinStripe"/>.</returns>
        Public Overridable Property Y2 As Integer
            Get
                Return _y2
            End Get
            Protected Set(value As Integer)
                _y2 = value
            End Set
        End Property
        '''<summary>Represents one of the x-values in the <see cref="KoerControls.PinStripe"/>.</summary>
        '''<remarks>When Type = Chevron Horizontal : The y-vale of the lead of the trail for the bottom line of the <see cref="KoerControls.PinStripe"/>.
        '''<para/>When Type = Chevron Vertical : Not used.
        '''<para/>When Type = Diagonal : Not used.</remarks>
        '''<returns>One of the x-values in the <see cref="KoerControls.PinStripe"/>.</returns>
        Public Overridable Property Y3 As Integer
            Get
                Return _y3
            End Get
            Protected Set(value As Integer)
                _y3 = value
            End Set
        End Property
        '''<summary>Represents one of the x-values in the <see cref="KoerControls.PinStripe"/>.</summary>
        '''<remarks>When Type = Chevron Horizontal :  The y-vale of the bottom side of the <see cref="KoerControls.PinStripe"/>.
        '''<para/>When Type = Chevron Vertical : Not used.
        '''<para/>When Type = Diagonal : Not used.</remarks>
        '''<returns>One of the x-values in the <see cref="KoerControls.PinStripe"/>.</returns>
        Public Overridable Property Y4 As Integer
            Get
                Return _y4
            End Get
            Protected Set(value As Integer)
                _y4 = value
            End Set
        End Property

#End Region

#Region "Functions"

        '''<summary>Refreshes the x and y values to be presented for painting the pin stripe.</summary>
        Protected Overridable Sub RefreshValues()
            If _thickness = 0 Then Return
            Dim pstType As PinStripeType = MyPinStripe._type
            If pstType = PinStripeType.None Then Return

            Dim iOwnerHalfThickness As Integer = Math.Floor(_thickness / 2)
            Dim dDirection As Direction = MyPinStripe._direction
            Dim iThickness As Integer = MyPinStripe._thickness

            'Set coordinates
            Select Case pstType
                Case PinStripeType.Chevron
                    Dim iOwnerThicknessAdder As Integer = -(_thickness + 1) Mod 2
                    Dim iThicknessAdder As Integer = iThickness * 0.25
                    Select Case dDirection
                        Case Direction.Left
                            _x1 = MyOwner.Right
                            _x2 = _x1 - iOwnerHalfThickness - iThicknessAdder - iThickness
                            _y1 = -iThickness + iOwnerThicknessAdder - _borderThickness
                            _y2 = iOwnerHalfThickness + iThicknessAdder + iOwnerThicknessAdder - _borderThickness
                            _y3 = _thickness + iThickness - iOwnerThicknessAdder - _borderThickness - 1
                            _y4 = iOwnerHalfThickness - iThicknessAdder - _borderThickness
                        Case Direction.Right
                            _x1 = MyOwner.Left
                            _x2 = _x1 - iOwnerHalfThickness - iThicknessAdder - iThickness
                            _y1 = -iThickness + iOwnerThicknessAdder - _borderThickness
                            _y2 = iOwnerHalfThickness + iThicknessAdder + iOwnerThicknessAdder - _borderThickness
                            _y3 = _thickness + iThickness - iOwnerThicknessAdder - _borderThickness - 1
                            _y4 = iOwnerHalfThickness - iThicknessAdder - _borderThickness
                        Case Direction.Up
                            _x1 = -iThickness + iOwnerThicknessAdder - _borderThickness
                            _x2 = iOwnerHalfThickness + iThicknessAdder + iOwnerThicknessAdder - _borderThickness
                            _x3 = _thickness + iThickness - iOwnerThicknessAdder - _borderThickness - 1
                            _x4 = iOwnerHalfThickness - iThicknessAdder - _borderThickness
                            _y1 = MyOwner.Bottom
                            _y2 = _y1 - iOwnerHalfThickness - iThicknessAdder - iThickness
                        Case Direction.Down
                            _x1 = -iThickness + iOwnerThicknessAdder - _borderThickness
                            _x2 = iOwnerHalfThickness + iThicknessAdder + iOwnerThicknessAdder - _borderThickness
                            _x3 = _thickness + iThickness - iOwnerThicknessAdder - _borderThickness - 1
                            _x4 = iOwnerHalfThickness - iThicknessAdder - _borderThickness
                            _y1 = MyOwner.Top
                            _y2 = _y1 - iOwnerHalfThickness - iThicknessAdder - iThickness
                    End Select

                Case PinStripeType.Diagonal
                    Select Case dDirection
                        Case Direction.Left
                            Dim iOwnerRight As Integer = MyOwner.Right
                            _x1 = iOwnerRight + iThickness + _thickness
                            _x2 = iOwnerRight - iThickness
                            _y1 = -iThickness
                            _y2 = iThickness + _thickness
                        Case Direction.Right
                            Dim iOwnerLeft As Integer = MyOwner.Left
                            _x1 = iOwnerLeft + iThickness
                            _x2 = iOwnerLeft - iThickness - _thickness
                            _y1 = -iThickness
                            _y2 = iThickness + _thickness
                        Case Direction.Up
                            _x1 = -iThickness
                            _x2 = iThickness + _thickness
                            Dim IOwnerBottom As Integer = MyOwner.Bottom
                            _y1 = IOwnerBottom + iThickness + _thickness
                            _y2 = IOwnerBottom - iThickness
                        Case Direction.Down
                            _x1 = -iThickness
                            _x2 = iThickness + _thickness
                            Dim iOwnerTop As Integer = MyOwner.Top
                            _y1 = iOwnerTop + iThickness
                            _y2 = iOwnerTop - iThickness - _thickness
                    End Select
            End Select
        End Sub

#End Region

    End Class

End Class
