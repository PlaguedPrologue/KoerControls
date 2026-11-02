Imports System.ComponentModel
Imports System.Drawing.Design

<ToolboxItem(True)>
Public Class DateScroller
    Inherits Panel

#Region "Private Items"

    Private WithEvents MyDateRange As DateRange = DateRange.Empty
    Private WithEvents MyHeaderOverlay As New HeaderOverlay(Me, AddressOf PaintHeaderOverlay)

    Private _borderWidth As Integer = Width - ClientSize.Width
    Private _columnWidth As Integer = 100
    Private _itemPlacement As DateScrollerItemPlacement = DateScrollerItemPlacement.Waterfall
    Private ReadOnly _itemGroups As New DateScrollerGroupCollection
    Private _itemHeight As Integer = 25

    Private Sub MyDateRange_EndDateChanged(sender As Object, e As EventArgs) Handles MyDateRange.EndDateChanged
        RefreshAutoScrollMinSize()
        Invalidate()
        OnEndDateChanged(e)
    End Sub
    Private Sub MyDateRange_StartDateChanged(sender As Object, e As EventArgs) Handles MyDateRange.StartDateChanged
        RefreshAutoScrollMinSize()
        Invalidate()
        OnStartDateChanged(e)
    End Sub
    Private Sub MyHeaderOverlay_HeightChanged(sender As Object, e As EventArgs) Handles MyHeaderOverlay.HeightChanged
        OnHeaderHeightChanged(e)
    End Sub
    Private Sub MyHeaderOverlay_VisibleChanged(sender As Object, e As EventArgs) Handles MyHeaderOverlay.VisibleChanged
        OnHeaderVisibleChanged(e)
    End Sub
    Private Sub RefreshAutoScrollMinSize()
        MyBase.AutoScrollMinSize = New Size((DateDiff(DateInterval.Day, MyDateRange.StartDate, MyDateRange.EndDate) + 1) * _columnWidth - _borderWidth, 0)
    End Sub

#End Region

#Region "Events"

    '''<summary>Occurs when the column width has changed.</summary>
    Public Event ColumnWidthChanged As EventHandler
    '''<summary>Occurs when the end date has changed.</summary>
    Public Event EndDateDateChanged As EventHandler
    '''<summary>Occurs when the header height has changed.</summary>
    Public Event HeaderHeightChanged As EventHandler
    '''<summary>Occurs when the header visibility has changed.</summary>
    Public Event HeaderVisibleChanged As EventHandler
    '''<summary>Occurs when the item height has changed.</summary>
    Public Event ItemHeightChanged As EventHandler
    '''<summary>Occurs when the start date has changed.</summary>
    Public Event StartDateChanged As EventHandler

#End Region

#Region "Initializers"

    Public Sub New()
        MyBase.AutoScroll = True
        DoubleBuffered = True
    End Sub

#End Region

