Imports System.ComponentModel
Imports System.Globalization

'''<summary>Hosts a collection of <see cref="DataGridViewDateRangePickerCell"/> objects.</summary>
Public Class DataGridViewDateRangePickerColumn
    Inherits DataGridViewColumn

#Region "Private Items"

    Private _calendarFont As Font = Control.DefaultFont
    Private _calendarForeColor As Color = SystemColors.ControlText
    Private _calendarMonthBackground As Color = SystemColors.Window
    Private _calendarTitleBackColor As Color = SystemColors.ActiveCaption
    Private _calendarTitleForeColor As Color = SystemColors.ActiveCaptionText
    Private _calendarTrailingForeColor As Color = SystemColors.GrayText
    Private _customFormat As String
    Private _dropDownAlign As LeftRightRelativeAlignment = LeftRightRelativeAlignment.Left
    Private _foreColor As Color = Control.DefaultForeColor
    Private _format As DateTimePickerFormat = DateTimePickerFormat.Long
    Private _maxDate As Date = DateTimePicker.MaximumDateTime
    Private _minDate As Date = DateTimePicker.MinimumDateTime
    Private _rightToLeftLayout As Boolean = False
    Private _showUpDown As Boolean = False

#End Region

#Region "Initializers"

    '''<summary>Initializes a new instance of the <see cref="DataGridViewDateRangePickerColumn"/> class using an existing <see cref="DataGridViewDateRangePickerCell"/> as a template.</summary>
    Public Sub New()
        MyBase.New(New DataGridViewDateRangePickerCell)
        ValueType = GetType(DateRange)
    End Sub

#End Region

