Friend Class TabGroupCollectionEditor
    Inherits StringCollectionEditor

    Public Sub New(type As Type)
        MyBase.New(type)
    End Sub

    Protected Overrides Function CreateCollectionForm() As CollectionForm
        Return New TabGroupCollectionForm(Me)
    End Function
    Protected Overrides Function CreateCollectionItemType() As Type
        Return GetType(TabGroup)
    End Function

End Class