#Region "Properties"

    '''<summary>Gets a value indicating whether the container enables the user to scroll to any controls placed outside of its vidible boundaries.</summary>
    '''<returns><see langword="True"/> if the container enables auto-scrolling; otherwise, <see langword="False"/>.</returns>
    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    <EditorBrowsable(EditorBrowsableState.Never)>
    Public Overloads ReadOnly Property AutoScroll As Boolean
        Get
            Return MyBase.AutoScroll
        End Get
    End Property
    '''<summary>Gets the minimum size of the auto-scroll.</summary>
    '''<returns>A <see cref="System.Drawing.Size"/> that determines the minimum size of the virtual area through which the user can scroll.</returns>
    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    <EditorBrowsable(EditorBrowsableState.Never)>
    Public Overloads ReadOnly Property AutoScrollMinSize As Size
        Get
            Return MyBase.AutoScrollMinSize
        End Get
    End Property
    '''<summary>Gets the minimum height of the auto-scroll.</summary>
    '''<returns>An <see cref="Integer"/> that determines the minimum height of the virtual area through which the user can scroll.</returns>
    <Browsable(True)>
    <Category("Layout")>
    <DefaultValue(0)>
    <Description("The minimum logical height for the auto scroll region..")>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
    Public Property AutoScrollMinHeight As Integer
        Get
            Return MyBase.AutoScrollMinSize.Height
        End Get
        Set(value As Integer)
            MyBase.AutoScrollMinSize = New Size(MyBase.AutoScrollMinSize.Width, value)
        End Set
    End Property
    '''<summary>Represents the width of each date column.</summary>
    '''<returns>An <see cref="Integer"/> that represents the width of each date column.</returns>
    <Category("Appearance")>
    <DefaultValue(100)>
    <Description("The width of each date column.")>
    Public Property ColumnWidth As Integer
        Get
            Return _columnWidth
        End Get
        Set(value As Integer)
            If value = _columnWidth Then Return
            _columnWidth = value
            RefreshAutoScrollMinSize()
            Invalidate()
            OnColumnWidthChanged(EventArgs.Empty)
        End Set
    End Property
    '''<summary>The currently scrollable <see cref="KoerControls.DateRange"/> of the <see cref="DateScroller"/>.</summary>
    '''<returns>The currently scrollable <see cref="KoerControls.DateRange"/> of the <see cref="DateScroller"/>.</returns>
    <Category("Data")>
    <Description("The currently scrollable date range of the control.")>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
    <TypeConverter(GetType(DateRangeConverter))>
    Public Overridable Property DateRange As DateRange
        Get
            Return MyDateRange
        End Get
        Set(value As DateRange)
            If value = MyDateRange Then Return
            Dim bStartDateChanged As Boolean = value.StartDate <> MyDateRange.StartDate
            Dim bEndDateChanged As Boolean = value.EndDate <> MyDateRange.EndDate
            MyDateRange = value
            RefreshAutoScrollMinSize()
            If bStartDateChanged Then OnStartDateChanged(EventArgs.Empty)
            If bEndDateChanged Then OnEndDateChanged(EventArgs.Empty)
        End Set
    End Property
    Private Sub ResetDateRange()
        DateRange = DateRange.Empty
    End Sub
    Private Function ShouldSerializeDateRange() As Boolean
        Return MyDateRange <> DateRange.Empty
    End Function
    '''<summary>Represents the header overlay that displays the dates.</summary>
    '''<remarks>The header overlay that displays the dates.</remarks>
    <TypeConverter(GetType(ExpandableObjectConverter))>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)>
    Public ReadOnly Property HeaderOverlay As HeaderOverlay
        Get
            Return MyHeaderOverlay
        End Get
    End Property
    '''<summary>Gets or sets each <see cref="DateScrollerGroup"/> assigned to the <see cref="DateScroller"/>.</summary>
    '''<returns>Each <see cref="DateScrollerGroup"/> assigned to the <see cref="DateScroller"/>.</returns>
    <Category("Data")>
    <Description("The item groups used to sort the date scroller items.")>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)>
    Public ReadOnly Property ItemGroups As DateScrollerGroupCollection
        Get
            Return _itemGroups
        End Get
    End Property
    Private Sub ResetItemGroups()
        _itemGroups.Clear()
    End Sub
    Private Function ShouldSerializeItemGroups() As Boolean
        Return _itemGroups.Any
    End Function
    '''<summary>Gets or sets the height of each <see cref="DateScrollerItem"/>. A <see cref="DateScrollerItem"/> can cover multiple columns of height.</summary>
    '''<returns>An <see cref="Integer"/> that represents the height of each <see cref="DateScrollerItem"/> in pixels.</returns>
    <Category("Layout")>
    <DefaultValue(25)>
    <Description("The height of each DateScrollerItem. A DateScrollerItem can cover multiple columns of height.")>
    Public Property ItemHeight As Integer
        Get
            Return _itemHeight
        End Get
        Set(value As Integer)
            If value = _itemHeight Then Return
            _itemHeight = value
            OnItemHeightChanged(EventArgs.Empty)
        End Set
    End Property
    '''<summary>Determines the placement method of each <see cref="DateScrollerItem"/> in the <see cref="DateScroller"/>.</summary>
    '''<returns>One of the <see cref="DateScrollerItemPlacement"/> values.</returns>
    <Category("Layout")>
    <DefaultValue(DateScrollerItemPlacement.Waterfall)>
    <Description("The placement method of each DateScrollerItem.")>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
    Public Property ItemPlacement As DateScrollerItemPlacement
        Get
            Return _itemPlacement
        End Get
        Set(value As DateScrollerItemPlacement)
            If value = _itemPlacement Then Return
            _itemPlacement = value
            PlaceItems()
        End Set
    End Property
    '''<summary>Gets or sets padding within the control.</summary>
    '''<returns>A <see cref="Windows.Forms.Padding"/> representing the control's internal spacing characteristics.</returns>
    Public Overloads Property Padding As Padding 'Overloaded, as base ResetPadding does not trigger the 'PaddingChanged' event.
        Get
            Return MyBase.Padding
        End Get
        Set(value As Padding)
            MyBase.Padding = value
        End Set
    End Property
    Private Sub ResetPadding()
        MyBase.Padding = Padding.Empty
    End Sub
    '''<summary>Gets each <see cref="DateScrollerItem"/> contained in the <see cref="DateScroller"/>.</summary>
    '''<returns>Each <see cref="DateScrollerItem"/> contained in the <see cref="DateScroller"/>.</returns>
    <Category("Data")>
    <Description("The items contained in the DateScroller.")>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)>
    <Editor(GetType(DateScrollerItemCollectionEditor), GetType(UITypeEditor))>
    Public ReadOnly Property Items As New DateScrollerItemCollection(Me)
    Private Sub ResetItems()
        Items.Clear()
    End Sub
    Private Function ShouldSerializeItems() As Boolean
        Return Items.Any
    End Function

