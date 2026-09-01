Imports System.ComponentModel

'''<summary>Represents a group that links TabControls and TabPages together.</summary>
Public Class TabGroup
    Inherits NamedComponent
    Implements IComponent, IDisposable

    '''<summary>Create a new empty instance of the <see cref="DataGridViewLinkingGroup"/> clas.</summary>
    Public Sub New()
        MyBase.New
    End Sub
    '''<summary>Creates a new instance of the <see cref="DataGridViewLinkingGroup"/> class.</summary>
    '''<param name="value">The name value of the <see cref="DataGridViewLinkingGroup"/>.</param>
    Public Sub New(value As String)
        MyBase.New(value)
    End Sub
    '''<summary>Creates a new instance of the <see cref="DataGridViewLinkingGroup"/> class.</summary>
    '''<param name="value">The name value of the <see cref="DataGridViewLinkingGroup"/>.</param>
    '''<param name="container">The owning <see cref="IContainer"/> of the <see cref="DataGridViewLinkingGroup"/>.</param>
    Public Sub New(value As String, container As IContainer)
        MyBase.New(value, container)
    End Sub

End Class
