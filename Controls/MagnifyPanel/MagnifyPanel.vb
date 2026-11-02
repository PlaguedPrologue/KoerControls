Imports System.ComponentModel
Imports System.Drawing.Drawing2D

'''<summary>Used to group collections of controls. Can be set to be drawn as a trapezoid.</summary>
Public Class MagnifyPanel
    Inherits Panel

#Region "Private Items"

    Private WithEvents MyMagnifyLocations As IntegerRange = IntegerRange.Empty

    Private _busyChangingMagnifyLocations As Boolean = False
    Private _magnifyDirection As Direction = Direction.None

    Private Sub MyMagnifyLocations_EndValueChagned(sender As Object, e As EventArgs) Handles MyMagnifyLocations.EndValueChanged
        If _busyChangingMagnifyLocations Then Return
        _busyChangingMagnifyLocations = True
        SetRegion()
        Invalidate()
        _busyChangingMagnifyLocations = False
    End Sub
    Private Sub MyMagnifyLocations_StartValueChagned(sender As Object, e As EventArgs) Handles MyMagnifyLocations.StartValueChanged
        If _busyChangingMagnifyLocations Then Return
        _busyChangingMagnifyLocations = True
        SetRegion()
        Invalidate()
        _busyChangingMagnifyLocations = False
    End Sub

#End Region

#Region "Properties"

    '''<summary>Indicates the border style for the control.</summary>
    '''<returns>One of the System.Windows.Forms.BorderStyle values. The default is BorderStyle.None.</returns>
    '''<exception cref="InvalidEnumArgumentException">The value assigned is not one of the <see cref="Windows.Forms.BorderStyle"/> values</exception>
    Public Overloads Property BorderStyle As BorderStyle
        Get
            Return MyBase.BorderStyle
        End Get
        Set(value As BorderStyle)
            If value = MyBase.BorderStyle Then Return
            MyBase.BorderStyle = value
            Invalidate()
        End Set
    End Property
    'Protected Overrides ReadOnly Property CreateParams As CreateParams
    '    Get
    '        Dim cpParams = MyBase.CreateParams
    '        cpParams.Style = cpParams.Style And Not &H2000000 'Can draw over child controls. A (failed) attempt to get the border to draw over children that clip into the edges.
    '        Return cpParams
    '    End Get
    'End Property
    '''<summary>Represents the flow direction of the larger section to the smaller section.</summary>
    '''<returns>The flow direction of the large section to the smaller section.</returns>
    <Category("Visual Elements")>
    <DefaultValue(Direction.None)>
    <Description("The direction in which the magnifying lines of the control point to.")>
    <RefreshProperties(RefreshProperties.Repaint)>
    Public Overridable Property MagnifyDirection As Direction
        Get
            Return _magnifyDirection
        End Get
        Set(value As Direction)
            If value = _magnifyDirection Then Return
            _magnifyDirection = value
            SetRegion()
            Invalidate()
        End Set
    End Property
    '''<summary>Represents the x or y values of the point of the smaller section in relation to the top or left of the control.</summary>
    '''<returns>The x or y values of the point of the smaller section in relation to the top or left of the control.</returns>
    <Category("Visual Elements")>
    <Description("The location on the edge of the panel in which the magnifying lines end at.")>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
    Public Overridable Property MagnifyLocations As IntegerRange
        Get
            Return MyMagnifyLocations
        End Get
        Set(value As IntegerRange)
            If value = MyMagnifyLocations Then Return
            MyMagnifyLocations = value
            If _busyChangingMagnifyLocations Then Return
            _busyChangingMagnifyLocations = True
            SetRegion()
            Invalidate()
            _busyChangingMagnifyLocations = False
        End Set
    End Property
    Private Sub ResetMagnifyLocations()
        Select Case _magnifyDirection
            Case Direction.Left, Direction.Right
                MagnifyLocations = New IntegerRange(0, Height)
            Case Direction.Up, Direction.Down
                MagnifyLocations = New IntegerRange(0, Width)
            Case Else
                MagnifyLocations = IntegerRange.Empty
        End Select
    End Sub
    Private Function ShouldSerializeMagnifyLocations() As Boolean
        Select Case _magnifyDirection
            Case Direction.Left, Direction.Right
                Return MyMagnifyLocations.StartValue <> 0 OrElse MyMagnifyLocations.EndValue <> Height
            Case Direction.Up, Direction.Down
                Return MyMagnifyLocations.StartValue <> 0 OrElse MyMagnifyLocations.EndValue <> Width
            Case Else
                Return MyMagnifyLocations <> IntegerRange.Empty
        End Select
    End Function

#End Region

