Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Drawing.Design

''''<summary>Repersents a collection of tab groups.</summary>
<Editor(GetType(TabGroupCollectionEditor), GetType(UITypeEditor))>
Public Class TabGroupCollection
    Inherits Collection(Of TabGroup)
End Class
