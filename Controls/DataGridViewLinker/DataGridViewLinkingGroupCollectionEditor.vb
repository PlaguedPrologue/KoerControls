Friend Class DataGridViewLinkingGroupCollectionEditor
    Inherits StringCollectionEditor

    Public Sub New(type As Type)
        MyBase.New(type)
    End Sub

    Protected Overrides Function CreateCollectionForm() As CollectionForm
        Return New DataGridViewLinkingGroupCollectionForm(Me)
    End Function
    Protected Overrides Function CreateCollectionItemType() As Type
        Return GetType(DataGridViewLinkingGroup)
    End Function

End Class
