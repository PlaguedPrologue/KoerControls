'''<summary>Represents a collection of weak references to class objects.</summary>
'''<typeparam name="T">The type of object the <see cref="ReadOnlyWeakCollection"/> contains.</typeparam>
Public Class ReadOnlyWeakCollection(Of T As {Class, IDisposable})
    Implements IEnumerable(Of T), IEnumerable, IReadOnlyCollection(Of T)

#Region "Private Items"

    Private ReadOnly _source As WeakCollection(Of T)

    Private Function GetEnumeratorNonGeneric() As IEnumerator Implements IEnumerable.GetEnumerator
        Return GetEnumerator()
    End Function

#End Region

#Region "Initializers"

    '''<summary>Initializes a new instacne of the <see cref="ReadOnlyWeakCollection"/> class tha contains elements copied from the specified collection.</summary>
    '''<param name="collection">The collection whose elements are copied to the new set.</param>
    '''<exception cref="ArgumentNullException"><paramref name="collection"/> collection is null.</exception>
    Public Sub New(collection As IEnumerable(Of T))
        If collection Is Nothing Then Throw New ArgumentNullException(NameOf(collection))
        If TypeOf collection Is ReadOnlyWeakCollection(Of T) Then
            _source = CType(collection, ReadOnlyWeakCollection(Of T))._source
        ElseIf TypeOf collection Is WeakCollection(Of T) Then
            _source = collection
        Else
            _source = New WeakCollection(Of T)(collection)
        End If
    End Sub

#End Region

#Region "Properties"

    Public ReadOnly Property Count As Integer Implements IReadOnlyCollection(Of T).Count
        Get
            Return _source.Count
        End Get
    End Property
    '''<summary>Iterates through an <see cref="IEnumerable"/> wrapper around the <see cref="WeakCollection"/>.</summary>
    '''<returns>An <see cref="IEnumerable"/> wapper around the <see cref="WeakCollection"/>.</returns>
    Public ReadOnly Iterator Property Items As IEnumerable(Of T)
        Get
            For Each tItem As T In _source
                Yield tItem
            Next
        End Get
    End Property

#End Region

#Region "Functions"

    Public Function Contains(item As T) As Boolean
        Return _source.Contains(item)
    End Function
    Public Sub CopyTo(array() As T, arrayIndex As Integer)
        _source.CopyTo(array, arrayIndex)
    End Sub
    '''<summary>Gets the first <typeparamref name="T"/> that fulfills the provided <see cref="Func(Of T, TResult)"/>.</summary>
    '''<returns>The first <typeparamref name="T"/> that fulfills the provided <see cref="Func(Of T, TResult)"/> if it exists; otherwise, <see langword="Nothing"/>.</returns>
    '''<param name="predicate">A function to test each element for a condition.</param>
    Public Function FirstOrDefault(predicate As Func(Of T, Boolean)) As T
        Return _source.FirstOrDefault(predicate)
    End Function
    Public Iterator Function GetEnumerator() As IEnumerator(Of T) Implements IEnumerable(Of T).GetEnumerator
        Yield _source.GetEnumerator
    End Function
    '''<summary>Iterates through each <typeparamref name="T"/> that fulfills the provided <see cref="Func(Of T, TResult)"/>.</summary>
    '''<returns>Each <typeparamref name="T"/> that fulfills the provided <see cref="Func(Of T, TResult)"/> if it exists; otherwise, <see langword="Nothing"/></returns>
    '''<param name="predicate">A function to test each element for a condition.</param>
    Public Iterator Function Where(predicate As Func(Of T, Boolean)) As IEnumerable(Of T)
        For Each tItem As T In _source.Where(predicate)
            Yield tItem
        Next
    End Function

#End Region

End Class