#Region "Functions"

    '''<summary>Supports rendering to the specified bitmap.</summary>
    '''<exception cref="ArgumentNullException"/>
    '''<param name="image">Image to be drawn to.</param>
    '''<param name="targetBounds">The bounds withing which the control is rendered.</param>
    Public Overloads Sub DrawToBitmap(ByRef image As Bitmap, targetBounds As Rectangle)
        Using bmpControl As New Bitmap(Width, Height)
            Using gControl As Graphics = Graphics.FromImage(bmpControl)

                'Create drawable background color
                Using bBursh As New SolidBrush(BackColor)
                    If BorderStyle = BorderStyle.FixedSingle Then
                        gControl.FillRectangle(bBursh, New Rectangle(0, 0, Width + 1, Height + 1))
                    Else
                        gControl.FillRectangle(bBursh, ClientRectangle)
                    End If
                End Using

                'Create drawable border
                If BorderStyle = BorderStyle.FixedSingle Then
                    Using pPen As New Pen(SystemColors.ControlDarkDark, 1)
                        gControl.DrawRectangle(pPen, New Rectangle(Math.Max(0, -Location.X), Math.Max(0, -Location.Y), Width - 1, Height - 1))
                    End Using

                    'Create drawable border around child controls
                    Dim ctrlChildren As List(Of Control) = Controls.Cast(Of Control)().ToList
                    For Each ctrlChild As Control In ctrlChildren.OrderBy(Function(ctrl) -ctrl.Parent?.Controls.IndexOf(ctrl))
                        If ctrlChild.Visible AndAlso ctrlChild.Width > 0 Then
                            If TypeOf ctrlChild Is Panel Then
                                DirectCast(ctrlChild, Panel).DrawToBitmap(bmpControl, ctrlChild.Bounds)
                            Else
                                ctrlChild.DrawToBitmap(bmpControl, ctrlChild.Bounds)
                            End If
                        End If
                    Next
                End If

                'Draw background
                Using gImage As Graphics = Graphics.FromImage(image)
                    gImage.DrawImage(bmpControl, targetBounds)
                End Using
            End Using
        End Using
    End Sub
    Protected Overrides Sub OnLocationChanged(e As EventArgs)
        MyBase.OnLocationChanged(e)
        SetRegion()
    End Sub
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        Select Case _magnifyDirection
            Case Direction.Left, Direction.Right
                If MyMagnifyLocations.StartValue = 0 AndAlso MyMagnifyLocations.EndValue = Height Then Return
            Case Direction.Up, Direction.Down
                If MyMagnifyLocations.StartValue = 0 AndAlso MyMagnifyLocations.EndValue = Width Then Return
            Case Direction.None
                Return
        End Select

        Select Case BorderStyle
            Case BorderStyle.Fixed3D
                Dim iStartValue As Integer = MyMagnifyLocations.StartValue
                Dim iEndValue As Integer = MyMagnifyLocations.EndValue
                Dim iHeight As Integer = Height
                Dim iWidth As Integer = Width
                Using pDark As New Pen(SystemColors.ControlDark, 3)
                    Using pDarkDark = New Pen(SystemColors.ControlDarkDark, 3)
                        Using pLight As New Pen(SystemColors.ControlLight, 3)
                            Using pLightLight As New Pen(SystemColors.ControlLightLight, 3)
                                Select Case _magnifyDirection
                                    Case Direction.Down
                                        Dim iLeftSlopeLight As Integer = Math.Floor(iStartValue / iHeight) - 2
                                        Dim iLeftSlopeDark As Integer = iLeftSlopeLight + 1
                                        Dim iRightSlopeDark As Integer = Math.Ceiling((iWidth - iEndValue) / iHeight) + 2
                                        Dim iRightSlopeLight As Integer = iRightSlopeDark - 1
                                        e.Graphics.DrawLine(pDarkDark, iLeftSlopeDark, 0, iStartValue + iLeftSlopeDark, iHeight)
                                        e.Graphics.DrawLine(pDark, iLeftSlopeLight, 0, iStartValue + iLeftSlopeLight, iHeight)
                                        e.Graphics.DrawLine(pLight, iWidth - iRightSlopeDark, 0, iEndValue - iRightSlopeDark, iHeight)
                                        e.Graphics.DrawLine(pLightLight, iWidth - iRightSlopeLight, 0, iEndValue - iRightSlopeLight, iHeight)
                                    Case Direction.Left
                                        Dim iTopSlopeLight As Integer = Math.Ceiling(iStartValue / iWidth) + 2
                                        Dim iTopSlopeDark As Integer = iTopSlopeLight - 1
                                        Dim iBottomSlopeDark As Integer = Math.Floor((iHeight - iEndValue) / iWidth) - 2
                                        Dim iBottomSlopeLight As Integer = iBottomSlopeDark + 1
                                        e.Graphics.DrawLine(pDarkDark, -1, iStartValue - iTopSlopeDark, iWidth, -iTopSlopeDark)
                                        e.Graphics.DrawLine(pDark, -1, iStartValue - iTopSlopeLight, iWidth, -iTopSlopeLight)
                                        e.Graphics.DrawLine(pLight, 0, iEndValue + iBottomSlopeDark, iWidth, iHeight + iBottomSlopeDark)
                                        e.Graphics.DrawLine(pLightLight, 0, iEndValue + iBottomSlopeLight, iWidth, iHeight + iBottomSlopeLight)
                                    Case Direction.Right
                                        Dim iTopSlopeLight As Integer = Math.Floor(iStartValue / iWidth) - 2
                                        Dim iTopSlopeDark As Integer = iTopSlopeLight + 1
                                        Dim iBottomSlopeDark As Integer = Math.Ceiling((iHeight - iEndValue) / iWidth) + 2
                                        Dim iBottomSlopeLight As Integer = iBottomSlopeDark - 1
                                        e.Graphics.DrawLine(pDarkDark, 0, iTopSlopeDark, Width, iStartValue + iTopSlopeDark)
                                        e.Graphics.DrawLine(pDark, 0, iTopSlopeLight, Width, iStartValue + iTopSlopeLight)
                                        e.Graphics.DrawLine(pLight, 0, iHeight - iBottomSlopeDark, iWidth, iEndValue - iBottomSlopeDark)
                                        e.Graphics.DrawLine(pLightLight, 0, iHeight - iBottomSlopeLight, iWidth, iEndValue - iBottomSlopeLight)
                                    Case Direction.Up
                                        Dim iLeftSlopeLight As Integer = Math.Ceiling(iStartValue / iHeight) + 2
                                        Dim iLeftSlopeDark As Integer = iLeftSlopeLight - 1
                                        Dim iRightSlopeDark As Integer = Math.Floor((iWidth - iEndValue) / iHeight) - 2
                                        Dim iRightSlopeLight As Integer = iRightSlopeLight + 1
                                        e.Graphics.DrawLine(pDarkDark, iStartValue - iLeftSlopeDark, -1, -iLeftSlopeDark, iHeight)
                                        e.Graphics.DrawLine(pDark, iStartValue - iLeftSlopeLight, -1, -iLeftSlopeLight, iHeight)
                                        e.Graphics.DrawLine(pLight, iEndValue + iRightSlopeDark, 0, iWidth + iRightSlopeDark, iHeight)
                                        e.Graphics.DrawLine(pLightLight, iEndValue + iRightSlopeLight, 0, iWidth + iRightSlopeLight, iHeight)
                                End Select
                            End Using
                        End Using
                    End Using
                End Using

            Case BorderStyle.FixedSingle
                Dim iStartValue As Integer = MyMagnifyLocations.StartValue
                Dim iEndValue As Integer = MyMagnifyLocations.EndValue
                Dim iHeight As Integer = Height
                Dim iWidth As Integer = Width
                Using pPen As New Pen(SystemColors.WindowFrame, 3)
                    Select Case _magnifyDirection
                        Case Direction.Down
                            Dim iLeftSlope As Integer = Math.Floor(iStartValue / iHeight) - 1
                            Dim iRightSlope As Integer = Math.Ceiling((iWidth - iEndValue) / iHeight)
                            e.Graphics.DrawLine(pPen, iLeftSlope, 0, iStartValue + iLeftSlope, iHeight)
                            e.Graphics.DrawLine(pPen, iWidth - iRightSlope, 0, iEndValue - iRightSlope, iHeight)
                        Case Direction.Left
                            Dim iTopSlope As Integer = Math.Ceiling(iStartValue / iWidth) + 1
                            Dim iBottomSlope As Integer = Math.Floor((iHeight - iEndValue) / iWidth)
                            e.Graphics.DrawLine(pPen, 0, iStartValue - iTopSlope, iWidth, -iTopSlope)
                            e.Graphics.DrawLine(pPen, 0, iEndValue + iBottomSlope, iWidth, iHeight + iBottomSlope)
                        Case Direction.Right
                            Dim iTopSlope As Integer = Math.Floor(iStartValue / iWidth) - 1
                            Dim iBottomSlope As Integer = Math.Ceiling((iHeight - iEndValue) / iWidth)
                            e.Graphics.DrawLine(pPen, 0, iTopSlope, Width, iStartValue + iTopSlope)
                            e.Graphics.DrawLine(pPen, 0, iHeight - iBottomSlope, iWidth, iEndValue - iBottomSlope)
                        Case Direction.Up
                            Dim iLeftSlope As Integer = Math.Ceiling(iStartValue / iHeight) + 1
                            Dim iRightSlope As Integer = Math.Floor((iWidth - iEndValue) / iHeight)
                            e.Graphics.DrawLine(pPen, iStartValue - iLeftSlope, 0, -iLeftSlope, iHeight)
                            e.Graphics.DrawLine(pPen, iEndValue + iRightSlope, 0, iWidth + iRightSlope, iHeight)
                    End Select
                End Using

            Case BorderStyle.None
                If Site IsNot Nothing AndAlso Site.DesignMode Then
                    Dim iStartValue As Integer = MyMagnifyLocations.StartValue
                    Dim iEndValue As Integer = MyMagnifyLocations.EndValue
                    Dim iHeight As Integer = Height
                    Dim iWidth As Integer = Width
                    Using pPen As New Pen(SystemColors.WindowFrame, 3)
                        pPen.DashStyle = DashStyle.Dot
                        Select Case _magnifyDirection
                            Case Direction.Down
                                Dim iLeftSlope As Integer = Math.Floor(iStartValue / iHeight)
                                Dim iRightSlope As Integer = Math.Ceiling((iWidth - iEndValue) / iHeight) - 1
                                e.Graphics.DrawLine(pPen, iLeftSlope, 0, iStartValue + iLeftSlope, iHeight)
                                e.Graphics.DrawLine(pPen, iWidth - iRightSlope, 0, iEndValue - iRightSlope, iHeight)
                            Case Direction.Left
                                Dim iTopSlope As Integer = Math.Ceiling(iStartValue / iWidth) - 1
                                Dim iBottomSlope As Integer = Math.Floor((iHeight - iEndValue) / iWidth)
                                e.Graphics.DrawLine(pPen, 0, iStartValue - iTopSlope, iWidth, -iTopSlope)
                                e.Graphics.DrawLine(pPen, 0, iEndValue + iBottomSlope, iWidth, iHeight + iBottomSlope)
                            Case Direction.Right
                                Dim iTopSlope As Integer = Math.Floor(iStartValue / iWidth)
                                Dim iBottomSlope As Integer = Math.Ceiling((iHeight - iEndValue) / iWidth) - 1
                                e.Graphics.DrawLine(pPen, 0, iTopSlope, Width, iStartValue + iTopSlope)
                                e.Graphics.DrawLine(pPen, 0, iHeight - iBottomSlope, iWidth, iEndValue - iBottomSlope)
                            Case Direction.Up
                                Dim iLeftSlope As Integer = Math.Ceiling(iStartValue / iHeight) - 1
                                Dim iRightSlope As Integer = Math.Floor((iWidth - iEndValue) / iHeight)
                                e.Graphics.DrawLine(pPen, iStartValue - iLeftSlope, 0, -iLeftSlope, iHeight)
                                e.Graphics.DrawLine(pPen, iEndValue + iRightSlope, 0, iWidth + iRightSlope, iHeight)
                        End Select
                    End Using
                End If
        End Select
    End Sub
    Protected Overrides Sub OnSizeChanged(e As EventArgs)
        MyBase.OnSizeChanged(e)
        SetRegion()
    End Sub
    '''<summary>Sets the drawable region of the control.</summary>
    Protected Overridable Sub SetRegion()
        Using gpRegion As New GraphicsPath
            Select Case _magnifyDirection
                Case Direction.Down
                    Dim iHeight As Integer = Height
                    gpRegion.AddPolygon({New Point(0, 0), New Point(MyMagnifyLocations.StartValue, iHeight), New Point(MyMagnifyLocations.EndValue, iHeight), New Point(Width, 0)})
                Case Direction.Left
                    Dim iWidth As Integer = Width
                    gpRegion.AddPolygon({New Point(0, MyMagnifyLocations.EndValue), New Point(iWidth, Height), New Point(iWidth, 0), New Point(0, MyMagnifyLocations.StartValue)})
                Case Direction.Right
                    Dim iWidth As Integer = Width
                    gpRegion.AddPolygon({New Point(0, 0), New Point(iWidth, MyMagnifyLocations.StartValue), New Point(iWidth, MyMagnifyLocations.EndValue), New Point(0, Height)})
                Case Direction.Up
                    Dim iHeight As Integer = Height
                    gpRegion.AddPolygon({New Point(MyMagnifyLocations.EndValue, 0), New Point(Width, iHeight), New Point(0, iHeight), New Point(MyMagnifyLocations.StartValue, 0)})
                Case Else
                    Dim iHeight As Integer = Height
                    Dim iWidth As Integer = Width
                    gpRegion.AddPolygon({New Point(0, 0), New Point(0, iHeight), New Point(iWidth, iHeight), New Point(iWidth, 0)})
            End Select
            Region = New Region(gpRegion)
        End Using
    End Sub

#End Region

End Class
