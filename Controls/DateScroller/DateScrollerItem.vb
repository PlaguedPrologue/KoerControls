Imports System.ComponentModel
Imports System.ComponentModel.Design

'''<summary>Represents an item with a date range that is inside a DateScroller.</summary>
<DesignTimeVisible(False)>
<ToolboxItem(False)>
Public Class DateScrollerItem
    Inherits Panel
    Implements IDisposable

#Region "Private Items"

    Private WithEvents MyDateRange As DateRange = DateRange.Empty
    Private WithEvents MyDateScroller As DateScroller

    Private _columnCount As Integer = 1
    Private _columnIndex As Integer = -1
    Private _itemGroup As DateScrollerGroup
    Private Shared ReadOnly _typeDescriptorProvidersAdded As New HashSet(Of Type)

    Private Function GetDesignerType() As Type
        Dim host = TryCast(Site?.GetService(GetType(IDesignerHost)), IDesignerHost)
        If host Is Nothing OrElse host.RootComponent Is Nothing Then Return MyBase.GetType
        Dim typeName = host.RootComponent.Site?.Name
        If String.IsNullOrEmpty(typeName) Then Return host.RootComponent.GetType
        Dim asm = host.RootComponent.GetType.Assembly
        Return If(asm.GetTypes.FirstOrDefault(Function(t) t.Name = typeName), host.RootComponent.GetType)
    End Function
    Private Sub MyDateRange_EndDateChanged(sender As Object, e As EventArgs) Handles MyDateRange.EndDateChanged
        PlaceItem()
        OnDateRangeChanged(e)
    End Sub
    Private Sub MyDateRange_StartDateChanged(sender As Object, e As EventArgs) Handles MyDateRange.StartDateChanged
        PlaceItem()
        OnDateRangeChanged(e)
    End Sub
    Private Sub MyDateScroller_ColumnWidthChanged(sender As Object, e As EventArgs) Handles MyDateScroller.ColumnWidthChanged
        PlaceItem()
    End Sub
    Private Sub MyDateScroller_HeaderHeightChanged(sender As Object, e As EventArgs) Handles MyDateScroller.HeaderHeightChanged
        PlaceItem()
    End Sub
    Private Sub MyDateScroller_HeaderOverlayVisibleChanged(sender As Object, e As EventArgs) Handles MyDateScroller.HeaderVisibleChanged
        PlaceItem()
    End Sub
    Private Sub MyDateScroller_ItemHeightChanged(sender As Object, e As EventArgs) Handles MyDateScroller.ItemHeightChanged
        PlaceItem()
    End Sub
    Private Sub MyDateScroller_PaddingChanged(sender As Object, e As EventArgs) Handles MyDateScroller.PaddingChanged
        PlaceItem()
    End Sub
    Private Sub MyDateScroller_StartDateChanged(sender As Object, e As EventArgs) Handles MyDateScroller.StartDateChanged
        PlaceItem()
    End Sub
    Private Sub NotifyPropertyChanged(propertyName As String, oldValue As Object, newValue As Object)
        Dim ccsChangeService As IComponentChangeService = TryCast(Site?.GetService(GetType(IComponentChangeService)), IComponentChangeService)
        If ccsChangeService Is Nothing Then Return
        Dim pdDescriptor As PropertyDescriptor = TypeDescriptor.GetProperties(Me)(propertyName)
        If pdDescriptor Is Nothing Then Return
        ccsChangeService.OnComponentChanged(Me, pdDescriptor, oldValue, newValue)
        TypeDescriptor.Refresh(Me)
    End Sub

#End Region

#Region "Initializers"

    '''<summary>Creates a new instance of the <see cref="DateScrollerItem"/> class.</summary>
    Public Sub New()
        MyBase.New

        Dim tType As Type = GetDesignerType()
        If _typeDescriptorProvidersAdded.Add(tType) Then TypeDescriptor.AddProvider(New DateScrollerItemTypeDescriptorProvider, tType)

        MyBase.AutoSize = False
    End Sub
    '''<summary>Creates a new instance of the <see cref="DateScrollerItem"/> class.</summary>
    '''<param name="dateScroller">The <see cref="KoerControls.DateScroller"/> that will contain the <see cref="DateScrollerItem"/>.</param>
    '''<exception cref="ArgumentNullException">dateScroller is null.</exception>
    Public Sub New(dateScroller As DateScroller)
        MyBase.New
        If dateScroller Is Nothing Then Throw New ArgumentNullException(NameOf(dateScroller))

        Dim tType As Type = GetDesignerType()
        If _typeDescriptorProvidersAdded.Add(tType) Then TypeDescriptor.AddProvider(New DateScrollerItemTypeDescriptorProvider, tType)

        MyBase.AutoSize = False
        dateScroller.Container?.Add(Me)
        dateScroller.Controls.Add(Me)
        dateScroller.Items.Add(Me)
    End Sub
    '''<summary>Creates a new instance of the <see cref="DateScrollerItem"/> class.</summary>
    '''<param name="dateScroller">The <see cref="KoerControls.DateScroller"/> that will contain the <see cref="DateScrollerItem"/>.</param>
    '''<exception cref="ArgumentNullException">dateScroller -or- dateRange is null.</exception>
    Public Sub New(dateScroller As DateScroller, dateRange As DateRange)
        MyBase.New
        If dateScroller Is Nothing Then Throw New ArgumentNullException(NameOf(dateScroller))
        If dateRange Is Nothing Then Throw New ArgumentNullException(NameOf(dateRange))

        Dim tType As Type = GetDesignerType()
        If _typeDescriptorProvidersAdded.Add(tType) Then TypeDescriptor.AddProvider(New DateScrollerItemTypeDescriptorProvider, tType)

        MyBase.AutoSize = False
        dateScroller.Container?.Add(Me)
        dateScroller.Controls.Add(Me)
        dateScroller.Items.Add(Me)
        Me.DateRange = dateRange
    End Sub

    Protected Overrides Sub OnHandleCreated(e As EventArgs)
        MyBase.OnHandleCreated(e)
        PlaceItem()
    End Sub

