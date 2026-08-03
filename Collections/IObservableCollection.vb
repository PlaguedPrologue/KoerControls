Imports System.Collections.Specialized

Public Interface IObservableCollection

    '''<summary>Occurs when the collection changes.</summary>
    Event CollectionChanged As NotifyCollectionChangedEventHandler

End Interface

'''<summary>Notifies listeners of dynamic chnages, such as when an item is added and removed or the whole list is cleared.</summary>
'''<typeparam name="T">The type of the elements.</typeparam>
Public Interface IObservableCollection(Of T)
    Inherits ICollection(Of T), IObservableCollection

End Interface