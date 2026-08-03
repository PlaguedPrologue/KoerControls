Imports System.ComponentModel

'''<summary>Used to visually separate groups of controls in a container.</summary>
<Designer(GetType(LinePanelDesigner))>
Public Class LinePanel
    Inherits Panel

#Region "Private Items"

    Private WithEvents MyParent As Control

    Private _docked As Boolean = True
    Private _orientation As Orientation = Orientation.Horizontal

    Private Sub MyParent_SizeChanged(sender As Object, e As EventArgs) Handles MyParent.SizeChanged
        OnParentSizeChanged(e)
    End Sub

#End Region

#Region "Initializers"

    Public Sub New()
        MyBase.New
        Height = 3
    End Sub

#End Region

#Region "Events"

    ''' <summary>Occurs when the thickness has changed.</summary>
    Public Event ThicknessChanged As EventHandler
    ''' <summary>Occurs when the size of the parent control has changed.</summary>
    Public Event ParentSizeChanged As EventHandler

#End Region

#Region "Properties"

    '''<summary>Gets or sets the bounding <see cref="LineSegment"/> that represents the size and location of the control, in pixels, relative to the parent control.</summary>
    '''<returns>The bounding <see cref="LineSegment"/> that represents the size and location of the control, in pixels, relative to of the parent control.</returns>
    '''<param name="thickness">The thickness of the control, in pixels. Sets to the current thicknes if no value is passed in.</param>
    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    <EditorBrowsable(EditorBrowsableState.Never)>
    Public Shadows Property Bounds(Optional thickness As Integer = 0) As LineSegment
        Get
            Return New LineSegment(Location, Length)
        End Get
        Set(value As LineSegment)
            If _orientation = Orientation.Horizontal Then
                If thickness = 0 Then
                    SetBoundsCore(value.X, value.Y, value.Length, Height, BoundsSpecified.All)
                Else
                    SetBoundsCore(value.X, value.Y, value.Length, thickness, BoundsSpecified.All)
                End If
            Else
                If thickness = 0 Then
                    SetBoundsCore(value.X, value.Y, Width, value.Length, BoundsSpecified.All)
                Else
                    SetBoundsCore(value.X, value.Y, thickness, value.Length, BoundsSpecified.All)
                End If
            End If
        End Set
    End Property
    '''<summary>Gets which control borders are docked to its parent control and determines how a control is resized with its parent.</summary>
    '''<returns>One of the <see cref="DockStyle"/> values. The defalut is <see cref="DockStyle.None"/>.</returns>
    <Browsable(False)>
    <DefaultValue(DockStyle.None)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    <EditorBrowsable(EditorBrowsableState.Never)>
    Public Overridable Shadows Property Dock As DockStyle
        Get
            If _docked Then Return DockStyle.Fill
            Return DockStyle.None
        End Get
        Set(value As DockStyle)
            If value = DockStyle.None Then
                Docked = False
            Else
                Docked = True
            End If
        End Set
    End Property
    '''<summary>Determines if the control will automatically span the width or the height of its container.</summary>
    '''<returns><see cref="Boolean">true</see> if the control automatically spans the width or the height of its container; otherwise <see cref="Boolean">false</see>.</returns>
    <Category("Layout")>
    <DefaultValue(True)>
    <Description("Determines if the control automatically spans the width or the height of its container. Default value is true.")>
    Public Overridable Property Docked As Boolean
        Get
            Return _docked
        End Get
        Set(value As Boolean)
            If value = _docked Then Return
            _docked = value
            SetBoundsCore(Left, Top, Width, Height, BoundsSpecified.All)
        End Set
    End Property
    '''<summary>Gets or sets the length of the control.</summary>
    '''<returns>The <see cref="Integer"/> that represents the length of the control in pixels.</returns>
    <Browsable(True)>
    <Category("Layout")>
    <DefaultValue(100)>
    <Description("The length of the control.")>
    Public Overridable Property Length As Integer
        Get
            If _orientation = Orientation.Horizontal Then Return Width
            Return Height
        End Get
        Set(value As Integer)
            If Not _docked Then
                If _orientation = Orientation.Horizontal Then
                    MyBase.SetBoundsCore(Left, Top, value, Height, BoundsSpecified.Width)
                Else
                    MyBase.SetBoundsCore(Left, Top, Width, value, BoundsSpecified.Height)
                End If
            End If
        End Set
    End Property
    '''<summary>Gets or sets the orientation of the control.</summary>
    '''<returns>The <see cref="Windows.Forms.Orientation"/> that represents the orientation of the control. The default value is <see cref="Windows.Forms.Orientation.Horizontal"/>.</returns>
    <Category("Layout")>
    <DefaultValue(Orientation.Horizontal)>
    <Description("The orientation of the line. Can be either Horizontal or Vertical. The default value is Horizontal.")>
    Public Overridable Property Orientation As Orientation
        Get
            Return _orientation
        End Get
        Set(value As Orientation)
            If value = _orientation Then Return
            _orientation = value
            SetBoundsCore(Left, Top, Height, Width, BoundsSpecified.All)
            Invalidate()
        End Set
    End Property
    '''<summary>Gets or sets the height and width of the control.</summary>
    '''<returns>The <see cref="System.Drawing.Size"/> that represents the height and width of the control in pixels.</returns>
    <Browsable(False)>
    <EditorBrowsable(EditorBrowsableState.Never)>
    Public Shadows Property Size As Size
        Get
            Return MyBase.Size
        End Get
        Set(value As Size)
            MyBase.Size = value
        End Set
    End Property
    '''<summary>Gets or sets the thicknes of the control.</summary>
    '''<returns>The <see cref="Integer"/> that represents the thickness of the control in pixels.</returns>
    <Category("Layout")>
    <DefaultValue(3)>
    <Description("The thickness of the control.")>
    Public Overridable Property Thickness As Integer
        Get
            If _orientation = Orientation.Horizontal Then Return Height
            Return Width
        End Get
        Set(value As Integer)
            If _orientation = Orientation.Horizontal Then
                If value = Height Then Return
                MyBase.SetBoundsCore(Left, Top, Width, value, BoundsSpecified.Height)
                OnThicknessChanged(EventArgs.Empty)
            Else
                If value = Width Then Return
                MyBase.SetBoundsCore(Left, Top, value, Height, BoundsSpecified.Width)
                OnThicknessChanged(EventArgs.Empty)
            End If
        End Set
    End Property
    '''<summary>Gets or sets the bounding <see cref="Rectangle"/> that represents the size and location of the control, in pixels, relative to the parent control.</summary>
    '''<returns>The bounding <see cref="Rectangle"/> that represents the size and location of the control, in pixels, relative to of the parent control.</returns>
    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    <EditorBrowsable(EditorBrowsableState.Never)>
    Public Property TrueBounds As Rectangle
        Get
            Return MyBase.Bounds
        End Get
        Set(value As Rectangle)
            SetBoundsCore(value.Left, value.Top, value.Width, value.Height, BoundsSpecified.All)
        End Set
    End Property

