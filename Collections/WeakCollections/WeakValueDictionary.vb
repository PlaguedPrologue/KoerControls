Imports System.Collections.Concurrent
Imports System.Collections.ObjectModel

'''<summary>Represents a <see cref="Dictionary"/> of where only the value and value in each <see cref="KeyValuePair"/> is a <see cref="WeakReference"/>.</summary>
'''<typeparam name="TKey">The <see cref="Type"/> of the key of each <see cref="KeyValuePair"/>.</typeparam>
'''<typeparam name="TValue">The <see cref="Type"/> of the value of each <see cref="KeyValuePair"/>.</typeparam>
Public Class WeakValueDictionary(Of TKey, TValue As {Class, IDisposable})
    Implements ICollection(Of KeyValuePair(Of TKey, TValue)), IDictionary(Of TKey, TValue), IEnumerable(Of KeyValuePair(Of TKey, TValue)), IReadOnlyCollection(Of KeyValuePair(Of TKey, TValue)), IReadOnlyDictionary(Of TKey, TValue)

#Region "Private Items"

    Private ReadOnly _instances As New ConcurrentDictionary(Of TKey, WeakReference(Of TValue))
    Private ReadOnly _removeDisposedEntries As Boolean

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
        If item.Key Is Nothing Then Throw New ArgumentNullException(NameOf(item.Key))
        If item.Value Is Nothing Then Throw New ArgumentNullException(NameOf(item.Value))
        If _removeDisposedEntries Then
            For Each kvpItem As KeyValuePair(Of TKey, WeakReference(Of TValue)) In _instances 'Go through each instance
                If Equals(kvpItem.Key, item.Key) Then 'Deteremine if desired key is found
                    Dim tValue As TValue = Nothing
                    If kvpItem.Value.TryGetTarget(tValue) AndAlso tValue IsNot Nothing AndAlso Not tValue.IsDisposed Then 'Deteremine if value is valid
                        Return ReferenceEquals(tValue, item.Value) 'Return true if values match
                    Else
                        _instances.TryRemove(kvpItem.Key, Nothing) 'Remove instance
                        Return False 'Value is not valid, so desired key and value are not found
                    End If
                End If
            Next
        Else
            For Each kvpItem As KeyValuePair(Of TKey, WeakReference(Of TValue)) In _instances 'Go through each instance
                If Equals(kvpItem.Key, item.Value) Then 'Determine if desired key is found
                    Dim tValue As TValue = Nothing
                    kvpItem.Value.TryGetTarget(tValue)
                    Return ReferenceEquals(tValue, item.Value) 'Return true if values match
                End If
            Next
        End If
        Return False
    End Function
    Public Function ContainsKey(key As TKey) As Boolean Implements IDictionary(Of TKey, TValue).ContainsKey
        If key Is Nothing Then Throw New ArgumentNullException(NameOf(key))
        For Each kvpItem As KeyValuePair(Of TKey, WeakReference(Of TValue)) In _instances 'Go through each instance
            If Equals(kvpItem.Key, key) Then Return True 'Return true if desired key is found
        Next
        Return False
    End Function

#End Region

#Region "Initializers"

    '''<summary>Initializes a new instance of the <see cref="WeakValueDictionary"/> class.</summary>
    '''<param name="removeDisposedEntries"><see langword="True"/> if all key value pairs with a disposed value should be removed during enumeration; otherwise <see langword="False"/>.</param>
    Public Sub New(Optional removeDisposedEntries As Boolean = True)
        _removeDisposedEntries = removeDisposedEntries
    End Sub
    '''<summary>Initializes a new instacne of the <see cref="WeakValueDictionary"/> class tha contains elements copied from the specified collection.</summary>
    '''<param name="dictionary">The dictionary whose elements are copied to the new set.</param>
    '''<param name="removeDisposedEntries"><see langword="True"/> if all key value pairs with a disposed value should be removed during enumeration; otherwise <see langword="False"/>.</param>
    '''<exception cref="ArgumentNullException"><paramref name="dictionary"/> collection is null.</exception>
    Public Sub New(dictionary As IDictionary(Of TKey, TValue), Optional removeDisposedEntries As Boolean = True)
        If dictionary Is Nothing Then Throw New ArgumentNullException(NameOf(dictionary))
        For Each kvpItem As KeyValuePair(Of TKey, TValue) In dictionary
            _instances.TryAdd(kvpItem.Key, New WeakReference(Of TValue)(kvpItem.Value))
        Next
        _removeDisposedEntries = removeDisposedEntries
    End Sub

#End Region

#Region "Properties"

    Public ReadOnly Property Count As Integer Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Count
        Get
            If _removeDisposedEntries Then
                Dim iCount As Integer = 0
                For Each kvpItem As KeyValuePair(Of TKey, WeakReference(Of TValue)) In _instances 'Go through each instance
                    Dim tValue As TValue = Nothing
                    If kvpItem.Value.TryGetTarget(tValue) AndAlso tValue IsNot Nothing AndAlso Not tValue.IsDisposed Then 'Determine if value is valid
                        iCount += 1 'Add to cound
                    Else 'Value not valid
                        _instances.TryRemove(kvpItem.Key, Nothing) 'Remove instance
                    End If
                Next
                Return iCount
            Else
                Return _instances.Count
            End If
        End Get
    End Property
    Public ReadOnly Property IsReadOnly As Boolean = False Implements ICollection(Of KeyValuePair(Of TKey, TValue)).IsReadOnly
    Default Public Property Item(key As TKey) As TValue Implements IDictionary(Of TKey, TValue).Item
        Get
            If key Is Nothing Then Throw New ArgumentNullException(NameOf(key))
            If _removeDisposedEntries Then
                For Each kvpItem As KeyValuePair(Of TKey, WeakReference(Of TValue)) In _instances 'Go through each instance
                    If Equals(kvpItem.Key, key) Then 'Determine if desired key is found
                        Dim tValue As TValue = Nothing
                        If kvpItem.Value.TryGetTarget(tValue) AndAlso tValue IsNot Nothing AndAlso Not tValue.IsDisposed Then Return tValue 'Return value if valid
                        _instances.TryRemove(kvpItem.Key, Nothing) 'Remove instance
                        Return Nothing
                    End If
                Next
            Else
                For Each kvpItem As KeyValuePair(Of TKey, WeakReference(Of TValue)) In _instances 'Go through each instance
                    If Equals(kvpItem.Key, key) Then
                        Dim tValue As TValue = Nothing
                        kvpItem.Value.TryGetTarget(tValue)
                        Return tValue 'Return value if desired key is found
                    End If
                Next
            End If
            Return Nothing
        End Get
        Set(value As TValue)
            If key Is Nothing Then Throw New ArgumentNullException(NameOf(key))
            If value Is Nothing Then Throw New ArgumentNullException(NameOf(value))
            For Each kvpItem As KeyValuePair(Of TKey, WeakReference(Of TValue)) In _instances 'Go through each item
                If Equals(kvpItem.Key, key) Then 'Determine if desired key is found
                    _instances(key) = New WeakReference(Of TValue)(value) 'Add to instances
                    Return 'Exit
                End If
            Next
            _instances.TryAdd(key, New WeakReference(Of TValue)(value)) 'Add new key value pair
        End Set
    End Property
    Private ReadOnly Iterator Property Keys As IEnumerable(Of TKey) Implements IReadOnlyDictionary(Of TKey, TValue).Keys
        Get
            If _removeDisposedEntries Then
                For Each kvpItem As KeyValuePair(Of TKey, WeakReference(Of TValue)) In _instances 'Go through each instance
                    Dim tValue As TValue = Nothing
                    If kvpItem.Value.TryGetTarget(tValue) AndAlso tValue IsNot Nothing AndAlso Not tValue.IsDisposed Then 'Determine if value is valid
                        Yield kvpItem.Key 'Yield key
                    Else 'Value not valid
                        _instances.TryRemove(kvpItem.Key, Nothing) 'Remove instance
                    End If
                Next
            Else
                For Each kvpItem As KeyValuePair(Of TKey, WeakReference(Of TValue)) In _instances 'Go through each instance
                    Yield kvpItem.Key 'Yield key
                Next
            End If
        End Get
    End Property
    Private ReadOnly Iterator Property Values As IEnumerable(Of TValue) Implements IReadOnlyDictionary(Of TKey, TValue).Values
        Get
            If _removeDisposedEntries Then
                For Each kvpItem As KeyValuePair(Of TKey, WeakReference(Of TValue)) In _instances 'Go through each instance
                    Dim tValue As TValue = Nothing
                    If kvpItem.Value.TryGetTarget(tValue) AndAlso tValue IsNot Nothing AndAlso Not tValue.IsDisposed Then 'Determine if value is valid
                        Yield tValue 'Yield value
                    Else 'Value not valid
                        _instances.TryRemove(kvpItem.Key, Nothing) 'Remove instance
                    End If
                Next
            Else
                For Each kvpItem As KeyValuePair(Of TKey, WeakReference(Of TValue)) In _instances 'Go through each instance
                    Dim tValue As TValue = Nothing
                    kvpItem.Value.TryGetTarget(tValue)
                    Yield tValue 'Yield value
                Next
            End If
        End Get
    End Property

#End Region

#Region "Functions"

    Public Sub Add(item As KeyValuePair(Of TKey, TValue)) Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Add
        If item.Key Is Nothing Then Throw New ArgumentNullException(NameOf(item.Key))
        If item.Value Is Nothing Then Throw New ArgumentNullException(NameOf(item.Value))
        _instances.TryAdd(item.Key, New WeakReference(Of TValue)(item.Value))
    End Sub
    Public Sub Add(key As TKey, value As TValue) Implements IDictionary(Of TKey, TValue).Add
        If key Is Nothing Then Throw New ArgumentNullException(NameOf(key))
        If value Is Nothing Then Throw New ArgumentNullException(NameOf(value))
        _instances.TryAdd(key, New WeakReference(Of TValue)(value))
    End Sub
    Public Sub Clear() Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Clear
        For Each kvpItem As KeyValuePair(Of TKey, WeakReference(Of TValue)) In _instances
            _instances.TryRemove(kvpItem.Key, Nothing)
        Next
    End Sub
    Public Sub CopyTo(array() As KeyValuePair(Of TKey, TValue), arrayIndex As Integer) Implements ICollection(Of KeyValuePair(Of TKey, TValue)).CopyTo
        Dim cValue As New Collection(Of KeyValuePair(Of TKey, TValue)) 'Setup collection copying from
        If _removeDisposedEntries Then
            For Each kvpItem As KeyValuePair(Of TKey, WeakReference(Of TValue)) In _instances 'Go through each instance
                Dim tValue As TValue = Nothing
                If kvpItem.Value.TryGetTarget(tValue) AndAlso tValue IsNot Nothing AndAlso Not tValue.IsDisposed Then 'Determine if value is valid
                    cValue.Add(New KeyValuePair(Of TKey, TValue)(kvpItem.Key, tValue)) 'Add key and value to collection
                Else 'Value not valid
                    _instances.TryRemove(kvpItem.Key, Nothing) 'Remove instance
                End If
            Next
        Else
            For Each kvpItem As KeyValuePair(Of TKey, WeakReference(Of TValue)) In _instances 'Go through each instance
                Dim tValue As TValue = Nothing
                kvpItem.Value.TryGetTarget(tValue)
                cValue.Add(New KeyValuePair(Of TKey, TValue)(kvpItem.Key, tValue)) 'Add key and value to collection
            Next
        End If
        cValue.CopyTo(array, arrayIndex) 'Copy collection to array
    End Sub
    Public Iterator Function GetEnumerator() As IEnumerator(Of KeyValuePair(Of TKey, TValue)) Implements IEnumerable(Of KeyValuePair(Of TKey, TValue)).GetEnumerator
        If _removeDisposedEntries Then
            For Each kvpItem As KeyValuePair(Of TKey, WeakReference(Of TValue)) In _instances 'Go through each instance
                Dim tValue As TValue = Nothing
                If kvpItem.Value.TryGetTarget(tValue) AndAlso tValue IsNot Nothing AndAlso Not tValue.IsDisposed Then 'Determine if value is valid
                    Yield New KeyValuePair(Of TKey, TValue)(kvpItem.Key, tValue) 'Yield the key and value
                Else 'Value not valid
                    _instances.TryRemove(kvpItem.Key, Nothing) 'Remove instance
                End If
            Next
        Else
            For Each kvpItem As KeyValuePair(Of TKey, WeakReference(Of TValue)) In _instances 'Go through each instance
                Dim tValue As TValue = Nothing
                kvpItem.Value.TryGetTarget(tValue)
                Yield New KeyValuePair(Of TKey, TValue)(kvpItem.Key, tValue) 'Yield the key and value
            Next
        End If
    End Function
    Public Function Remove(item As KeyValuePair(Of TKey, TValue)) As Boolean Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Remove
        For Each kvpItem As KeyValuePair(Of TKey, WeakReference(Of TValue)) In _instances 'Go through each instance
            If Equals(kvpItem.Key, item.Key) Then 'Determine if desired key is found
                Return _instances.TryRemove(kvpItem.Key, Nothing) 'Remove instance
            End If
        Next
        Return False 'Key not found, so value not found
    End Function
    Public Function Remove(key As TKey) As Boolean Implements IDictionary(Of TKey, TValue).Remove
        For Each kvpItem As KeyValuePair(Of TKey, WeakReference(Of TValue)) In _instances 'Go through each instance
            If Equals(kvpItem.Key, key) Then 'Determine if desired key is found
                Return _instances.TryRemove(kvpItem.Key, Nothing) 'Remove instance
            End If
        Next
        Return False 'Key not found, so value not found
    End Function
    Public Function TryGetValue(key As TKey, ByRef value As TValue) As Boolean Implements IDictionary(Of TKey, TValue).TryGetValue
        For Each kvpItem As KeyValuePair(Of TKey, WeakReference(Of TValue)) In _instances 'Go through each instance
            If Equals(kvpItem.Key, key) Then 'Determine if desired key is found
                Dim tValue As TValue = Nothing
                kvpItem.Value.TryGetTarget(tValue)
                value = tValue 'Set value
                Return True 'Value found for key
            End If
        Next
        value = Nothing 'Key not found, remove instance
        Return False 'Key not found, so value not found
    End Function

#End Region

End Class
