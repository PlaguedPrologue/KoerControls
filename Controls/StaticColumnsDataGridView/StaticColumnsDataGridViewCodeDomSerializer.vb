Imports System.CodeDom
Imports System.ComponentModel.Design.Serialization

'Used to keep columns from being edited outside of a direct StaticColumnsDataGridView object.
Public Class StaticColumnsDataGridViewCodeDomSerializer
    Inherits CodeDomSerializer

    'Checks if a statement is trying to add columns
    Private Function IsColumnAddStatement(statement As CodeStatement) As Boolean
        Dim cesStatement As CodeExpressionStatement = TryCast(statement, CodeExpressionStatement)
        If cesStatement Is Nothing Then Return False
        Dim cmieInvoke As CodeMethodInvokeExpression = TryCast(cesStatement.Expression, CodeMethodInvokeExpression)
        If cmieInvoke Is Nothing Then Return False
        If cmieInvoke.Method.MethodName <> "Add" AndAlso cmieInvoke.Method.MethodName <> "AddRange" Then Return False
        Return cmieInvoke.Method.TargetObject?.ToString.Contains("Columns")
    End Function
    'Checks if a statement is trying to create columns
    Private Function IsColumnCreationStatement(statement As CodeStatement) As Boolean
        Dim casAssign As CodeAssignStatement = TryCast(statement, CodeAssignStatement)
        If casAssign Is Nothing Then Return False
        Dim coceCreate As CodeObjectCreateExpression = TryCast(casAssign.Right, CodeObjectCreateExpression)
        If coceCreate Is Nothing Then Return False
        Dim tType As Type = Type.GetType(coceCreate.CreateType.BaseType)
        Return tType IsNot Nothing AndAlso GetType(DataGridViewColumn).IsAssignableFrom(tType)
    End Function
    'Checks if a statement is trying to assign a new column in the Designer.vb file
    Private Function IsColumnFieldAssignment(statement As CodeStatement) As Boolean
        Dim casAssign As CodeAssignStatement = TryCast(statement, CodeAssignStatement)
        If casAssign Is Nothing Then Return False
        Dim cfreField As CodeFieldReferenceExpression = TryCast(casAssign.Left, CodeFieldReferenceExpression)
        If cfreField Is Nothing Then Return False
        Return cfreField.FieldName.EndsWith("Column", StringComparison.OrdinalIgnoreCase)
    End Function
    'Removes all statements that are trying to add, create, or assign a column
    Private Sub RemoveColumnStatements(statements As CodeStatementCollection)
        For i As Integer = statements.Count - 1 To 0 Step -1
            Dim csStatment As CodeStatement = statements(i)
            If IsColumnCreationStatement(csStatment) OrElse IsColumnAddStatement(csStatment) OrElse IsColumnFieldAssignment(csStatment) Then statements.RemoveAt(i)
        Next
    End Sub
    'Make the designer.vb file containing a StaticColumnsDataGridView serialize the columns in a way that plays friendly when the StaticColumnsDataGridView class is in a different project.
    Private Sub RewriteReferences(value As Object, statements As CodeStatementCollection)
        For iIndex As Integer = statements.Count - 1 To 0 Step -1
            Dim csStatement As CodeStatement = statements(iIndex)

            'Get the StaticColumnsDataGridView being worked on
            Dim scdgvDataGridView As StaticColumnsDataGridView = TryCast(value, StaticColumnsDataGridView)
            If scdgvDataGridView Is Nothing Then Continue For

            'Get code assign statement for property
            Dim casAssign As CodeAssignStatement = TryCast(csStatement, CodeAssignStatement)
            If casAssign Is Nothing Then Continue For

            'Get property
            Dim cpreProperty As CodePropertyReferenceExpression = TryCast(casAssign.Left, CodePropertyReferenceExpression)
            If cpreProperty Is Nothing Then Continue For

            'Get value of property
            Dim cpreColumnReference As CodePropertyReferenceExpression = TryCast(cpreProperty.TargetObject, CodePropertyReferenceExpression)
            If cpreColumnReference Is Nothing Then Continue For

            'Determine if value type is a DataGridViewColumn
            Dim dgvcColumn As DataGridViewColumn = scdgvDataGridView.Columns(cpreColumnReference.PropertyName)
            If dgvcColumn IsNot Nothing Then

                'Get the type of the DataGridViewColumn
                Dim tColumnType As Type = dgvcColumn.GetType
                'Add the GetStaticColumn function for the StaticColumnsDataGridView to the statement
                Dim cmieGetColumn As New CodeMethodInvokeExpression(cpreColumnReference.TargetObject, NameOf(StaticColumnsDataGridView.GetStaticColumn), New CodePrimitiveExpression(cpreColumnReference.PropertyName))
                'Add cast to the column type to the statement
                Dim cceColumnCast As New CodeCastExpression(tColumnType, cmieGetColumn)
                'Make designer set property assignment to be like DirectCast([DataGridView].Columns([ColumnName]).[Property], [ColumnType]) = [value]
                casAssign.Left = New CodePropertyReferenceExpression(cceColumnCast, cpreProperty.PropertyName)
                Continue For
            End If

            If scdgvDataGridView.PreDefinedCotextMenuStrip Is Nothing Then 'Determine if StaticColumnsDataGridView has no pre defined context menu stirp
                statements.RemoveAt(iIndex) 'Remove statement, non-pre defined tool stirp items should NEVER contain a statement from the StaticColumnsDataGridView. It is already a statement in the form designer.
                Continue For
            Else
                Dim tsiItem As ToolStripItem = scdgvDataGridView.ContextMenuStrip?.Items(cpreColumnReference.PropertyName)
                If tsiItem IsNot Nothing Then

                    'Get the type of the ToolStripItem
                    Dim tToolStripItemType As Type = tsiItem.GetType
                    'Add the GetContextMenuStripItem function for the StaticColumnsDataGridView to the statement
                    Dim cmieGetColumn As New CodeMethodInvokeExpression(cpreColumnReference.TargetObject, NameOf(StaticColumnsDataGridView.GetContextMenuStripItem), New CodePrimitiveExpression(cpreColumnReference.PropertyName))
                    'Add cast to the tool strip item type to the statement
                    Dim cceColumnCast As New CodeCastExpression(tToolStripItemType, cmieGetColumn)
                    'Make designer set property assignment to be like DirectCast([DataGridView].ContextMenuStrip.Items([ItemName]).[Property], [ItemType]) = [value]
                    casAssign.Left = New CodePropertyReferenceExpression(cceColumnCast, cpreProperty.PropertyName)
                    Continue For
                End If
            End If
        Next
    End Sub

    'Serializes all properties To display In the Properties window When editing a StaticColumnsDataGridView In a Form. Removes all properties that can edit the number Of columns.
    Public Overrides Function Serialize(manager As IDesignerSerializationManager, value As Object) As Object
        If manager Is Nothing OrElse value Is Nothing Then Return Nothing
        If Not GetType(StaticColumnsDataGridView).IsAssignableFrom(value.GetType()) Then
            'MessageBox.Show("LincolnControls reference has separated into two instance. Please clear out the bin/Debug and obj/Debug folders of your project and rebuild.")
            Return Nothing
        End If
        Dim baseSerializer As CodeDomSerializer = TryCast(manager.GetSerializer(GetType(DataGridView), GetType(CodeDomSerializer)), CodeDomSerializer)
        Dim oSerializedValue = baseSerializer.Serialize(manager, value)
        Dim cscStetments = TryCast(oSerializedValue, CodeStatementCollection)
        If cscStetments IsNot Nothing Then
            RemoveColumnStatements(cscStetments)
            RewriteReferences(value, cscStetments)
        End If
        Return oSerializedValue
    End Function

End Class
