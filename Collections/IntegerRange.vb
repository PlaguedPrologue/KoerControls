Imports System.ComponentModel
Imports System.Globalization

'''<summary>Represents a set of consecutive dates expressed by a start <see cref="Integer"/> and end <see cref="Integer"/>.</summary>
<TypeConverter(GetType(IntegerRangeConverter))>
Public Class IntegerRange
    Implements IComparable(Of IntegerRange), IEnumerable, IEnumerable(Of Integer), IRange(Of Integer, IntegerRange), IReadOnlyCollection(Of Integer)

#Region "Private Items"

    Private _endValue As Integer
    Private _startValue As Integer

    Private Function GetEnumeratorNonGeneric() As IEnumerator Implements IEnumerable.GetEnumerator
        Return GetEnumerator()
    End Function

#End Region

#Region "Operators"

    Public Shared Operator =(left As IntegerRange, right As IntegerRange) As Boolean
        If left Is Nothing OrElse right Is Nothing Then Return left Is Nothing AndAlso right Is Nothing
        Return left._startValue = right._startValue AndAlso left._endValue = right._endValue
    End Operator
    Public Shared Operator <>(left As IntegerRange, right As IntegerRange) As Boolean
        Return Not left = right
    End Operator
    Public Shared Operator >(left As IntegerRange, right As IntegerRange) As Boolean
        If left._endValue = right._endValue Then Return left._startValue > right._startValue 'EndValues match, check StartValues
        Return left._endValue > right._endValue
    End Operator
    Public Shared Operator <(left As IntegerRange, right As IntegerRange) As Boolean
        If left._startValue = right._startValue Then Return left._endValue < right._endValue 'StartValues match, check EndValues
        Return left._startValue < right._startValue
    End Operator
    Public Shared Operator >=(left As IntegerRange, right As IntegerRange) As Boolean
        Return left._startValue >= right._startValue AndAlso left._endValue >= right._endValue
    End Operator
    Public Shared Operator <=(left As IntegerRange, right As IntegerRange) As Boolean
        Return left._startValue <= right._startValue AndAlso left._endValue <= right._endValue
    End Operator

    Public Shared Operator =(left As IntegerRange, right As Integer) As Boolean
        Return left._startValue = right AndAlso left._endValue = right
    End Operator
    Public Shared Operator <>(left As IntegerRange, right As Integer) As Boolean
        Return Not left = right
    End Operator
    Public Shared Operator >(left As IntegerRange, right As Integer) As Boolean
        Return left._startValue > right
    End Operator
    Public Shared Operator <(left As IntegerRange, right As Integer) As Boolean
        Return left._endValue < right
    End Operator
    Public Shared Operator >=(left As IntegerRange, right As Integer) As Boolean
        Return left._endValue >= right
    End Operator
    Public Shared Operator <=(left As IntegerRange, right As Integer) As Boolean
        Return left._startValue <= right
    End Operator

    Public Shared Operator =(left As Integer, right As IntegerRange)
        Return left = right._startValue AndAlso left = right._endValue
    End Operator
    Public Shared Operator <>(left As Integer, right As IntegerRange)
        Return Not left = right
    End Operator
    Public Shared Operator >(left As Integer, right As IntegerRange) As Boolean
        Return left > right._endValue
    End Operator
    Public Shared Operator <(left As Integer, right As IntegerRange) As Boolean
        Return left < right._startValue
    End Operator
    Public Shared Operator >=(left As Integer, right As IntegerRange) As Boolean
        Return left >= right._startValue
    End Operator
    Public Shared Operator <=(left As Integer, right As IntegerRange) As Boolean
        Return left <= right._endValue
    End Operator

    Public Function CompareTo(obj As IntegerRange) As Integer Implements IComparable(Of IntegerRange).CompareTo
        Dim iComparison As Integer = _startValue.CompareTo(obj._startValue) 'Compare StartValues
        If iComparison = 0 Then Return _endValue.CompareTo(obj._endValue) 'StartValues match, compare EndValues
        Return iComparison 'StartValues do not match, retunr StartValue comparison
    End Function
    Public Function Contains(value As Integer) As Boolean Implements IRange(Of Integer, IntegerRange).Contains
        Return value >= _startValue AndAlso value <= _endValue
    End Function
    Public Function ContainsAll(ParamArray values() As Integer) As Boolean Implements IRange(Of Integer, IntegerRange).ContainsAll
        For Each iValue As Integer In values
            If iValue <= _startValue OrElse iValue >= _endValue Then Return False
        Next
        Return True
    End Function
    Public Function ContainsAny(ParamArray values() As Integer) As Boolean Implements IRange(Of Integer, IntegerRange).ContainsAny
        For Each iValue As Integer In values
            If iValue >= _startValue AndAlso iValue <= _endValue Then Return True
        Next
        Return False
    End Function
    Public Function IsContinuousWith(other As IntegerRange) As Boolean Implements IRange(Of Integer, IntegerRange).IsContinuousWith
        If _startValue <= other._endValue AndAlso other._startValue <= _endValue Then Return True 'Ranges overlap
        Return _startValue - other._endValue = 1 OrElse other._startValue - _endValue = 1 'Ranges do not overlap. Determine if ranges are one value away from each other
    End Function
    Public Function IsProperSubsetOf(other As IntegerRange) As Boolean Implements IRange(Of Integer, IntegerRange).IsProperSubsetOf
        Return _startValue > other._startValue AndAlso _endValue < other._endValue
    End Function
    Public Function IsProperSupersetOf(other As IntegerRange) As Boolean Implements IRange(Of Integer, IntegerRange).IsProperSupersetOf
        Return _startValue < other._startValue AndAlso _endValue > other._endValue
    End Function
    Public Function IsSubsetOf(other As IntegerRange) As Boolean Implements IRange(Of Integer, IntegerRange).IsSubsetOf
        Return _startValue >= other._startValue AndAlso _endValue <= other._endValue
    End Function
    Public Function IsSupersetOf(other As IntegerRange) As Boolean Implements IRange(Of Integer, IntegerRange).IsSupersetOf
        Return _startValue <= other._startValue AndAlso _endValue >= other._endValue
    End Function
    Public Function Overlaps(other As IntegerRange) As Boolean Implements IRange(Of Integer, IntegerRange).Overlaps
        If _startValue <= other._startValue Then Return _endValue >= other._startValue
        Return _startValue <= other._endValue
    End Function

#End Region

#Region "Initializers"

    '''<summary>Initializes a new empty instance of the <see cref="IntegerRange"/> collection at value (0, 0).</summary>
    Public Sub New()
        _startValue = 0
        _endValue = 0
    End Sub
    '''<summary>Initializes a new instance of the <see cref="IntegerRange"/> class.</summary>
    '''<param name="value">The start and end <see cref="Integer"/> of the <see cref="IntegerRange"/>.</param>
    Public Sub New(value As Integer)
        _startValue = value
        _endValue = value
    End Sub
    '''<summary>Initializes a new instance of the <see cref="IntegerRange"/> class.</summary>
    '''<param name="startValue">The start <see cref="Integer"/> of the <see cref="IntegerRange"/>.</param>
    '''<param name="endValue">The end <see cref="Integer"/> of the <see cref="IntegerRange"/>.</param>
    Public Sub New(startValue As Integer, endValue As Integer)
        If startValue < endValue Then
            _startValue = startValue
            _endValue = endValue
        Else
            _startValue = endValue
            _endValue = startValue
        End If
    End Sub

#End Region

#Region "Events"

    '''<summary>Occurs when the end value is been changed.</summary>
    Public Event EndValueChanged As EventHandler
    '''<summary>Occurs when the start value is been changed.</summary>
    Public Event StartValueChanged As EventHandler

#End Region

#Region "Properties"

    '''<summary>Represents an empty integer range at value 0.</summary>
    '''<returns>An empty integer range at value 0.</returns>
    Public Shared ReadOnly Property Empty As IntegerRange
        Get
            Return New IntegerRange
        End Get
    End Property
    <DefaultValue(0)>
    <Description("The ending value of the range.")>
    Public Property EndValue As Integer Implements IRange(Of Integer, IntegerRange).EndValue
        Get
            Return _endValue
        End Get
        Set(value As Integer)
            If value = _endValue Then Return
            If value < _startValue Then
                _endValue = _startValue
                _startValue = value
                OnStartValueChanged(EventArgs.Empty)
            Else
                _endValue = value
            End If
            OnEndValueChanged(EventArgs.Empty)
        End Set
    End Property
    <Browsable(False)>
    Public ReadOnly Property Length As Integer Implements IRange(Of Integer, IntegerRange).Length, IReadOnlyCollection(Of Integer).Count
        Get
            Return _endValue - _startValue
        End Get
    End Property
    '''<summary>Represents the largest possible value of <see cref="IntegerRange"/>.</summary>
    '''<returns>The largest possible value of <see cref="IntegerRange"/>.</returns>
    <Browsable(False)>
    Public Shared ReadOnly Property MaxValue As IntegerRange
        Get
            Return New IntegerRange(Integer.MaxValue, Integer.MaxValue)
        End Get
    End Property
    '''<summary>Represents the smallest possible value of <see cref="IntegerRange"/>.</summary>
    '''<returns>The smallest possible value of <see cref="IntegerRange"/>.</returns>
    <Browsable(False)>
    Public Shared ReadOnly Property MinValue As IntegerRange
        Get
            Return New IntegerRange(Integer.MinValue, Integer.MinValue)
        End Get
    End Property
    <DefaultValue(0)>
    <Description("The starting value of the range.")>
    Public Property StartValue As Integer Implements IRange(Of Integer, IntegerRange).StartValue
        Get
            Return _startValue
        End Get
        Set(value As Integer)
            If value = _startValue Then Return
            If value > _endValue Then
                _startValue = _endValue
                _endValue = value
                OnEndValueChanged(EventArgs.Empty)
            Else
                _startValue = value
            End If
            OnStartValueChanged(EventArgs.Empty)
        End Set
    End Property

#End Region

#Region "Functions"

    Public Function Add(value As Integer) As IntegerRange Implements IRange(Of Integer, IntegerRange).AddValue
        Return New IntegerRange(_startValue + value, _endValue + value)
    End Function
    Public Function AddToEnd(value As Integer) As IntegerRange Implements IRange(Of Integer, IntegerRange).AddValueToEnd
        If -value > _endValue - _startValue Then Throw New ArgumentOutOfRangeException("Cannot set EndValue of IntegerRange to be less than the current StartValue.")
        Return New IntegerRange(_startValue, _endValue + value)
    End Function
    Public Function AddToStart(value As Integer) As IntegerRange Implements IRange(Of Integer, IntegerRange).AddValueToStart
        If value > _endValue - _startValue Then Throw New ArgumentOutOfRangeException("Cannot set StartValue of IntegerRange to be greater than the current EndValue.")
        Return New IntegerRange(_startValue + value, _endValue)
    End Function
    Public Iterator Function GetEnumerator() As IEnumerator(Of Integer) Implements IEnumerable(Of Integer).GetEnumerator
        Dim iCurrentValue As Integer
        While iCurrentValue <= _endValue
            Yield iCurrentValue
            iCurrentValue += 1
        End While
    End Function
    Public Function InnerJoin(other As IntegerRange) As IntegerRange Implements IRange(Of Integer, IntegerRange).InnerJoin
        If _startValue < other._startValue Then
            If _endValue > other._endValue Then Return New IntegerRange(other._startValue, other._endValue)
            Return New IntegerRange(other._startValue, _endValue)
        End If
        If _endValue > other._endValue Then Return New IntegerRange(_startValue, other._endValue)
        Return New IntegerRange(_startValue, _endValue)
    End Function
    '''<summary>Merges each overlapping <see cref="IntegerRange"/>, iterating through the each joined <see cref="IntegerRange"/>, sorted from least to greatest.</summary>
    '''<returns>Each joined <see cref="IntegerRange"/>, sorted from least to greatest.</returns>
    Public Shared Iterator Function Join(ranges() As IntegerRange) As IEnumerable(Of IntegerRange)
        Dim irBuildingRange As IntegerRange = Nothing
        For Each irIntegerRange As IntegerRange In ranges.OrderBy(Function(ir) ir)
            If irBuildingRange Is Nothing Then
                irBuildingRange = New IntegerRange(irIntegerRange.StartValue, irIntegerRange.EndValue)
            Else
                If irBuildingRange.IsContinuousWith(irIntegerRange) Then
                    irBuildingRange._endValue = irIntegerRange._endValue
                Else
                    Yield irBuildingRange
                    irBuildingRange = New IntegerRange(irIntegerRange.StartValue, irIntegerRange.EndValue)
                End If
            End If
        Next
        If irBuildingRange IsNot Nothing Then
            Yield irBuildingRange
        End If
    End Function
    '''<summary>Merges each overlapping <see cref="IntegerRange"/>, iterating through the each joined <see cref="IntegerRange"/>, sorted from least to greatest.</summary>
    '''<returns>Each joined <see cref="IntegerRange"/>, sorted from least to greatest.</returns>
    Public Shared Iterator Function Join(ranges As IntegerRangeCollection) As IEnumerable(Of IntegerRange)
        Dim irBuildingRange As IntegerRange = Nothing
        For Each irIntegerRange As IntegerRange In ranges.OrderBy(Function(ir) ir)
            If irBuildingRange Is Nothing Then
                irBuildingRange = irIntegerRange
            Else
                If irBuildingRange.IsContinuousWith(irIntegerRange) Then
                    irBuildingRange.EndValue = irIntegerRange.EndValue
                Else
                    Yield irBuildingRange
                    irBuildingRange = irIntegerRange
                End If
            End If
        Next
        If irBuildingRange IsNot Nothing Then
            Yield irBuildingRange
        End If
    End Function
    '''<summary>Raises the <see cref="EndValueChanged"/> event.</summary>
    '''<param name="e">The <see cref="EventArgs"/> containing the event data.</param>
    Protected Overridable Sub OnEndValueChanged(e As EventArgs)
        RaiseEvent EndValueChanged(Me, e)
    End Sub
    '''<summary>Raises the <see cref="StartValueChanged"/> event.</summary>
    '''<param name="e">The <see cref="EventArgs"/> containing the event data.</param>
    Protected Overridable Sub OnStartValueChanged(e As EventArgs)
        RaiseEvent StartValueChanged(Me, e)
    End Sub
    Public Overrides Function ToString() As String
        Return $"{_startValue}, {_endValue}"
    End Function
    Public Overloads Function ToString(Optional delimiter As String = "to") As String Implements IRange(Of Integer, IntegerRange).ToString
        delimiter = delimiter.Trim 'Get the delimiter
        If String.IsNullOrEmpty(delimiter) Then Return $"{_startValue} {_endValue}" 'Delimiter is empty
        If Char.IsPunctuation(delimiter(0)) Then Return $"{_startValue}{delimiter} {_endValue}" 'Delimiter starts with punctuation
        Return $"{_startValue} {delimiter} {_endValue}" 'Standard delimiter
    End Function
    Public Overloads Function ToString(font As Font, Optional delimiter As String = "to") As String Implements IRange(Of Integer, IntegerRange).ToString
        If font Is Nothing Then font = SystemFonts.DefaultFont 'Get font
        Dim iTargetWidth As Integer = TextRenderer.MeasureText("00/00/0000", font).Width 'Get target width of start date
        Dim iSpaceWidth As Integer = TextRenderer.MeasureText("  ", font).Width - TextRenderer.MeasureText(" ", font).Width 'Get width of [space] characters in the provided font

        'Get the date strings
        Dim sStartValue As String = _startValue.ToString
        delimiter = delimiter.Trim 'Get the delimiter
        If String.IsNullOrEmpty(delimiter) Then Return $"{sStartValue}{New String(" ", Math.Max(0, (iTargetWidth - TextRenderer.MeasureText(sStartValue, font).Width) / iSpaceWidth))} {_endValue}" 'Delimiter is empty
        If Char.IsPunctuation(delimiter(0)) Then Return $"{sStartValue}{New String(" ", Math.Max(0, (iTargetWidth - TextRenderer.MeasureText(sStartValue, font).Width) / iSpaceWidth))}{delimiter} {_endValue}" 'Delimiter starts with punctuation
        Return $"{sStartValue}{New String(" ", Math.Max(0, (iTargetWidth - TextRenderer.MeasureText(sStartValue, font).Width) / iSpaceWidth))} {delimiter} {_endValue}" 'Standard delimiter
    End Function
    '''<summary>Returns a new <see cref="IntegerRange"/> that contains all the values that fall within and between each specified <see cref="IntegerRange"/>.</summary>
    '''<returns>A new <see cref="IntegerRange"/> that contains all the values that fall within each specified <see cref="IntegerRange"/>.</returns>
    '''<param name="ranges">Each <see cref="IntegerRange"/> to combine.</param>
    Public Shared Function Union(ranges() As IntegerRange) As IntegerRange
        If ranges.FirstOrDefault Is Nothing Then Return MinValue
        Dim iStartValue As Integer = ranges(0)._startValue
        Dim iEndValue As Integer = ranges(0)._endValue
        For Each irRange As IntegerRange In ranges.Skip(1)
            If irRange._startValue < iStartValue Then iStartValue = irRange._startValue
            If irRange._endValue > iEndValue Then iEndValue = irRange._endValue
        Next
        Return New IntegerRange(iStartValue, iEndValue)
    End Function
    Public Function UnionWith(other As IntegerRange) As IntegerRange Implements IRange(Of Integer, IntegerRange).UnionWith
        If _startValue < other._startValue Then
            If _endValue > other._endValue Then Return New IntegerRange(_startValue, _endValue)
            Return New IntegerRange(_startValue, other._endValue)
        End If
        If _endValue > other._endValue Then Return New IntegerRange(other._startValue, _endValue)
        Return New IntegerRange(other._startValue, other._endValue)
    End Function

