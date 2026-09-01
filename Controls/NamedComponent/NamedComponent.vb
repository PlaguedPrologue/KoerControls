Imports System.ComponentModel
Imports System.Text

'''<summary>Represents a simple, named component that is not visible in the toolbox, often an element in a string-like list.</summary>
<DesignTimeVisible(False)>
<ToolboxItem(False)>
Public MustInherit Class NamedComponent
    Implements IComponent, IDisposable

#Region "Private Items"

    Private _name As String
    Private _value As String

    Private Function GenerateUniqueName(value As String) As String
        Dim sBaseName As String = MakeValidIdentifier(value)
        If String.IsNullOrEmpty(sBaseName) Then sBaseName = [GetType].Name

        Dim sComponentNames As New HashSet(Of String)
        Dim cContainer As IContainer = Site?.Container
        If cContainer Is Nothing Then Return sBaseName
        For Each cComponent As IComponent In cContainer.Components
            If ReferenceEquals(cComponent, Me) Then Continue For
            Dim sComponentName As String = cComponent.Site.Name
            If sComponentName.StartsWith(sBaseName, StringComparison.OrdinalIgnoreCase) Then sComponentNames.Add(sComponentName)
        Next

        If sComponentNames.Count = 0 Then Return sBaseName
        Dim sName As String = sBaseName
        Dim iNameIndex As Integer = 0

        While sComponentNames.Any(Function(s) String.Equals(s, sName, StringComparison.OrdinalIgnoreCase))
            iNameIndex += 1
            sName = $"{sBaseName}{iNameIndex}"
        End While

        Return sName
    End Function
    Private Function MakeValidIdentifier(value As String)
        If String.IsNullOrWhiteSpace(value) Then Return [GetType].Name
        Dim sbValue As New StringBuilder
        For Each cLetter As Char In value.Trim
            If Char.IsLetterOrDigit(cLetter) Then
                sbValue.Append(cLetter)
            Else
                sbValue.Append("_")
            End If
        Next
        Dim sIdentifier As String = sbValue.ToString.Trim("_")
        If sbValue.Length = 0 Then Return [GetType].Name
        If Char.IsDigit(sIdentifier(0)) Then Return $"_{sIdentifier}"
        Return sIdentifier
    End Function

#End Region

#Region "Initializers"

    '''<summary>Create a new empty instance of the <see cref="NamedComponent"/> clas.</summary>
    Public Sub New()
    End Sub
    '''<summary>Creates a new instance of the <see cref="NamedComponent"/> class.</summary>
    '''<param name="value">The name value of the <see cref="NamedComponent"/>.</param>
    Public Sub New(value As String)
        Me.Value = value
    End Sub
    '''<summary>Creates a new instance of the <see cref="NamedComponent"/> class.</summary>
    '''<param name="value">The name value of the <see cref="NamedComponent"/>.</param>
    '''<param name="container">The owning <see cref="IContainer"/> of the <see cref="NamedComponent"/>.</param>
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
            value = If(value, String.Empty)
            If value = _name Then Return
            _name = value
        End Set
    End Property
    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property Site As ISite Implements IComponent.Site
    '''<summary>Gets the value name assigned to the <see cref="NamedComponent"/>.</summary>
    '''<returns>The value name assigned to the <see cref="NamedComponent"/>.</returns>
    Public Property Value As String
        Get
            Return _value
        End Get
        Set(value As String)
            value = If(value, String.Empty)
            If value = _value Then Return
            _value = value
            _name = GenerateUniqueName(value)
            If Site IsNot Nothing Then Site.Name = _name
        End Set
    End Property

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
