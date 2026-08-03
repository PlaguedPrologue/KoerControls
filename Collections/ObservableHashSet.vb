Imports System.Collections.Specialized

Public Class ObservableHashSet(Of T)
    Implements ICollection(Of T), IEnumerable(Of T), IEnumerable, IObservableCollection(Of T), IReadOnlyCollection(Of T), ISet(Of T)

#Region "Private Items"

    Private ReadOnly _sourceNonGeneric As IEnumerable

    Private ReadOnly Property Source As ISet(Of T)
        Get
            Return _sourceNonGeneric
        End Get
    End Property

    Private Sub AddNonGeneric(item As T) Implements ICollection(Of T).Add
        Throw New NotImplementedException
    End Sub
    Private Function GetEnumeratorNonGeneric() As IEnumerator Implements IEnumerable.GetEnumerator
        Return Source.GetEnumerator
    End Function

#End Region

#Region "Events"

    '''<summary>Occurs when an item is added, removed, changed, or the entire list is refreshed.</summary>
    Public Event CollectionChanged As NotifyCollectionChangedEventHandler Implements IObservableCollection.CollectionChanged

#End Region

#Region "Operators"

    Public Function Contains(item As T) As Boolean Implements ICollection(Of T).Contains
        Return Source.Contains(item)
    End Function
    Public Sub IntersectWith(other As IEnumerable(Of T)) Implements ISet(Of T).IntersectWith
        Dim tItemsRemoving As HashSet(Of T) = Source.Intersect(other).ToHashSet
        If tItemsRemoving.Count = 0 Then Return
        Source.IntersectWith(other)
        OnCollectionChanged(New NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset, tItemsRemoving))
    End Sub
    Public Function IsProperSubsetOf(other As IEnumerable(Of T)) As Boolean Implements ISet(Of T).IsProperSubsetOf
        Return Source.IsProperSubsetOf(other)
    End Function
    Public Function IsProperSupersetOf(other As IEnumerable(Of T)) As Boolean Implements ISet(Of T).IsProperSupersetOf
        Return Source.IsProperSupersetOf(other)
    End Function
    Public Function IsSubsetOf(other As IEnumerable(Of T)) As Boolean Implements ISet(Of T).IsSubsetOf
        Return Source.IsSubsetOf(other)
    End Function
    Public Function IsSupersetOf(other As IEnumerable(Of T)) As Boolean Implements ISet(Of T).IsSupersetOf
        Return Source.IsSupersetOf(other)
    End Function
    Public Function Overlaps(other As IEnumerable(Of T)) As Boolean Implements ISet(Of T).Overlaps
        Return Source.Overlaps(other)
    End Function

#End Region

#Region "Initializers"

    '''<summary>Initializes a new instance of the <see cref="ObservableHashSet"/> class that is empty and uses the default equality comparer for the set type.</summary>
    Public Sub New()
        _sourceNonGeneric = New HashSet(Of T)
    End Sub
    '''<summary>Initializes a new instance of the <see cref="ObservableHashSet"/> class that is empty, but has reserved space for capacity items and uses the default equality comparer for the set type.</summary>
    '''<param name="capacity">The initial size of the <see cref="ObservableHashSet"/>.</param>
    Public Sub New(capacity As Integer)
        _sourceNonGeneric = New HashSet(Of T)(capacity)
    End Sub
    '''<summary>Initializes a new instance of the <see cref="ObservableHashSet"/> class that is empty and uses the specified equality comparer for the set type.</summary>
    '''<param name="comparer">The <see cref="IEqualityComparer"/> implementation to use when comparing values in the set, or null to use the default <see cref="IEqualityComparer"/> implementation for the set type.</param>
    Public Sub New(comparer As IEqualityComparer(Of T))
        Me.Comparer = comparer
        _sourceNonGeneric = New HashSet(Of T)(comparer)
    End Sub
    '''<summary>Initializes a new instacne of the <see cref="ObservableHashSet"/> class that uses the default equality comparer for the set type, contains elements copied from the specified collection, and has sufficient capacity to accommodate the number of elements compied.</summary>
    '''<param name="collection">The collection whose elements are copied to the new set.</param>
    '''<exception cref="ArgumentNullException"><paramref name="collection"/>collection is null.</exception>
    Public Sub New(collection As IEnumerable(Of T))
        If collection Is Nothing Then Throw New ArgumentNullException(NameOf(collection))
        _sourceNonGeneric = collection
        'Dim tType As Type = collection.GetType
        'If TypeOf collection Is IObservableCollection Then
        '    AddHandler CType(collection, IObservableCollection).CollectionChanged, Sub(sender, e) OnCollectionChanged(e)
        'End If
        'If tType.IsGenericType AndAlso tType.GetGenericTypeDefinition Is GetType(HashSet(Of )) Then
        '    _source = collection
        'ElseIf TypeOf collection Is IObservableCollection Then
        '    tType = CType(collection, IObservableCollection).Source.GetType
        '    If tType.IsGenericType AndAlso tType.GetGenericTypeDefinition Is GetType(HashSet(Of )) Then
        '        Dim tType As Type = CType(collection, IObservableCollection).Sou
        '        _source = CType(CType(collection, IObservableCollection).Source, IEnumerable(Of T))
        '    Else
        '        _source = New HashSet(Of T)(collection)
        '    End If
        'Else
        '    _source = New HashSet(Of T)(collection)
        'End If
    End Sub
    '''<summary>Initializes a new instance of the <see cref="ObservableHashSet"/> class that uses the specified equality comparer for the set type, and has sufficient capacity to accommodate <paramref name="capacity"/> elements.</summary>
    '''<param name="capacity">The initial size of the <see cref="ObservableHashSet"/>.</param>
    '''<param name="comparer">The <see cref="IEqualityComparer"/> implementation to use when comparing values in the set, or null to use the default <see cref="IEqualityComparer"/> implementation for the set type.</param>
    Public Sub New(capacity As Integer, comparer As IEqualityComparer)
        Me.Comparer = comparer
        _sourceNonGeneric = New HashSet(Of T)(capacity, comparer)
    End Sub

