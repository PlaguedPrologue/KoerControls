Imports System.Collections.Concurrent

'''<summary>Represents a collection of weak references to class objects.</summary>
'''<typeparam name="T">The type of object the <see cref="WeakCollection"/> contains.</typeparam>
Public Class WeakCollection(Of T As {Class, IDisposable})
    Implements ICollection(Of T), IEnumerable(Of T), IEnumerable, IReadOnlyCollection(Of T)

#Region "Private Items"

    Private ReadOnly _instances As New ConcurrentDictionary(Of WeakReference(Of T), Byte)(ReferenceEqualityComparer.Instance)

    Private Function GetEnumeratorNonGeneric() As IEnumerator Implements IEnumerable.GetEnumerator
        Return GetEnumerator()
    End Function

#End Region

#Region "Operators"

    Public Function Contains(item As T) As Boolean Implements ICollection(Of T).Contains
        If item Is Nothing Then Return False
        For Each kvpItem As KeyValuePair(Of WeakReference(Of T), Byte) In _instances
            Dim tItem As T = Nothing
            If kvpItem.Key.TryGetTarget(tItem) AndAlso tItem IsNot Nothing AndAlso Not tItem.IsDisposed Then
                If ReferenceEquals(tItem, item) Then Return True
            Else
                _instances.TryRemove(kvpItem.Key, Nothing)
            End If
        Next
        Return False
    End Function

#End Region

#Region "Initializers"

    '''<summary>Initializes a new instance of the <see cref="WeakCollection"/> class.</summary>
    Public Sub New()
    End Sub
    '''<summary>Initializes a new instacne of the <see cref="WeakCollection"/> class tha contains elements copied from the specified collection.</summary>
    '''<param name="collection">The collection whose elements are copied to the new set.</param>
    '''<exception cref="ArgumentNullException"><paramref name="collection"/> collection is null.</exception>
    Public Sub New(collection As IEnumerable(Of T))
        If collection Is Nothing Then Throw New ArgumentNullException(NameOf(collection))
        For Each tItem As T In collection
            _instances.TryAdd(New WeakReference(Of T)(tItem), 0)
        Next
    End Sub

#End Region

#Region "Properties"

    Public ReadOnly Property Count As Integer Implements ICollection(Of T).Count, IReadOnlyCollection(Of T).Count
        Get
            Dim iCount As Integer = 0
            For Each kvpItem As KeyValuePair(Of WeakReference(Of T), Byte) In _instances
                Dim tItem As T = Nothing
                If kvpItem.Key.TryGetTarget(tItem) AndAlso tItem IsNot Nothing AndAlso Not tItem.IsDisposed Then
                    iCount += 1
                Else
                    _instances.TryRemove(kvpItem.Key, Nothing)
                End If
            Next
            Return iCount
        End Get
    End Property
    Public ReadOnly Property IsReadOnly As Boolean = False Implements ICollection(Of T).IsReadOnly
    '''<summary>Gets a <see cref="IEnumerable"/> wrapper around the <see cref="WeakCollection"/>.</summary>
    '''<returns>A <see cref="IEnumerable"/> wapper around the <see cref="WeakCollection"/>.</returns>
    Public ReadOnly Iterator Property Items As IEnumerable(Of T)
        Get
            For Each kvpItem As KeyValuePair(Of WeakReference(Of T), Byte) In _instances
                Dim tItem As T = Nothing
                If kvpItem.Key.TryGetTarget(tItem) AndAlso tItem IsNot Nothing AndAlso Not tItem.IsDisposed Then
                    Yield tItem
                Else
                    _instances.TryRemove(kvpItem.Key, Nothing)
                End If
            Next
        End Get
    End Property

#End Region

