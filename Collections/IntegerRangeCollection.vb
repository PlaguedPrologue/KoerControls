Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Drawing.Design

'''<summary>Repersents a collection of integer ranges.</summary>
<Editor(GetType(IntegerRangeCollectionEditor), GetType(UITypeEditor))>
Public Class IntegerRangeCollection
    Inherits ObservableCollection(Of IntegerRange)
End Class
