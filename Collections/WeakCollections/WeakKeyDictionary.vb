Imports System.Collections.Concurrent
Imports System.Collections.ObjectModel

'''<summary>Represents a <see cref="Dictionary"/> of where only the key and value in each <see cref="KeyValuePair"/> is a <see cref="WeakReference"/>.</summary>
'''<typeparam name="TKey">The <see cref="Type"/> of the key of each <see cref="KeyValuePair"/>.</typeparam>
'''<typeparam name="TValue">The <see cref="Type"/> of the value of each <see cref="KeyValuePair"/>.</typeparam>
Public Class WeakKeyDictionary(Of TKey As {Class, IDisposable}, TValue)
    Implements ICollection(Of KeyValuePair(Of TKey, TValue)), IDictionary(Of TKey, TValue), IEnumerable(Of KeyValuePair(Of TKey, TValue)), IReadOnlyCollection(Of KeyValuePair(Of TKey, TValue)), IReadOnlyDictionary(Of TKey, TValue)

#Region "Private Items"

    Private ReadOnly _instances As New ConcurrentDictionary(Of WeakReference(Of TKey), TValue)

    Private ReadOnly Property CountNonGeneric As Integer Implements IReadOnlyCollection(Of KeyValuePair(Of TKey, TValue)).Count
        Get
            Return Count
        End Get
    End Property
    Private ReadOnly Property ItemNonGeneric(key As TKey) As TValue Implements IReadOnlyDictionary(Of TKey, TValue).Item
        Get
            Return Item(key)
        End Get
    End Property
    Private ReadOnly Property KeysNonGeneric As ICollection(Of TKey) Implements IDictionary(Of TKey, TValue).Keys
        Get
            Return Keys
        End Get
    End Property
    Private ReadOnly Property ValuesNonGeneric As ICollection(Of TValue) Implements IDictionary(Of TKey, TValue).Values
        Get
            Return Values
        End Get
    End Property

    Private Function ContainsKeyNonGeneric(key As TKey) As Boolean Implements IReadOnlyDictionary(Of TKey, TValue).ContainsKey
        Return ContainsKey(key)
    End Function
    Private Function GetEnumeratorNonGeneric() As IEnumerator Implements IEnumerable.GetEnumerator
        Return GetEnumerator()
    End Function
    Private Function TryGetValueNonGeneric(key As TKey, ByRef value As TValue) As Boolean Implements IReadOnlyDictionary(Of TKey, TValue).TryGetValue
        Return TryGetValue(key, value)
    End Function

#End Region

#Region "Operators"

    Public Function Contains(item As KeyValuePair(Of TKey, TValue)) As Boolean Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Contains
        If item.Key Is Nothing OrElse item.Value Is Nothing Then Throw New ArgumentNullException("key and value cannot be null.")
        For Each kvpItem As KeyValuePair(Of WeakReference(Of TKey), TValue) In _instances 'Go through each instance
            Dim tKey As TKey = Nothing
            If kvpItem.Key.TryGetTarget(tKey) AndAlso tKey IsNot Nothing AndAlso Not tKey.IsDisposed Then 'Determine if key is valid
                If ReferenceEquals(tKey, item.Key) AndAlso Equals(kvpItem.Value, item.Value) Then 'Determine if desired key is found
                    If kvpItem.Value IsNot Nothing Then Return True 'Return true if value is valid
                    _instances.TryRemove(kvpItem.Key, Nothing) 'Value not valid, remove instance
                    Return False
                End If
            Else 'Key is not valid
                _instances.TryRemove(kvpItem.Key, Nothing) 'Remove instance
            End If
        Next
        Return False
    End Function
    Public Function ContainsKey(key As TKey) As Boolean Implements IDictionary(Of TKey, TValue).ContainsKey
        If key Is Nothing Then Throw New ArgumentNullException("key cannot be null.")
        For Each kvpItem As KeyValuePair(Of WeakReference(Of TKey), TValue) In _instances 'Go through each instance
            Dim tKey As TKey = Nothing
            If kvpItem.Key.TryGetTarget(tKey) AndAlso tKey IsNot Nothing AndAlso Not tKey.IsDisposed Then 'Determine if key is valid
                If ReferenceEquals(tKey, key) Then 'Determine if desired key is found
                    If kvpItem.Value IsNot Nothing Then Return True 'Return true if value valid
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

    '''<summary>Initializes a new instance of the <see cref="WeakKeyDictionary"/> class.</summary>
    Public Sub New()
    End Sub
    '''<summary>Initializes a new instacne of the <see cref="WeakKeyDictionary"/> class tha contains elements copied from the specified collection.</summary>
    '''<param name="dictionary">The dictionary whose elements are copied to the new set.</param>
    '''<exception cref="ArgumentNullException"><paramref name="dictionary"/> collection is null.</exception>
    Public Sub New(dictionary As IDictionary(Of TKey, TValue))
        If dictionary Is Nothing Then Throw New ArgumentNullException(NameOf(dictionary))
        For Each kvpItem As KeyValuePair(Of TKey, TValue) In dictionary
            _instances.TryAdd(New WeakReference(Of TKey)(kvpItem.Key), kvpItem.Value)
        Next
    End Sub

#End Region

#Region "Properties"

    Public ReadOnly Property Count As Integer Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Count
        Get
            Dim iCount As Integer = 0
            For Each kvpItem As KeyValuePair(Of WeakReference(Of TKey), TValue) In _instances 'Go through each instance
                Dim tKey As TKey = Nothing
                If kvpItem.Key.TryGetTarget(tKey) AndAlso tKey IsNot Nothing AndAlso Not tKey.IsDisposed AndAlso kvpItem.Value IsNot Nothing Then 'Determine if key and value are valid
                    iCount += 1 'Add to cound
                Else 'Key or value not valid
                    _instances.TryRemove(kvpItem.Key, Nothing) 'Remove instance
                End If
            Next
            Return iCount
        End Get
    End Property
    Public ReadOnly Property IsReadOnly As Boolean = False Implements ICollection(Of KeyValuePair(Of TKey, TValue)).IsReadOnly
    Default Public Property Item(key As TKey) As TValue Implements IDictionary(Of TKey, TValue).Item
        Get
            If key Is Nothing Then Throw New ArgumentNullException("key cannot be null.")
            For Each kvpItem As KeyValuePair(Of WeakReference(Of TKey), TValue) In _instances 'Go through each instance
                Dim tKey As TKey = Nothing
                If kvpItem.Key.TryGetTarget(tKey) AndAlso tKey IsNot Nothing AndAlso Not tKey.IsDisposed Then 'Determine if key is valid
                    If ReferenceEquals(tKey, key) Then 'Determine if desired key is found
                        If kvpItem.Value IsNot Nothing Then Return kvpItem.Value 'Return value if valid
                        _instances.TryRemove(kvpItem.Key, Nothing) 'Value not valid, remove instance
                        Return Nothing
                    End If
                Else 'Key not valid
                    _instances.TryRemove(kvpItem.Key, Nothing) 'Remove instance
                End If
            Next
            Return Nothing
        End Get
        Set(value As TValue)
            If key Is Nothing OrElse value Is Nothing Then Throw New ArgumentNullException("key and value cannot be null.")
            For Each kvpItem As KeyValuePair(Of WeakReference(Of TKey), TValue) In _instances 'Go through each item
                Dim tKey As TKey = Nothing
                If kvpItem.Key.TryGetTarget(tKey) AndAlso tKey IsNot Nothing AndAlso Not tKey.IsDisposed Then 'Determine if key is valid
                    If ReferenceEquals(tKey, key) Then 'Determine if desired key is found
                        _instances(kvpItem.Key) = value 'Add to instances
                        Return 'Exit
                    ElseIf kvpItem.Value Is Nothing Then 'Determine if valus is not valid when key is not the desired key
                        _instances.TryRemove(kvpItem.Key, Nothing) 'Remove instance
                    End If
                Else 'Key is not valid
                    _instances.TryRemove(kvpItem.Key, Nothing) 'Remove instance
                End If
            Next
            _instances.TryAdd(New WeakReference(Of TKey)(key), value) 'Add new key value pair
        End Set
    End Property
    Private ReadOnly Iterator Property Keys As IEnumerable(Of TKey) Implements IReadOnlyDictionary(Of TKey, TValue).Keys
        Get
            For Each kvpItem As KeyValuePair(Of WeakReference(Of TKey), TValue) In _instances 'Go through each instance
                Dim tKey As TKey = Nothing
                If kvpItem.Key.TryGetTarget(tKey) AndAlso tKey IsNot Nothing AndAlso Not tKey.IsDisposed AndAlso kvpItem.Value IsNot Nothing Then 'Determine if key and value are valid
                    Yield tKey 'Yeild key
                Else 'Key or value not valid
                    _instances.TryRemove(kvpItem.Key, Nothing) 'Remove instance
                End If
            Next
        End Get
    End Property
    Private ReadOnly Iterator Property Values As IEnumerable(Of TValue) Implements IReadOnlyDictionary(Of TKey, TValue).Values
        Get
            For Each kvpItem As KeyValuePair(Of WeakReference(Of TKey), TValue) In _instances 'Go through each instance
                Dim tKey As TKey = Nothing
                If kvpItem.Key.TryGetTarget(tKey) AndAlso tKey IsNot Nothing AndAlso Not tKey.IsDisposed AndAlso kvpItem.Value IsNot Nothing Then 'Determine if key and value are valid
                    Yield kvpItem.Value 'Yeild value
                Else 'Key or value not valid
                    _instances.TryRemove(kvpItem.Key, Nothing) 'Remove instance
                End If
            Next
        End Get
    End Property

#End Region

#Region "Functions"

    Public Sub Add(item As KeyValuePair(Of TKey, TValue)) Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Add
        If item.Key Is Nothing OrElse item.Value Is Nothing Then Throw New ArgumentNullException("key and value cannot be null.")
        _instances.TryAdd(New WeakReference(Of TKey)(item.Key), item.Value)
    End Sub
    Public Sub Add(key As TKey, value As TValue) Implements IDictionary(Of TKey, TValue).Add
        If key Is Nothing OrElse value Is Nothing Then Throw New ArgumentNullException("key and value cannot be null.")
        _instances.TryAdd(New WeakReference(Of TKey)(key), value)
    End Sub
    Public Sub Clear() Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Clear
        For Each kvpItem As KeyValuePair(Of WeakReference(Of TKey), TValue) In _instances
            _instances.TryRemove(kvpItem.Key, Nothing)
        Next
    End Sub
    Public Sub CopyTo(array() As KeyValuePair(Of TKey, TValue), arrayIndex As Integer) Implements ICollection(Of KeyValuePair(Of TKey, TValue)).CopyTo
        Dim cValue As New Collection(Of KeyValuePair(Of TKey, TValue)) 'Setup collection copying from
        For Each kvpItem As KeyValuePair(Of WeakReference(Of TKey), TValue) In _instances 'Go through each instance
            Dim tKey As TKey = Nothing
            If kvpItem.Key.TryGetTarget(tKey) AndAlso tKey IsNot Nothing AndAlso Not tKey.IsDisposed AndAlso kvpItem.Value IsNot Nothing Then 'Determine if key and value are valid
                cValue.Add(New KeyValuePair(Of TKey, TValue)(tKey, kvpItem.Value)) 'Add key and value to collection
            Else 'Key or value not valid
                _instances.TryRemove(kvpItem.Key, Nothing) 'Remove instance
            End If
        Next
        cValue.CopyTo(array, arrayIndex) 'Copy collection to array
    End Sub
    Public Iterator Function GetEnumerator() As IEnumerator(Of KeyValuePair(Of TKey, TValue)) Implements IEnumerable(Of KeyValuePair(Of TKey, TValue)).GetEnumerator
        For Each kvpItem As KeyValuePair(Of WeakReference(Of TKey), TValue) In _instances 'Go through each instance
            Dim tKey As TKey = Nothing
            If kvpItem.Key.TryGetTarget(tKey) AndAlso tKey IsNot Nothing AndAlso Not tKey.IsDisposed AndAlso kvpItem.Value IsNot Nothing Then 'Determine if key and value are valid
                Yield New KeyValuePair(Of TKey, TValue)(tKey, kvpItem.Value) 'Yeild the key and value
            Else 'Key or value not valid
                _instances.TryRemove(kvpItem.Key, Nothing) 'Remove instance
            End If
        Next
    End Function
    Public Function Remove(item As KeyValuePair(Of TKey, TValue)) As Boolean Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Remove
        For Each kvpItem As KeyValuePair(Of WeakReference(Of TKey), TValue) In _instances 'Go through each instance
            Dim tKey As TKey = Nothing
            If kvpItem.Key.TryGetTarget(tKey) AndAlso tKey IsNot Nothing AndAlso Not tKey.IsDisposed Then 'Determine if key is valid
                If ReferenceEquals(tKey, item.Key) Then 'Determine if desired key is found
                    _instances.TryRemove(kvpItem.Key, Nothing) 'Remove instance
                    Return kvpItem.Value IsNot Nothing 'Return true if value for key is valid; otherwise, false
                End If
            Else 'Key not valid
                _instances.TryRemove(kvpItem.Key, Nothing) 'Remove instance
            End If
        Next
        Return False 'Key not found, so value not found
    End Function
    Public Function Remove(key As TKey) As Boolean Implements IDictionary(Of TKey, TValue).Remove
        If key Is Nothing Then Throw New ArgumentNullException("key cannot be null.")
        For Each kvpItem As KeyValuePair(Of WeakReference(Of TKey), TValue) In _instances 'Go through each instance
            Dim tKey As TKey = Nothing
            If kvpItem.Key.TryGetTarget(tKey) AndAlso tKey IsNot Nothing AndAlso Not tKey.IsDisposed Then 'Determine if key is valid
                If ReferenceEquals(tKey, key) Then 'Determine if desired key is found
                    _instances.TryRemove(kvpItem.Key, Nothing) 'Remove instance
                    Return kvpItem.Value IsNot Nothing 'Return true if value for key is valid; otherwise, false
                End If
            Else 'Key not valid
                _instances.TryRemove(kvpItem.Key, Nothing) 'Remove instance
            End If
        Next
        Return False 'Key not found, so value not found
    End Function
    Public Function TryGetValue(key As TKey, ByRef value As TValue) As Boolean Implements IDictionary(Of TKey, TValue).TryGetValue
        If key Is Nothing Then Throw New ArgumentNullException("key cannot be null.")
        For Each kvpItem As KeyValuePair(Of WeakReference(Of TKey), TValue) In _instances 'Go through each instance
            Dim tKey As TKey = Nothing
            If kvpItem.Key.TryGetTarget(tKey) AndAlso tKey IsNot Nothing AndAlso Not tKey.IsDisposed Then 'Determine if key is valid
                If ReferenceEquals(tKey, key) Then 'Determine if desired key is found
                    If kvpItem.Value IsNot Nothing Then 'Determine if value for key is valid
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
