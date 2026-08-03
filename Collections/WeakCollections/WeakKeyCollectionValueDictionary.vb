Imports System.Collections.Concurrent
Imports System.Collections.ObjectModel

'''<summary>Represents a <see cref="Dictionary"/> of where the key in each <see cref="KeyValuePair"/> is a <see cref="WeakReference"/> and the value in each <see cref="KeyValuePair"/> is a <see cref="ICollection"/>.</summary>
'''<typeparam name="TKey">The <see cref="Type"/> of the key of each <see cref="KeyValuePair"/>.</typeparam>
'''<typeparam name="TValue">The <see cref="Type"/> of each item in the <see cref="ICollection"/> value of each <see cref="KeyValuePair"/>.</typeparam>
Public Class WeakKeyCollectionValueDictionary(Of TKey As {Class, IDisposable}, TValue)
    Implements ICollection(Of KeyValuePair(Of TKey, ICollection(Of TValue))), IDictionary(Of TKey, ICollection(Of TValue)), IEnumerable(Of KeyValuePair(Of TKey, ICollection(Of TValue))), IReadOnlyCollection(Of KeyValuePair(Of TKey, ICollection(Of TValue))), IReadOnlyDictionary(Of TKey, ICollection(Of TValue))

#Region "Private Items"

    Private ReadOnly _instances As New ConcurrentDictionary(Of WeakReference(Of TKey), ICollection(Of TValue))

    Private ReadOnly Property CountNonGeneric As Integer Implements IReadOnlyCollection(Of KeyValuePair(Of TKey, ICollection(Of TValue))).Count
        Get
            Return Count
        End Get
    End Property
    Private ReadOnly Property ItemNonGeneric(key As TKey) As ICollection(Of TValue) Implements IReadOnlyDictionary(Of TKey, ICollection(Of TValue)).Item
        Get
            Return Item(key)
        End Get
    End Property
    Private ReadOnly Property KeysNonGeneric As ICollection(Of TKey) Implements IDictionary(Of TKey, ICollection(Of TValue)).Keys
        Get
            Return Keys
        End Get
    End Property
    Private ReadOnly Property ValuesNonGeneric As ICollection(Of ICollection(Of TValue)) Implements IDictionary(Of TKey, ICollection(Of TValue)).Values
        Get
            Return Values
        End Get
    End Property

    Private Function ContainsKeyNonGeneric(key As TKey) As Boolean Implements IReadOnlyDictionary(Of TKey, ICollection(Of TValue)).ContainsKey
        Return ContainsKey(key)
    End Function
    Private Function GetEnumeratorNonGeneric() As IEnumerator Implements IEnumerable.GetEnumerator
        Return GetEnumerator()
    End Function
    Private Function TryGetValueNonGeneric(key As TKey, ByRef value As ICollection(Of TValue)) As Boolean Implements IReadOnlyDictionary(Of TKey, ICollection(Of TValue)).TryGetValue
        Return TryGetValue(key, value)
    End Function

#End Region

#Region "Operators"

    Public Function Contains(item As KeyValuePair(Of TKey, ICollection(Of TValue))) As Boolean Implements ICollection(Of KeyValuePair(Of TKey, ICollection(Of TValue))).Contains
        If item.Key Is Nothing OrElse item.Value Is Nothing Then Throw New ArgumentNullException("key and value cannot be null.")
        For Each kvpItem As KeyValuePair(Of WeakReference(Of TKey), ICollection(Of TValue)) In _instances 'Go through each instance
            Dim tKey As TKey = Nothing
            If kvpItem.Key.TryGetTarget(tKey) AndAlso tKey IsNot Nothing AndAlso Not tKey.IsDisposed Then 'Determine if key is valid
                If ReferenceEquals(tKey, item.Key) Then 'Determine if desired key is found
                    If kvpItem.Value IsNot Nothing AndAlso kvpItem.Value.Any Then Return True 'Return true if value valid
                    _instances.TryRemove(kvpItem.Key, Nothing) 'Value not valid, remove instance
                    Return False
                End If
            Else 'Key not valid
                _instances.TryRemove(kvpItem.Key, Nothing) 'Remove instance
            End If
        Next
        Return False
    End Function
    Public Function ContainsKey(key As TKey) As Boolean Implements IDictionary(Of TKey, ICollection(Of TValue)).ContainsKey
        If key Is Nothing Then Throw New ArgumentNullException("key cannot be null.")
        For Each kvpItem As KeyValuePair(Of WeakReference(Of TKey), ICollection(Of TValue)) In _instances 'Go through each instance
            Dim tKey As TKey = Nothing
            If kvpItem.Key.TryGetTarget(tKey) AndAlso tKey IsNot Nothing AndAlso Not tKey.IsDisposed Then 'Determine if key is valid
                If ReferenceEquals(tKey, key) Then 'Determine if desired key is found
                    If kvpItem.Value IsNot Nothing AndAlso kvpItem.Value.Any Then Return True 'Return true if value valid
                    _instances.TryRemove(kvpItem.Key, Nothing) 'Value not valid, remove instance
                    Return False
                End If
            Else 'Key not valid
                _instances.TryRemove(kvpItem.Key, Nothing) 'Remove instance
            End If
        Next
        Return False
    End Function

#End Region

#Region "Initializers"

    '''<summary>Initializes a new instance of the <see cref="WeakKeyCollectionValueDictionary"/> class.</summary>
    Public Sub New()
    End Sub
    '''<summary>Initializes a new instacne of the <see cref="WeakKeyCollectionValueDictionary"/> class tha contains elements copied from the specified collection.</summary>
    '''<param name="dictionary">The dictionary whose elements are copied to the new set.</param>
    '''<exception cref="ArgumentNullException"><paramref name="dictionary"/> collection is null.</exception>
    Public Sub New(dictionary As IDictionary(Of TKey, TValue))
        If dictionary Is Nothing Then Throw New ArgumentNullException(NameOf(dictionary))
        For Each kvpItem As KeyValuePair(Of TKey, TValue) In dictionary
            _instances.TryAdd(New WeakReference(Of TKey)(kvpItem.Key), kvpItem.Value)
        Next
    End Sub
    '''<summary>Initializes a new instacne of the <see cref="WeakKeyCollectionValueDictionary"/> class tha contains elements copied from the specified collection.</summary>
    '''<param name="dictionary">The dictionary whose elements are copied to the new set.</param>
    '''<exception cref="ArgumentNullException"><paramref name="dictionary"/> collection is null.</exception>
    Public Sub New(dictionary As IDictionary(Of TKey, ICollection(Of TValue)))
        If dictionary Is Nothing Then Throw New ArgumentNullException(NameOf(dictionary))
        For Each kvpItem As KeyValuePair(Of TKey, ICollection(Of TValue)) In dictionary
            _instances.TryAdd(New WeakReference(Of TKey)(kvpItem.Key), kvpItem.Value)
        Next
    End Sub