#Region "Properties"

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
            Dim dgvdrpcTemplate As DataGridViewDateRangePickerCell = TryCast(CellTemplate, DataGridViewDateRangePickerCell)
            If dgvdrpcTemplate IsNot Nothing Then dgvdrpcTemplate.CalendarFont = value
            If DataGridView Is Nothing Then Return
            For Each dgvrRow As DataGridViewRow In DataGridView.Rows
                Dim dgvdrpcCell As DataGridViewDateRangePickerCell = TryCast(dgvrRow.Cells(Index), DataGridViewDateRangePickerCell)
                If dgvdrpcCell IsNot Nothing Then dgvdrpcCell.CalendarFont = value
            Next
        End Set
    End Property
    Private Sub ResetCalendarFont()
        If DataGridView Is Nothing Then
            CalendarFont = Control.DefaultFont
        Else
            CalendarFont = DataGridView.Font
        End If
    End Sub
    Private Function ShouldSerializeCalendarFont() As Boolean
        If DataGridView Is Nothing Then
            Return Not _calendarFont.Equals(Control.DefaultFont)
        Else
            Return Not _calendarFont.Equals(DataGridView.Font)
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
            Dim dgvdrpcTemplate As DataGridViewDateRangePickerCell = TryCast(CellTemplate, DataGridViewDateRangePickerCell)
            If dgvdrpcTemplate IsNot Nothing Then dgvdrpcTemplate.CalendarForeColor = value
            If DataGridView Is Nothing Then Return
            For Each dgvrRow As DataGridViewRow In DataGridView.Rows
                Dim dgvdrpcCell As DataGridViewDateRangePickerCell = TryCast(dgvrRow.Cells(Index), DataGridViewDateRangePickerCell)
                If dgvdrpcCell IsNot Nothing Then dgvdrpcCell.CalendarForeColor = value
            Next
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
            Dim dgvdrpcTemplate As DataGridViewDateRangePickerCell = TryCast(CellTemplate, DataGridViewDateRangePickerCell)
            If dgvdrpcTemplate IsNot Nothing Then dgvdrpcTemplate.CalendarMonthBackground = value
            If DataGridView Is Nothing Then Return
            For Each dgvrRow As DataGridViewRow In DataGridView.Rows
                Dim dgvdrpcCell As DataGridViewDateRangePickerCell = TryCast(dgvrRow.Cells(Index), DataGridViewDateRangePickerCell)
                If dgvdrpcCell IsNot Nothing Then dgvdrpcCell.CalendarMonthBackground = value
            Next
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
            Dim dgvdrpcTemplate As DataGridViewDateRangePickerCell = TryCast(CellTemplate, DataGridViewDateRangePickerCell)
            If dgvdrpcTemplate IsNot Nothing Then dgvdrpcTemplate.CalendarTitleBackColor = value
            If DataGridView Is Nothing Then Return
            For Each dgvrRow As DataGridViewRow In DataGridView.Rows
                Dim dgvdrpcCell As DataGridViewDateRangePickerCell = TryCast(dgvrRow.Cells(Index), DataGridViewDateRangePickerCell)
                If dgvdrpcCell IsNot Nothing Then dgvdrpcCell.CalendarTitleBackColor = value
            Next
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
            Dim dgvdrpcTemplate As DataGridViewDateRangePickerCell = TryCast(CellTemplate, DataGridViewDateRangePickerCell)
            If dgvdrpcTemplate IsNot Nothing Then dgvdrpcTemplate.CalendarTitleForeColor = value
            If DataGridView Is Nothing Then Return
            For Each dgvrRow As DataGridViewRow In DataGridView.Rows
                Dim dgvdrpcCell As DataGridViewDateRangePickerCell = TryCast(dgvrRow.Cells(Index), DataGridViewDateRangePickerCell)
                If dgvdrpcCell IsNot Nothing Then dgvdrpcCell.CalendarTitleForeColor = value
            Next
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
            Dim dgvdrpcTemplate As DataGridViewDateRangePickerCell = TryCast(CellTemplate, DataGridViewDateRangePickerCell)
            If dgvdrpcTemplate IsNot Nothing Then dgvdrpcTemplate.CalendarTrailingForeColor = value
            If DataGridView Is Nothing Then Return
            For Each dgvrRow As DataGridViewRow In DataGridView.Rows
                Dim dgvdrpcCell As DataGridViewDateRangePickerCell = TryCast(dgvrRow.Cells(Index), DataGridViewDateRangePickerCell)
                If dgvdrpcCell IsNot Nothing Then dgvdrpcCell.CalendarTrailingForeColor = value
            Next
        End Set
    End Property
    '''<summary>Gets or sets the custom format of the date and time displayed in the control.</summary>
    '''<returns>The custom format of the date and time displayed int eh control.</returns>
    <Category("Appearance")>
    <DefaultValue("")>
    <Description("The custom format of the date and time displayed in the control.")>
    Public Overridable Property CustomFormat As String
        Get
            Return _customFormat
        End Get
        Set(value As String)
            If value = _customFormat Then Return
            _customFormat = value
            Dim dgvdrpcTemplate As DataGridViewDateRangePickerCell = TryCast(CellTemplate, DataGridViewDateRangePickerCell)
            If dgvdrpcTemplate IsNot Nothing Then dgvdrpcTemplate.CustomFormat = value
            If DataGridView Is Nothing Then Return
            For Each dgvrRow As DataGridViewRow In DataGridView.Rows
                Dim dgvdrpcCell As DataGridViewDateRangePickerCell = TryCast(dgvrRow.Cells(Index), DataGridViewDateRangePickerCell)
                If dgvdrpcCell IsNot Nothing Then dgvdrpcCell.CustomFormat = value
            Next
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
            Dim dgvdrpcTemplate As DataGridViewDateRangePickerCell = TryCast(CellTemplate, DataGridViewDateRangePickerCell)
            If dgvdrpcTemplate IsNot Nothing Then dgvdrpcTemplate.DropDownAlign = value
            If DataGridView Is Nothing Then Return
            For Each dgvrRow As DataGridViewRow In DataGridView.Rows
                Dim dgvdrpcCell As DataGridViewDateRangePickerCell = TryCast(dgvrRow.Cells(Index), DataGridViewDateRangePickerCell)
                If dgvdrpcCell IsNot Nothing Then dgvdrpcCell.DropDownAlign = value
            Next
        End Set
    End Property
    '''<summary>Gets or sets the foreground color of each <see cref="DateTimePicker"/> control.</summary>
    '''<returns>The foreground <see cref="Color"/> of each <see cref="DateTimePicker"/>.</returns>
    <Browsable(False)>
    <EditorBrowsable(EditorBrowsableState.Never)>
    Public Overridable Property ForeColor As Color
        Get
            Return _foreColor
        End Get
        Set(value As Color)
            If value = _foreColor Then Return
            _foreColor = value
            Dim dgvdrpcTemplate As DataGridViewDateRangePickerCell = TryCast(CellTemplate, DataGridViewDateRangePickerCell)
            If dgvdrpcTemplate IsNot Nothing Then dgvdrpcTemplate.ForeColor = value
            If DataGridView Is Nothing Then Return
            For Each dgvrRow As DataGridViewRow In DataGridView.Rows
                Dim dgvdrpcCell As DataGridViewDateRangePickerCell = TryCast(dgvrRow.Cells(Index), DataGridViewDateRangePickerCell)
                If dgvdrpcCell IsNot Nothing Then dgvdrpcCell.ForeColor = value
            Next
        End Set
    End Property
    '''<summary>Gets or sets the format of the date and time displayed in the control.</summary>
    '''<returns>One of the <see cref="DateTimePickerFormat"/> values. The default is <see cref="DateTimePickerFormat.Long"/>.</returns>
    <Category("Appearance")>
    <DefaultValue(DateTimePickerFormat.Long)>
    <Description("The format of the date and time displayed in the control.")>
    Public Overridable Property Format As DateTimePickerFormat
        Get
            Return _format
        End Get
        Set(value As DateTimePickerFormat)
            If value = _format Then Return
            _format = value
            Dim dgvdrpcTemplate As DataGridViewDateRangePickerCell = TryCast(CellTemplate, DataGridViewDateRangePickerCell)
            If dgvdrpcTemplate IsNot Nothing Then dgvdrpcTemplate.Format = value
            If DataGridView Is Nothing Then Return
            For Each dgvrRow As DataGridViewRow In DataGridView.Rows
                Dim dgvdrpcCell As DataGridViewDateRangePickerCell = TryCast(dgvrRow.Cells(Index), DataGridViewDateRangePickerCell)
                If dgvdrpcCell IsNot Nothing Then dgvdrpcCell.Format = value
            Next
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
            Dim dgvdrpcTemplate As DataGridViewDateRangePickerCell = TryCast(CellTemplate, DataGridViewDateRangePickerCell)
            If dgvdrpcTemplate IsNot Nothing Then dgvdrpcTemplate.MaxDate = value
            If DataGridView Is Nothing Then Return
            For Each dgvrRow As DataGridViewRow In DataGridView.Rows
                Dim dgvdrpcCell As DataGridViewDateRangePickerCell = TryCast(dgvrRow.Cells(Index), DataGridViewDateRangePickerCell)
                If dgvdrpcCell IsNot Nothing Then dgvdrpcCell.MaxDate = value
            Next
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
            Dim dgvdrpcTemplate As DataGridViewDateRangePickerCell = TryCast(CellTemplate, DataGridViewDateRangePickerCell)
            If dgvdrpcTemplate IsNot Nothing Then dgvdrpcTemplate.MinDate = value
            If DataGridView Is Nothing Then Return
            For Each dgvrRow As DataGridViewRow In DataGridView.Rows
                Dim dgvdrpcCell As DataGridViewDateRangePickerCell = TryCast(dgvrRow.Cells(Index), DataGridViewDateRangePickerCell)
                If dgvdrpcCell IsNot Nothing Then dgvdrpcCell.MinDate = value
            Next
        End Set
    End Property
    Private Sub ResetMinDate()
        MinDate = DateTimePicker.MinimumDateTime
    End Sub
    Private Function ShouldSerializeMinDate() As Boolean
        Return _minDate <> DateTimePicker.MinimumDateTime
    End Function
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
            Dim dgvdrpcTemplate As DataGridViewDateRangePickerCell = TryCast(CellTemplate, DataGridViewDateRangePickerCell)
            If dgvdrpcTemplate IsNot Nothing Then dgvdrpcTemplate.RightToLeftLayout = value
            If DataGridView Is Nothing Then Return
            For Each dgvrRow As DataGridViewRow In DataGridView.Rows
                Dim dgvdrpcCell As DataGridViewDateRangePickerCell = TryCast(dgvrRow.Cells(Index), DataGridViewDateRangePickerCell)
                If dgvdrpcCell IsNot Nothing Then dgvdrpcCell.RightToLeftLayout = value
            Next
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
            Dim dgvdrpcTemplate As DataGridViewDateRangePickerCell = TryCast(CellTemplate, DataGridViewDateRangePickerCell)
            If dgvdrpcTemplate IsNot Nothing Then dgvdrpcTemplate.ShowUpDown = value
            If DataGridView Is Nothing Then Return
            For Each dgvrRow As DataGridViewRow In DataGridView.Rows
                Dim dgvdrpcCell As DataGridViewDateRangePickerCell = TryCast(dgvrRow.Cells(Index), DataGridViewDateRangePickerCell)
                If dgvdrpcCell IsNot Nothing Then dgvdrpcCell.ShowUpDown = value
            Next
        End Set
    End Property

