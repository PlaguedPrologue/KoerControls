Imports System.ComponentModel.Design

Friend Class StringCollectionEditor
    Inherits CollectionEditor

    Public Sub New(type As Type)
        MyBase.New(type)
    End Sub

    Protected Overrides Function CreateCollectionForm() As CollectionForm
        Return New StringCollectionForm(Me)
    End Function
    Protected Overrides Function CreateCollectionItemType() As Type
        Return GetType(String)
    End Function

End Class