#End Region

#Region "Properties"

    Public ReadOnly Property Count As Integer Implements ICollection(Of KeyValuePair(Of TKey, ICollection(Of TValue))).Count
        Get
            Dim iCount As Integer = 0
            For Each kvpItem As KeyValuePair(Of WeakReference(Of TKey), ICollection(Of TValue)) In _instances 'Go through each instance
                Dim tKey As TKey = Nothing
                If kvpItem.Key.TryGetTarget(tKey) AndAlso tKey IsNot Nothing AndAlso Not tKey.IsDisposed AndAlso kvpItem.Value IsNot Nothing AndAlso kvpItem.Value.Any Then 'Determine if key and value are valid
                    iCount += 1 'Add to cound
                Else 'Key or value not valid
                    _instances.TryRemove(kvpItem.Key, Nothing) 'Remove instance
                End If
            Next
            Return iCount
        End Get
    End Property
    Public ReadOnly Property IsReadOnly As Boolean = False Implements ICollection(Of KeyValuePair(Of TKey, ICollection(Of TValue))).IsReadOnly
    Default Public Property Item(key As TKey) As ICollection(Of TValue) Implements IDictionary(Of TKey, ICollection(Of TValue)).Item
        Get
            If key Is Nothing Then Throw New ArgumentNullException("key cannot be null.")
            For Each kvpItem As KeyValuePair(Of WeakReference(Of TKey), ICollection(Of TValue)) In _instances 'Go through each instance
                Dim tKey As TKey = Nothing
                If kvpItem.Key.TryGetTarget(tKey) AndAlso tKey IsNot Nothing AndAlso Not tKey.IsDisposed Then 'Determine if key is valid
                    If ReferenceEquals(kvpItem.Key, key) Then 'Determine if desired key is found
                        If kvpItem.Value IsNot Nothing AndAlso kvpItem.Value.Any Then Return kvpItem.Value 'Return value if valid
                        _instances.TryRemove(kvpItem.Key, Nothing) 'Value not valid, remove instance
                        Return Nothing
                    End If
                Else 'Key not valid
                    _instances.TryRemove(kvpItem.Key, Nothing) 'Remove instance
                End If
            Next
            Return Nothing
        End Get
        Set(value As ICollection(Of TValue))
            If key Is Nothing OrElse value Is Nothing Then Throw New ArgumentNullException("key and value cannot be null.")
            If Not value.Any Then Throw New ArgumentOutOfRangeException("value is required to contain elements.")
            For Each kvpItem As KeyValuePair(Of WeakReference(Of TKey), ICollection(Of TValue)) In _instances 'Go through each item
                Dim tKey As TKey = Nothing
                If kvpItem.Key.TryGetTarget(tKey) AndAlso tKey IsNot Nothing AndAlso Not tKey.IsDisposed Then 'Determine if key is valid
                    If ReferenceEquals(kvpItem.Key, key) Then 'Determine if desired key is found
                        _instances(kvpItem.Key) = value 'Add to instances
                        Return 'Exit
                    ElseIf kvpItem.Value Is Nothing OrElse Not kvpItem.Value.Any Then 'Determine if valus is not valid when key is not the desired key
                        _instances.TryRemove(kvpItem.Key, Nothing) 'Remove instance
                    End If
                Else 'Key is not valid
                    _instances.TryRemove(kvpItem.Key, Nothing) 'Remove instance
                End If
            Next
            _instances.TryAdd(New WeakReference(Of TKey)(key), value) 'Add new key value pair
        End Set
    End Property
    Private ReadOnly Iterator Property Keys As IEnumerable(Of TKey) Implements IReadOnlyDictionary(Of TKey, ICollection(Of TValue)).Keys
        Get
            For Each kvpItem As KeyValuePair(Of WeakReference(Of TKey), ICollection(Of TValue)) In _instances 'Go through each instance
                Dim tKey As TKey = Nothing
                If kvpItem.Key.TryGetTarget(tKey) AndAlso tKey IsNot Nothing AndAlso Not tKey.IsDisposed AndAlso kvpItem.Value IsNot Nothing AndAlso kvpItem.Value.Any Then 'Determine if key and value are valid
                    Yield tKey 'Yeild key
                Else 'Key or value not valid
                    _instances.TryRemove(kvpItem.Key, Nothing) 'Remove instance
                End If
            Next
        End Get
    End Property
    Private ReadOnly Iterator Property Values As IEnumerable(Of ICollection(Of TValue)) Implements IReadOnlyDictionary(Of TKey, ICollection(Of TValue)).Values
        Get
            For Each kvpItem As KeyValuePair(Of WeakReference(Of TKey), ICollection(Of TValue)) In _instances 'Go through each instance
                Dim tKey As TKey = Nothing
                If kvpItem.Key.TryGetTarget(tKey) AndAlso tKey IsNot Nothing AndAlso Not tKey.IsDisposed AndAlso kvpItem.Value IsNot Nothing AndAlso kvpItem.Value.Any Then 'Determine if key and value are valid
                    Yield kvpItem.Value 'Yeild value
                Else 'Key or value not valid
                    _instances.TryRemove(kvpItem.Key, Nothing) 'Remove instance
                End If
            Next
        End Get
    End Property

