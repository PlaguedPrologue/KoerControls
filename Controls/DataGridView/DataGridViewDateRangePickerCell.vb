Imports System.ComponentModel
Imports System.Globalization
'''<summary>Represents an individual cell, containing two instances of <see cref="DateTimePicker"/>, in a <see cref="Windows.Forms.DataGridView"/> control.</summary>
Public Class DataGridViewDateRangePickerCell
    Inherits DataGridViewTextBoxCell

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

#Region "Properties"

    '''<summary>Gets or sets the font style applied to each calendar.</summary>
    '''<returns>A <see cref="Font"/> that represents the font style applied to each calendar.</returns>
    Public Property CalendarFont As Font
        Get
            Return _calendarFont
        End Get
        Set(value As Font)
            If value.Equals(_calendarFont) Then Return
            _calendarFont = value
            DataGridView?.InvalidateCell(Me)
        End Set
    End Property
    '''<summary>Gets or sets the foreground color of each calendar.</summary>
    '''<returns>A <see cref="Color"/> that represents the foreground color of the calendar.</returns>
    '''<exception cref="ArgumentException">The value assigned is null.</exception>
    Public Property CalendarForeColor As Color
        Get
            Return _calendarForeColor
        End Get
        Set(value As Color)
            If value = _calendarForeColor Then Return
            _calendarForeColor = value
            DataGridView?.InvalidateCell(Me)
        End Set
    End Property
    '''<summary>Gets or sets the background color of the calendar month.</summary>
    '''<returns>A System.Drawing.Color that represents the background color of the calendar month.</returns>
    '''<exception cref="ArgumentException">The value assigned is null.</exception>
    Public Property CalendarMonthBackground As Color
        Get
            Return _calendarMonthBackground
        End Get
        Set(value As Color)
            If value = _calendarMonthBackground Then Return
            _calendarMonthBackground = value
            DataGridView?.InvalidateCell(Me)
        End Set
    End Property
    '''<summary>Gets or sets the background color of each calendar title.</summary>
    '''<returns>A <see cref="Color"/> that represents the background color of the calendar title.</returns>
    '''<exception cref="ArgumentException">The value assigned is null.</exception>
    Public Property CalendarTitleBackColor As Color
        Get
            Return _calendarTitleBackColor
        End Get
        Set(value As Color)
            If value = _calendarTitleBackColor Then Return
            _calendarTitleBackColor = value
            DataGridView?.InvalidateCell(Me)
        End Set
    End Property
    '''<summary>Gets or sets the foreground color of each calendar title.</summary>
    '''<returns>A <see cref="Color"/> that represents the foreground color of the calendar title.</returns>
    '''<exception cref="ArgumentException">The value assigned is null.</exception>
    Public Property CalendarTitleForeColor As Color
        Get
            Return _calendarTitleForeColor
        End Get
        Set(value As Color)
            If value = _calendarTitleForeColor Then Return
            _calendarTitleForeColor = value
            DataGridView?.InvalidateCell(Me)
        End Set
    End Property
    '''<summary>Gets or sets the foreground color of each calendar trailing dates.</summary>
    '''<returns>A System.Drawing.Color that represents the foreground color of each calendar trailing dates.</returns>
    '''<exception cref="ArgumentException">The value assigned is null.</exception>
    Public Property CalendarTrailingForeColor As Color
        Get
            Return _calendarTrailingForeColor
        End Get
        Set(value As Color)
            If value = _calendarTrailingForeColor Then Return
            _calendarTrailingForeColor = value
            DataGridView?.InvalidateCell(Me)
        End Set
    End Property
    '''<summary>Gets or sets the custom format of the date and time displayed in the control.</summary>
    '''<returns>The custom format of the date and time displayed int eh control.</returns>
    Public Overridable Property CustomFormat As String
        Get
            Return _customFormat
        End Get
        Set(value As String)
            If value = _customFormat Then Return
            _customFormat = value
            DataGridView?.InvalidateCell(Me)
        End Set
    End Property
    '''<summary>Gets the value of a new cell.</summary>
    '''<returns>The value of a new cell.</returns>
    Public Overloads ReadOnly Property DefaultNewRowValue As Object
        Get
            Return DateRange.Empty
        End Get
    End Property
    '''<summary>Gets or sets the alignment of the drop-down calendar on each <see cref="DateTimePicker"/> control.</summary>
    '''<returns>The alignment of the drop-down calendar on the control. The default is <see cref="LeftRightRelativeAlignment.Left"/></returns>
    '''<exception cref="InvalidEnumArgumentException">The value assigned is not one of the <see cref="LeftRightRelativeAlignment"/> values.</exception>
    Public Property DropDownAlign As LeftRightRelativeAlignment
        Get
            Return _dropDownAlign
        End Get
        Set(value As LeftRightRelativeAlignment)
            If value = _dropDownAlign Then Return
            _dropDownAlign = value
            DataGridView?.InvalidateCell(Me)
        End Set
    End Property
    Public Overrides ReadOnly Property EditType As Type
        Get
            Return GetType(DataGridViewDateRangePickerEditingControl)
        End Get
    End Property
    '''<summary>Gets or sets the foreground color of each <see cref="DateTimePicker"/> control.</summary>
    '''<returns>The foreground <see cref="Color"/> of each <see cref="DateTimePicker"/>.</returns>
    Public Property ForeColor As Color
        Get
            Return _foreColor
        End Get
        Set(value As Color)
            If value = _foreColor Then Return
            _foreColor = value
            DataGridView?.InvalidateCell(Me)
        End Set
    End Property
    '''<summary>Gets or sets the format of the date and time displayed in the control.</summary>
    '''<returns>One of the <see cref="DateTimePickerFormat"/> values. The default is <see cref="DateTimePickerFormat.Long"/>.</returns>
    Public Property Format As DateTimePickerFormat
        Get
            Return _format
        End Get
        Set(value As DateTimePickerFormat)
            If value = _format Then Return
            _format = value
            DataGridView?.InvalidateCell(Me)
        End Set
    End Property
    Public Overrides ReadOnly Property FormattedValueType As Type
        Get
            Return GetType(DateRange)
        End Get
    End Property
    '''<summary>Gets or sets the maximum date and time that can be selected in the control.</summary>
    '''<returns>The maximum date and time that can be selected in the control. The default is determined as the minimum of the CurrentCulture’s Calendar’s <see cref="Calendar.MaxSupportedDateTime"/> property and December 31st 9998 12 am.</returns>
    '''<exception cref="ArgumentException">The value assigned is less than the <see cref="MinDate"/> value.</exception>
    '''<exception cref="SystemException">The value assigned is greater than the <see cref="DateTimePicker.MaxDateTime"/> value.</exception>
    Public Property MaxDate As Date
        Get
            Return _maxDate
        End Get
        Set(value As Date)
            If value = _maxDate Then Return
            _maxDate = value
            DataGridView?.InvalidateCell(Me)
        End Set
    End Property
    '''<summary>Gets or sets the minimum date and time that can be selected in the control.</summary>
    '''<returns>The minimum date and time that can be selected in the control. The default is 1/1/1753 00:00:00.</returns>
    '''<exception cref="ArgumentException">The value assigned is greater than the <see cref="MaxDate"/> value.</exception>
    '''<exception cref="SystemException">The value assigned is less than the <see cref="DateTimePicker.MinDate"/> value.</exception>
    Public Property MinDate As Date
        Get
            Return _minDate
        End Get
        Set(value As Date)
            If value = _minDate Then Return
            _minDate = value
            DataGridView?.InvalidateCell(Me)
        End Set
    End Property
    '''<summary>Gets or sets whether the contents of each <see cref="DateTimePicker"/> are laid out from right to left.</summary>
    '''<returns><see langword="True"/> if the layout of each <see cref="DateTimePicker"/> contents is from right to left; otherwise, <see langword="False"/>. The default is <see langword="False"/>.</returns>
    Public Overridable Property RightToLeftLayout As Boolean
        Get
            Return _rightToLeftLayout
        End Get
        Set(value As Boolean)
            If value = _rightToLeftLayout Then Return
            _rightToLeftLayout = value
            DataGridView?.InvalidateCell(Me)
        End Set
    End Property
    '''<summary>Gets or sets a value indicating whether a spin button control (also known as an up-down control) is used to adjust the date/time value.</summary>
    '''<returns><see langword="True"/> if a spin button control is used to adjust the date/time value; otherwise, <see langword="False"/>. The default is <see langword="False"/>.</returns>
    Public Property ShowUpDown As Boolean
        Get
            Return _showUpDown
        End Get
        Set(value As Boolean)
            If value = _showUpDown Then Return
            _showUpDown = value
            DataGridView?.InvalidateCell(Me)
        End Set
    End Property
    '''<summary>Gets or sets the value associated with this cell.</summary>
    '''<returns>Gets or sets the data to be displayed by the cell. The default is <see cref="DateRange.Empty"/>.</returns>
    '''<exception cref="ArgumentOutOfRangeException"><see cref="RowIndex"/> is outside the valid range of 0 to the number of rows in the control minus 1.</exception>
    '''<exception cref="InvalidOperationException"><see cref="ColumnIndex"/> is less than 0, indicating that the cell is a row header cell.</exception>
    Public Overloads Property Value As Object
        Get
            Dim oValue As Object = MyBase.Value
            If oValue Is Nothing OrElse TypeOf oValue IsNot DateRange Then MyBase.Value = DateRange.Empty
            Return MyBase.Value
        End Get
        Set(value As Object)
            If value Is Nothing OrElse TypeOf value IsNot DateRange Then
                MyBase.Value = DateRange.Empty
            Else
                MyBase.Value = value
            End If
        End Set
    End Property
    '''<summary>Gets or sets the <see cref="Type"/> of value assigned to the control.</summary>
    '''<returns>The <see cref="Type"/> of value assigned to the control.</returns>
    Public Overrides ReadOnly Property ValueType As Type
        Get
            Return GetType(DateRange)
        End Get
    End Property

#End Region

#Region "Functions"

    Public Overrides Function Clone() As Object
        Dim dgvdrpcClone As DataGridViewDateRangePickerCell = MyBase.Clone
        dgvdrpcClone._calendarFont = _calendarFont
        dgvdrpcClone._calendarForeColor = _calendarForeColor
        dgvdrpcClone._calendarMonthBackground = _calendarMonthBackground
        dgvdrpcClone._calendarTitleBackColor = _calendarTitleBackColor
        dgvdrpcClone._calendarTitleForeColor = _calendarTitleForeColor
        dgvdrpcClone._calendarTrailingForeColor = _calendarTrailingForeColor
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
    Protected Overrides Function GetFormattedValue(value As Object, rowIndex As Integer, ByRef cellStyle As DataGridViewCellStyle, valueTypeConverter As TypeConverter, formattedValueTypeConverter As TypeConverter, context As DataGridViewDataErrorContexts) As Object
        If TypeOf value Is DateRange Then
            Select Case _format
                Case DateTimePickerFormat.Long
                    Return CType(value, DateRange).ToString("D")
                Case DateTimePickerFormat.Short
                    Return CType(value, DateRange).ToString("d")
                Case DateTimePickerFormat.Time
                    Return CType(value, DateRange).ToString("t")
                Case DateTimePickerFormat.Custom
                    Return CType(value, DateRange).ToString(CustomFormat)
                Case Else
                    Return CType(value, DateRange).ToShortDateFormat
            End Select
        Else
            Select Case _format
                Case DateTimePickerFormat.Long
                    Return DateRange.Empty.ToString("D")
                Case DateTimePickerFormat.Short
                    Return DateRange.Empty.ToString("d")
                Case DateTimePickerFormat.Time
                    Return DateRange.Empty.ToString("t")
                Case DateTimePickerFormat.Custom
                    Return DateRange.Empty.ToString(CustomFormat)
                Case Else
                    Return DateRange.Empty.ToShortDateFormat
            End Select
        End If
    End Function
    Protected Overrides Function GetValue(rowIndex As Integer) As Object
        Dim oValue As Object = MyBase.Value
        If oValue Is Nothing OrElse TypeOf oValue IsNot DateRange Then MyBase.Value = DateRange.Empty
        Return MyBase.Value
    End Function
    Public Overrides Sub InitializeEditingControl(rowIndex As Integer, initialFormattedValue As Object, dataGridViewCellStyle As DataGridViewCellStyle)
        MyBase.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle)
        Dim drpEditor As DateRangePicker = TryCast(DataGridView.EditingControl, DateRangePicker)
        If drpEditor Is Nothing Then Return
        drpEditor.CalendarFont = _calendarFont
        drpEditor.CalendarForeColor = _calendarForeColor
        drpEditor.CalendarMonthBackground = _calendarMonthBackground
        drpEditor.CalendarTitleBackColor = _calendarTitleBackColor
        drpEditor.CalendarTitleForeColor = _calendarTitleForeColor
        drpEditor.CalendarTrailingForeColor = _calendarTrailingForeColor
        drpEditor.CustomFormat = _customFormat
        drpEditor.DropDownAlign = _dropDownAlign
        drpEditor.ForeColor = _foreColor
        drpEditor.Format = _format
        drpEditor.MaxDate = _maxDate
        drpEditor.MinDate = _minDate
        drpEditor.RightToLeftLayout = _rightToLeftLayout
        drpEditor.ShowUpDown = _showUpDown
        If TypeOf Value Is DateRange Then drpEditor.Value = Value
    End Sub

#End Region

End Class