#End Region

#Region "Functions"

    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposing Then
            For Each dsiDateScrollerItem As DateScrollerItem In Items.ToArray
                dsiDateScrollerItem.Dispose()
            Next
            For Each dsgGroup As DateScrollerGroup In _itemGroups.ToArray
                dsgGroup.Dispose()
            Next
        End If
        MyBase.Dispose(disposing)
    End Sub
    '''<summary>Raises the <see cref="ColumnWidthChanged"/> event.</summary>
    '''<param name="e">The <see cref="EventArgs"/> that contains the event data.</param>
    Protected Overridable Sub OnColumnWidthChanged(e As EventArgs)
        RaiseEvent ColumnWidthChanged(Me, e)
    End Sub
    '''<summary>Raises the <see cref="EndDateDateChanged"/> event.</summary>
    '''<param name="e">The <see cref="EventArgs"/> that contains the event data.</param>
    Protected Overridable Sub OnEndDateChanged(e As EventArgs)
        RaiseEvent EndDateDateChanged(Me, e)
    End Sub
    '''<summary>Raises the <see cref="HeaderHeightChanged"/> event.</summary>
    '''<param name="e">The <see cref="EventArgs"/> that contains the event data.</param>
    Protected Overridable Sub OnHeaderHeightChanged(e As EventArgs)
        RaiseEvent HeaderHeightChanged(Me, e)
    End Sub
    '''<summary>Raises the <see cref="HeaderVisibleChanged"/> event.</summary>
    '''<param name="e">The <see cref="EventArgs"/> that contains the event data.</param>
    Protected Overridable Sub OnHeaderVisibleChanged(e As EventArgs)
        RaiseEvent HeaderVisibleChanged(Me, e)
    End Sub
    '''<summary>Raises the <see cref="ItemHeightChanged"/> event.</summary>
    '''<param name="e">The <see cref="EventArgs"/> that contains the event data.</param>
    Protected Overridable Sub OnItemHeightChanged(e As EventArgs)
        RaiseEvent ItemHeightChanged(Me, e)
    End Sub
    Protected Overrides Sub OnMarginChanged(e As EventArgs)
        MyBase.OnMarginChanged(e)
        AutoScrollMargin = New Size(Math.Max(Margin.Left, Margin.Right), Math.Max(Margin.Top, Margin.Bottom))
    End Sub
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)

        Dim sClientSize As Size = ClientSize
        Dim iClientWidth As Integer = sClientSize.Width
        Dim iClientHeight As Integer = sClientSize.Height
        Dim iCurrentXLocation As Integer = e.ClipRectangle.X.RoundToFloor(_columnWidth) - HorizontalScroll.Value - 1
        If BorderStyle = BorderStyle.FixedSingle Then iCurrentXLocation -= 1

        Using pPen As New Pen(SystemColors.ControlDarkDark, 1)
            Dim iHeaderYValue As Integer = Font.Height + MyBase.Padding.Vertical
            While iCurrentXLocation < iClientWidth - 1
                e.Graphics.DrawLine(pPen, iCurrentXLocation, 0, iCurrentXLocation, iClientHeight)
                iCurrentXLocation += _columnWidth
            End While
        End Using
    End Sub
    '''<summary>Raises the <see cref="StartDateChanged"/> event.</summary>
    '''<param name="e">The <see cref="EventArgs"/> that contains the event data.</param>
    Protected Overridable Sub OnStartDateChanged(e As EventArgs)
        RaiseEvent StartDateChanged(Me, e)
    End Sub
    Protected Overrides Sub OnStyleChanged(e As EventArgs)
        MyBase.OnStyleChanged(e)

        Dim iBorderWidth As Integer = Width - ClientSize.Width
        If iBorderWidth <> _borderWidth Then
            _borderWidth = iBorderWidth
            RefreshAutoScrollMinSize()
        End If
    End Sub
    '''<summary>Paints the header overlay onto the control.</summary>
    '''<param name="graphics">The <see cref="Graphics"/> used to paint the item.</param>
    '''<param name="clipRect">The <see cref="Rectangle"/> that represents the rectangle in which to paint.</param>
    Protected Overridable Sub PaintHeaderOverlay(graphics As Graphics, clipRect As Rectangle)
        Dim iClientWidth As Integer = ClientSize.Width
        Dim fHeaderFont As Font = HeaderOverlay.Font
        Dim pHeaderPadding As Padding = HeaderOverlay.Padding
        Dim iClientHeight As Integer = fHeaderFont.Height + pHeaderPadding.Vertical
        Dim iCurrentXLocation As Integer = -(HorizontalScroll.Value Mod _columnWidth) - 1
        If BorderStyle = BorderStyle.FixedSingle Then iCurrentXLocation -= 1
        Dim dCurrentDate As Date = MyDateRange.StartDate.AddDays(HorizontalScroll.Value / _columnWidth)

        Using pPen As New Pen(SystemColors.ControlDarkDark, 1)
            Using bForeColor As New SolidBrush(HeaderOverlay.ForeColor)
                graphics.DrawLine(pPen, 0, iClientHeight - 1, iClientWidth, iClientHeight - 1)
                While iCurrentXLocation < iClientWidth - 1
                    graphics.DrawLine(pPen, iCurrentXLocation, 0, iCurrentXLocation, iClientHeight)

                    Dim sDate As String = dCurrentDate.ToShortDateString
                    graphics.DrawString(sDate, fHeaderFont, bForeColor, iCurrentXLocation + ((_columnWidth - graphics.MeasureString(sDate, fHeaderFont).Width) / 2.0), pHeaderPadding.Top)

                    iCurrentXLocation += _columnWidth
                    dCurrentDate = dCurrentDate.AddDays(1)
                End While
            End Using
        End Using
    End Sub
    '''<summary>Places <see cref="DateScrollerItem"/> item in the <see cref="DateScroller"/> with the current <see cref="DateScrollerItemPlacement"/> method.</summary>
    Public Overridable Sub PlaceItems()
        Try
            SuspendLayout()
            Select Case _itemPlacement
                Case DateScrollerItemPlacement.Compact
                    PlaceItemsCompact(0, Items)

                Case DateScrollerItemPlacement.Waterfall
                    PlaceItemsWaterfall(0, Items)

                Case DateScrollerItemPlacement.GroupedCompact
                    Dim iStartColumnIndex As Integer = 0
                    For Each dsgGroup As DateScrollerGroup In ItemGroups
                        iStartColumnIndex = PlaceItemsCompact(iStartColumnIndex, Items.Where(Function(dsi) ReferenceEquals(dsi.ItemGroup, dsgGroup))) + 1
                    Next
                    PlaceItemsCompact(iStartColumnIndex, Items.Where(Function(dsi) dsi.ItemGroup Is Nothing))

                Case DateScrollerItemPlacement.GroupedWaterfall
                    Dim iStartColumnIndex As Integer = 0
                    For Each dsgGroup As DateScrollerGroup In ItemGroups
                        iStartColumnIndex = PlaceItemsWaterfall(iStartColumnIndex, Items.Where(Function(dsi) ReferenceEquals(dsi.ItemGroup, dsgGroup))) + 1
                    Next
                    PlaceItemsWaterfall(iStartColumnIndex, Items.Where(Function(dsi) dsi.ItemGroup Is Nothing))
            End Select
        Catch
            Throw
        Finally
            ResumeLayout(True)
        End Try
    End Sub
    '''<summary>Places each specified <see cref="DateScrollerItem"/> in the date scroller with the current <see cref="DateScrollerItemPlacement.Compact"/> method.</summary>
    '''<returns>The last column index placed at.</returns>
    '''<param name="startColumnIndex">The column index to begin placing items at.</param>
    '''<param name="items">Each <see cref="DateScrollerItem"/> to place.</param>
    Protected Overridable Function PlaceItemsCompact(startColumnIndex As Integer, items As IEnumerable(Of DateScrollerItem)) As Integer
        Dim dsiItemsToPlace As List(Of DateScrollerItem) = items.OrderBy(Function(dsi) dsi.DateRange).ToList 'Setup list of all items to place
        If Not dsiItemsToPlace.Any Then Return startColumnIndex 'No items found, return the start column index
        Dim dUnavailableDateIndices As New Dictionary(Of Date, List(Of Integer)) 'Setup 2-d array of dates and what indices in each date is used up
        Dim dsiItemPlacing As DateScrollerItem = dsiItemsToPlace.First 'Get the first item to place
        Dim drItemPlacingDateRange As DateRange = dsiItemPlacing.DateRange 'Get the date range of the item being placed
        Dim iEndColumnIndex As Integer = startColumnIndex 'State the last index the 

        For Each dDate As Date In DateRange.Union(items.Select(Function(dsi) dsi.DateRange)) 'Go through each date
            While dsiItemsToPlace.Any 'Continue checks while there are items toplace
                If Not drItemPlacingDateRange.Contains(dDate) Then Exit While 'Go to next date of item to place does not contain current date

                Dim iItemUnavailableIndices As New List(Of Integer) From {startColumnIndex - 1} 'Get all indices unavailable across the entire item's date range
                For Each dItemDate As Date In drItemPlacingDateRange 'Go through each date in the date range of the item placing
                    Dim iDateUnavailableIndices As List(Of Integer) = Nothing
                    If dUnavailableDateIndices.TryGetValue(dItemDate, iDateUnavailableIndices) Then 'Determine if date already has unavailable indices logged
                        For Each iDateUnavailableIndex As Integer In iDateUnavailableIndices 'Go through each unavailable index for the date
                            If Not iItemUnavailableIndices.Contains(iDateUnavailableIndex) Then iItemUnavailableIndices.Add(iDateUnavailableIndex) 'Add unavailable index for the item placing
                        Next
                    Else
                        dUnavailableDateIndices(dItemDate) = New List(Of Integer) 'Add date for unavailable index logging
                    End If
                Next

                Dim iItemColumnCount As Integer = dsiItemPlacing.ColumnCount 'Place item at first available index
                For i As Integer = 0 To iItemUnavailableIndices.Count - 1 'Go through each unavailable index
                    Dim iIndexChecking As Integer = iItemUnavailableIndices(i) 'Get the index checking
                    If iIndexChecking < startColumnIndex - 1 Then Continue For 'Go to next index if less than the index to start placing at
                    If i = iItemUnavailableIndices.Count - 1 OrElse iItemUnavailableIndices(i + 1) - iIndexChecking > iItemColumnCount Then 'Determine if item placing fits between the unavialable indix and the next
                        Dim iIndexPlacing As Integer = iIndexChecking + 1 'Get the index to place the item at
                        dsiItemPlacing.ColumnIndex = iIndexPlacing 'Set the index of the item
                        For Each dItemDate As Date In drItemPlacingDateRange 'Go through each date of the item placing
                            Dim iUnavailableDateIndicies As List(Of Integer) = dUnavailableDateIndices(dItemDate) 'Get the unavailable indeces logged for the date
                            Dim iItemEndColumnIndex As Integer = iIndexPlacing + iItemColumnCount - 1 'Get the end inex of the item placing
                            For iUnavailableIndex As Integer = iIndexPlacing To iItemEndColumnIndex 'Go through each index the item placing contains
                                iUnavailableDateIndicies.Add(iUnavailableIndex) 'Add unavailable index for the date
                            Next
                            iUnavailableDateIndicies.Sort() 'Sort the unavailable indices for the date
                            iEndColumnIndex = Math.Max(iEndColumnIndex, iItemEndColumnIndex) 'Get the new end column index
                        Next
                        Exit For 'Item placed, stop trying for next index to place at
                    End If
                Next

                dsiItemsToPlace.RemoveAt(0) 'Item placed, remove from list of items to place
                If Not dsiItemsToPlace.Any Then Return iEndColumnIndex 'If there are not more items to place, return the last column index taken by items
                dsiItemPlacing = dsiItemsToPlace.First 'Get the next item to place
                drItemPlacingDateRange = dsiItemPlacing.DateRange 'Get the date range of the item being placed
            End While
        Next

        Return iEndColumnIndex 'Return the last column index taken by items
    End Function
    '''<summary>Places each specified <see cref="DateScrollerItem"/> in the date scroller with the current <see cref="DateScrollerItemPlacement.Waterfall"/> method.</summary>
    '''<returns>The last column index placed at.</returns>
    '''<param name="startColumnIndex">The column index to begin placing items at.</param>
    '''<param name="items">Each <see cref="DateScrollerItem"/> to place.</param>
    Protected Overridable Function PlaceItemsWaterfall(startColumnIndex As Integer, items As IEnumerable(Of DateScrollerItem)) As Integer
        For Each dsiItem As DateScrollerItem In items.OrderBy(Function(dsi) dsi.DateRange) 'Go through each item ordered by dates
            dsiItem.ColumnIndex = startColumnIndex 'Set the column index of the item placing
            startColumnIndex += dsiItem.ColumnCount 'Add the items column count to the next item's column index
        Next
        Return startColumnIndex - 1 'Return the last column index taken by items
    End Function

#End Region

End Class