#End Region

#Region "Functions"

    Public Sub Add(item As KeyValuePair(Of TKey, ICollection(Of TValue))) Implements ICollection(Of KeyValuePair(Of TKey, ICollection(Of TValue))).Add
        If item.Key Is Nothing OrElse item.Value Is Nothing Then Throw New ArgumentNullException("key and value cannot be null.")
        If Not item.Value.Any Then Throw New ArgumentOutOfRangeException("value is required to contain elements.")
        _instances.TryAdd(New WeakReference(Of TKey)(item.Key), item.Value)
    End Sub
    Public Sub Add(key As TKey, value As ICollection(Of TValue)) Implements IDictionary(Of TKey, ICollection(Of TValue)).Add
        If key Is Nothing OrElse value Is Nothing Then Throw New ArgumentNullException("key and value cannot be null.")
        If Not value.Any Then Throw New ArgumentOutOfRangeException("value is required to contain elements.")
        _instances.TryAdd(New WeakReference(Of TKey)(key), value)
    End Sub
    '''<summary>Adds an element with the provided key to the <see cref="WeakKeyCollectionValueDictionary"/>.</summary>
    '''<param name="key">The object to use as the key of the element to add.</param>
    '''<param name="item">The element to add to the collection associted with the key.</param>
    '''<exception cref="ArgumentNullException">key or value is null.</exception>
    Public Sub AddItem(key As TKey, item As TValue)
        If key Is Nothing OrElse item Is Nothing Then Throw New ArgumentNullException("key and value cannot be null.")
        Dim tValue As ICollection(Of TValue)
        For Each kvpItem As KeyValuePair(Of WeakReference(Of TKey), ICollection(Of TValue)) In _instances 'Go through each item
            Dim tKey As TKey = Nothing
            If kvpItem.Key.TryGetTarget(tKey) AndAlso tKey IsNot Nothing AndAlso Not tKey.IsDisposed Then 'Determine if key is valid
                If ReferenceEquals(kvpItem.Key, key) Then 'Determine if desired key is found
                    If kvpItem.Value IsNot Nothing AndAlso kvpItem.Value.Any Then 'Determine if value is valid
                        _instances(kvpItem.Key).Add(item) 'Add item to instance
                    Else 'Value is not valid
                        tValue = Activator.CreateInstance(Of ICollection(Of TValue)) 'Create new value
                        tValue.Add(item) 'Add item to value
                        _instances.TryAdd(kvpItem.Key, tValue) 'Add new key value pair
                    End If
                    Return 'Exit
                End If
            Else 'Key is not valid
                _instances.TryRemove(kvpItem.Key, Nothing) 'Remove instance
            End If
        Next
        tValue = Activator.CreateInstance(Of TValue) 'Create new value
        tValue.Add(item) 'Add item to value
        _instances.TryAdd(New WeakReference(Of TKey)(key), tValue) 'Add new key value pair
    End Sub
    Public Sub Clear() Implements ICollection(Of KeyValuePair(Of TKey, ICollection(Of TValue))).Clear
        For Each kvpItem As KeyValuePair(Of WeakReference(Of TKey), ICollection(Of TValue)) In _instances
            _instances.TryRemove(kvpItem.Key, Nothing)
        Next
    End Sub
    Public Sub CopyTo(array() As KeyValuePair(Of TKey, ICollection(Of TValue)), arrayIndex As Integer) Implements ICollection(Of KeyValuePair(Of TKey, ICollection(Of TValue))).CopyTo
        Dim cValue As New Collection(Of KeyValuePair(Of TKey, ICollection(Of TValue))) 'Setup collection copying from
        For Each kvpItem As KeyValuePair(Of WeakReference(Of TKey), ICollection(Of TValue)) In _instances 'Go through each instance
            Dim tKey As TKey = Nothing
            If kvpItem.Key.TryGetTarget(tKey) AndAlso tKey IsNot Nothing AndAlso Not tKey.IsDisposed AndAlso kvpItem.Value IsNot Nothing AndAlso kvpItem.Value.Any Then 'Determine if key and value are valid
                cValue.Add(New KeyValuePair(Of TKey, ICollection(Of TValue))(tKey, kvpItem.Value)) 'Add key and value to collection
            Else 'Key or value not valid
                _instances.TryRemove(kvpItem.Key, Nothing) 'Remove instance
            End If
        Next
        cValue.CopyTo(array, arrayIndex) 'Copy collection to array
    End Sub
    Public Iterator Function GetEnumerator() As IEnumerator(Of KeyValuePair(Of TKey, ICollection(Of TValue))) Implements IEnumerable(Of KeyValuePair(Of TKey, ICollection(Of TValue))).GetEnumerator
        For Each kvpItem As KeyValuePair(Of WeakReference(Of TKey), ICollection(Of TValue)) In _instances 'Go through each instance
            Dim tKey As TKey = Nothing
            If kvpItem.Key.TryGetTarget(tKey) AndAlso tKey IsNot Nothing AndAlso Not tKey.IsDisposed AndAlso kvpItem.Value IsNot Nothing AndAlso kvpItem.Value.Any Then 'Determine if key and value are valid
                Yield New KeyValuePair(Of TKey, ICollection(Of TValue))(tKey, kvpItem.Value) 'Yeild the key and value
            Else 'Key or value not valid
                _instances.TryRemove(kvpItem.Key, Nothing) 'Remove instance
            End If
        Next
    End Function
    Public Function Remove(item As KeyValuePair(Of TKey, ICollection(Of TValue))) As Boolean Implements ICollection(Of KeyValuePair(Of TKey, ICollection(Of TValue))).Remove
        For Each kvpItem As KeyValuePair(Of WeakReference(Of TKey), ICollection(Of TValue)) In _instances 'Go through each instance
            Dim tKey As TKey = Nothing
            If kvpItem.Key.TryGetTarget(tKey) AndAlso tKey IsNot Nothing AndAlso Not tKey.IsDisposed Then 'Determine if key is valid
                If ReferenceEquals(tKey, item.Key) Then 'Determine if desired key is found
                    _instances.TryRemove(kvpItem.Key, Nothing) 'Remove instance
                    Return kvpItem.Value IsNot Nothing AndAlso kvpItem.Value.Any 'Return true if value for key is valid; otherwise, false
                End If
            Else 'Key not valid
                _instances.TryRemove(kvpItem.Key, Nothing) 'Remove instance
            End If
        Next
        Return False 'Key not found, so value not found
    End Function
    Public Function Remove(key As TKey) As Boolean Implements IDictionary(Of TKey, ICollection(Of TValue)).Remove
        If key Is Nothing Then Throw New ArgumentNullException("key cannot be null.")
        For Each kvpItem As KeyValuePair(Of WeakReference(Of TKey), ICollection(Of TValue)) In _instances 'Go through each instance
            Dim tKey As TKey = Nothing
            If kvpItem.Key.TryGetTarget(tKey) AndAlso tKey IsNot Nothing AndAlso Not tKey.IsDisposed Then 'Determine if key is valid
                If ReferenceEquals(tKey, key) Then 'Determine if desired key is found
                    _instances.TryRemove(kvpItem.Key, Nothing) 'Remove instance
                    Return kvpItem.Value IsNot Nothing AndAlso kvpItem.Value.Any 'Return true if value for key is valid; otherwise, false
                End If
            Else 'Key not valid
                _instances.TryRemove(kvpItem.Key, Nothing) 'Remove instance
            End If
        Next
        Return False 'Key not found, so value not found
    End Function
    '''<summary>removes an element with the provided key to the <see cref="WeakKeyCollectionValueDictionary"/>.</summary>
    '''<returns><see langword="True"/> if the element is successfully removed; otherwise <see langword="False"/>. This method also returns <see langword="False"/> if the key was not found or the collection assiciated with the key is not valid.</returns>
    '''<param name="key">The object to use as the key of the element to add.</param>
    '''<param name="item">The element to remove to the collection associted with the key.</param>
    Public Function RemoveItem(key As TKey, item As TValue) As Boolean
        If key Is Nothing OrElse item Is Nothing Then Throw New ArgumentNullException("key and item cannot be null.")
        For Each kvpItem As KeyValuePair(Of WeakReference(Of TKey), ICollection(Of TValue)) In _instances 'Go through each instance
            Dim tKey As TKey = Nothing
            If kvpItem.Key.TryGetTarget(tKey) AndAlso tKey IsNot Nothing AndAlso Not tKey.IsDisposed Then 'Determine if key is valid
                If ReferenceEquals(tKey, key) Then 'Determine if desired key is found
                    If kvpItem.Value IsNot Nothing AndAlso kvpItem.Value.Any Then 'Determine if value is valid
                        Return kvpItem.Value.Remove(item) 'Return wether the item was succesfully removed
                    Else 'Value not valid
                        Return False 'Value not valid
                    End If
                End If
            Else 'Key not valid
                _instances.TryRemove(kvpItem.Key, Nothing) 'Remove instance
            End If
        Next
        Return False 'Key not found, so value not found
    End Function
    Public Function TryGetValue(key As TKey, ByRef value As ICollection(Of TValue)) As Boolean Implements IDictionary(Of TKey, ICollection(Of TValue)).TryGetValue
        If key Is Nothing Then Throw New ArgumentNullException("key cannot be null.")
        For Each kvpItem As KeyValuePair(Of WeakReference(Of TKey), ICollection(Of TValue)) In _instances 'Go through each instance
            Dim tKey As TKey = Nothing
            If kvpItem.Key.TryGetTarget(tKey) AndAlso tKey IsNot Nothing AndAlso Not tKey.IsDisposed Then 'Determine if key is valid
                If ReferenceEquals(tKey, key) Then 'Determine if desired key is found
                    If kvpItem.Value IsNot Nothing AndAlso kvpItem.Value.Any Then 'Determine if value for key is valid
                        value = kvpItem.Value 'Set value
                        Return True 'Value found for key
                    End If
                    _instances.TryRemove(kvpItem.Key, Nothing) 'Key not valid, remove instance
                    value = Nothing 'Set value
                    Return False 'Value not found for key
                End If
            Else 'Key not valid
                _instances.TryRemove(kvpItem.Key, Nothing) 'Remove instance
            End If
        Next
        value = Nothing 'Key not found, remove instance
        Return False 'Key not found, so value not found
    End Function

#End Region

End Class
