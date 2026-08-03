Imports System.ComponentModel

'''<summary>Represents a group that links TabControls and TabPages together.</summary>
<DesignTimeVisible(False)>
<ToolboxItem(False)>
Public Class TabGroup
    Implements IComponent, IDisposable

#Region "Private Items"

    Private _name As String

#End Region

#Region "Initializers"

    '''<summary>Create a new empty instance of the <see cref="TabGroup"/> clas.</summary>
    Public Sub New()
    End Sub
    '''<summary>Creates a new instance of the <see cref="TabGroup"/> class.</summary>
    '''<param name="value">The name value of the <see cref="TabGroup"/>.</param>
    Public Sub New(value As String)
        Me.Value = value
    End Sub
    '''<summary>Creates a new instance of the <see cref="TabGroup"/> class.</summary>
    '''<param name="value">The name value of the <see cref="TabGroup"/>.</param>
    '''<param name="container">The owning <see cref="IContainer"/> of the <see cref="TabGroup"/>.</param>
    Public Sub New(value As String, container As IContainer)
        container?.Add(Me)
        Me.Value = value
    End Sub

#End Region

#Region "Events"

    Public Event Disposed As EventHandler Implements IComponent.Disposed

#End Region

#Region "Properties"

    '''<summary>Gets or sets the name of the control in its site.</summary>
    '''<returns>The name of the control in its site.</returns>
    Public Property Name As String
        Get
            If Site IsNot Nothing AndAlso Not String.IsNullOrEmpty(Site.Name) Then _name = Site.Name
            Return _name
        End Get
        Set(value As String)
            If String.IsNullOrEmpty(value) Then
                _name = String.Empty
            Else
                _name = value
            End If
        End Set
    End Property
    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property Site As ISite Implements IComponent.Site
    '''<summary>Gets the value name assigned to the <see cref="TabGroup"/>.</summary>
    '''<returns>The value name assigned to the <see cref="TabGroup"/>.</returns>
    Public Property Value As String

#End Region

#Region "Functions"

    Public Sub Dispose() Implements IDisposable.Dispose
        Site?.Container?.Remove(Me)
        RaiseEvent Disposed(Me, EventArgs.Empty)
    End Sub
    Public Overrides Function ToString() As String
        Return Value
    End Function

#End Region

End Class
