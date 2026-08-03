Imports System.ComponentModel
Imports System.ComponentModel.Design

Partial Friend Class TabGroupCollectionEditor

    Protected Class TabGroupCollectionForm
        Inherits StringCollectionForm

        Public Sub New(editor As CollectionEditor)
            MyBase.New(editor)
        End Sub

        Protected Overrides Sub OnAccepted(e As CancelEventArgs)
            Dim tgValues As New List(Of TabGroup)
            Dim sNewValues() As String = GetTextItems()
            Dim sDuplicateValueChecker As New HashSet(Of String)
            For iValueIndex As Integer = 0 To sNewValues.Length - 1 'Go through each item that will be set
                Dim sNewValue As String = sNewValues(iValueIndex) 'Get the new value at the specified index

                If Not sDuplicateValueChecker.Add(sNewValue) Then 'Determine if trying to add a duplicate value
                    MessageBox.Show($"Multiple TabGroups of name {sNewValue} attempted to be added. Items will not be changed.") 'Notify user error
                    e.Cancel = True 'Cancel accept event to keep editng form open
                    Return 'Exit
                End If

                Dim tgTabGroup As TabGroup
                If iValueIndex < Items.Length Then 'Determine if potentially changing an existing index
                    tgTabGroup = Items(iValueIndex) 'Get the item at the index
                    Dim sOldValue As String = tgTabGroup.Value 'Get the old value of the item
                    If sNewValue <> sOldValue Then 'Determine if item changes value
                        Dim ccsChangeService As IComponentChangeService = TryCast(Context.GetService(GetType(IComponentChangeService)), IComponentChangeService) 'Get change service for form designer
                        Dim pdProperty As PropertyDescriptor = TypeDescriptor.GetProperties(tgTabGroup)(NameOf(TabGroup.Value)) 'Get the property descriptor
                        ccsChangeService?.OnComponentChanging(tgTabGroup, pdProperty) 'State the component is changing
                        tgTabGroup.Value = sNewValue 'Set value
                        ccsChangeService?.OnComponentChanged(tgTabGroup, pdProperty, sOldValue, sNewValue) 'State the componenet has changed
                    End If
                Else 'Adding new item
                    tgTabGroup = New TabGroup(sNewValue, Context.Container) 'Create new item
                End If
                If tgTabGroup IsNot Nothing Then tgValues.Add(tgTabGroup) 'Add new item to new list
            Next

            For iValueIndex As Integer = sNewValues.Length To Items.Length - 1 'Go through each item that will end up being remove
                TryCast(Items(iValueIndex), TabGroup)?.Dispose() 'Dispose item to remove from form designer
            Next

            Items = tgValues.ToArray 'Set new items array
        End Sub

    End Class

End Class
