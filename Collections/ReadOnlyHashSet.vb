'''<summary>Represents a read only set of values.</summary>
'''<typeparam name="T">The type of elements in the read only hash set.</typeparam>
Public Class ReadOnlyHashSet(Of T)
    Implements IEnumerable, IEnumerable(Of T), IReadOnlyCollection(Of T)

#Region "Private Items"

    Private ReadOnly _source As HashSet(Of T)

#End Region

#Region "Initializers"

    '''<summary>Initializes a new instance of the <see cref="ReadOnlyHashSet"/> class that uses the default equality comparer for the set type, contains elements copied from the specified collection, and has sufficient capacity to accommodate the number of elements compied.</summary>
    '''<param name="collection">The collection whose elements are copied to the new set.</param>
    '''<exception cref="ArgumentNullException"><paramref name="collection"/>collection is null.</exception>
    Public Sub New(collection As IEnumerable(Of T))
        If collection Is Nothing Then Throw New ArgumentNullException(NameOf(collection))
        If TypeOf collection Is ObservableHashSet(Of T) Then
            _source = CType(collection, ReadOnlyHashSet(Of T))._source
        ElseIf TypeOf collection Is HashSet(Of T) Then
            _source = collection
        Else
            _source = New HashSet(Of T)(collection)
        End If
    End Sub

#End Region

#Region "Properties"

    '''<summary>Gets the <see cref="IEqualityComparer"/> object that is used to determine equality for the values in the set.</summary>
    '''<returns>The <see cref="IEqualityComparer"/> object that is used to determine equality for the values in the set.</returns>
    Public ReadOnly Property Comparer As IEqualityComparer(Of T)
        Get
            Return _source.Comparer
        End Get
    End Property
    Public ReadOnly Property Count As Integer Implements IReadOnlyCollection(Of T).Count
        Get
            Return _source.Count
        End Get
    End Property

#End Region

#Region "Functions"

    '''<summary>Determines whether a <see cref="ReadOnlyHashSet"/> object contains the specified element.</summary>
    '''<returns><see langword="True"/> if the <see cref="ReadOnlyHashSet"/> object contains the specified element; otherwise, <see langword="False"/>.</returns>
    '''<param name="item">The element to locate in the <see cref="ReadOnlyHashSet"/> object.</param>
    Public Function Contains(item As T) As Boolean
        Return _source.Contains(item)
    End Function
    '''<summary>Copies the elements of a <see cref="ReadOnlyHashSet"/> object to an array.</summary>
    '''<param name="array">The one-dimensional array that is the destination fo the elements copied from the <see cref="ReadOnlyHashSet"/> object. The array must have zero-based indexing.</param>
    '''<exception cref="ArgumentNullException"><paramref name="array"/> is null.</exception>
    Public Sub CopyTo(array() As T)
        _source.CopyTo(array)
    End Sub
    '''<summary>Copies the specified elements of the <see cref="ReadOnlyHashSet"/> object to an array, starting at the specified array index.</summary>
    '''<param name="array">The one-dimensional array that is the destination fo the elements copied from the <see cref="ReadOnlyHashSet"/> object. The array must have zero-based indexing.</param>
    '''<param name="arrayIndex">The zero-based index in the array at which copying begins.</param>
    '''<exception cref="ArgumentNullException"><paramref name="array"/> is null.</exception>
    '''<exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception>
    '''<exception cref="ArgumentException"><paramref name="arrayIndex"/> is greater than the length of the destination array.</exception>
    Public Sub CopyTo(array() As T, arrayIndex As Integer)
        _source.CopyTo(array, arrayIndex)
    End Sub
    '''<summary>Copies the specified elements of the <see cref="ReadOnlyHashSet"/> object to an array, starting at the specified array index.</summary>
    '''<param name="array">The one-dimensional array that is the destination fo the elements copied from the <see cref="ReadOnlyHashSet"/> object. The array must have zero-based indexing.</param>
    '''<param name="arrayIndex">The zero-based index in the array at which copying begins.</param>
    '''<param name="count">The number of elements to copy to the array.</param>
    '''<exception cref="ArgumentNullException"><paramref name="array"/> is null.</exception>
    '''<exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0. -or- <paramref name="count"/> is less than 0.</exception>
    '''<exception cref="ArgumentException"><paramref name="arrayIndex"/> is greater than the length of the destination array. -or- <paramref name="count"/> is greater than the available space from the index to the end of the destination array.</exception>
    Public Sub CopyTo(array() As T, arrayIndex As Integer, count As Integer)
        _source.CopyTo(array, arrayIndex, count)
    End Sub
    '''<summary>returnsan <see cref="IEqualityComparer"/> object that can be used for equality testing of a <see cref="ReadOnlyHashSet"/> object.</summary>
    '''<returns>A <see cref="IEqualityComparer"/> object that can be used for deep equality testing of the <see cref="ReadOnlyHashSet"/> object.</returns>
    Public Shared Function CreateSetComparer() As IEqualityComparer(Of ReadOnlyHashSet(Of T))
        Return HashSet(Of T).CreateSetComparer()
    End Function
    Public Function GetEnumerator() As IEnumerator(Of T) Implements IEnumerable(Of T).GetEnumerator
        Return _source.GetEnumerator()
    End Function
    Private Function GetEnumeratorNonGeneric() As IEnumerator Implements IEnumerable.GetEnumerator
        Return _source.GetEnumerator
    End Function
    '''<summary>Determines whether a <see cref="ReadOnlyHashSet"/> object is a proper subset of the specified collection.</summary>
    '''<returns><see langword="True"/> if the <see cref="ReadOnlyHashSet"/> object is a proper subset of <paramref name="other"/>; otherwise, <see langword="False"/>.</returns>
    '''<param name="other">The collection to compare to the <see cref="ReadOnlyHashSet"/> object.</param>
    '''<exception cref="ArgumentNullException"><paramref name="other"/> is null.</exception>
    Public Function IsProperSubsetOf(other As IEnumerable(Of T)) As Boolean
        Return _source.IsProperSubsetOf(other)
    End Function
    '''<summary>Determines whether a <see cref="ReadOnlyHashSet"/> object is a proper superset of the specified collection.</summary>
    '''<returns><see langword="True"/> if the <see cref="ReadOnlyHashSet"/> object is a proper superset of <paramref name="other"/>; otherwise, <see langword="False"/>.</returns>
    '''<param name="other">The collection to compare to the <see cref="ReadOnlyHashSet"/> object.</param>
    '''<exception cref="ArgumentNullException"><paramref name="other"/> is null.</exception>
    Public Function IsProperSupersetOf(other As IEnumerable(Of T)) As Boolean
        Return _source.IsProperSupersetOf(other)
    End Function
    '''<summary>Determines whether a <see cref="ReadOnlyHashSet"/> object is a subset of the specified collection.</summary>
    '''<returns><see langword="True"/> if the <see cref="ReadOnlyHashSet"/> object is a subset of <paramref name="other"/>; otherwise, <see langword="False"/>.</returns>
    '''<param name="other">The collection to compare to the <see cref="ReadOnlyHashSet"/> object.</param>
    '''<exception cref="ArgumentNullException"><paramref name="other"/> is null.</exception>
    Public Function IsSubsetOf(other As IEnumerable(Of T)) As Boolean
        Return _source.IsSubsetOf(other)
    End Function
    '''<summary>Determines whether a <see cref="ReadOnlyHashSet"/> object is a superset of the specified collection.</summary>
    '''<returns><see langword="True"/> if the <see cref="ReadOnlyHashSet"/> object is a superset of <paramref name="other"/>; otherwise, <see langword="False"/>.</returns>
    '''<param name="other">The collection to compare to the <see cref="ReadOnlyHashSet"/> object.</param>
    '''<exception cref="ArgumentNullException"><paramref name="other"/> is null.</exception>
    Public Function IsSupersetOf(other As IEnumerable(Of T)) As Boolean
        Return _source.IsSupersetOf(other)
    End Function
    '''<summary>Determines whether the current <see cref="ReadOnlyHashSet"/> object and the specified collection share common elements.</summary>
    '''<returns><see langword="True"/> if the <see cref="ReadOnlyHashSet"/> object and <paramref name="other"/> share at least one commone element; otherwise, <see langword="False"/>.</returns>
    '''<param name="other">The collection to compare to the current <see cref="ReadOnlyHashSet"/> object.</param>
    '''<exception cref="ArgumentNullException"><paramref name="other"/> is null.</exception>
    Public Function Overlaps(other As IEnumerable(Of T)) As Boolean
        Return _source.Overlaps(other)
    End Function
    '''<summary>Determines whether a <see cref="ReadOnlyHashSet"/> object and the specified collection contains the same elements.</summary>
    '''<returns><see langword="True"/> if the <see cref="ReadOnlyHashSet"/> object is equal to the <paramref name="other"/>; otherwise, <see langword="False"/>.</returns>
    '''<param name="other">The collection to compare to the <see cref="ReadOnlyHashSet"/> object.</param>
    '''<exception cref="ArgumentNullException"><paramref name="other"/> is null.</exception>
    Public Function SetEquals(other As IEnumerable(Of T)) As Boolean
        Return _source.SetEquals(other)
    End Function
    '''<summary>Creates a <see cref="HashSet"/> from a <see cref="ReadOnlyHashSet"/>.</summary>
    '''<returns>A <see cref="HashSet"/> that contains the elements from the input sequence.</returns>
    Public Function ToHashset() As HashSet(Of T)
        Return New HashSet(Of T)(_source, _source.Comparer)
    End Function
    '''<summary>Searches the set for a given value and returns the equal value it finds, if any.</summary>
    '''<returns>A value indicating whether the search was successful.</returns>
    '''<param name="equalValue">The value to search for.</param>
    '''<param name="actualValue">The value from the set that the search found, or the default value of <typeparamref name="T"/> when the search yielded no match.</param>
    Public Function TryGetValue(equalValue As T, ByRef actualValue As T) As Boolean
        Return _source.TryGetValue(equalValue, actualValue)
    End Function

#End Region

End Class
