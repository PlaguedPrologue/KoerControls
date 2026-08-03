Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Drawing.Design

'''<summary>Repersents a collection of pin stripes.</summary>
<Editor(GetType(PinStripeCollectionEditor), GetType(UITypeEditor))>
Public Class PinStripeCollection
    Inherits Collection(Of PinStripe)
End Class
