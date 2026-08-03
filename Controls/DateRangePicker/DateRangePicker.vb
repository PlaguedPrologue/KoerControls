Imports System.ComponentModel
Imports System.Globalization

'''<summary>Represents a <see cref="Windows"/> control that allows the user to select a date and a time range and to display the date and time range with a specified format.</summary>
<DefaultEvent("ValueChanged")>
<DefaultProperty("Value")>
<Designer(GetType(DateRangePickerDesigner))>
Public Class DateRangePicker
    Inherits Control

#Region "Private Items"

    Private WithEvents MyEndDatePicker As New DateTimePicker With {.Dock = DockStyle.Right}
    Private WithEvents MyStartDatePicker As New DateTimePicker With {.Dock = DockStyle.Left}
    Private WithEvents MyValue As DateRange = DateRange.Empty

    Private _busyChangingValue As Boolean = False
    Private _calendarFont As Font = DefaultFont
    Private _calendarForeColor As Color = SystemColors.ControlText
    Private _calendarMonthBackground As Color = SystemColors.Window
    Private _calendarTitleBackColor As Color = SystemColors.ActiveCaption
    Private _calendarTitleForeColor As Color = SystemColors.ActiveCaptionText
    Private _calendarTrailingForeColor As Color = SystemColors.GrayText
    Private _customFormat As String
    Private _dropDownAlign As LeftRightRelativeAlignment = LeftRightRelativeAlignment.Left
    Private _foreColor As Color = DefaultForeColor
    Private _format As DateTimePickerFormat = DateTimePickerFormat.Long
    Private _maxDate As Date = DateTimePicker.MaximumDateTime
    Private _minDate As Date = DateTimePicker.MinimumDateTime
    Private _rightToLeftLayout As Boolean = False
    Private _showUpDown As Boolean = False

    Private Sub MyEndDatePicker_SizeChanged(sender As Object, e As EventArgs) Handles MyEndDatePicker.SizeChanged
        If IsHandleCreated Then Height = Math.Max(MyEndDatePicker.Size.Height, MyStartDatePicker.Size.Height)
    End Sub
    Private Sub MyEndDatePicker_ValueChanged(sender As Object, e As EventArgs) Handles MyEndDatePicker.ValueChanged
        If _busyChangingValue Then Return
        _busyChangingValue = True
        Dim dValue1 As Date = MyStartDatePicker.Value
        Dim dValue2 As Date = MyEndDatePicker.Value
        If dValue1 > dValue2 Then
            MyValue.EndDate = dValue2
            MyStartDatePicker.Value = dValue2
            MyEndDatePicker.Value = dValue1
            If MyEndDatePicker.Focused Then MyStartDatePicker.Focus()
        Else
            MyValue.EndDate = dValue2
        End If
        OnValueChanged(EventArgs.Empty)
        _busyChangingValue = False
    End Sub
    Private Sub MyStartDatePicker_SizeChanged(sender As Object, e As EventArgs) Handles MyStartDatePicker.SizeChanged
        If IsHandleCreated Then Height = Math.Max(MyEndDatePicker.Size.Height, MyStartDatePicker.Size.Height)
    End Sub
    Private Sub MyStartDatePicker_ValueChanged(sender As Object, e As EventArgs) Handles MyStartDatePicker.ValueChanged
        If _busyChangingValue Then Return
        _busyChangingValue = True
        Dim dValue1 As Date = MyStartDatePicker.Value
        Dim dValue2 As Date = MyEndDatePicker.Value
        If dValue1 > dValue2 Then
            MyValue.StartDate = dValue2
            MyStartDatePicker.Value = dValue2
            MyEndDatePicker.Value = dValue1
            If MyStartDatePicker.Focused Then MyEndDatePicker.Focus()
        Else
            MyValue.StartDate = dValue1
        End If
        OnValueChanged(EventArgs.Empty)
        _busyChangingValue = False
    End Sub
    Private Sub MyValue_EndDateChanged(sender As Object, e As EventArgs) Handles MyValue.EndDateChanged
        If _busyChangingValue Then Return
        _busyChangingValue = True
        MyEndDatePicker.Value = MyValue.EndDate
        OnValueChanged(EventArgs.Empty)
        _busyChangingValue = False
    End Sub
    Private Sub MyValue_StartDateChanged(sender As Object, e As EventArgs) Handles MyValue.StartDateChanged
        If _busyChangingValue Then Return
        _busyChangingValue = True
        MyStartDatePicker.Value = MyValue.StartDate
        OnValueChanged(EventArgs.Empty)
        _busyChangingValue = False
    End Sub

#End Region

#Region "Events"

    ''' <summary>Occurs when the value has changed.</summary>
    Public Event ValueChanged As EventHandler