#Region "Functions"

    '''<summary>Adds an object to the end of the <see cref="WeakCollection"/> if it does not already exist in the <see cref="WeakCollection"/>.</summary>
    '''<param name="item">The object to be added to the end of the <see cref="WeakCollection"/>. The value can be null for reference types.</param>
    Public Sub Add(item As T) Implements ICollection(Of T).Add
        _instances.TryAdd(New WeakReference(Of T)(item), 0)
    End Sub
    '''<summary>Removes all elements from the <see cref="WeakCollection"/>.</summary>
    Public Sub Clear() Implements ICollection(Of T).Clear
        For Each kvpItem As KeyValuePair(Of WeakReference(Of T), Byte) In _instances
            _instances.TryRemove(kvpItem.Key, Nothing)
        Next
    End Sub
    Public Sub CopyTo(array() As T, arrayIndex As Integer) Implements ICollection(Of T).CopyTo
        Dim tItems As New List(Of T)
        For Each kvpItem As KeyValuePair(Of WeakReference(Of T), Byte) In _instances.Skip(arrayIndex)
            Dim tItem As T = Nothing
            If kvpItem.Key.TryGetTarget(tItem) AndAlso tItem IsNot Nothing AndAlso Not tItem.IsDisposed Then
                tItems.Add(tItem)
            Else
                _instances.TryRemove(kvpItem.Key, Nothing)
            End If
        Next
        tItems.CopyTo(array, arrayIndex)
    End Sub
    '''<summary>Gets the first <typeparamref name="T"/> that fulfills the provided <see cref="Func(Of T, TResult)"/>.</summary>
    '''<returns>The first <typeparamref name="T"/> that fulfills the provided <see cref="Func(Of T, TResult)"/> if it exists; otherwise, <see langword="Nothing"/>.</returns>
    '''<param name="predicate">A function to test each element for a condition.</param>
    Public Function FirstOrDefault(predicate As Func(Of T, Boolean)) As T
        For Each kvpItem As KeyValuePair(Of WeakReference(Of T), Byte) In _instances
            Dim tItem As T = Nothing
            If kvpItem.Key.TryGetTarget(tItem) AndAlso tItem IsNot Nothing AndAlso Not tItem.IsDisposed Then
                If predicate(tItem) Then Return tItem
            Else
                _instances.TryRemove(kvpItem.Key, Nothing)
            End If
        Next
        Return Nothing
    End Function
    Public Iterator Function GetEnumerator() As IEnumerator(Of T) Implements IEnumerable(Of T).GetEnumerator
        For Each kvpItem As KeyValuePair(Of WeakReference(Of T), Byte) In _instances
            Dim tItem As T = Nothing
            If kvpItem.Key.TryGetTarget(tItem) AndAlso tItem IsNot Nothing AndAlso Not tItem.IsDisposed Then
                Yield tItem
            Else
                _instances.TryRemove(kvpItem.Key, Nothing)
            End If
        Next
    End Function
    '''<summary>Removes the first occurence of a specified object from the <see cref="WeakCollection"/>.</summary>
    '''<returns><see langword="True"/> if <paramref name="item"/> was succesfully removed; otherwise, <see langword="False"/>. This method also returns <see langword="False"/> if <paramref name="item"/> was not found in the original <see cref="WeakCollection"/>.</returns>
    '''<param name="item">The object to remove from the <see cref="WeakCollection"/>. The value can be null for reference types.</param>
    Public Function Remove(item As T) As Boolean Implements ICollection(Of T).Remove
        For Each kvpItem As KeyValuePair(Of WeakReference(Of T), Byte) In _instances
            Dim tItem As T = Nothing
            If kvpItem.Key.TryGetTarget(tItem) AndAlso tItem IsNot Nothing AndAlso Not tItem.IsDisposed Then
                If ReferenceEquals(tItem, item) Then
                    _instances.TryRemove(kvpItem.Key, Nothing)
                    Return True
                End If
            Else
                _instances.TryRemove(kvpItem.Key, Nothing)
            End If
        Next
        Return False
    End Function
    '''<summary>Gets each <typeparamref name="T"/> that fulfills the provided <see cref="Func(Of T, TResult)"/>.</summary>
    '''<returns>Each <typeparamref name="T"/> that fulfills the provided <see cref="Func(Of T, TResult)"/> if it exists; otherwise, <see langword="Nothing"/></returns>
    '''<param name="predicate">A function to test each element for a condition.</param>
    Public Iterator Function Where(predicate As Func(Of T, Boolean)) As IEnumerable(Of T)
        For Each kvpItem As KeyValuePair(Of WeakReference(Of T), Byte) In _instances
            Dim tItem As T = Nothing
            If kvpItem.Key.TryGetTarget(tItem) AndAlso tItem IsNot Nothing AndAlso Not tItem.IsDisposed Then
                If predicate(tItem) Then Yield tItem
            Else
                _instances.TryRemove(kvpItem.Key, Nothing)
            End If
        Next
    End Function

#End Region

End Class
