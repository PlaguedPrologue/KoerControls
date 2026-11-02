'''<summary>Represents a set of consecutive values expressed by a start value and end value.</summary>
'''<typeparam name="T">The <see cref="Type"/> of value the range encompasses.</typeparam>
'''<typeparam name="TSelf">The <see cref="Type"/> implementing the <see cref="IRange"/> interface.</typeparam>
Public Interface IRange(Of T, TSelf As IRange(Of T, TSelf))
    Inherits IEnumerable, IEnumerable(Of T), IReadOnlyCollection(Of T)

    '''<summary>Gets or sets the end value associated with the <see cref="IRange"/>.</summary>
    '''<returns>The end value associated with the <see cref="IRange"/>.</returns>
    ReadOnly Property EndValue As T
    '''<summary>Gets the total number of elements in the <see cref="IRange"/>.</summary>
    '''<returns>The total number of elements in the <see cref="IRange"/>.</returns>
    ReadOnly Property Length As Integer
    '''<summary>Gets or sets the start value associated with the <see cref="IRange"/>.</summary>
    '''<returns>The start value associated with the <see cref="IRange"/>.</returns>
    ReadOnly Property StartValue As T

    '''<summary>Determines whether a <see cref="IRange"/> object contains the specified value.</summary>
    '''<returns><see langword="True"/> if the <see cref="IRange"/> object contains the specified value; otherwise, <see langword="False"/>.</returns>
    '''<param name="value">The value to locate in the <see cref="IRange"/>.</param>
    Function Contains(value As T) As Boolean
    '''<summary>Determines if each value is contained within the <see cref="IRange"/>.</summary>
    '''<returns><see langword="True"/> if each value is contained within the <see cref="IRange"/>; otherwise, <see langword="False"/>.</returns>
    '''<param name="values">Each value to compare to the <see cref="IRange"/>.</param>
    Function ContainsAll(ParamArray values() As T) As Boolean
    '''<summary>Determines if any value is contained within the <see cref="IRange"/>.</summary>
    '''<returns><see langword="True"/> if any value is contained within the <see cref="IRange"/>; otherwise, <see langword="False"/>.</returns>
    '''<param name="values">Each value to compare to the <see cref="IRange"/>.</param>
    Function ContainsAny(ParamArray values() As T) As Boolean
    '''<summary>Determines whether the current <see cref="IRange"/> is at most one <typeparamref name="T"/> value away from the specified <see cref="IRange"/> object.</summary>
    '''<returns><see langword="True"/> if the current <see cref="IRange"/> is at most one <typeparamref name="T"/> value away from the specified <see cref="IRange"/>.</returns>
    '''<param name="other">The <see cref="IRange"/> to compare to the current <see cref="IRange"/> object.</param>
    Function IsContinuousWith(other As TSelf) As Boolean
    '''<summary>Determines whether the current <see cref="IRange"/> is a proper subset of the specified <see cref="IRange"/> object.</summary>
    '''<returns><see langword="True"/> if the current <see cref="IRange"/> object is a proper subset of <paramref name="other"/>; otherwise, <see langword="False"/>.</returns>
    '''<param name="other">The <see cref="IRange"/> to compare to the current <see cref="IRange"/> object.</param>
    Function IsProperSubsetOf(other As TSelf) As Boolean
    '''<summary>Determines whether the current <see cref="IRange"/> object is a proper superset of the specified <see cref="IRange"/> object.</summary>
    '''<returns><see langword="True"/> if the current <see cref="IRange"/> object is a proper superset of <paramref name="other"/>; otherwise, <see langword="False"/>.</returns>
    '''<param name="other">The <see cref="IRange"/> to compare to the current <see cref="IRange"/> object.</param>
    Function IsProperSupersetOf(other As TSelf) As Boolean
    '''<summary>Determines whether the current <see cref="IRange"/> is a subset of the specified <see cref="IRange"/> object.</summary>
    '''<returns><see langword="True"/> if the current <see cref="IRange"/> object is a subset of <paramref name="other"/>; otherwise, <see langword="False"/>.</returns>
    '''<param name="other">The <see cref="IRange"/> to compare to the current <see cref="IRange"/> object.</param>
    Function IsSubsetOf(other As TSelf) As Boolean
    '''<summary>Determines whether the current <see cref="IRange"/> object is a superset of the specified <see cref="IRange"/> object.</summary>
    '''<returns><see langword="True"/> if the current <see cref="IRange"/> object is a superset of <paramref name="other"/>; otherwise, <see langword="False"/>.</returns>
    '''<param name="other">The <see cref="IRange"/> to compare to the current <see cref="IRange"/> object.</param>
    Function IsSupersetOf(other As TSelf) As Boolean
    '''<summary>Determines whether the current <see cref="IRange"/> object and the specified <see cref="IRange"/> object share at least one common value.</summary>
    '''<returns><see langword="True"/> if the <see cref="IRange"/> object and <paramref name="other"/> share at least one common value; otherwise, <see langword="False"/>.</returns>
    '''<param name="other">The <see cref="IRange"/> to compare to the current <see cref="IRange"/> object.</param>
    Function Overlaps(other As TSelf) As Boolean

    '''<summary>Returns a new <see cref="IRange"/> that adds the specified number of values to the start and end values of this instance.</summary>
    '''<returns>A new <see cref="IRange"/> that adds the specified number of values to the start and end values of this instance.</returns>
    '''<param name="value">The number of values to add to the <see cref="IRange"/>. The value parameter can be positive or negative.</param>
    Function AddValue(value As Integer) As TSelf
    '''<summary>Returns a new <see cref="IRange"/> that adds the specified number of values to the end value of this instance.</summary>
    '''<returns>A new <see cref="IRange"/> that adds the specified number of values to the end value of this instance.</returns>
    '''<param name="value">The number of values to add to the <see cref="EndValue"/> of the <see cref="IRange"/>. The value parameter can be positive or negative.</param>
    '''<exception cref="ArgumentOutOfRangeException">New <see cref="EndValue"/> lies before the <see cref="StartValue"/>.</exception>
    Function AddValueToEnd(value As Integer) As TSelf
    '''<summary>Returns a new <see cref="IRange"/> that adds the specified number of values to the start value of this instance.</summary>
    '''<returns>A new <see cref="IRange"/> that adds the specified number of values to the start value of this instance.</returns>
    '''<param name="value">The number of values to add to the <see cref="StartValue"/> of the <see cref="IRange"/>. The value parameter can be positive or negative.</param>
    '''<exception cref="ArgumentOutOfRangeException">New <see cref="StartValue"/> lies before the <see cref="EndValue"/>.</exception>
    Function AddValueToStart(value As Integer) As TSelf
    '''<summary>Returns a new <see cref="IRange"/> that contains all values that are in both the current <see cref="IRange"/> and the specified <see cref="IRange"/>.</summary>
    '''<returns>A new <see cref="IRange"/> that contains all values that are in both the current <see cref="IRange"/> and the specified <see cref="IRange"/>.</returns>
    '''<param name="other">The <see cref="IRange"/> to combine with the current <see cref="IRange"/> object.</param>
    Function InnerJoin(other As TSelf) As TSelf
    '''<summary>Converts the values of the current <see cref="IRange"/> objects to their equivalent string representation.</summary>
    '''<returns>A string that contains the string representations of the current <see cref="IRange"/> objects.</returns>
    '''<param name="delimiter">The separating string between the start value and end value in the <see cref="IRange"/>.</param>
    Function ToString(Optional delimiter As String = "to") As String
    '''<summary>Converts the values of the current <see cref="IRange"/> objects to their equivalent string representation.</summary>
    '''<remarks>Extra space is added after the <see cref="StartValue"/> to make lining up string vertically look crisp.</remarks>
    '''<returns>A string that contains the string representations of the current <see cref="IRange"/> objects.</returns>
    '''<param name="font">The <see cref="Font"/> used to display the string.</param>
    '''<param name="delimiter">The separating string between the start value and end value in the <see cref="IRange"/>.</param>
    Function ToString(font As Font, Optional delimiter As String = "to") As String
    '''<summary>Returns a new <see cref="IRange"/> that contains all the values that fall within and between the current <see cref="IRange"/> and the specified <see cref="IRange"/>.</summary>
    '''<returns>A new <see cref="IRange"/> that contains all the values that fall within and between the current <see cref="IRange"/> and the specified <see cref="IRange"/>.</returns>
    '''<param name="other">The <see cref="IRange"/> to combine with the current <see cref="IRange"/> object.</param>
    Function UnionWith(other As TSelf) As TSelf

End Interface
