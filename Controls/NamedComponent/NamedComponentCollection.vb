Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Drawing.Design

''''<summary>Repersents a collection of named components.</summary>
<Editor(GetType(NamedComponentCollectionEditor), GetType(UITypeEditor))>
Public MustInherit Class NamedComponentCollection(Of T As NamedComponent)
    Inherits Collection(Of T)
End Class
