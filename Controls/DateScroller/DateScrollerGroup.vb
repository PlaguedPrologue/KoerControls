Imports System.ComponentModel

'''<summary>Represents a group that links DateScrollerItems together.</summary>
Public Class DateScrollerGroup
    Inherits NamedComponent
    Implements IComponent, IDisposable

    '''<summary>Create a new empty instance of the <see cref="DateScrollerGroup"/> clas.</summary>
    Public Sub New()
        MyBase.New
    End Sub
    '''<summary>Creates a new instance of the <see cref="DateScrollerGroup"/> class.</summary>
    '''<param name="value">The name value of the <see cref="DateScrollerGroup"/>.</param>
    Public Sub New(value As String)
        MyBase.New(value)
    End Sub
    '''<summary>Creates a new instance of the <see cref="DateScrollerGroup"/> class.</summary>
    '''<param name="value">The name value of the <see cref="DateScrollerGroup"/>.</param>
    '''<param name="container">The owning <see cref="IContainer"/> of the <see cref="DateScrollerGroup"/>.</param>
    Public Sub New(value As String, container As IContainer)
        MyBase.New(value, container)
    End Sub

End Class