#End Region

#Region "Events"

    '''<summary>Occurs when the column count has changed.</summary>
    Public Event ColumnCountChanged As EventHandler
    '''<summary>Occurs when the column index has changed</summary>
    Public Event ColumnIndexChanged As EventHandler
    '''<summary>Occurs when the date range has chagned.</summary>
    Public Event DateRangeChanged As EventHandler
    '''<summary>Occurs when the item group has changed.</summary>
    Public Event ItemGroupChanged As EventHandler

#End Region

#Region "Properties"

    '''<summary>Gets the edges of the container to which a control is bound and determines how a control is resized with its parent.</summary>
    '''<returns>A bitwise combination of the <see cref="AnchorStyles"/> values. The default is <see cref="AnchorStyles.Top"/> and <see cref="AnchorStyles.Left"/>.</returns>
    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Overloads ReadOnly Property Anchor As AnchorStyles
        Get
            Return MyBase.Anchor
        End Get
    End Property
    '''<summary>Gets a value that indicates whether the control resizes based on its contents.</summary>
    '''<returns><see langword="True"/> if the control automatically resizes based on its contents; otherwise, <see langword="False"/>.</returns>
    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Overloads ReadOnly Property AutoSize As Boolean
        Get
            Return MyBase.AutoSize
        End Get
    End Property
    '''<summary>Indicates the automatic sizing behavior of the control.</summary>
    '''<returns>One of the <see cref="Windows.Forms.AutoSizeMode"/> values.</returns>
    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Overloads ReadOnly Property AutoSizeMode As AutoSizeMode
        Get
            Return MyBase.AutoSizeMode
        End Get
    End Property
    '''<summary>Gets the size and location of the control including its nonclient elements, in pixels, relative to the parent control.</summary>
    '''<returns>A <see cref="Rectangle"/> in pixels relative to the parent control that represents the size and location of the control including its nonclient elements.</returns>
    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Overloads ReadOnly Property Bounds As Rectangle
        Get
            Return MyBase.Bounds
        End Get
    End Property
    '''<summary>Gets or sets the total number of columns the <see cref="DateScrollerItem"/> occupies in the <see cref="KoerControls.DateScroller"/>.</summary>
    '''<returns>The total number of columns the <see cref="DateScrollerItem"/> occupies in the <see cref="KoerControls.DateScroller"/>.</returns>
    '''<exception cref="ArgumentOutOfRangeException">The new value is less than or equal to 0</exception>
    <Category("Layout")>
    <DefaultValue(1)>
    <Description("The number of columns the DateScrollerItem occupies in its DateScroller.")>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
    Public Property ColumnCount As Integer
        Get
            Return _columnCount
        End Get
        Set(value As Integer)
            If value <= 0 Then Throw New ArgumentOutOfRangeException(NameOf(ColumnCount))
            If value = _columnCount Then Return
            _columnCount = value

            PlaceItem()
            MyDateScroller?.PlaceItems()
            OnColumnCountChanged(EventArgs.Empty)
        End Set
    End Property
    '''<summary>Gets the column index of the <see cref="DateScrollerItem"/> in the <see cref="KoerControls.DateScroller"/></summary>
    '''<returns>The column index of the <see cref="DateScrollerItem"/> in the <see cref="KoerControls.DateScroller"/>.</returns>
    <Category("Layout")>
    <DefaultValue(0)>
    <Description("The column index of the DateScrollerItem in its DateScroller.")>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
    Public Property ColumnIndex As Integer
        Get
            Return _columnIndex
        End Get
        Set(value As Integer)
            If value = _columnIndex Then Return
            _columnIndex = value

            PlaceItem()
            OnColumnIndexChanged(EventArgs.Empty)
        End Set
    End Property
    '''<summary>Gets or sets the <see cref="KoerControls.DateRange"/> the <see cref="DateScrollerItem"/> covers.</summary>
    '''<returns>The <see cref="KoerControls.DateRange"/> the <see cref="DateScrollerItem"/> covers.</returns>
    <Category("Data")>
    <Description("The date range the item covers.")>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
    <TypeConverter(GetType(DateRangeConverter))>
    Public Property DateRange As DateRange
        Get
            Return MyDateRange
        End Get
        Set(value As DateRange)
            If value = MyDateRange Then Return
            MyDateRange = value

            PlaceItem()
            MyDateScroller?.PlaceItems()
            OnDateRangeChanged(EventArgs.Empty)
        End Set
    End Property
    '''<summary>Gets the owning <see cref="KoerControls.DateScroller"/> of the <see cref="DateScrollerItem"/>.</summary>
    '''<returns>The owning <see cref="KoerControls.DateScroller"/> of the <see cref="DateScrollerItem"/>.</returns>
    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public ReadOnly Property DateScroller As DateScroller
        Get
            Return MyDateScroller
        End Get
    End Property
    '''<summary>Gets which control borders are docked to its parent control and determines how a control is resized with its parent.</summary>
    '''<returns>One of the <see cref="DockStyle"/> values.</returns>
    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Overloads ReadOnly Property Dock As DockStyle
        Get
            Return MyBase.Dock
        End Get
    End Property
    '''<summary>Gets the height of the control.</summary>
    '''<returns>The <see cref="Integer"/> that represents the height of the control in pixels.</returns>
    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Overloads ReadOnly Property Height As Integer
        Get
            Return MyBase.Height
        End Get
    End Property
    '''<summary>Gets the <see cref="DateScrollerGroup"/> that contains the <see cref="DateScrollerItem"/>.</summary>
    '''<returns>The <see cref="DateScrollerGroup"/> that contains the <see cref="DateScrollerItem"/>.</returns>
    <Category("Data")>
    <DefaultValue(GetType(DateScrollerGroup), Nothing)>
    <Description("The item group used to sort with other date scroller items.")>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
    <TypeConverter(GetType(DateScrollerGroupConverter))>
    Public Property ItemGroup As DateScrollerGroup
        Get
            Return _itemGroup
        End Get
        Set(value As DateScrollerGroup)
            If ReferenceEquals(value, _itemGroup) Then Return
            _itemGroup = value
            DateScroller?.PlaceItems()
            OnItemGroupChanged(EventArgs.Empty)
        End Set
    End Property
    '''<summary>Gets the distance, in pixels, between the left edge of the control and the left edge of its container's client area.</summary>
    '''<returns>An <see cref="Integer"/> representing the distance, in pixels, between the left edge of the control and the left edge of its container's client area.</returns>
    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Overloads ReadOnly Property Left As Integer
        Get
            Return MyBase.Left
        End Get
    End Property
    '''<summary>Gets the coordinates of the upper-left corner of the control relative to the upper-left corner of its container.</summary>
    '''<returns>The <see cref="Point"/> that represents the upper-left coner of the control relative to the upper-left corner of its container.</returns>
    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Overloads ReadOnly Property Location As Point
        Get
            Return MyBase.Location
        End Get
    End Property
    '''<summary>Gets the size that is the upper limit that <see cref="Control.GetPreferredSize(Size)"/> can specify.</summary>
    '''<returns>An ordered pair of <see cref="System.Drawing.Size"/> representing the width and height of a rectangle.</returns>
    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Overloads ReadOnly Property MaximumSize As Size
        Get
            Return MyBase.MaximumSize
        End Get
    End Property
    '''<summary>Gets the size that is the lower limit that <see cref="Control.GetPreferredSize(Size)"/> can specify.</summary>
    '''<returns>An ordered pair of <see cref="System.Drawing.Size"/> representing the width and height of a rectangle.</returns>
    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Overloads ReadOnly Property MinimumSize As Size
        Get
            Return MyBase.MinimumSize
        End Get
    End Property
    '''<summary>Gets the height and width of the control.</summary>
    '''<returns>The <see cref="System.Drawing.Size"/> that represents the height and width of the control in pixels.</returns>
    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Overloads ReadOnly Property Size As Size
        Get
            Return MyBase.Size
        End Get
    End Property
    '''<summary>Gets the distance, in pixels, between the top edge of the control and the top edge of its container's client area.</summary>
    '''<returns>An <see cref="Integer"/> representing the distance, in pixels, between the top edge of the control and the top edge of its container's client area.</returns>
    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Overloads ReadOnly Property Top As Integer
        Get
            Return MyBase.Top
        End Get
    End Property
    '''<summary>Gets the width of the control.</summary>
    '''<returns>The width of the control in pixels.</returns>
    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Overloads ReadOnly Property Width As Integer
        Get
            Return MyBase.Width
        End Get
    End Property

#End Region

#Region "Functions"

    Protected Overrides Sub OnParentChanged(e As EventArgs)
        MyBase.OnParentChanged(e)

        If ReferenceEquals(MyDateScroller, Parent) Then Return

        If MyDateScroller IsNot Nothing Then
            MyDateScroller.Items.Remove(Me)
            MyDateScroller.PlaceItems()
        End If
        MyDateScroller = TryCast(Parent, DateScroller)
        If MyDateScroller IsNot Nothing Then
            MyDateScroller.Items.Add(Me)
            Dim iOwnerColumnWidth As Integer = MyDateScroller.ColumnWidth
            Dim pOwnerPadding As Padding = MyDateScroller.Padding
            Dim iOwnerVerticalPadding As Integer = pOwnerPadding.Vertical
            Dim iOwnerItemHeight As Integer = MyDateScroller.ItemHeight
            MyBase.Bounds = New Rectangle(DateDiff(DateInterval.Day, MyDateScroller.DateRange.StartDate, MyDateRange.StartDate) * iOwnerColumnWidth + pOwnerPadding.Left - 1, MyDateScroller.HeaderOverlay.DisplayedHeight + pOwnerPadding.Top + (iOwnerItemHeight + iOwnerVerticalPadding) * _columnIndex, (MyDateRange.Length + 1) * iOwnerColumnWidth - pOwnerPadding.Horizontal - 1, iOwnerItemHeight * _columnCount + iOwnerVerticalPadding * (_columnCount - 1))
            MyDateScroller.PlaceItems()
        End If
    End Sub
    '''<summary>Raises the <see cref="ColumnCountChanged"/> event.</summary>
    '''<param name="e">The <see cref="EventArgs"/> that contains the event data.</param>
    Protected Overridable Sub OnColumnCountChanged(e As EventArgs)
        RaiseEvent ColumnCountChanged(Me, e)
    End Sub
    '''<summary>Raises the <see cref="ColumnIndexChanged"/> event.</summary>
    '''<param name="e">The <see cref="EventArgs"/> that contains the event data.</param>
    Protected Overridable Sub OnColumnIndexChanged(e As EventArgs)
        RaiseEvent ColumnCountChanged(Me, e)
    End Sub
    '''<summary>Raises the <see cref="DateRangeChanged"/> event.</summary>
    '''<param name="e">The <see cref="EventArgs"/> that contains the event data.</param>
    Protected Overridable Sub OnDateRangeChanged(e As EventArgs)
        RaiseEvent DateRangeChanged(Me, e)
    End Sub
    '''<summary>Raises the <see cref="ItemGroupChanged"/> event.</summary>
    '''<param name="e">The <see cref="EventArgs"/> that contains the event data.</param>
    Protected Overridable Sub OnItemGroupChanged(e As EventArgs)
        RaiseEvent ItemGroupChanged(Me, e)
    End Sub
    '''<summary>Places the item based on its index, column count, date range, and its parent DateScroller's properties.</summary>
    Protected Overridable Sub PlaceItem()
        If MyDateScroller Is Nothing Then Return

        Dim iOwnerColumnWidth As Integer = MyDateScroller.ColumnWidth
        Dim pOwnerPadding As Padding = MyDateScroller.Padding
        Dim iOwnerVerticalPadding As Integer = pOwnerPadding.Vertical
        Dim iOwnerItemHeight As Integer = MyDateScroller.ItemHeight
        MyBase.Bounds = New Rectangle(DateDiff(DateInterval.Day, MyDateScroller.DateRange.StartDate, MyDateRange.StartDate) * iOwnerColumnWidth + pOwnerPadding.Left - 1, MyDateScroller.HeaderOverlay.DisplayedHeight + pOwnerPadding.Top + (iOwnerItemHeight + iOwnerVerticalPadding) * _columnIndex, (MyDateRange.Length + 1) * iOwnerColumnWidth - pOwnerPadding.Horizontal - 1, iOwnerItemHeight * _columnCount + iOwnerVerticalPadding * (_columnCount - 1))
    End Sub

#End Region

End Class
