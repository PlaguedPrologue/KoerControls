Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Drawing.Design

'''<summary>Repersents a collection of data grid view linking groups.</summary>
<Editor(GetType(DataGridViewLinkingGroupCollectionEditor), GetType(UITypeEditor))>
Public Class DataGridViewLinkingGroupCollection
    Inherits Collection(Of DataGridViewLinkingGroup)
End Class