#End Region

#Region "Functions"

    Protected Overrides Sub OnParentChanged(e As EventArgs)
        MyParent = Parent
        MyBase.OnParentChanged(e)
    End Sub
    '''<summary>Raises the <see cref="ParentSizeChanged"/> event on the parent control.</summary>
    '''<param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    Protected Overridable Sub OnParentSizeChanged(e As EventArgs)
        If _docked AndAlso Parent IsNot Nothing Then
            If _orientation = Orientation.Horizontal Then
                Width = Parent.ClientSize.Width
            Else
                Width = Parent.ClientSize.Height
            End If
        End If
        RaiseEvent ParentSizeChanged(Me, e)
    End Sub
    '''<summary>Raises the <see cref="ThicknessChanged"/> event.</summary>
    '''<param name="e">The <see cref="EventArgs"/> containing the event data.</param>
    Protected Overridable Sub OnThicknessChanged(e As EventArgs)
        RaiseEvent ThicknessChanged(Me, e)
    End Sub
    Protected Overrides Sub SetBoundsCore(x As Integer, y As Integer, width As Integer, height As Integer, specified As BoundsSpecified)
        Dim bThicknessChanged As Boolean
        If _orientation = Orientation.Horizontal Then 'Horizontal orientation
            bThicknessChanged = height <> Me.Height

            'Get new x-value and thickness
            If _docked AndAlso Parent IsNot Nothing Then
                If TypeOf Parent Is TableLayoutPanel Then

                    Dim tlpParent As TableLayoutPanel = Parent
                    Dim iColumnCount As Integer = tlpParent.ColumnCount
                    Dim iColumnStart As Integer = tlpParent.GetColumn(Me)
                    Dim iColumnSpan As Integer = tlpParent.GetColumnSpan(Me)

                    If iColumnStart = 0 AndAlso iColumnSpan = iColumnCount Then
                        x = 0
                        width = tlpParent.ClientSize.Width
                    Else
                        'the .Take(tlpParent.ColumnCount) is required due to the way columns are automatically generated in the TableLayoutPanel. I don't entirely understand why the control works the way it does, but this fixes the issue.
                        Dim csColumns As ColumnStyle() = tlpParent.ColumnStyles.Cast(Of ColumnStyle)().Take(iColumnCount).ToArray
                        Dim iRemainingPercentagesWidth = tlpParent.ClientSize.Width
                        For iColumnIndex As Integer = 0 To iColumnCount - 1
                            Dim csColumn As ColumnStyle = csColumns(iColumnIndex)
                            If csColumn.SizeType = SizeType.Absolute Then iRemainingPercentagesWidth -= csColumn.Width
                        Next

                        Dim dX As Decimal = 0
                        For iColumnIndex As Integer = 0 To iColumnStart - 1
                            Dim csColumn As ColumnStyle = csColumns(iColumnIndex)
                            If csColumn.SizeType = SizeType.Absolute Then
                                dX += csColumn.Width
                            Else
                                dX += csColumn.Width / 100.0 * iRemainingPercentagesWidth
                            End If
                        Next
                        x = Math.Floor(dX)
                        Dim dWidth As Decimal = 0
                        For iColumnIndex As Integer = iColumnStart To iColumnStart + iColumnSpan - 1
                            Dim csColumn As ColumnStyle = csColumns(iColumnIndex)
                            If csColumn.SizeType = SizeType.Absolute Then
                                dWidth += csColumn.Width
                            Else
                                dWidth += csColumn.Width / 100.0 * iRemainingPercentagesWidth
                            End If
                        Next
                        width = Math.Ceiling(dWidth)
                    End If
                Else
                    x = 0
                    width = Parent.ClientSize.Width
                End If
            End If

        Else 'Vertical orientation
            bThicknessChanged = width <> Me.Width

            'Get new y-value and thickness
            If _docked AndAlso Parent IsNot Nothing Then
                If TypeOf Parent Is TableLayoutPanel Then

                    Dim tlpParent As TableLayoutPanel = Parent
                    Dim iRowCount As Integer = tlpParent.RowCount
                    Dim iRowStart As Integer = tlpParent.GetRow(Me)
                    Dim iRowSpan As Integer = tlpParent.GetRowSpan(Me)

                    If iRowStart = 0 AndAlso iRowSpan = iRowCount Then
                        y = 0
                        height = tlpParent.ClientSize.Height
                    Else
                        'the .Take(tlpParent.ColumnCount) is required due to the way columns are automatically generated in the TableLayoutPanel. I don't entirely understand why the control works the way it does, but this fixes the issue.
                        Dim rsRows As RowStyle() = tlpParent.RowStyles.Cast(Of RowStyle)().Take(iRowCount).ToArray
                        Dim iRemainingPercentagesHeight = tlpParent.ClientSize.Height
                        For iRowIndex As Integer = 0 To iRowCount - 1
                            Dim rsRow As RowStyle = rsRows(iRowIndex)
                            If rsRow.SizeType = SizeType.Absolute Then iRemainingPercentagesHeight -= rsRow.Height
                        Next

                        Dim dY As Decimal = 0
                        For iRowIndex As Integer = 0 To iRowStart - 1
                            Dim rsRow As RowStyle = rsRows(iRowIndex)
                            If rsRow.SizeType = SizeType.Absolute Then
                                dY += rsRow.Height
                            Else
                                dY += rsRow.Height / 100.0 * iRemainingPercentagesHeight
                            End If
                        Next
                        y = Math.Floor(dY)
                        Dim dHeight As Decimal = 0
                        For iRowIndex As Integer = iRowStart To iRowStart + iRowSpan - 1
                            Dim rsRow As RowStyle = rsRows(iRowIndex)
                            If rsRow.SizeType = SizeType.Absolute Then
                                dHeight += rsRow.Height
                            Else
                                dHeight += rsRow.Height / 100.0 * iRemainingPercentagesHeight
                            End If
                        Next
                        height = Math.Ceiling(dHeight)
                    End If
                Else
                    y = 0
                    height = Parent.ClientSize.Height
                End If
            End If
        End If

        MyBase.SetBoundsCore(x, y, width, height, specified)
        If bThicknessChanged Then OnThicknessChanged(EventArgs.Empty)
    End Sub

#End Region

End Class
