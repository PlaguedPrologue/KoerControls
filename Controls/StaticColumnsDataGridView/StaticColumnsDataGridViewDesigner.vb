Imports System.ComponentModel.Design
Imports System.ComponentModel.Design.Serialization
Imports System.Windows.Forms.Design

'This class is used to help with displaying a StaticColumnsDataGridView.
Public Class StaticColumnsDataGridViewDesigner
    Inherits ParentControlDesigner

    'keeps the '[Form].Designer.vb]' file from serializing each column in the StaticColumnsDataGridView (creating a separate object for each column in the Form).
    '   Instead, it makes the columns become similar to properties, going down the list of [StaticColumnsDataGridView].[Column1].[Column1Property] = [value].
    Protected Overrides Sub PreFilterAttributes(attributes As IDictionary)
        MyBase.PreFilterAttributes(attributes)
        If TypeOf Component Is StaticColumnsDataGridView Then attributes(GetType(DesignerSerializerAttribute)) = New DesignerSerializerAttribute(GetType(StaticColumnsDataGridViewCodeDomSerializer), GetType(CodeDomSerializer))
    End Sub
    'Removes the 'Columns' property from any StaticColumnsDataGridView being dropped into a form. Removes 'ContextMenuStrip' property from any StaticColumnsDataGridView with a predefined context menu strip
    Protected Overrides Sub PreFilterProperties(properties As IDictionary)
        MyBase.PreFilterProperties(properties)
        If TypeOf Component Is StaticColumnsDataGridView Then
            Dim dhHost As IDesignerHost = CType(GetService(GetType(IDesignerHost)), IDesignerHost)
            If dhHost IsNot Nothing AndAlso Not ReferenceEquals(dhHost.RootComponent, Component) Then properties.Remove(NameOf(DataGridView.Columns))
            If DirectCast(Component, StaticColumnsDataGridView).PreDefinedCotextMenuStrip IsNot Nothing Then properties.Remove(NameOf(Windows.Forms.Control.ContextMenuStrip))
        End If
    End Sub

End Class
