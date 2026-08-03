Imports System.Collections.ObjectModel

Partial Public Class PinStripe

    Friend Class PainterCollection
        Inherits Collection(Of Painter)
        Implements IDisposable

#Region "Private Items"

        Private WithEvents MyOwner As Control

        Private Sub MyOwner_Paint(sender As Object, e As PaintEventArgs) Handles MyOwner.Paint
            Paint(e.Graphics)
        End Sub
        Private Sub MyOwner_SizeChanged(sender As Object, e As EventArgs) Handles MyOwner.SizeChanged
            Dim bAnyThicknessChanged As Boolean = False
            For Each pPainter As Painter In Items 'Go through each Painter in the PainterCollection
                Dim iOwnerThickness As Integer
                Select Case pPainter.PinStripe._direction 'Get thickness of owning Control
                    Case Direction.Left, Direction.Right
                        iOwnerThickness = MyOwner.Height
                    Case Direction.Up, Direction.Down
                        iOwnerThickness = MyOwner.Width
                    Case Else
                        iOwnerThickness = 0
                End Select
                If iOwnerThickness = pPainter.Thickness Then Continue For 'Thickness does not change, go to next Painter for the Control
                pPainter.Thickness = iOwnerThickness 'Set Control thickness for Painter
                bAnyThicknessChanged = True
            Next
            If bAnyThicknessChanged Then MyOwner.Invalidate() 'If Painter any changes, repaint
        End Sub

#End Region

#Region "Initializers"

        Public Sub New(owner As Control)
            MyOwner = owner
            owner.AddDoubleBufferReason(Me)
        End Sub

#End Region

