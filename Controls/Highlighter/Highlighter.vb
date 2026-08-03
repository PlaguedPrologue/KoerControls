Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Reflection
Imports System.Runtime.CompilerServices

'''<summary>A component that can allow any Control to highlight its background when the cursor enters it.</summary>
<Designer(GetType(ComponentDesigner))>
<ProvideProperty("HighlightOnMouseEnter", GetType(Control))>
<ProvideProperty("PinStripe", GetType(Control))>
<ToolboxItem(True)>
Public Class Highlighter
    Inherits Component
    Implements IExtenderProvider

#Region "Private Items"

    Private Shared ReadOnly _controlPainter As New ConditionalWeakTable(Of Control, ColorPainter)
    Private Shared ReadOnly _managedControls As New WeakCollection(Of Control)

    Private Function CanExtend(extendee As Object) As Boolean Implements IExtenderProvider.CanExtend
        Return TypeOf extendee Is Control AndAlso extendee.GetType.GetProperty("BackColor", BindingFlags.Instance Or BindingFlags.Public)?.PropertyType Is GetType(Color)
    End Function

#End Region

#Region "Initializers"

    '''<summary>Creates a new instance of the <see cref="Highlighter"/> class.</summary>
    '''<param name="container">The <see cref="IContainer"/> that will contain the <see cref="Highlighter"/>.</param>
    Public Sub New(container As IContainer)
        container?.Add(Me)
    End Sub

#End Region

#Region "Properties"

    '''<summary>Determines if the <see cref="Control"/> should highlight upon the mouse entering its bounds.</summary>
    '''<returns><see langword="True"/> if the <see cref="Control"/> should highlight upon the mouse entering its bounds; otherwise <see langword="False"/>.</returns>
    '''<param name="control">The <see cref="Control"/> to get the HighlightOnMouseEnter value of.</param>
    <Category("Behavior")>
    <Description("Determines if the Control should highlight upon the mouse entering its bounds.")>
    Public Function GetHighlightOnMouseEnter(control As Control) As Boolean
        Return _managedControls.Contains(control)
    End Function
    '''<summary>Determines if the <see cref="Control"/> should highlight upon the mouse entering its bounds.</summary>
    '''<param name="control">The <see cref="Control"/> to get the HighlightOnMouseEnter value of.</param>
    '''<param name="value"><see langword="True"/> if the <see cref="Control"/> should highlight upon the mouse entering its bounds; otherwise <see langword="False"/>.</param>
    <Category("Behavior")>
    <Description("Determines if the Control should highlight upon the mouse entering its bounds.")>
    Public Sub SetHighlightOnMouseEnter(control As Control, value As Boolean)
        Dim cpPainter As ColorPainter = Nothing
        Dim bControlHasPainter As Boolean = _controlPainter.TryGetValue(control, cpPainter) 'Get the ColorPainter from the all Highlighters assigned to the Control
        If bControlHasPainter = value Then Return 'HighlightOnMouseEnter value does not change for the Control, exit
        If value Then 'Determine if setting the HighlightOnMouseEnter value for the Control to True
            _controlPainter.Add(control, New ColorPainter(control)) 'Add new ControlPainter to the Control
            _managedControls.Add(control) 'Add Control to the list of managed Controls
        Else 'Setting the HighlightOnMouseEnter value of the control to False
            _controlPainter.Remove(control) 'Remove current ColorPainter from the Control
            _managedControls.Remove(control)
        End If
    End Sub
    Friend Sub ResetHighlightOnMouseEnter(control As Control)
        Dim cpPainter As ColorPainter = Nothing
        If _controlPainter.TryGetValue(control, cpPainter) Then
            cpPainter.Dispose()
        End If
        _controlPainter.Remove(control)
        _managedControls.Remove(control)
    End Sub
    Friend Function ShouldSerializeHighlightOnMouseEnter(control As Control) As Boolean
        Return _managedControls.Contains(control)
    End Function

#End Region

#Region "Functions"

    '''<summary>Determines if the <see cref="Control"/> should highlight upon the mouse entering its bounds.</summary>
    '''<returns><see langword="True"/> if the <see cref="Control"/> should highlight upon the mouse entering its bounds; otherwise <see langword="False"/>.</returns>
    '''<param name="control">The <see cref="Control"/> to get the HighlightOnMouseEnter value of.</param>
    Public Shared Function IsHighlighted(control As Control) As Boolean
        Dim cpPainter As ColorPainter = Nothing
        If Not _controlPainter.TryGetValue(control, cpPainter) Then Return False
        Return cpPainter.Highlighted
    End Function

#End Region

End Class