#End Region

#Region "Properties"

    '''<summary>Gets the <see cref="IEqualityComparer"/> object that is used to determine equality for the values in the set.</summary>
    '''<returns>The <see cref="IEqualityComparer"/> object that is used to determine equality for the values in the set.</returns>
    Public ReadOnly Property Comparer As IEqualityComparer(Of T)
    Public ReadOnly Property Count As Integer Implements ICollection(Of T).Count, IReadOnlyCollection(Of T).Count
        Get
            Return Source.Count
        End Get
    End Property
    Private ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of T).IsReadOnly
        Get
            Return False
        End Get
    End Property

#End Region

#Region "Functions"

    Public Function Add(item As T) As Boolean Implements ISet(Of T).Add
        If Not Source.Add(item) Then Return False
        OnCollectionChanged(New NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item))
        Return True
    End Function
    Public Sub Clear() Implements ICollection(Of T).Clear
        If Source.Count = 0 Then Return
        Source.Clear()
        OnCollectionChanged(New NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset))
    End Sub
    '''<summary>Copies the elements of a <see cref="ObservableHashSet"/> object to an array.</summary>
    '''<param name="array">The one-dimensional array that is the destination fo the elements copied from the <see cref="ObservableHashSet"/> object. The array must have zero-based indexing.</param>
    '''<exception cref="ArgumentNullException"><paramref name="array"/> is null.</exception>
    Public Sub CopyTo(array() As T)
        Source.CopyTo(array, 0)
    End Sub
    Public Sub CopyTo(array() As T, arrayIndex As Integer) Implements ICollection(Of T).CopyTo
        Source.CopyTo(array, arrayIndex)
    End Sub
    '''<summary>Copies the specified elements of the <see cref="ObservableHashSet"/> object to an array, starting at the specified array index.</summary>
    '''<param name="array">The one-dimensional array that is the destination fo the elements copied from the <see cref="ObservableHashSet"/> object. The array must have zero-based indexing.</param>
    '''<param name="arrayIndex">The zero-based index in the array at which copying begins.</param>
    '''<param name="count">The number of elements to copy to the array.</param>
    '''<exception cref="ArgumentNullException"><paramref name="array"/> is null.</exception>
    '''<exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0. -or- <paramref name="count"/> is less than 0.</exception>
    '''<exception cref="ArgumentException"><paramref name="arrayIndex"/> is greater than the length of the destination array. -or- <paramref name="count"/> is greater than the available space from the index to the end of the destination array.</exception>
    Public Sub CopyTo(array() As T, arrayIndex As Integer, count As Integer)
        Source.CopyTo(array.Take(count), arrayIndex)
    End Sub
    '''<summary>returns an <see cref="IEqualityComparer"/> object that can be used for equality testing of a <see cref="ObservableHashSet"/> object.</summary>
    '''<returns>A <see cref="IEqualityComparer"/> object that can be used for deep equality testing of the <see cref="ObservableHashSet"/> object.</returns>
    Public Shared Function CreateSetComparer() As IEqualityComparer(Of ObservableHashSet(Of T))
        Return HashSet(Of T).CreateSetComparer()
    End Function
    Public Sub ExceptWith(other As IEnumerable(Of T)) Implements ISet(Of T).ExceptWith
        Dim tItemsRemoving As HashSet(Of T) = Source.Except(other).ToHashSet
        If tItemsRemoving.Count = 0 Then Return
        Source.ExceptWith(other)
        OnCollectionChanged(New NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, tItemsRemoving))
    End Sub
    Public Function GetEnumerator() As IEnumerator(Of T) Implements IEnumerable(Of T).GetEnumerator
        Return Source.GetEnumerator
    End Function
    '''<summary>Raises the <see cref="CollectionChanged"/> event.</summary>
    '''<param name="e">The <see cref="NotifyCollectionChangedEventArgs"/> that contains the event data.</param>
    Protected Overridable Sub OnCollectionChanged(e As NotifyCollectionChangedEventArgs)
        RaiseEvent CollectionChanged(Me, e)
    End Sub
    Public Function Remove(item As T) As Boolean Implements ICollection(Of T).Remove
        If Not Source.Remove(item) Then Return False
        OnCollectionChanged(New NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item))
        Return True
    End Function
    Public Function SetEquals(other As IEnumerable(Of T)) As Boolean Implements ISet(Of T).SetEquals
        Return Source.SetEquals(other)
    End Function
    Public Sub SymmetricExceptWith(other As IEnumerable(Of T)) Implements ISet(Of T).SymmetricExceptWith
        Dim tItemsRemoving = Source.ToHashSet
        Source.SymmetricExceptWith(other)
        tItemsRemoving = tItemsRemoving.Except(Source)
        If tItemsRemoving.Count = 0 Then Return
        OnCollectionChanged(New NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, tItemsRemoving))
    End Sub
    '''<summary>Creates a <see cref="HashSet"/> from a <see cref="ObservableHashSet"/>.</summary>
    '''<returns>A <see cref="HashSet"/> that contains the elements from the input sequence.</returns>
    Public Function ToHashset() As HashSet(Of T)
        Return New HashSet(Of T)(Source)
    End Function
    Public Sub UnionWith(other As IEnumerable(Of T)) Implements ISet(Of T).UnionWith
        Dim tItemsAdding As HashSet(Of T) = other.Except(Source).ToHashSet
        If tItemsAdding.Count = 0 Then Return
        Source.UnionWith(other)
        OnCollectionChanged(New NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, tItemsAdding))
    End Sub

#End Region

End Class
