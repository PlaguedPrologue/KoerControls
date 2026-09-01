Imports System.ComponentModel
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
        If TypeOf Component IsNot StaticColumnsDataGridView Then Return 'Exit if component is not a StaticColumnsDataGridView
        Dim dhHost As IDesignerHost = CType(GetService(GetType(IDesignerHost)), IDesignerHost) 'Get designer displaying properties
        If dhHost Is Nothing Then Return 'Exit if host does not exist

        If ReferenceEquals(dhHost.RootComponent, Component) Then 'Deal with properties of a StaticColumnsDataGridView inheriting designer
            properties.Remove(NameOf(DataGridView.ContextMenuStrip)) 'Remove 'ContextMenuStrip' property

        Else 'Deal with properties of StaticColumnsDataGridView inheriting controls inside another designer
            properties.Remove(NameOf(DataGridView.Columns)) 'Remove 'Columns' property
            properties.Remove(NameOf(StaticColumnsDataGridView.DefaultContextMenuStrip)) 'Remove 'DefaultContextMenuStrip' property
            Dim pdContextmenuStrip As PropertyDescriptor = TryCast(properties(NameOf(Windows.Forms.Control.ContextMenuStrip)), PropertyDescriptor)
            If pdContextmenuStrip IsNot Nothing Then properties(NameOf(Windows.Forms.Control.ContextMenuStrip)) = New StaticColumnsDataGridViewContextMenuStripPropertyDescriptor(pdContextmenuStrip)
        End If
    End Sub

End Class
