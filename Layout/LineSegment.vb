'''<summary>Represents an ordered pair of integer x- and y-coordinates with a length that defines a line segment in a 2D plane.</summary>
Public Structure LineSegment

#Region "Operators"

    Public Shared Operator =(left As LineSegment, right As LineSegment)
        Return left.Length = right.Length AndAlso left.X = right.X AndAlso left.Y = right.Y
    End Operator
    Public Shared Operator <>(left As LineSegment, right As LineSegment)
        Return Not left = right
    End Operator

#End Region

#Region "Initializers"

    '''<summary>Initializes a new instance of the <see cref="LineSegment"/> class with the specified coordinates.</summary>
    '''<param name="x">The horizontal position of the <see cref="LineSegment"/>.</param>
    '''<param name="y">The vertical position of the <see cref="LineSegment"/>.</param>
    '''<param name="length">The length the <see cref="LineSegment"/>.</param>
    Public Sub New(x As Integer, y As Integer, length As Integer)
        Me.X = x
        Me.Y = y
        Me.Length = length
    End Sub
    '''<summary>Initializes a new instance of the <see cref="LineSegment"/> class with the specified coordinates.</summary>
    '''<param name="point">The top left <see cref="Point"/> of the <see cref="LineSegment"/>.</param>
    '''<param name="length">The length the <see cref="LineSegment"/>.</param>
    Public Sub New(point As Point, length As Integer)
        X = point.X
        Y = point.Y
        Me.Length = length
    End Sub
    '''<summary>Initializes a new instance of the <see cref="LineSegment"/> class with the specified coordinates.</summary>
    '''<param name="point1">The first <see cref="Point"/> of the <see cref="LineSegment"/>.</param>
    '''<param name="point2">The second <see cref="Point"/> of the <see cref="LineSegment"/>.</param>
    Public Sub New(point1 As Point, point2 As Point)
        If point1.X = point2.X Then
            X = point1.X
            Y = Math.Min(point1.Y, point2.Y)
            Length = Math.Abs(point1.Y - point2.Y)
        ElseIf point1.Y = point2.Y Then
            X = Math.Min(point1.X, point2.X)
            Y = point1.Y
            Length = Math.Abs(point1.X - point2.X)
        Else
            Throw New InvalidConstraintException
        End If
    End Sub

#End Region

#Region "Properties"

    '''<summary>Gets or sets the length of this <see cref="LineSegment"/>.</summary>
    '''<returns>The length of this <see cref="LineSegment"/>.</returns>
    Public ReadOnly Property Length As Integer
    '''<summary>Gets or sets the start <see cref="Point"/> of the <see cref="LineSegment"/>.</summary>
    '''<returns>The start <see cref="Point"/> of the <see cref="LineSegment"/></returns>
    Public ReadOnly Property Location
        Get
            Return New Point(X, Y)
        End Get
    End Property
    '''<summary>Gets or sets the X-coordinate of this <see cref="LineSegment"/>.</summary>
    '''<returns>The X-coordinate of this <see cref="LineSegment"/>.</returns>
    Public ReadOnly Property X As Integer
    '''<summary>Gets or sets the Y-coordinate of this <see cref="LineSegment"/>.</summary>
    '''<returns>The Y-coordinate of this <see cref="LineSegment"/>.</returns>
    Public ReadOnly Property Y As Integer

#End Region

End Structure