#End Region

#Region "Properties"

    <Browsable(False)>
    <EditorBrowsable(EditorBrowsableState.Never)>
    Public Overrides Property BackColor As Color
        Get
            Return MyBase.BackColor
        End Get
        Set(value As Color)
            If value = MyBase.BackColor Then Return
            MyBase.BackColor = value
            MyEndDatePicker.BackColor = value
            MyStartDatePicker.CalendarMonthBackground = value
        End Set
    End Property
    <Browsable(False)>
    <DebuggerBrowsable(DebuggerBrowsableState.Never)>
    Public Overrides Property BackgroundImage As Image
        Get
            Return MyBase.BackgroundImage
        End Get
        Set(value As Image)
            If ReferenceEquals(value, MyBase.BackgroundImage) Then Return
            MyBase.BackgroundImage = value
            MyEndDatePicker.BackgroundImage = value
            MyStartDatePicker.BackgroundImage = value
        End Set
    End Property
    <Browsable(False)>
    <EditorBrowsable(EditorBrowsableState.Never)>
    Public Overrides Property BackgroundImageLayout As ImageLayout
        Get
            Return MyBase.BackgroundImageLayout
        End Get
        Set(value As ImageLayout)
            If value = MyBase.BackgroundImageLayout Then Return
            MyBase.BackgroundImageLayout = value
            MyEndDatePicker.BackgroundImageLayout = value
            MyStartDatePicker.BackgroundImageLayout = value
        End Set
    End Property
    '''<summary>Gets or sets the font style applied to each calendar.</summary>
    '''<returns>A <see cref="Font"/> that represents the font style applied to each calendar.</returns>
    <Category("Appearance")>
    <Description("The font used to display each calendar.")>
    Public Property CalendarFont As Font
        Get
            Return _calendarFont
        End Get
        Set(value As Font)
            If value.Equals(_calendarFont) Then Return
            _calendarFont = value
            MyEndDatePicker.CalendarFont = value
            MyStartDatePicker.CalendarFont = value
        End Set
    End Property
    Private Sub ResetCalendarFont()
        If Parent Is Nothing Then
            CalendarFont = DefaultFont
        Else
            CalendarFont = Parent.Font
        End If
    End Sub
    Private Function ShouldSerializeCalendarFont() As Boolean
        If Parent Is Nothing Then
            Return Not _calendarFont.Equals(DefaultFont)
        Else
            Return Not _calendarFont.Equals(Parent.Font)
        End If
    End Function
    '''<summary>Gets or sets the foreground color of each calendar.</summary>
    '''<returns>A <see cref="Color"/> that represents the foreground color of the calendar.</returns>
    '''<exception cref="ArgumentException">The value assigned is null.</exception>
    <Category("Appearance")>
    <DefaultValue(GetType(Color), "ControlText")>
    <Description("The color used to display text within a month.")>
    Public Property CalendarForeColor As Color
        Get
            Return _calendarForeColor
        End Get
        Set(value As Color)
            If value = _calendarForeColor Then Return
            _calendarForeColor = value
            MyEndDatePicker.CalendarForeColor = value
            MyStartDatePicker.CalendarForeColor = value
        End Set
    End Property
    '''<summary>Gets or sets the background color of the calendar month.</summary>
    '''<returns>A System.Drawing.Color that represents the background color of the calendar month.</returns>
    '''<exception cref="ArgumentException">The value assigned is null.</exception>
    <Category("Appearance")>
    <DefaultValue(GetType(Color), "Window")>
    <Description("The background color displayed within the month.")>
    Public Property CalendarMonthBackground As Color
        Get
            Return _calendarMonthBackground
        End Get
        Set(value As Color)
            If value = _calendarMonthBackground Then Return
            _calendarMonthBackground = value
            MyEndDatePicker.CalendarMonthBackground = value
            MyStartDatePicker.CalendarMonthBackground = value
        End Set
    End Property
    '''<summary>Gets or sets the background color of each calendar title.</summary>
    '''<returns>A <see cref="Color"/> that represents the background color of the calendar title.</returns>
    '''<exception cref="ArgumentException">The value assigned is null.</exception>
    <Category("Appearance")>
    <DefaultValue(GetType(Color), "ActiveCaption")>
    <Description("The background color displayed in each calendar's title.")>
    Public Property CalendarTitleBackColor As Color
        Get
            Return _calendarTitleBackColor
        End Get
        Set(value As Color)
            If value = _calendarTitleBackColor Then Return
            _calendarTitleBackColor = value
            MyEndDatePicker.CalendarTitleBackColor = value
            MyStartDatePicker.CalendarTitleBackColor = value
        End Set
    End Property
    '''<summary>Gets or sets the foreground color of each calendar title.</summary>
    '''<returns>A <see cref="Color"/> that represents the foreground color of the calendar title.</returns>
    '''<exception cref="ArgumentException">The value assigned is null.</exception>
    <Category("Appearance")>
    <DefaultValue(GetType(Color), "ActiveCaptionText")>
    <Description("The color uesd to display text within each calendar's title.")>
    Public Property CalendarTitleForeColor As Color
        Get
            Return _calendarTitleForeColor
        End Get
        Set(value As Color)
            If value = _calendarTitleForeColor Then Return
            _calendarTitleForeColor = value
            MyEndDatePicker.CalendarTitleForeColor = value
            MyStartDatePicker.CalendarTitleForeColor = value
        End Set
    End Property
    '''<summary>Gets or sets the foreground color of each calendar trailing dates.</summary>
    '''<returns>A System.Drawing.Color that represents the foreground color of each calendar trailing dates.</returns>
    '''<exception cref="ArgumentException">The value assigned is null.</exception>
    <Category("Appearance")>
    <DefaultValue(GetType(Color), "GrayText")>
    <Description("The color used to display header day and trailing day text. Header ant drailing days are the days from the previous and following months that appear on the current month calendar.")>
    Public Property CalendarTrailingForeColor As Color
        Get
            Return _calendarTrailingForeColor
        End Get
        Set(value As Color)
            If value = _calendarTrailingForeColor Then Return
            _calendarTrailingForeColor = value
            MyEndDatePicker.CalendarTrailingForeColor = value
            MyStartDatePicker.CalendarTrailingForeColor = value
        End Set
    End Property
    '''<summary>Gets or sets the custom date/time format string.</summary>
    '''<returns>A string that represents the custom date/time format. The default is null.</returns>
    <Category("Behavior")>
    <DefaultValue("")>
    <Description("The custom format string used to format the date and/or times displayed in the control.")>
    Public Property CustomFormat As String
        Get
            Return _customFormat
        End Get
        Set(value As String)
            If value = _customFormat Then Return
            _customFormat = value
            MyEndDatePicker.CustomFormat = value
            MyStartDatePicker.CustomFormat = value
        End Set
    End Property
    '''<summary>Gets or sets the alignment of the drop-down calendar on each <see cref="DateTimePicker"/> control.</summary>
    '''<returns>The alignment of the drop-down calendar on the control. The default is <see cref="LeftRightRelativeAlignment.Left"/></returns>
    '''<exception cref="InvalidEnumArgumentException">The value assigned is not one of the <see cref="LeftRightRelativeAlignment"/> values.</exception>
    <Category("Appearance")>
    <DefaultValue(LeftRightRelativeAlignment.Left)>
    <Description("Controls how the month drop-down is aligned for each DateTimePicker in the DateRangePicker. Can be LEFT, RIGHT, INNER or OUTER.")>
    Public Property DropDownAlign As LeftRightRelativeAlignment
        Get
            Return _dropDownAlign
        End Get
        Set(value As LeftRightRelativeAlignment)
            If value = _dropDownAlign Then Return
            _dropDownAlign = value
            Select Case value
                Case LeftRightRelativeAlignment.Left
                    MyEndDatePicker.DropDownAlign = LeftRightAlignment.Left
                    MyStartDatePicker.DropDownAlign = LeftRightAlignment.Left
                Case LeftRightRelativeAlignment.Right
                    MyEndDatePicker.DropDownAlign = LeftRightAlignment.Right
                    MyStartDatePicker.DropDownAlign = LeftRightAlignment.Right
                Case LeftRightRelativeAlignment.Outer
                    MyEndDatePicker.DropDownAlign = LeftRightAlignment.Right
                    MyStartDatePicker.DropDownAlign = LeftRightAlignment.Left
                Case LeftRightRelativeAlignment.Inner
                    MyEndDatePicker.DropDownAlign = LeftRightAlignment.Left
                    MyStartDatePicker.DropDownAlign = LeftRightAlignment.Right
            End Select
        End Set
    End Property
    '''<summary>Gets or sets the foreground color of each <see cref="DateTimePicker"/> control.</summary>
    '''<returns>The foreground <see cref="Color"/> of each <see cref="DateTimePicker"/>.</returns>
    <Browsable(False)>
    <EditorBrowsable(EditorBrowsableState.Never)>
    Public Overrides Property ForeColor As Color
        Get
            Return _foreColor
        End Get
        Set(value As Color)
            If value = _foreColor Then Return
            _foreColor = value
            MyEndDatePicker.ForeColor = value
            MyStartDatePicker.ForeColor = value
        End Set
    End Property
    '''<summary>Gets or sets the format of the date and time displayed in the control.</summary>
    '''<returns>One of the <see cref="DateTimePickerFormat"/> values. The default is <see cref="DateTimePickerFormat.Long"/>.</returns>
    '''<exception cref="InvalidEnumArgumentException">The value assigned is not one of the <see cref="DateTimePickerFormat"/> values.</exception>
    <Category("Appearance")>
    <DefaultValue(DateTimePickerFormat.Long)>
    <Description("Determines whether dates and times are displayed using standard or custom formatting.")>
    Public Property Format As DateTimePickerFormat
        Get
            Return _format
        End Get
        Set(value As DateTimePickerFormat)
            If value = _format Then Return
            _format = value
            MyEndDatePicker.Format = value
            MyStartDatePicker.Format = value
        End Set
    End Property
    '''<summary>Gets or sets the maximum date and time that can be selected in the control.</summary>
    '''<returns>The maximum date and time that can be selected in the control. The default is determined as the minimum of the CurrentCulture’s Calendar’s <see cref="Calendar.MaxSupportedDateTime"/> property and December 31st 9998 12 am.</returns>
    '''<exception cref="ArgumentException">The value assigned is less than the <see cref="MinDate"/> value.</exception>
    '''<exception cref="SystemException">The value assigned is greater than the <see cref="DateTimePicker.MaxDateTime"/> value.</exception>
    <Category("Behavior")>
    <Description("The maximum date that can be selected.")>
    Public Property MaxDate As Date
        Get
            Return _maxDate
        End Get
        Set(value As Date)
            If value = _maxDate Then Return
            _maxDate = value
            MyEndDatePicker.MaxDate = value
            MyStartDatePicker.MaxDate = value
        End Set
    End Property
    Private Sub ResetMaxDate()
        MaxDate = DateTimePicker.MaximumDateTime
    End Sub
    Private Function ShouldSerializeMaxDate() As Boolean
        Return _maxDate <> DateTimePicker.MaximumDateTime
    End Function
    '''<summary>Gets or sets the minimum date and time that can be selected in the control.</summary>
    '''<returns>The minimum date and time that can be selected in the control. The default is 1/1/1753 00:00:00.</returns>
    '''<exception cref="ArgumentException">The value assigned is greater than the <see cref="MaxDate"/> value.</exception>
    '''<exception cref="SystemException">The value assigned is less than the <see cref="DateTimePicker.MinDate"/> value.</exception>
    <Category("Behavior")>
    <Description("The minimum date that can be selected.")>
    Public Property MinDate As Date
        Get
            Return _minDate
        End Get
        Set(value As Date)
            If value = _minDate Then Return
            _minDate = value
            MyEndDatePicker.MinDate = value
            MyStartDatePicker.MinDate = value
        End Set
    End Property
    Private Sub ResetMinDate()
        MinDate = DateTimePicker.MinimumDateTime
    End Sub
    Private Function ShouldSerializeMinDate() As Boolean
        Return _minDate <> DateTimePicker.MinimumDateTime
    End Function
    '''<summary>Gets the preferred height of each <see cref="DateTimePicker"/> control.</summary>
    '''<returns>The preferred height, in pixels, of each <see cref="DateTimePicker"/> control.</returns>
    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public ReadOnly Property PreferredHeight As Integer
        Get
            If MyEndDatePicker Is Nothing Then Return 0
            Return MyEndDatePicker.PreferredHeight
        End Get
    End Property
    '''<summary>Gets or sets whether the contents of each <see cref="DateTimePicker"/> are laid out from right to left.</summary>
    '''<returns><see langword="True"/> if the layout of each <see cref="DateTimePicker"/> contents is from right to left; otherwise, <see langword="False"/>. The default is <see langword="False"/>.</returns>
    <Category("Appearance")>
    <DefaultValue(False)>
    <Description("Indicates whether the control layout is right-to-left when the RightToLeft property is set to Yes.")>
    Public Overridable Property RightToLeftLayout As Boolean
        Get
            Return _rightToLeftLayout
        End Get
        Set(value As Boolean)
            If value = _rightToLeftLayout Then Return
            _rightToLeftLayout = value
            MyEndDatePicker.RightToLeftLayout = value
            MyStartDatePicker.RightToLeftLayout = value
        End Set
    End Property
    '''<summary>Gets or sets a value indicating whether a spin button control (also known as an up-down control) is used to adjust the date/time value.</summary>
    '''<returns><see langword="True"/> if a spin button control is used to adjust the date/time value; otherwise, <see langword="False"/>. The default is <see langword="False"/>.</returns>
    <Category("Appearance")>
    <DefaultValue(False)>
    <Description("Indicates whether a spin box rather than a drop-down calendar is displayed for modifying the control value.")>
    Public Property ShowUpDown As Boolean
        Get
            Return _showUpDown
        End Get
        Set(value As Boolean)
            If value = _showUpDown Then Return
            _showUpDown = value
            MyEndDatePicker.ShowUpDown = value
            MyStartDatePicker.ShowUpDown = value
        End Set
    End Property
    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    <EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Overrides Property Text As String
        Get
            Return Value.ToShortDateFormat
        End Get
        Set(value As String)
            Dim drValue As DateRange = DateRange.Parse(value)
            MyValue = drValue
            MyEndDatePicker.Value = drValue.EndDate
            MyStartDatePicker.Value = drValue.StartDate
        End Set
    End Property
    '''<summary>Gets or sets the date/time values assigned to the control.</summary>
    '''<returns>The <see cref="DateRange"/> value assign to the control.</returns>
    '''<exception cref="ArgumentOutOfRangeException">The set value is less than <see cref="DateRangePicker.MinDate"/> or more than <see cref="DateRangePicker.MaxDate"/>.</exception>
    <Category("Behavior")>
    <Description("The current date/time values for this control.")>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)>
    Public Property Value As DateRange
        Get
            Return MyValue
        End Get
        Set(value As DateRange)
            If value = MyValue Then Return
            MyValue = value
            If Not _busyChangingValue Then
                _busyChangingValue = True
                MyEndDatePicker.Value = value.EndDate
                MyStartDatePicker.Value = value.StartDate
                _busyChangingValue = False
            End If
            OnValueChanged(EventArgs.Empty)
        End Set
    End Property
    Private Sub ResetValue()
        Value = DateRange.Empty
    End Sub
    Private Function ShouldSerializeValue() As Boolean
        Return MyValue <> Today
    End Function

#End Region

#Region "Functions"

    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle()
        Controls.Add(MyEndDatePicker)
        Controls.Add(MyStartDatePicker)
    End Sub
    Protected Overrides Sub OnSizeChanged(e As EventArgs)
        MyBase.OnSizeChanged(e)
        MyEndDatePicker.Width = Math.Floor(Width / 2)
        MyStartDatePicker.Width = Math.Ceiling(Width / 2)
    End Sub
    '''<summary>Raises the <see cref="ValueChanged"/> event.</summary>
    '''<param name="e">The <see cref="EventArgs"/> containing the event data.</param>
    Protected Overridable Sub OnValueChanged(e As EventArgs)
        RaiseEvent ValueChanged(Me, e)
    End Sub

#End Region

End Class