#End Region

#Region "Functions"

    Public Overrides Function Clone() As Object
        Dim dgvdrpcClone As DataGridViewDateRangePickerColumn = MyBase.Clone
        dgvdrpcClone._calendarFont = _calendarFont
        dgvdrpcClone._calendarForeColor = _calendarForeColor
        dgvdrpcClone._calendarMonthBackground = _calendarMonthBackground
        dgvdrpcClone._calendarTitleBackColor = _calendarTitleBackColor
        dgvdrpcClone._calendarTitleForeColor = _calendarTitleForeColor
        dgvdrpcClone._calendarTrailingForeColor = _calendarTrailingForeColor
        dgvdrpcClone.CellTemplate = CType(CellTemplate, DataGridViewDateRangePickerCell).Clone
        dgvdrpcClone._customFormat = _customFormat
        dgvdrpcClone._dropDownAlign = _dropDownAlign
        dgvdrpcClone._foreColor = _foreColor
        dgvdrpcClone._format = _format
        dgvdrpcClone._maxDate = _maxDate
        dgvdrpcClone._minDate = _minDate
        dgvdrpcClone._rightToLeftLayout = _rightToLeftLayout
        dgvdrpcClone._showUpDown = _showUpDown
        Return dgvdrpcClone
    End Function
    Public Overrides Function ToString() As String
        Return $"DataGridViewDateRangePickerColumn {{ Name={Name}, Index={Index} }}"
    End Function

#End Region

End Class