#End Region

#Region "Parsing"

    '''<summary>Converts the string representation of two numbers to its 32-bit signed integer range equivalent.</summary>
    '''<returns>A 32-bit signed integer range equivalent to the numbers contained in <paramref name="s"/>.</returns>
    '''<param name="s">A string containing the numbers to convert.</param>
    '''<exception cref="ArgumentNullException"><paramref name="s"/> is null.</exception>
    '''<exception cref="FormatException"><paramref name="s"/> is not in the correct format.</exception>
    '''<exception cref="OverflowException"><paramref name="s"/> represents a number less than <see cref="Int32.MinValue"/> or greater than <see cref="Int32.MaxValue"/>. -or- <paramref name="s"/> includes non-zero, fractional digits.</exception>
    Public Shared Function Parse(s As String) As IntegerRange
        Dim iStringLength As Integer = s.Length
        Dim bStartValueFound As Boolean = False
        Dim bEndValueFound As Boolean = False
        Dim iStartValue As Integer
        Dim iEndValue As Integer
        For iIndex As Integer = 0 To iStringLength - 1 'Index through string
            Dim sStartValue As String = s.Substring(0, iStringLength - iIndex)
            bStartValueFound = bStartValueFound OrElse Integer.TryParse(sStartValue, iStartValue) 'Try to get start value from front portion of string
            If Not bStartValueFound AndAlso Double.TryParse(sStartValue, Nothing) Then Exit For 'Double found, kick out for incorrect format.
            Dim sEndValue As String = s.Substring(iIndex, iStringLength - iIndex)
            bEndValueFound = bEndValueFound OrElse Integer.TryParse(sEndValue, iEndValue) 'Try to get end value from end portion of string
            If Not bEndValueFound AndAlso Double.TryParse(sEndValue, Nothing) Then Exit For 'Double found, kick out for incorrect format.
            If bStartValueFound AndAlso bEndValueFound Then Return New IntegerRange(iStartValue, iEndValue)  'Determine if both start value and end value have been found
        Next
        Throw New FormatException($"Cannot convert the string '{s}' to IntegerRange.")
    End Function
    '''<summary>Converts the string representation of two numbers in a specified style and culture-specific format to its 32-bit signed integer range equivalent.</summary>
    '''<returns>A 32-bit signed integer range equivalent to the two numbers specified in s<paramref name="s"/></returns>
    '''<param name="s">A string containing the numbers to convert.</param>
    '''<param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="s"/>. A typical value to specify is <see cref="NumberStyles.Integer"/>.</param>
    '''<param name="provider">An object that supplies culture-specific information about the format of <paramref name="s"/>.</param>
    '''<exception cref="ArgumentException"><paramref name="style"/> is not a <see cref="NumberStyles"/> value. -or- <paramref name="style"/> is not a combination of <see cref="NumberStyles.AllowHexSpecifier"/> and <see cref="NumberStyles.HexNumber"/> values.</exception>
    '''<exception cref="ArgumentNullException"><paramref name="s"/> is null.</exception>
    '''<exception cref="FormatException"><paramref name="s"/> is not in a format compliant with <paramref name="style"/>.</exception>
    '''<exception cref="OverflowException"><paramref name="s"/> represents a number less than <see cref="Int32.MinValue"/> or greater than <see cref="Int32.MaxValue"/>. -or- <paramref name="s"/> includes non-zero, fractional digits.</exception>
    Public Shared Function Parse(s As String, style As NumberStyles, provider As IFormatProvider) As IntegerRange
        Dim iStringLength As Integer = s.Length
        Dim bStartValueFound As Boolean = False
        Dim bEndValueFound As Boolean = False
        Dim iStartValue As Integer
        Dim iEndValue As Integer
        For iIndex As Integer = 0 To iStringLength - 1 'Index through string
            Dim sStartValue As String = s.Substring(0, iStringLength - iIndex)
            bStartValueFound = bStartValueFound OrElse Integer.TryParse(sStartValue, style, provider, iStartValue) 'Try to get start value from front portion of string
            If Not bStartValueFound AndAlso Double.TryParse(sStartValue, style, provider, Nothing) Then Exit For 'Double found, kick out for incorrect format.
            Dim sEndValue As String = s.Substring(iIndex, iStringLength - iIndex)
            bEndValueFound = bEndValueFound OrElse Integer.TryParse(sEndValue, style, provider, iEndValue) 'Try to get end value from end portion of string
            If Not bEndValueFound AndAlso Double.TryParse(sEndValue, style, provider, Nothing) Then Exit For 'Double found, kick out for incorrrect format.
            If bStartValueFound AndAlso bEndValueFound Then Return New IntegerRange(iStartValue, iEndValue)  'Determine if both start value and end value have been found
        Next
        Throw New FormatException($"Cannot convert the string '{s}' to IntegerRange using the specified provider and styles.")
    End Function
    '''<summary>Converts the string representation of two numbers to its 32-bit signed integer range equivalent. A return value indicates whether the conversion succeeded.</summary>
    '''<returns><see langword="True"/> if <paramref name="s"/> was converted succesfully; otherwise <see langword="False"/></returns>
    '''<param name="s">A string containing the numbers to convert.</param>
    '''<param name="result">When this method returns, contains the 32-bit signed integer range value equivalent of the numbers contained in <paramref name="s"/>, if the conversion succeeded, or <see cref="Empty"/> if the conversion failed. The conversion fails if the <paramref name="s"/> parameter is null or <see cref="System.String.Empty"/>, is not of the correct format, or represents a any number less than <see cref="Int32.MinValue"/> or greater than <see cref="Int32.MaxValue"/>. This parameter is passed uninitialized; any value originally supplied in result will be overwritten.</param>
    Public Shared Function TryParse(s As String, ByRef result As IntegerRange) As Boolean
        Dim iStringLength As Integer = s.Length
        Dim bStartValueFound As Boolean = False
        Dim bEndValueFound As Boolean = False
        Dim iStartValue As Integer
        Dim iEndValue As Integer
        For iIndex As Integer = 0 To iStringLength - 1 'Index through string
            Dim sStartValue As String = s.Substring(0, iStringLength - iIndex)
            bStartValueFound = bStartValueFound OrElse Integer.TryParse(sStartValue, iStartValue) 'Try to get start value from front portion of string
            If Not bStartValueFound AndAlso Double.TryParse(sStartValue, Nothing) Then Exit For 'Double found, kick out for incorrect format.
            Dim sEndValue As String = s.Substring(iIndex, iStringLength - iIndex)
            bEndValueFound = bEndValueFound OrElse Integer.TryParse(sEndValue, iEndValue) 'Try to get end value from end portion of string
            If Not bEndValueFound AndAlso Double.TryParse(sEndValue, Nothing) Then Exit For 'Double found, kick out for incorrect format.
            If bStartValueFound AndAlso bEndValueFound Then 'Determine if both start value and end value have been found
                result = New IntegerRange(iStartValue, iEndValue) 'Set result
                Return True 'Conversion was succesfull
            End If
        Next
        result = Empty
        Return False 'Conversion was not successful.
    End Function
    '''<summary>Converts the string representation of two numbers in a specified style and culture-specific format to its 32-bit signed integer range equivalent. A return value indicates whether the conversion succeeded.</summary>
    '''<returns><see langword="True"/> if <paramref name="s"/> was converted succesfully; otherwise <see langword="False"/></returns>
    '''<param name="s">A string containing the numbers to convert. The string is interpreted using the style specified by style.</param>
    '''<param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="s"/>. A typical value to specify is <see cref="NumberStyles.Integer"/>.</param>
    '''<param name="provider">An object that supplies culture-specific formatting information about <paramref name="s"/>.</param>
    '''<param name="result">When this method returns, contains the 32-bit signed integer range value equivalent of the numbers contained in <paramref name="s"/>, if the conversion succeeded, or <see cref="Empty"/> if the conversion failed. The conversion fails if the <paramref name="s"/> parameter is null or <see cref="System.String.Empty"/>, is not in a format compliant with <paramref name="style"/>, or represents a number less than <see cref="Int32.MinValue"/> or greater than <see cref="Int32.MaxValue"/>. This parameter is passed uninitialized; any value originally supplied in result will be overwritten.</param>
    '''<exception cref="ArgumentException"><paramref name="style"/> is not a <see cref="NumberStyles"/> value. -or- <paramref name="style"/> is not a combination of <see cref="NumberStyles.AllowHexSpecifier"/> and <see cref="NumberStyles.HexNumber"/> values.</exception>
    Public Shared Function TryParse(s As String, style As NumberStyles, provider As IFormatProvider, ByRef result As IntegerRange) As Boolean
        Dim iStringLength As Integer = s.Length
        Dim bStartValueFound As Boolean = False
        Dim bEndValueFound As Boolean = False
        Dim iStartValue As Integer
        Dim iEndValue As Integer
        For iIndex As Integer = 0 To iStringLength - 1 'Index through string
            Dim sStartValue As String = s.Substring(0, iStringLength - iIndex)
            bStartValueFound = bStartValueFound OrElse Integer.TryParse(sStartValue, style, provider, iStartValue) 'Try to get start value from front portion of string
            If Not bStartValueFound AndAlso Double.TryParse(sStartValue, style, provider, Nothing) Then Exit For 'Double found, kick out for incorrect format.
            Dim sEndValue As String = s.Substring(iIndex, iStringLength - iIndex)
            bEndValueFound = bEndValueFound OrElse Integer.TryParse(sEndValue, style, provider, iEndValue) 'Try to get end value from end portion of string
            If Not bEndValueFound AndAlso Double.TryParse(sEndValue, style, provider, Nothing) Then Exit For 'Double found, kick out for incorrect format.
            If bStartValueFound AndAlso bEndValueFound Then 'Determine if both start value and end value have been found
                result = New IntegerRange(iStartValue, iEndValue) 'Set result
                Return True 'Conversion was succesfull
            End If
        Next
        result = Empty
        Return False 'Conversion was not successful.
    End Function

#End Region

End Class
