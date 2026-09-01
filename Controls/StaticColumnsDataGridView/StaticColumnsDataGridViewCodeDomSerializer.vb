Imports System.CodeDom
Imports System.ComponentModel
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
    Private Function IsDefaultPropertyValue(item As Object, defaultItem As Object, propertyName As String) As Boolean
        If item Is Nothing OrElse defaultItem Is Nothing Then Return False 'If either item or defaultItem is nothing, the value does not match
        Dim pdProperty As PropertyDescriptor = TypeDescriptor.GetProperties(item)(propertyName) 'Get the property descriptor
        If pdProperty Is Nothing Then Return False 'If the property descriptor is nothing, the value does not match
        Return Equals(pdProperty.GetValue(item), pdProperty.GetValue(defaultItem)) 'Determine if the item value matches the default item value
    End Function
    'Removes all statements that are trying to add, create, or assign a column
    Private Sub RemoveColumnStatements(statements As CodeStatementCollection)
        For i As Integer = statements.Count - 1 To 0 Step -1
            Dim csStatment As CodeStatement = statements(i)
            If IsColumnCreationStatement(csStatment) OrElse IsColumnAddStatement(csStatment) OrElse IsColumnFieldAssignment(csStatment) Then statements.RemoveAt(i)
        Next
    End Sub
    'Removes all empty comment statements not related to the header statements
    Private Sub RemoveEmptyComments(statements As CodeStatementCollection)
        For iIndex As Integer = statements.Count - 1 To 5 Step -1
            Dim ccsComment As CodeCommentStatement = TryCast(statements(iIndex), CodeCommentStatement)
            If ccsComment IsNot Nothing AndAlso String.IsNullOrEmpty(ccsComment.Comment.Text) Then statements.RemoveAt(iIndex)
        Next
    End Sub
    'Make the designer.vb file containing a StaticColumnsDataGridView serialize the columns in a way that plays friendly when the StaticColumnsDataGridView class is in a different project.
    Private Sub RewriteReferences(value As Object, defaultValue As Object, statements As CodeStatementCollection)
        'Get the StaticColumnsDataGridView being worked on
        Dim scdgvValue As StaticColumnsDataGridView = TryCast(value, StaticColumnsDataGridView)
        If scdgvValue Is Nothing Then Return
        Dim scdgvDefault As StaticColumnsDataGridView = TryCast(defaultValue, StaticColumnsDataGridView)
        If scdgvDefault Is Nothing Then Return

        For iIndex As Integer = statements.Count - 1 To 0 Step -1
            Dim csStatement As CodeStatement = statements(iIndex)

            'Get code assign statement for property
            Dim casAssign As CodeAssignStatement = TryCast(csStatement, CodeAssignStatement)
            If casAssign Is Nothing Then Continue For

            'Get property
            Dim cpreProperty As CodePropertyReferenceExpression = TryCast(casAssign.Left, CodePropertyReferenceExpression)
            If cpreProperty Is Nothing Then Continue For

            ''Determine if the property is ContextMenuStrip
            'If cpreProperty.PropertyName = NameOf(StaticColumnsDataGridView.DefaultContextMenuStrip) Then
            '    statements.RemoveAt(iIndex) 'Remove statement, non-pre defined tool stirp items should NEVER contain a statement from the StaticColumnsDataGridView. It is already a statement in the form designer.
            '    Continue For
            'End If



            'Get value of property
            Dim cpreValueReference As CodePropertyReferenceExpression = TryCast(cpreProperty.TargetObject, CodePropertyReferenceExpression)
            If cpreValueReference Is Nothing Then Continue For

            'Determine if value is in the Columns
            Dim dgvcColumn As DataGridViewColumn = scdgvValue.Columns(cpreValueReference.PropertyName)
            If dgvcColumn IsNot Nothing Then

                'Determine if property value is equal to the default property value
                If IsDefaultPropertyValue(dgvcColumn, scdgvDefault.Columns(cpreValueReference.PropertyName), cpreProperty.PropertyName) Then 'Determine if column property value is default value
                    statements.RemoveAt(iIndex) 'Remove statement
                    Continue For
                End If

                'Get the type of the DataGridViewColumn
                Dim tColumnType As Type = dgvcColumn.GetType
                'Add the GetStaticColumn function for the StaticColumnsDataGridView to the statement
                Dim cmieGetColumn As New CodeMethodInvokeExpression(cpreValueReference.TargetObject, NameOf(StaticColumnsDataGridView.GetStaticColumn), New CodePrimitiveExpression(cpreValueReference.PropertyName))
                'Add cast to the column type to the statement
                Dim cceColumnCast As New CodeCastExpression(tColumnType, cmieGetColumn)
                'Make designer set property assignment to be like DirectCast([DataGridView].Columns([ColumnName]).[Property], [ColumnType]) = [value]
                casAssign.Left = New CodePropertyReferenceExpression(cceColumnCast, cpreProperty.PropertyName)
                Continue For
            End If

            'Determine if value in the ContextMenuStrip Items
            Dim tsiItem As ToolStripItem = scdgvValue.GetDefaultContextMenuStripItem(cpreValueReference.PropertyName)
            If tsiItem IsNot Nothing Then

                'Determine if property value is equal to the default property value
                If IsDefaultPropertyValue(tsiItem, scdgvDefault.DefaultContextMenuStrip?.Items(cpreValueReference.PropertyName), cpreProperty.PropertyName) Then 'Determine if tool strip item property value is default value
                    statements.RemoveAt(iIndex) 'Remove statement
                    Continue For
                End If

                'Get the type of the ToolStripItem
                Dim tToolStripItemType As Type = tsiItem.GetType
                'Add the GetDefaultContextMenuStripItem function for the StaticColumnsDataGridView to the statement
                Dim cmieGetColumn As New CodeMethodInvokeExpression(cpreValueReference.TargetObject, NameOf(StaticColumnsDataGridView.GetDefaultContextMenuStripItem), New CodePrimitiveExpression(cpreValueReference.PropertyName))
                'Add cast to the tool strip item type to the statement
                Dim cceColumnCast As New CodeCastExpression(tToolStripItemType, cmieGetColumn)
                'Make designer set property assignment to be like DirectCast([DataGridView].DefaultContextMenuStrip.Items([ItemName]).[Property], [ItemType]) = [value]
                casAssign.Left = New CodePropertyReferenceExpression(cceColumnCast, cpreProperty.PropertyName)
                Continue For
            End If
        Next
    End Sub

    'Serializes all properties To display In the Properties window When editing a StaticColumnsDataGridView In a Form. Removes all properties that can edit the number Of columns.
    Public Overrides Function Serialize(manager As IDesignerSerializationManager, value As Object) As Object
        If manager Is Nothing OrElse value Is Nothing Then Return Nothing 'If manager or value is nothing, exit
        If Not GetType(StaticColumnsDataGridView).IsAssignableFrom(value.GetType()) Then Return Nothing 'If value is not a StaticColumnsDataGridView, exit
        Dim baseSerializer As CodeDomSerializer = TryCast(manager.GetSerializer(GetType(DataGridView), GetType(CodeDomSerializer)), CodeDomSerializer) 'Get the DataGridViewCodeDomSerializer
        Dim oSerializedValue = baseSerializer.Serialize(manager, value) 'Serialize the items of a the value as a DataGridView
        Dim cscStetments = TryCast(oSerializedValue, CodeStatementCollection) 'Attempt to cast the items as CodeStatements 
        If cscStetments Is Nothing Then Return oSerializedValue 'If items are not CodeStatments, return the default DataGridView serialization
        RemoveColumnStatements(cscStetments) 'Remove column add/remove statements
        RewriteReferences(value, Activator.CreateInstance(value.GetType()), cscStetments) 'Rewrite statement reference to remove unnecessary assignments
        RemoveEmptyComments(cscStetments) 'Remove extra empty comment statements not related to the header statement
        Return oSerializedValue 'Return new items
    End Function

End Class
