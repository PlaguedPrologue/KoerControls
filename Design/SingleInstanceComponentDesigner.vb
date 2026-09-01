Imports System.ComponentModel
Imports System.ComponentModel.Design

'''<summary>Extends the design mode behavior of a component. Allows only one instance per designer.</summary>
Public Class SingleInstanceComponentDesigner
    Inherits ComponentDesigner

    Public Overrides Sub Initialize(component As IComponent)
        MyBase.Initialize(component)

        Dim cContainer As IContainer = component.Site?.Container
        If cContainer Is Nothing Then Return
        Dim tComponentType As Type = component.GetType
        For Each cComponent As IComponent In cContainer.Components
            If cComponent.GetType Is tComponentType AndAlso Not ReferenceEquals(cComponent, component) Then
                MessageBox.Show($"Only one {tComponentType.Name} may be added to a form or control designer at a time.", tComponentType.Name, MessageBoxButtons.OK, MessageBoxIcon.Information)
                cContainer.Remove(component)
                component.Dispose()
                Return
            End If
        Next
    End Sub

End Class
