Friend Class NamedComponentCollectionEditor
    Inherits StringCollectionEditor

    Public Sub New()
        MyBase.New(GetType(NamedComponentCollection(Of NamedComponent)))
    End Sub
    Public Sub New(type As Type)
        MyBase.New(type)
    End Sub

    Protected Overrides Function CreateCollectionForm() As CollectionForm
        Return New EditorForm(Me, CollectionItemType)
    End Function
    Protected Overrides Function CreateCollectionItemType() As Type
        Dim tCollectionType As Type = Context.PropertyDescriptor.PropertyType
        While tCollectionType IsNot Nothing
            If tCollectionType.IsGenericType AndAlso tCollectionType.GetGenericTypeDefinition() = GetType(NamedComponentCollection(Of )) Then Return tCollectionType.GetGenericArguments()(0)
            tCollectionType = tCollectionType.BaseType
        End While
        Return MyBase.CreateCollectionItemType
    End Function

End Class
