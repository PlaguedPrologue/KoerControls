'''<summary>Notifies listeners of dynamic chnages, such as when an item is added and removed or the whole list is cleared.</summary>
Public Interface INotifyHashSetChanged

    '''<summary>Occurs when the hash set changes.</summary>
    Event CollectionChanged As NotifyHashSetChangedEventHandler

End Interface
