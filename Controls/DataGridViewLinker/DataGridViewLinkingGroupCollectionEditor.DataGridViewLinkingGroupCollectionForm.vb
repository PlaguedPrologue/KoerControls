Imports System.ComponentModel
Imports System.ComponentModel.Design

Partial Friend Class DataGridViewLinkingGroupCollectionEditor

    Protected Class DataGridViewLinkingGroupCollectionForm
        Inherits StringCollectionForm

        Public Sub New(editor As CollectionEditor)
            MyBase.New(editor)
        End Sub

        Protected Overrides Sub OnAccepted(e As CancelEventArgs)
            Dim dgvlgValues As New List(Of DataGridViewLinkingGroup)
            Dim sNewValues() As String = GetTextItems()
            Dim sDuplicateValueChecker As New HashSet(Of String)
            For iValueIndex As Integer = 0 To sNewValues.Length - 1 'Go through each item that will be set
                Dim sNewValue As String = sNewValues(iValueIndex) 'Get the new value at the specified index

                If Not sDuplicateValueChecker.Add(sNewValue) Then 'Determine if trying to add a duplicate value
                    MessageBox.Show($"Multiple LinkingGroups of name {sNewValue} attempted to be added. Items will not be changed.") 'Notify user error
                    e.Cancel = True 'Cancel accept event to keep editng form open
                    Return 'Exit
                End If

                Dim dgvlgLinkingGroup As DataGridViewLinkingGroup
                If iValueIndex < Items.Length Then 'Determine if potentially changing an existing index
                    dgvlgLinkingGroup = Items(iValueIndex) 'Get the item at the index
                    Dim sOldValue As String = dgvlgLinkingGroup.Value 'Get the old value of the item
                    If sNewValue <> dgvlgLinkingGroup.Value Then 'Determine if item changes value
                        Dim ccsChangeService As IComponentChangeService = TryCast(Context.GetService(GetType(IComponentChangeService)), IComponentChangeService) 'Get change service for form designer
                        Dim pdProperty As PropertyDescriptor = TypeDescriptor.GetProperties(dgvlgLinkingGroup)(NameOf(DataGridViewLinkingGroup.Value)) 'Get the property descriptor
                        ccsChangeService?.OnComponentChanging(dgvlgLinkingGroup, pdProperty) 'State the component is changing
                        dgvlgLinkingGroup.Value = sNewValue 'Set value
                        ccsChangeService?.OnComponentChanged(dgvlgLinkingGroup, pdProperty, sOldValue, sNewValue) 'State the compenent has changed
                    End If
                Else 'Adding new item
                    dgvlgLinkingGroup = New DataGridViewLinkingGroup(sNewValue, Context.Container) 'Create new item
                End If
                If dgvlgLinkingGroup IsNot Nothing Then dgvlgValues.Add(dgvlgLinkingGroup) 'Add new item to new list
            Next

            For iValueIndex As Integer = sNewValues.Length To Items.Length - 1 'Go through each item that will end up being remove
                TryCast(Items(iValueIndex), DataGridViewLinkingGroup)?.Dispose() 'Dispose item to remove from form designer
            Next

            Items = dgvlgValues.ToArray 'Set new items array
        End Sub

    End Class

End Class
