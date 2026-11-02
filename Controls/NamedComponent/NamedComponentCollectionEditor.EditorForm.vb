Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Reflection

Partial Friend Class NamedComponentCollectionEditor

    Protected Class EditorForm
        Inherits StringCollectionForm

        Private ReadOnly _itemType As Type

        Public Sub New(editor As CollectionEditor, itemType As Type)
            MyBase.New(editor)
            _itemType = itemType
        End Sub

        Protected Overrides Sub OnAccepted(e As CancelEventArgs)
            Dim oValues As New List(Of Object)
            Dim sNewValues() As String = GetTextItems()
            Dim sDuplicateValueChecker As New HashSet(Of String)

            Dim ciConstructor As ConstructorInfo = _itemType.GetConstructor({GetType(String), GetType(IContainer)}) 'Check if there is a constructor with a string and IContainer input
            Dim bContainedType As Boolean = ciConstructor IsNot Nothing
            If Not bContainedType Then ciConstructor = _itemType.GetConstructor({GetType(String)}) 'Check if there is a constructor with a string input
            If ciConstructor Is Nothing Then 'No valid constructor found
                MessageBox.Show($"No valid constructor found for {_itemType.Name}.{Environment.NewLine}Please make sure there is at least a constructor with a string input, and preferrable a IContainer input as well.")
                e.Cancel = True 'Cancel accept event to keep editng form open
                Return 'Exit
            End If

            For Each sNewValue As String In sNewValues
                If Not sDuplicateValueChecker.Add(sNewValue) Then 'Determine if trying to add a duplicate value
                    MessageBox.Show($"Multiple Components of name {sNewValue} attempted to be added. Items will not be changed.") 'Notify user error
                    e.Cancel = True 'Cancel accept event to keep editng form open
                    Return 'Exit
                End If
            Next

            For iValueIndex As Integer = 0 To sNewValues.Length - 1 'Go through each item that will be set
                Dim sNewValue As String = sNewValues(iValueIndex) 'Get the new value at the specified index
                Dim ncItem As NamedComponent
                If iValueIndex < Items.Length Then 'Determine if potentially changing an existing index
                    ncItem = Items(iValueIndex)
                    Dim sOldValue As String = ncItem.Value
                    If sNewValue <> sOldValue Then
                        Dim ccsChangeService As IComponentChangeService = TryCast(Context.GetService(GetType(IComponentChangeService)), IComponentChangeService) 'Get change service for form designer
                        Dim pdProperty As PropertyDescriptor = TypeDescriptor.GetProperties(ncItem)(NameOf(NamedComponent.Value)) 'Get the property descriptor
                        ccsChangeService?.OnComponentChanging(ncItem, pdProperty) 'State the component is changing
                        ncItem.Value = sNewValue 'Set value
                        ccsChangeService?.OnComponentChanged(ncItem, pdProperty, sOldValue, sNewValue) 'State the componenet has changed
                    End If
                ElseIf bContainedType Then
                    ncItem = ciConstructor.Invoke(New Object() {sNewValue, Context.Container}) 'Constructor with a IContainer found.
                Else
                    ncItem = ciConstructor.Invoke(New Object() {sNewValue}) 'Constructor with a name input found.
                End If
                If ncItem IsNot Nothing Then oValues.Add(ncItem)
            Next

            For iValueIndex As Integer = sNewValues.Length To Items.Length - 1 'Go through each item that will end up being remove
                TryCast(Items(iValueIndex), NamedComponent)?.Dispose() 'Dispose item to remove from form designer
            Next

            Items = oValues.ToArray 'Set new items array
        End Sub

    End Class

End Class