#Region "Functions"

        Public Overridable Sub Dispose() Implements IDisposable.Dispose
            Clear()
            MyOwner.RemoveDoubleBufferReason(Me)
        End Sub
        '''<summary>Paints the <see cref="PinStripe"/> onto the <see cref="Control"/> assigned to the <see cref="PainterCollection"/>.</summary>
        '''<param name="graphics">The <see cref="Graphics"/> used to paint the <see cref="PinStripe"/>.</param>
        Protected Overridable Sub Paint(graphics As Graphics)
            'Fill background to reset PinStripes painted on
            Dim rPaintBounds As RectangleF = graphics.ClipBounds
            Using bBrush As New SolidBrush(MyOwner.GetDisplayedBackColor)
                graphics.FillRectangle(bBrush, rPaintBounds)
            End Using

            'Paint each PinStripe
            For Each pPainter As Painter In Items
                Dim psPinStripe As PinStripe = pPainter.PinStripe
                Dim dDirection As Direction = psPinStripe._direction
                If dDirection = Direction.None Then Continue For
                Dim iSpacing As Integer = psPinStripe._spacing
                If iSpacing <= 0 Then Continue For
                Dim iThickness As Integer = psPinStripe._thickness
                If iThickness <= 0 Then Continue For
                Dim pstType As PinStripeType = psPinStripe._type
                If pstType = PinStripeType.None Then Continue For
                Dim cColor As Color = psPinStripe._highlightColor.GetColor(Highlighter.IsHighlighted(MyOwner))
                If cColor = Color.Transparent Then Continue For

                Dim irPaintBounds As IntegerRange = rPaintBounds.ToIntegerRange(dDirection)
                Dim irPaintClips As IEnumerable(Of IntegerRange)
                If TypeOf psPinStripe Is BrokenPinStripe Then
                    irPaintClips = CType(psPinStripe, BrokenPinStripe).JoinedPinStripeLocations
                Else
                    irPaintClips = {irPaintBounds}
                End If

                For Each irPaintClip As IntegerRange In irPaintClips
                    If Not irPaintBounds.Overlaps(irPaintClip) Then Continue For
                    irPaintClip = irPaintClip.InnerJoin(irPaintBounds)
                    Select Case dDirection
                        Case Direction.Left, Direction.Right
                            graphics.SetClip(New RectangleF(irPaintClip.StartValue, rPaintBounds.Top, irPaintClip.Length, rPaintBounds.Height))
                        Case Direction.Up, Direction.Down
                            graphics.SetClip(New Rectangle(rPaintBounds.Left, irPaintClip.StartValue, rPaintBounds.Width, irPaintClip.Length))
                    End Select

                    Dim iPaintSpacing As Integer = iSpacing + iThickness
                    Dim iX1 As Integer = pPainter.X1
                    Dim iX2 As Integer = pPainter.X2
                    Dim iX3 As Integer = pPainter.X3
                    Dim iX4 As Integer = pPainter.X4
                    Dim iY1 As Integer = pPainter.Y1
                    Dim iY2 As Integer = pPainter.Y2
                    Dim iY3 As Integer = pPainter.Y3
                    Dim iY4 As Integer = pPainter.Y4

                    Using pPen As New Pen(cColor, iThickness / 1.414)
                        Select Case dDirection
                            Case Direction.Down
                                Dim iOffset As Integer = (irPaintClip.StartValue - iY1).RoundToFloor(iPaintSpacing) - Math.Floor(iThickness / 2)
                                iY1 += iOffset
                                iY2 += iOffset
                                Select Case pstType
                                    Case PinStripeType.Chevron
                                        While iY2 < irPaintClip.EndValue
                                            graphics.DrawLine(pPen, iX1, iY2, iX2, iY1)
                                            graphics.DrawLine(pPen, iX3, iY2, iX4, iY1)
                                            iY1 += iPaintSpacing
                                            iY2 += iPaintSpacing
                                        End While
                                    Case PinStripeType.Diagonal
                                        While iY2 < irPaintClip.EndValue
                                            graphics.DrawLine(pPen, iX1, iY1, iX2, iY2)
                                            iY1 += iPaintSpacing
                                            iY2 += iPaintSpacing
                                        End While
                                End Select

                            Case Direction.Left
                                Dim iOffset As Integer = (iX2 - irPaintClip.EndValue).RoundToFloor(iPaintSpacing) - Math.Floor(iThickness / 2)
                                iX1 -= iOffset
                                iX2 -= iOffset
                                Select Case pstType
                                    Case PinStripeType.Chevron
                                        While iX1 > irPaintClip.StartValue
                                            graphics.DrawLine(pPen, iX1, iY1, iX2, iY2)
                                            graphics.DrawLine(pPen, iX1, iY3, iX2, iY4)
                                            iX1 -= iPaintSpacing
                                            iX2 -= iPaintSpacing
                                        End While
                                    Case PinStripeType.Diagonal
                                        While iX1 > irPaintClip.StartValue
                                            graphics.DrawLine(pPen, iX1, iY1, iX2, iY2)
                                            iX1 -= iPaintSpacing
                                            iX2 -= iPaintSpacing
                                        End While
                                End Select

                            Case Direction.Right
                                Dim iOffset As Integer = (irPaintClip.StartValue - iX1).RoundToFloor(iPaintSpacing) - Math.Floor(iThickness / 2)
                                iX1 += iOffset
                                iX2 += iOffset
                                Select Case pstType
                                    Case PinStripeType.Chevron
                                        While iX2 < irPaintClip.EndValue
                                            graphics.DrawLine(pPen, iX2, iY1, iX1, iY2)
                                            graphics.DrawLine(pPen, iX2, iY3, iX1, iY4)
                                            iX1 += iPaintSpacing
                                            iX2 += iPaintSpacing
                                        End While
                                    Case PinStripeType.Diagonal
                                        While iX2 < irPaintClip.EndValue
                                            graphics.DrawLine(pPen, iX1, iY1, iX2, iY2)
                                            iX1 += iPaintSpacing
                                            iX2 += iPaintSpacing
                                        End While
                                End Select

                            Case Direction.Up
                                Dim iOffset As Integer = (iY2 - irPaintClip.EndValue).RoundToFloor(iPaintSpacing) - Math.Floor(iThickness / 2)
                                iY1 -= iOffset
                                iY2 -= iOffset
                                Select Case pstType
                                    Case PinStripeType.Chevron
                                        While iY1 > irPaintClip.StartValue
                                            graphics.DrawLine(pPen, iX1, iY1, iX2, iY2)
                                            graphics.DrawLine(pPen, iX3, iY1, iX4, iY2)
                                            iY1 -= iPaintSpacing
                                            iY2 -= iPaintSpacing
                                        End While
                                    Case PinStripeType.Diagonal
                                        While iY1 > irPaintClip.StartValue
                                            graphics.DrawLine(pPen, iX1, iY1, iX2, iY2)
                                            iY1 -= iPaintSpacing
                                            iY2 -= iPaintSpacing
                                        End While
                                End Select
                        End Select
                    End Using

                    If TypeOf pPainter.PinStripe Is BrokenPinStripe AndAlso pPainter.BorderStyle = BorderStyle.FixedSingle Then
                        Select Case dDirection
                            Case Direction.Left, Direction.Right
                                Dim rClipBounds As Rectangle = Rectangle.Ceiling(graphics.ClipBounds)
                                If rClipBounds.Left <= 0 AndAlso rClipBounds.Top <= 0 AndAlso rClipBounds.Width >= MyOwner.Width - 2 AndAlso rClipBounds.Height >= MyOwner.Height - 2 Then Return
                                If rClipBounds.Left <= 0 Then
                                    rClipBounds.Offset(-1, -1)
                                Else
                                    rClipBounds.Offset(0, -1)
                                End If
                                If rClipBounds.Right < MyOwner.Width - 2 Then rClipBounds.Width -= 1
                                rClipBounds.Height += 1
                                Using pPen As New Pen(SystemColors.WindowFrame)
                                    graphics.DrawRectangle(pPen, rClipBounds)
                                End Using
                            Case Direction.Up, Direction.Down
                                Dim rClipBounds As Rectangle = Rectangle.Ceiling(graphics.ClipBounds)
                                If rClipBounds.Left <= 0 AndAlso rClipBounds.Top <= 0 AndAlso rClipBounds.Width >= MyOwner.Width - 2 AndAlso rClipBounds.Height >= MyOwner.Height - 2 Then Return
                                If rClipBounds.Top <= 0 Then
                                    rClipBounds.Offset(-1, -1)
                                Else
                                    rClipBounds.Offset(-1, 0)
                                End If
                                If rClipBounds.Bottom < MyOwner.Height - 2 Then rClipBounds.Height -= 1
                                rClipBounds.Width += 1
                                Using pPen As New Pen(SystemColors.WindowFrame)
                                    graphics.DrawRectangle(pPen, rClipBounds)
                                End Using
                        End Select
                    End If
                Next
            Next
        End Sub

#End Region

    End Class

End Class
