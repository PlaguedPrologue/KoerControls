Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Windows.Forms.Design

Partial Public Class Overlay

    Private Class HostEditor
        Inherits UITypeEditor

        Public Overrides Function EditValue(context As ITypeDescriptorContext, provider As IServiceProvider, value As Object) As Object
            If context Is Nothing OrElse provider Is Nothing Then Return value
            Dim oOverlay As Overlay = TryCast(context.Instance, Overlay)
            If oOverlay Is Nothing Then Return value
            Dim sSite As ISite = oOverlay.Site
            If sSite Is Nothing Then Return value
            Dim wfesService As IWindowsFormsEditorService = TryCast(provider.GetService(GetType(IWindowsFormsEditorService)), IWindowsFormsEditorService)
            If wfesService Is Nothing Then Return value
            Dim lbList As New ListBox With {.BorderStyle = BorderStyle.None, .IntegralHeight = True}
            lbList.Items.Add("(None)")
            For Each cComponent As IComponent In sSite.Container.Components
                If TypeOf cComponent Is Control Then lbList.Items.Add(cComponent)
            Next
            Dim cValue As Control = TryCast(value, Control)
            If cValue IsNot Nothing AndAlso lbList.Items.Contains(cValue) Then
                lbList.SelectedItem = cValue
            Else
                lbList.SelectedIndex = 0
            End If
            AddHandler lbList.Click, Sub(sender, e) wfesService.CloseDropDown()
            AddHandler lbList.DoubleClick, Sub(sender, e) wfesService.CloseDropDown()
            AddHandler lbList.SelectedIndexChanged, Sub(sender, e) If lbList.SelectedIndex = 0 Then Return
            wfesService.DropDownControl(lbList)
            If lbList.SelectedIndex <= 0 Then Return Nothing
            Return TryCast(lbList.SelectedItem, Control)
        End Function
        Public Overrides Function GetEditStyle(context As ITypeDescriptorContext) As UITypeEditorEditStyle
            Return UITypeEditorEditStyle.DropDown
        End Function
        Public Overrides Function GetPaintValueSupported(context As ITypeDescriptorContext) As Boolean
            Return False
        End Function

    End Class

End Class
