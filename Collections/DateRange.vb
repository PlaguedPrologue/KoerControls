Imports System.ComponentModel
Imports System.Globalization

'''<summary>Represents a set of consecutive dates expressed by a start <see cref="Date"/> and end <see cref="Date"/>. This structure can be treated as an <see cref="IEnumerable"/> in many ways.</summary>
<TypeConverter(GetType(DateRangeConverter))>
Public Class DateRange
    Implements IComparable(Of DateRange), IEnumerable, IEnumerable(Of Date), IRange(Of Date, DateRange), IReadOnlyCollection(Of Date)

#Region "Private Items"

    Private _endDate As Date
    Private _startDate As Date

    Private Function GetEnumeratorNonGeneric() As IEnumerator Implements IEnumerable.GetEnumerator
        Return GetEnumerator()
    End Function
    Private Sub ResetEndDate()
        _endDate = Today
    End Sub
    Private Sub ResetStartDate()
        _startDate = Today
    End Sub
    Private Function ShouldSerializeEndDate() As Boolean
        Return _endDate <> Today
    End Function
    Private Function ShouldSerializeStartDate() As Boolean
        Return _startDate <> Today
    End Function

#End Region

#Region "Operators"

    Public Shared Operator =(left As DateRange, right As DateRange) As Boolean
        If left Is Nothing OrElse right Is Nothing Then Return left Is Nothing AndAlso right Is Nothing
        Return left._startDate = right._startDate AndAlso left._endDate = right._endDate
    End Operator
    Public Shared Operator <>(left As DateRange, right As DateRange) As Boolean
        Return Not left = right
    End Operator
    Public Shared Operator >(left As DateRange, right As DateRange) As Boolean
        If left._endDate = right._endDate Then Return left._startDate > right._startDate 'EndDates match, check StartDates
        Return left._endDate > right._endDate
    End Operator
    Public Shared Operator <(left As DateRange, right As DateRange) As Boolean
        If left._startDate = right._startDate Then Return left._endDate < right._endDate 'StartDates match, check EndDates
        Return left._startDate < right._startDate
    End Operator
    Public Shared Operator >=(left As DateRange, right As DateRange) As Boolean
        Return left._startDate >= right._startDate AndAlso left._endDate >= right._endDate
    End Operator
    Public Shared Operator <=(left As DateRange, right As DateRange) As Boolean
        Return left._startDate <= right._startDate AndAlso left._endDate <= right._endDate
    End Operator

    Public Shared Operator =(left As DateRange, right As Date) As Boolean
        Return left._startDate = right AndAlso left._endDate = right
    End Operator
    Public Shared Operator <>(left As DateRange, right As Date) As Boolean
        Return Not left = right
    End Operator
    Public Shared Operator >(left As DateRange, right As Date) As Boolean
        Return left._startDate > right
    End Operator
    Public Shared Operator <(left As DateRange, right As Date) As Boolean
        Return left._endDate < right
    End Operator
    Public Shared Operator >=(left As DateRange, right As Date) As Boolean
        Return left._endDate >= right
    End Operator
    Public Shared Operator <=(left As DateRange, right As Date) As Boolean
        Return left._startDate <= right
    End Operator

    Public Shared Operator =(left As Date, right As DateRange) As Boolean
        Return left = right._startDate AndAlso left = right._endDate
    End Operator
    Public Shared Operator <>(left As Date, right As DateRange) As Boolean
        Return Not left = right
    End Operator
    Public Shared Operator >(left As Date, right As DateRange) As Boolean
        Return left > right._endDate
    End Operator
    Public Shared Operator <(left As Date, right As DateRange) As Boolean
        Return left < right._startDate
    End Operator
    Public Shared Operator >=(left As Date, right As DateRange) As Boolean
        Return left >= right._startDate
    End Operator
    Public Shared Operator <=(left As Date, right As DateRange) As Boolean
        Return left <= right._endDate
    End Operator

    Public Function CompareTo(obj As DateRange) As Integer Implements IComparable(Of DateRange).CompareTo
        Dim iComparison As Integer = _startDate.CompareTo(obj._startDate) 'Compare StartDates
        If iComparison = 0 Then Return _endDate.CompareTo(obj._endDate) 'StartDates match, compare EndDates
        Return iComparison 'StartDates do not match, return StartDate comparison
    End Function
    Public Function Contains(value As Date) As Boolean Implements IRange(Of Date, DateRange).Contains
        Return value >= _startDate AndAlso value <= _endDate
    End Function
    Public Function ContainsAll(ParamArray values() As Date) As Boolean Implements IRange(Of Date, DateRange).ContainsAll
        For Each dDate As Date In values
            If dDate <= _startDate OrElse dDate >= _endDate Then Return False
        Next
        Return True
    End Function
    Public Function ContainsAny(ParamArray values() As Date) As Boolean Implements IRange(Of Date, DateRange).ContainsAny
        For Each dDate As Date In values
            If dDate >= _startDate AndAlso dDate <= _endDate Then Return True
        Next
        Return False
    End Function
    Public Function IsContinuousWith(other As DateRange) As Boolean Implements IRange(Of Date, DateRange).IsContinuousWith
        If _startDate <= other._endDate AndAlso other._startDate <= _endDate Then Return True 'Ranges overlap
        Return (_startDate - other._endDate).TotalDays = 1 OrElse (other._startDate - _endDate).TotalDays = 1 'Ranges do not overlap. Determine if ranges are one day away from each other
    End Function
    Public Function IsProperSubsetOf(other As DateRange) As Boolean Implements IRange(Of Date, DateRange).IsProperSubsetOf
        Return _startDate > other._startDate AndAlso _endDate < other._endDate
    End Function
    Public Function IsProperSupersetOf(other As DateRange) As Boolean Implements IRange(Of Date, DateRange).IsProperSupersetOf
        Return _startDate < other._startDate AndAlso _endDate > other._endDate
    End Function
    Public Function IsSubsetOf(other As DateRange) As Boolean Implements IRange(Of Date, DateRange).IsSubsetOf
        Return _startDate >= other._startDate AndAlso _endDate <= other._endDate
    End Function
    Public Function IsSupersetOf(other As DateRange) As Boolean Implements IRange(Of Date, DateRange).IsSupersetOf
        Return _startDate <= other._startDate AndAlso _endDate >= other._endDate
    End Function
    Public Function Overlaps(other As DateRange) As Boolean Implements IRange(Of Date, DateRange).Overlaps
        If _startDate <= other._startDate Then Return _endDate >= other._startDate
        Return _startDate <= other._endDate
    End Function

#End Region

#Region "Initializers"

    '''<summary>Initializes a new empty instance of <see cref="DateRange"/> with values of the current date.</summary>
    Public Sub New()
        _startDate = Today
        _endDate = Today
    End Sub
    '''<summary>Initializes a new instance of the <see cref="DateRange"/> class.</summary>
    '''<param name="value">The start and end <see cref="Date"/> of the <see cref="DateRange"/>.</param>
    Public Sub New(value As Date)
        _startDate = value
        _endDate = value
    End Sub
    '''<summary>Initializes a new instance of the <see cref="DateRange"/> class.</summary>
    '''<param name="startDate">The start <see cref="Date"/> of the <see cref="DateRange"/>.</param>
    '''<param name="endDate">The end <see cref="Date"/> of the <see cref="DateRange"/>.</param>
    Public Sub New(startDate As Date, endDate As Date)
        If startDate < endDate Then
            _startDate = startDate
            _endDate = endDate
        Else
            _startDate = endDate
            _endDate = startDate
        End If
    End Sub
    '''<summary>Initializes a new instance of the <see cref="DateRange"/> class.</summary>
    '''<param name="startDate">The initial <see cref="Date"/> to reference for the <see cref="DateRange"/>.</param>
    '''<param name="length">The number of days the <see cref="DateRange"/> will span .</param>
    Public Sub New(startDate As Date, length As Integer)
        _startDate = startDate
        _endDate = startDate.AddDays(length)
    End Sub
    '''<summary>Initializes a new instance of the <see cref="DateRange"/> class.</summary>
    '''<param name="length">The number of days the <see cref="DateRange"/> will span </param>
    '''<param name="endDate">The last <see cref="Date"/> to reference for the <see cref="DateRange"/>.</param>
    Public Sub New(length As Integer, endDate As Date)
        _startDate = endDate.AddDays(-length)
        _endDate = endDate
    End Sub

#End Region

#Region "Events"

    '''<summary>Occurs when the end date is been changed.</summary>
    Public Event EndDateChanged As EventHandler
    '''<summary>Occurs when the start date is been changed.</summary>
    Public Event StartDateChanged As EventHandler

#End Region

#Region "Properties"

    '''<summary>Represents an empty date range at value <see cref="Today"/>.</summary>
    '''<returns>An empty date range at value <see cref="Today"/>.</returns>
    Public Shared ReadOnly Property Empty As DateRange
        Get
            Return New DateRange
        End Get
    End Property
    <Description("The ending value of the range.")>
    Public Property EndDate As Date Implements IRange(Of Date, DateRange).EndValue
        Get
            Return _endDate
        End Get
        Set(value As Date)
            If value = _endDate Then Return
            If value < _startDate Then
                _endDate = _startDate
                _startDate = value
                OnStartDateChanged(EventArgs.Empty)
            Else
                _endDate = value
            End If
            OnEndDateChanged(EventArgs.Empty)
        End Set
    End Property
    <Browsable(False)>
    Public ReadOnly Property Length As Integer Implements IRange(Of Date, DateRange).Length, IReadOnlyCollection(Of Date).Count
        Get
            Return (_endDate - _startDate).TotalDays
        End Get
    End Property
    '''<summary>Represents the largest possible value of <see cref="DateRange"/>.</summary>
    '''<returns>The largest possible value of <see cref="DateRange"/>.</returns>
    <Browsable(False)>
    Public Shared ReadOnly Property MaxValue As DateRange
        Get
            Return New DateRange(Date.MaxValue, Date.MaxValue)
        End Get
    End Property
    '''<summary>Represents the smallest possible value of <see cref="DateRange"/>.</summary>
    '''<returns>The smallest possible value of <see cref="DateRange"/>.</returns>
    <Browsable(False)>
    Public Shared ReadOnly Property MinValue As DateRange
        Get
            Return New DateRange(Date.MinValue, Date.MinValue)
        End Get
    End Property
    <Description("The starting value of the range.")>
    Public Property StartDate As Date Implements IRange(Of Date, DateRange).StartValue
        Get
            Return _startDate
        End Get
        Set(value As Date)
            If value = _startDate Then Return
            If value > _endDate Then
                _startDate = _endDate
                _endDate = value
                OnEndDateChanged(EventArgs.Empty)
            Else
                _startDate = value
            End If
            OnStartDateChanged(EventArgs.Empty)
        End Set
    End Property

#End Region

#Region "Functions"

    Public Function AddDays(value As Integer) As DateRange Implements IRange(Of Date, DateRange).AddValue
        Return New DateRange(_startDate.AddDays(value), _endDate.AddDays(value))
    End Function
    Public Function AddDaysToEnd(value As Integer) As DateRange Implements IRange(Of Date, DateRange).AddValueToEnd
        If -value > (_endDate - _startDate).TotalDays Then Throw New ArgumentOutOfRangeException("Cannot set EndDate of DateRange to be less than the current StartDate.")
        Return New DateRange(_startDate, _endDate.AddDays(value))
    End Function
    Public Function AddDaysToStart(value As Integer) As DateRange Implements IRange(Of Date, DateRange).AddValueToStart
        If value > (_endDate - _startDate).TotalDays Then Throw New ArgumentOutOfRangeException("Cannot set StartDate of DateRange to be greater than the current EndDate.")
        Return New DateRange(_startDate.AddDays(value), _endDate)
    End Function
    Public Iterator Function GetEnumerator() As IEnumerator(Of Date) Implements IEnumerable(Of Date).GetEnumerator
        Dim dCurrentDate As Date = _startDate
        While dCurrentDate <= _endDate
            Yield dCurrentDate
            dCurrentDate = dCurrentDate.AddDays(1)
        End While
    End Function
    Public Function InnerJoin(other As DateRange) As DateRange Implements IRange(Of Date, DateRange).InnerJoin
        If _startDate < other._startDate Then
            If _endDate > other._endDate Then Return New DateRange(other._startDate, other._endDate)
            Return New DateRange(other._startDate, _endDate)
        End If
        If _endDate > other._endDate Then Return New DateRange(_startDate, other._endDate)
        Return New DateRange(_startDate, _endDate)
    End Function
    '''<summary>Merges each overlapping <see cref="DateRange"/>, iterating through the each joined <see cref="DateRange"/>, sorted from least to greatest.</summary>
    '''<returns>Each joined <see cref="DateRange"/>, sorted from least to greatest.</returns>
    Public Shared Iterator Function Join(ranges() As DateRange) As IEnumerable(Of DateRange)
        Dim drBuildingRange As DateRange = Nothing
        For Each drDateRange As DateRange In ranges.OrderBy(Function(ir) ir)
            If drBuildingRange Is Nothing Then
                drBuildingRange = New DateRange(drDateRange.StartDate, drDateRange.EndDate)
            Else
                If drBuildingRange.IsContinuousWith(drDateRange) Then
                    drBuildingRange._endDate = drDateRange._endDate
                Else
                    Yield drBuildingRange
                    drBuildingRange = New DateRange(drDateRange.StartDate, drDateRange.EndDate)
                End If
            End If
        Next
        If drBuildingRange IsNot Nothing Then
            Yield drBuildingRange
        End If
    End Function
    '''<summary>Raises the <see cref="EndDateChanged"/> event.</summary>
    '''<param name="e">The <see cref="EventArgs"/> containing the event data.</param>
    Protected Overridable Sub OnEndDateChanged(e As EventArgs)
        RaiseEvent EndDateChanged(Me, e)
    End Sub
    '''<summary>Raises the <see cref="StartDateChanged"/> event.</summary>
    '''<param name="e">The <see cref="EventArgs"/> containing the event data.</param>
    Protected Overridable Sub OnStartDateChanged(e As EventArgs)
        RaiseEvent StartDateChanged(Me, e)
    End Sub
    '''<summary>Converts the values of the current <see cref="DateRange"/> objects to their equivalent short date string representation.</summary>
    '''<returns>A string that contains the short date string representations of the current <see cref="DateRange"/> objects.</returns>
    '''<param name="delimiter">The separating string between each <see cref="Date"/> in the <see cref="DateRange"/>.</param>
    Public Function ToShortDateFormat(Optional delimiter As String = "to") As String Implements IRange(Of Date, DateRange).ToString
        delimiter = delimiter.Trim 'Get the delimiter
        If String.IsNullOrEmpty(delimiter) Then Return $"{_startDate.ToShortDateString} {_endDate.ToShortDateString}" 'Delimiter is empty
        If Char.IsPunctuation(delimiter(0)) Then Return $"{_startDate.ToShortDateString}{delimiter} {_endDate}.ToShortDateString" 'Delimiter starts with punctuation
        Return $"{_startDate.ToShortDateString} {delimiter} {_endDate.ToShortDateString}" 'Standard delimiter
    End Function
    '''<summary>Converts the values of the current <see cref="DateRange"/> objects to their equivalent short date string representation.</summary>
    '''<remarks>Extra space is added after the <see cref="StartDate"/> to make lining up string vertically look crisp.</remarks>
    '''<returns>A string that contains the short date string representations of the current <see cref="DateRange"/> objects.</returns>
    '''<param name="font">The <see cref="Font"/> used to display the string.</param>
    '''<param name="delimiter">The separating string between each <see cref="Date"/> in the <see cref="DateRange"/>.</param>
    Public Function ToShortDateFormat(font As Font, Optional delimiter As String = "to") As String Implements IRange(Of Date, DateRange).ToString
        If font Is Nothing Then font = SystemFonts.DefaultFont 'Get font
        Dim iTargetWidth As Integer = TextRenderer.MeasureText("00/00/0000", font).Width 'Get target width of start date
        Dim iSpaceWidth As Integer = TextRenderer.MeasureText("  ", font).Width - TextRenderer.MeasureText(" ", font).Width 'Get width of [space] characters in the provided font

        'Get the date strings
        Dim sStartDate As String = _startDate.ToShortDateString
        delimiter = delimiter.Trim 'Get the delimiter
        If String.IsNullOrEmpty(delimiter) Then Return $"{sStartDate}{New String(" ", Math.Max(0, (iTargetWidth - TextRenderer.MeasureText(sStartDate, font).Width) / iSpaceWidth))} {_endDate.ToShortDateString}" 'Delimiter is empty
        If Char.IsPunctuation(delimiter(0)) Then Return $"{sStartDate}{New String(" ", Math.Max(0, (iTargetWidth - TextRenderer.MeasureText(sStartDate, font).Width) / iSpaceWidth))}{delimiter} {_endDate.ToShortDateString}" 'Delimiter starts with punctuation
        Return $"{sStartDate}{New String(" ", Math.Max(0, (iTargetWidth - TextRenderer.MeasureText(sStartDate, font).Width) / iSpaceWidth))} {delimiter} {_endDate.ToShortDateString}" 'Standard delimiter
    End Function
    Public Overrides Function ToString() As String
        Return $"{_startDate.ToShortDateString}, {_endDate.ToShortDateString}"
    End Function
    '''<summary>Converts the value of the current <see cref="DateRange"/> object to its equivalent string representation using the specified format and the formatting conventions of the current culture.</summary>
    '''<remarks>Extra space is added after the <see cref="StartDate"/> to make lining up string vertically look crisp.</remarks>
    '''<returns>A <see cref="String"/> that represent the <see cref="DateRange"/>.</returns>
    '''<param name="format">A standard or custom date and time format string.</param>
    '''<param name="delimiter">The separating string between each <see cref="Date"/> in the <see cref="DateRange"/>.</param>
    Public Overloads Function ToString(Optional format As String = "", Optional delimiter As String = "to") As String
        delimiter = delimiter.Trim 'Get the delimiter
        If String.IsNullOrEmpty(delimiter) Then Return $"{_startDate.ToString(format)} {_endDate.ToString(format)}" 'Delimiter is empty
        If Char.IsPunctuation(delimiter(0)) Then Return $"{_startDate.ToString(format)}{delimiter} {_endDate.ToString(format)}" 'Delimiter starts with punctuation
        Return $"{_startDate.ToString(format)} {delimiter} {_endDate.ToString(format)}" 'Standard delimiter
    End Function
    '''<summary>Converts the value of the current <see cref="DateRange"/> object to its equivalent string representation using the specified format and the formatting conventions of the current culture.</summary>
    '''<remarks>Extra space is added after the <see cref="StartDate"/> to make lining up string vertically look crisp.</remarks>
    '''<returns>A <see cref="String"/> that represent the <see cref="DateRange"/>.</returns>
    '''<param name="font">The <see cref="Font"/> used to display the string.</param>
    '''<param name="format">A standard or custom date and time format string.</param>
    '''<param name="delimiter">The separating string between each <see cref="Date"/> in the <see cref="DateRange"/>.</param>
    Public Overloads Function ToString(font As Font, Optional format As String = "", Optional delimiter As String = "to") As String
        If font Is Nothing Then font = SystemFonts.DefaultFont 'Get font
        Dim iTargetWidth As Integer = TextRenderer.MeasureText(Date.MinValue.ToString(format), font).Width 'Get target width of start date
        Dim iSpaceWidth As Integer = TextRenderer.MeasureText("  ", font).Width - TextRenderer.MeasureText(" ", font).Width 'Get width of [space] characters in the provided font

        'Get the date strings
        Dim sStartDate As String = _startDate.ToString(format)
        delimiter = delimiter.Trim 'Get the delimiter
        If String.IsNullOrEmpty(delimiter) Then Return $"{sStartDate}{New String(" ", Math.Max(0, (iTargetWidth - TextRenderer.MeasureText(sStartDate, font).Width) / iSpaceWidth))} {_endDate.ToString(format)}" 'Delimiter is empty
        If Char.IsPunctuation(delimiter(0)) Then Return $"{sStartDate}{New String(" ", Math.Max(0, (iTargetWidth - TextRenderer.MeasureText(sStartDate, font).Width) / iSpaceWidth))}{delimiter} {_endDate.ToString(format)}" 'Delimiter starts with punctuation
        Return $"{sStartDate}{New String(" ", Math.Max(0, (iTargetWidth - TextRenderer.MeasureText(sStartDate, font).Width) / iSpaceWidth))} {delimiter} {_endDate.ToString(format)}" 'Standard delimiter
    End Function
    '''<summary>Returns a new <see cref="DateRange"/> that contains all the values that fall within and between each specified <see cref="DateRange"/>.</summary>
    '''<returns>A new <see cref="DateRange"/> that contains all the values that fall within each specified <see cref="DateRange"/>.</returns>
    '''<param name="ranges">Each <see cref="DateRange"/> to combine.</param>
    Public Shared Function Union(ranges As IEnumerable(Of DateRange)) As DateRange
        If ranges.FirstOrDefault Is Nothing Then Return MinValue
        Dim dStartDate As Date = ranges(0)._startDate
        Dim dEndDate As Date = ranges(0)._endDate
        For Each drRange As DateRange In ranges.Skip(1)
            If drRange Is Nothing Then Continue For
            If drRange._startDate < dStartDate Then dStartDate = drRange._startDate
            If drRange._endDate > dEndDate Then dEndDate = drRange._endDate
        Next
        Return New DateRange(dStartDate, dEndDate)
    End Function
    Public Function UnionWith(other As DateRange) As DateRange Implements IRange(Of Date, DateRange).UnionWith
        If _startDate < other._startDate Then
            If _endDate > other._endDate Then Return New DateRange(_startDate, _endDate)
            Return New DateRange(_startDate, other._endDate)
        End If
        If _endDate > other._endDate Then Return New DateRange(other._startDate, _endDate)
        Return New DateRange(other._startDate, other._endDate)
    End Function

#End Region

#Region "Parsing"

    '''<summary>Converts the string representation of a date and time range to its <see cref="DateRange"/> equivalent by using culture-specific format information and formatting style.</summary>
    '''<returns>An object that is equivalent to the date and time range contained in <paramref name="s"/>, as specified by provider and styles.</returns>
    '''<param name="s">A string that contains a date range to convert.</param>
    '''<exception cref="FormatException"><paramref name="s"/> is an empty string. -or- The hour component and the AM/PM designator in <paramref name="s"/> do not agree.</exception>
    Public Shared Function Parse(s As String) As DateRange
        Dim iStringLength As Integer = s.Length
        Dim bStartDateFound As Boolean = False
        Dim bEndDateFound As Boolean = False
        Dim dStartValue As Date
        Dim dEndValue As Date
        For iIndex As Integer = 0 To iStringLength - 1 'Index through string
            bStartDateFound = bStartDateFound OrElse Date.TryParse(s.Substring(0, iStringLength - iIndex), dStartValue) 'Try to get start date from front portion of string
            bEndDateFound = bEndDateFound OrElse Date.TryParse(s.Substring(iIndex, iStringLength - iIndex), dEndValue) 'Try to get end date from end portion of string
            If bStartDateFound AndAlso bEndDateFound Then Return New DateRange(dStartValue, dEndValue) 'Determine if both start date and end date have been found
        Next
        Throw New FormatException($"Cannot convert the string '{s}' to DateRange.")
    End Function
    '''<summary>Converts the string representation of a date and time range to its <see cref="DateRange"/> equivalent by using culture-specific format information and formatting style.</summary>
    '''<returns>An object that is equivalent to the date and time range contained in <paramref name="s"/>, as specified by provider and styles.</returns>
    '''<param name="s">A string that contains a date range to convert.</param>
    '''<param name="provider">An object that supplies culture-specific format information about <paramref name="s"/>.</param>
    '''<param name="styles">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="s"/>. A typical value to specify is <see cref="DateTimeStyles.None"/>.</param>
    '''<exception cref="ArgumentException"><paramref name="styles"/> is not a valid <see cref="DateTimeStyles"/> value.-or- <paramref name="styles"/> contains an invalid combination of <see cref="DateTimeStyles"/> values (for example, both <see cref="DateTimeStyles.AssumeLocal"/> and <see cref="DateTimeStyles.AssumeUniversal"/>).</exception>
    '''<exception cref="FormatException"><paramref name="s"/> is an empty string. -or- The hour component and the AM/PM designator in <paramref name="s"/> do not agree.</exception>
    Public Shared Function Parse(s As String, provider As IFormatProvider, styles As DateTimeStyles) As DateRange
        Dim iStringLength As Integer = s.Length
        Dim bStartDateFound As Boolean = False
        Dim bEndDateFound As Boolean = False
        Dim dStartValue As Date
        Dim dEndValue As Date
        For iIndex As Integer = 0 To iStringLength - 1 'Index through string
            bStartDateFound = bStartDateFound OrElse Date.TryParse(s.Substring(0, iStringLength - iIndex), provider, styles, dStartValue) 'Try to get start date from front portion of string
            bEndDateFound = bEndDateFound OrElse Date.TryParse(s.Substring(iIndex, iStringLength - iIndex), provider, styles, dEndValue) 'Try to get end date from end portion of string
            If bStartDateFound AndAlso bEndDateFound Then Return New DateRange(dStartValue, dEndValue) 'Determine if both start date and end date have been found
        Next
        Throw New FormatException($"Cannot convert the string '{s}' to DateRange using the specified provider and styles.")
    End Function
    '''<summary>Converts the specified string representation of a date and time range to its <see cref="DateRange"/> equivalent using the specified format, culture-specific format information, and  style. The format of the string representation must match the specified format exactly. The method returns a value that indicates whether the conversion succeeded.</summary>
    '''<returns><see langword="True"/> if the <paramref name="s"/> parameter was converted successfully; otherwise, <see langword="False"/>.</returns>
    '''<param name="s">A string that contains a date range to convert.</param>
    '''<param name="format">The of required format of <paramref name="s"/>.</param>
    '''<param name="provider">An object that supplies culture-specific format information about <paramref name="s"/>.</param>
    '''<param name="style">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="s"/>. A typical value to specify is <see cref="DateTimeStyles.None"/>.</param>
    '''<exception cref="ArgumentException"><paramref name="style"/> is not a valid <see cref="DateTimeStyles"/> value.-or- <paramref name="style"/> contains an invalid combination of <see cref="DateTimeStyles"/> values (for example, both <see cref="DateTimeStyles.AssumeLocal"/> and <see cref="DateTimeStyles.AssumeUniversal"/>).</exception>
    '''<exception cref="FormatException"><paramref name="s"/> is an empty string. -or- the <paramref name="format"/> is an empty string. -or- <paramref name="s"/> does not contain a date and time that corresponds to the <paramref name="format"/>. -or-The hour component and the AM/PM designator in <paramref name="s"/> do not agree.</exception>
    Public Shared Function ParseExact(s As String, format As String, provider As IFormatProvider, style As DateTimeStyles) As DateRange
        Dim iStringLength As Integer = s.Length
        Dim bStartDateFound As Boolean = False
        Dim bEndDateFound As Boolean = False
        Dim dStartValue As Date
        Dim dEndValue As Date
        For iIndex As Integer = 0 To iStringLength - 1 'Index through string
            bStartDateFound = bStartDateFound OrElse Date.TryParseExact(s.Substring(0, iStringLength - iIndex), format, provider, style, dStartValue) 'Try to get start date from front portion of string
            bEndDateFound = bEndDateFound OrElse Date.TryParseExact(s.Substring(iIndex, iStringLength - iIndex), format, provider, style, dEndValue) 'Try to get end date from end portion of string
            If bStartDateFound AndAlso bEndDateFound Then Return New DateRange(dStartValue, dEndValue) 'Determine if both start date and end date have been found
        Next
        Throw New FormatException($"Cannot convert the string '{s}' to DateRange using the specified formats, provider, and styles.")
    End Function
    '''<summary>Converts the specified string representation of a date and time range to its <see cref="DateRange"/> equivalent using the specified array of formats, culture-specific format information, and style. The format of the string representation must match at least one of the specified formats exactly or an exception is thrown.</summary>
    '''<returns>An object that is equivalent to the date and time range contained in s, as specified by formats, provider, and style.</returns>
    '''<param name="s">A string that contains a date and time to convert.</param>
    '''<param name="formats">An array of allowable formats of <paramref name="s"/>.</param>
    '''<param name="provider">An object that supplies culture-specific format information about <paramref name="s"/>.</param>
    '''<param name="style">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="s"/>. A typical value to specify is <see cref="DateTimeStyles.None"/>.</param>
    '''<exception cref="ArgumentException"><paramref name="style"/> is not a valid <see cref="DateTimeStyles"/> value.-or- <paramref name="style"/> contains an invalid combination of <see cref="DateTimeStyles"/> values (for example, both <see cref="DateTimeStyles.AssumeLocal"/> and <see cref="DateTimeStyles.AssumeUniversal"/>).</exception>
    '''<exception cref="FormatException"><paramref name="s"/> is an empty string. -or- an element of <paramref name="formats"/> is an empty string. -or- <paramref name="s"/> does not contain a date and time that corresponds to any element of <paramref name="formats"/>. -or-The hour component and the AM/PM designator in <paramref name="s"/> do not agree.</exception>
    Public Shared Function ParseExact(s As String, formats() As String, provider As IFormatProvider, style As DateTimeStyles) As DateRange
        Dim iStringLength As Integer = s.Length
        Dim bStartDateFound As Boolean = False
        Dim bEndDateFound As Boolean = False
        Dim dStartValue As Date
        Dim dEndValue As Date
        For iIndex As Integer = 0 To iStringLength - 1 'Index through string
            bStartDateFound = bStartDateFound OrElse Date.TryParseExact(s.Substring(0, iStringLength - iIndex), formats, provider, style, dStartValue) 'Try to get start date from front portion of string
            bEndDateFound = bEndDateFound OrElse Date.TryParseExact(s.Substring(iIndex, iStringLength - iIndex), formats, provider, style, dEndValue) 'Try to get end date from end portion of string
            If bStartDateFound AndAlso bEndDateFound Then Return New DateRange(dStartValue, dEndValue) 'Determine if both start date and end date have been found
        Next
        Throw New FormatException($"Cannot convert the string '{s}' to DateRange using the specified formats, provider, and styles.")
    End Function
    '''<summary>Converts the specified string representation of a date and time range to its <see cref="DateRange"/> equivalent and returns a value that indicates whether the conversion succeeded.</summary>
    '''<returns><see langword="True"/> if the <paramref name="s"/> parameter was converted successfully; otherwise, <see langword="False"/>.</returns>
    '''<param name="s">A string that contains a date range to convert.</param>
    '''<param name="result">When this method returns, contains the <see cref="DateRange"/> value equivalent to the date and times contained in <paramref name="s"/>, if the conversion succeeded, or <see cref="Empty"/> if the conversion failed. The conversion fails if the <paramref name="s"/> parameter is null, is an empty string (""), or does not contain a valid string representation of a date and time range. This parameter is passed uninitialized.</param>
    Public Shared Function TryParse(s As String, ByRef result As DateRange) As Boolean
        Dim iStringLength As Integer = s.Length
        Dim bStartDateFound As Boolean = False
        Dim bEndDateFound As Boolean = False
        Dim dStartDate As Date
        Dim dEndDate As Date
        For iIndex As Integer = 0 To iStringLength - 1 'Index through string
            bStartDateFound = bStartDateFound OrElse Date.TryParse(s.Substring(0, iStringLength - iIndex), dStartDate) 'Try to get start date from front portion of string
            bEndDateFound = bEndDateFound OrElse Date.TryParse(s.Substring(iIndex, iStringLength - iIndex), dEndDate) 'Try to get end date from end portion of string
            If bStartDateFound AndAlso bEndDateFound Then 'Determine if both start date and end date have been found
                result = New DateRange(dStartDate, dEndDate) 'set result
                Return True 'Conversion was succesfull
            End If
        Next
        result = Empty
        Return False 'Conversion was not succesfull
    End Function
    '''<summary>Converts the specified string representation of a date and time range to its <see cref="DateRange"/> equivalent using the specified culture-specific format information and formatting style, and returns a value that indicates whether the conversion succeeded.</summary>
    '''<returns><see langword="True"/> if the <paramref name="s"/> parameter was converted successfully; otherwise, <see langword="False"/>.</returns>
    '''<param name="s">A string that contains a date and time to convert.</param>
    '''<param name="provider">An object that supplies culture-specific format information about <paramref name="s"/>.</param>
    '''<param name="styles">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="s"/>. A typical value to specify is <see cref="DateTimeStyles.None"/>.</param>
    '''<exception cref="ArgumentException"><paramref name="styles"/> is not a valid <see cref="DateTimeStyles"/> value.-or- <paramref name="styles"/> contains an invaid combination of <see cref="DateTimeStyles"/> values (for example, both <see cref="DateTimeStyles.AssumeLocal"/> and <see cref="DateTimeStyles.AssumeUniversal"/>).</exception>
    '''<exception cref="NotSupportedException"><paramref name="provider"/> is a neutral culture and cannot be used in a parsing operation.</exception>
    Public Shared Function TryParse(s As String, provider As IFormatProvider, styles As DateTimeStyles, ByRef result As DateRange) As Boolean
        Dim iStringLength As Integer = s.Length
        Dim bStartDateFound As Boolean = False
        Dim bEndDateFound As Boolean = False
        Dim dStartDate As Date
        Dim dEndDate As Date
        For iIndex As Integer = 0 To iStringLength - 1 'Index through string
            bStartDateFound = bStartDateFound OrElse Date.TryParse(s.Substring(0, iStringLength - iIndex), provider, styles, dStartDate) 'Try to get start date from front portion of string
            bEndDateFound = bEndDateFound OrElse Date.TryParse(s.Substring(iIndex, iStringLength - iIndex), provider, styles, dEndDate) 'Try to get end date from end portion of string
            If bStartDateFound AndAlso bEndDateFound Then 'Determine if both start date and end date have been found
                result = New DateRange(dStartDate, dEndDate) 'set result
                Return True 'Conversion was succesfull
            End If
        Next
        result = Empty
        Return False 'Conversion was not succesfull
    End Function
    '''<summary>Converts the specified string representation of a date and time range to its <see cref="DateRange"/> equivalent using the specified format, culture-specific format information, and  style. The format of the string representation must match the specified format exactly. The method returns a value that indicates whether the conversion succeeded.</summary>
    '''<returns><see langword="True"/> if the <paramref name="s"/> parameter was converted successfully; otherwise, <see langword="False"/>.</returns>
    '''<param name="s">A string that contains a date range to convert.</param>
    '''<param name="format">The of required formats of <paramref name="s"/>.</param>
    '''<param name="provider">An object that supplies culture-specific format information about <paramref name="s"/>.</param>
    '''<param name="style">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="s"/>. A typical value to specify is <see cref="DateTimeStyles.None"/>.</param>
    '''<param name="result">When this method returns, contains the <see cref="DateRange"/> value equivalent to the date and times contained in <paramref name="s"/>, if the conversion succeeded, or <see cref="Empty"/> if the conversion failed. The conversion fails if <paramref name="s"/> or <paramref name="format"/> is null, <paramref name="s"/> or <paramref name="format"/> is an empty string, or the format of <paramref name="s"/> is not exactly as specified by at the format patterns in <paramref name="format"/>. This parameter is passed uninitialized.</param>
    '''<exception cref="ArgumentException"><paramref name="style"/> is not a valid <see cref="DateTimeStyles"/> value.-or- <paramref name="style"/> contains an invalid combination of <see cref="DateTimeStyles"/> values (for example, both <see cref="DateTimeStyles.AssumeLocal"/> and <see cref="DateTimeStyles.AssumeUniversal"/>).</exception>
    Public Shared Function TryParseExact(s As String, format As String, provider As IFormatProvider, style As DateTimeStyles, ByRef result As DateRange) As Boolean
        Dim iStringLength As Integer = s.Length
        Dim bStartDateFound As Boolean = False
        Dim bEndDateFound As Boolean = False
        Dim dStartValue As Date
        Dim dEndValue As Date
        For iIndex As Integer = 0 To iStringLength - 1 'Index through string
            bStartDateFound = bStartDateFound OrElse Date.TryParseExact(s.Substring(0, iStringLength - iIndex), format, provider, style, dStartValue) 'Try to get start date from front portion of string
            bEndDateFound = bEndDateFound OrElse Date.TryParseExact(s.Substring(iIndex, iStringLength - iIndex), format, provider, style, dEndValue) 'Try to get end date from end portion of string
            If bStartDateFound AndAlso bEndDateFound Then 'Determine if both start date and end date have been found
                result = New DateRange(dStartValue, dEndValue) 'set result
                Return True 'Conversion was succesfull
            End If
        Next
        result = Empty
        Return False 'Conversion was not succesfull
    End Function
    '''<summary>Converts the specified string representation of a date range to its <see cref="DateRange"/> equivalent using the specified array of formats, culture-specific format information, and style. The format of the string representation must match at least one of the specified formats exactly. The method returns a value that indicates whether the conversion succeeded.</summary>
    '''<returns><see langword="True"/> if the <paramref name="s"/> parameter was converted successfully; otherwise, <see langword="False"/>.</returns>
    '''<param name="s">A string that contains a date range to convert.</param>
    '''<param name="formats">An array of allowable formats of <paramref name="s"/>.</param>
    '''<param name="provider">An object that supplies culture-specific format information about <paramref name="s"/>.</param>
    '''<param name="style">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="s"/>. A typical value to specify is <see cref="DateTimeStyles.None"/>.</param>
    '''<param name="result">When this method returns, contains the <see cref="DateRange"/> value equivalent to the date and times contained in <paramref name="s"/>, if the conversion succeeded, or <see cref="Empty"/> if the conversion failed. The conversion fails if <paramref name="s"/> or <paramref name="formats"/> is null, <paramref name="s"/> or an element of <paramref name="formats"/> is an empty string, or the format of <paramref name="s"/> is not exactly as specified by at least one of the format patterns in <paramref name="formats"/>. This parameter is passed uninitialized.</param>
    '''<exception cref="ArgumentException"><paramref name="style"/> is not a valid <see cref="DateTimeStyles"/> value.-or- <paramref name="style"/> contains an invalid combination of <see cref="DateTimeStyles"/> values (for example, both <see cref="DateTimeStyles.AssumeLocal"/> and <see cref="DateTimeStyles.AssumeUniversal"/>).</exception>
    Public Shared Function TryParseExact(s As String, formats() As String, provider As IFormatProvider, style As DateTimeStyles, ByRef result As DateRange) As Boolean
        Dim iStringLength As Integer = s.Length
        Dim bStartDateFound As Boolean = False
        Dim bEndDateFound As Boolean = False
        Dim dStartValue As Date
        Dim dEndValue As Date
        For iIndex As Integer = 0 To iStringLength - 1 'Index through string
            bStartDateFound = bStartDateFound OrElse Date.TryParseExact(s.Substring(0, iStringLength - iIndex), formats, provider, style, dStartValue) 'Try to get start date from front portion of string
            bEndDateFound = bEndDateFound OrElse Date.TryParseExact(s.Substring(iIndex, iStringLength - iIndex), formats, provider, style, dEndValue) 'Try to get end date from end portion of string
            If bStartDateFound AndAlso bEndDateFound Then 'Determine if both start date and end date have been found
                result = New DateRange(dStartValue, dEndValue) 'set result
                Return True 'Conversion was succesfull
            End If
        Next
        result = Empty
        Return False 'Conversion was not succesfull
    End Function

#End Region

End Class
