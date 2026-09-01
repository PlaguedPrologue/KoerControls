Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Reflection
Imports System.Runtime.CompilerServices

'''<summary>A component that can allow any Control to highlight its background when the cursor enters it.</summary>
<Designer(GetType(ComponentDesigner))>
<ProvideProperty("HighlightOnMouseEnter", GetType(Control))>
<ProvideProperty("KeepHighlightedOnTabStopChildEntry", GetType(Control))>
<ToolboxItem(True)>
Public Class Highlighter
    Inherits Component
    Implements IExtenderProvider

#Region "Private Items"

    Private Shared ReadOnly _controlPainter As New ConditionalWeakTable(Of Control, ColorPainter)
    Private Shared ReadOnly _highlightOnMouseEnterManagedControls As New WeakCollection(Of Control)
    Private Shared ReadOnly _keepHighlightedOnTabStopChildEntryManagedControls As New WeakCollection(Of Control)

    Private Function CanExtend(extendee As Object) As Boolean Implements IExtenderProvider.CanExtend
        Return TypeOf extendee Is Control AndAlso extendee.GetType.GetProperty("BackColor", BindingFlags.Instance Or BindingFlags.Public)?.PropertyType Is GetType(Color)
    End Function
    Private Sub NotifyPropertyChanged(control As Control, propertyName As String, oldValue As Object, newValue As Object)
        Dim ccsChangeService As IComponentChangeService = TryCast(GetService(GetType(IComponentChangeService)), IComponentChangeService)
        If ccsChangeService Is Nothing Then Return
        Dim pdDescriptor As PropertyDescriptor = TypeDescriptor.GetProperties(control)(propertyName)
        If pdDescriptor Is Nothing Then Return
        ccsChangeService.OnComponentChanged(control, pdDescriptor, oldValue, newValue)
        TypeDescriptor.Refresh(control)
    End Sub

#End Region

#Region "Initializers"

    '''<summary>Creates a new instance of the <see cref="Highlighter"/> class.</summary>
    '''<param name="container">The <see cref="IContainer"/> that will contain the <see cref="Highlighter"/>.</param>
    Public Sub New(container As IContainer)
        container?.Add(Me)
    End Sub

#End Region

#Region "Properties"

    '''<summary>Gets the currently highlighted control if one is highlighted; otherwise, <see langword="Nothing"/>.</summary>
    '''<returns>The currently highlighted control if one is highlighted; otherwise, <see langword="Nothing"/>.</returns>
    <Browsable(False)>
    Public ReadOnly Property HIghlightedControl As Control
        Get
            For Each cControl As Control In _highlightOnMouseEnterManagedControls
                Dim cpColorPainter As ColorPainter = Nothing
                If _controlPainter.TryGetValue(cControl, cpColorPainter) AndAlso cpColorPainter.Highlighted Then Return cControl
            Next
            Return Nothing
        End Get
    End Property
    '''<summary>Determines if the <see cref="Control"/> should highlight upon the mouse entering its bounds.</summary>
    '''<returns><see langword="True"/> if the <see cref="Control"/> should highlight upon the mouse entering its bounds; otherwise <see langword="False"/>.</returns>
    '''<param name="control">The <see cref="Control"/> to get the HighlightOnMouseEnter value of.</param>
    <Category("Behavior")>
    <DefaultValue(False)>
    <Description("Determines if the Control should highlight upon the mouse entering its bounds.")>
    Public Function GetHighlightOnMouseEnter(control As Control) As Boolean
        Return _highlightOnMouseEnterManagedControls.Contains(control)
    End Function
    '''<summary>Determines if the <see cref="Control"/> should highlight upon the mouse entering its bounds.</summary>
    '''<param name="control">The <see cref="Control"/> to set the HighlightOnMouseEnter value of.</param>
    '''<param name="value"><see langword="True"/> if the <see cref="Control"/> should highlight upon the mouse entering its bounds; otherwise <see langword="False"/>.</param>
    <Category("Behavior")>
    <DefaultValue(False)>
    <Description("Determines if the Control should highlight upon the mouse entering its bounds.")>
    Public Sub SetHighlightOnMouseEnter(control As Control, value As Boolean)
        Dim cpPainter As ColorPainter = Nothing
        Dim bControlHasPainter As Boolean = _controlPainter.TryGetValue(control, cpPainter) 'Get the ColorPainter from the all Highlighters assigned to the Control
        If bControlHasPainter = value Then Return 'HighlightOnMouseEnter value does not change for the Control, exit
        If value Then 'Determine if setting the HighlightOnMouseEnter value for the Control to True
            _controlPainter.Add(control, New ColorPainter(control)) 'Add new ControlPainter to the Control
            _highlightOnMouseEnterManagedControls.Add(control) 'Add Control to the list of managed Controls
        Else 'Setting the HighlightOnMouseEnter value of the control to False
            cpPainter?.Dispose()
            _controlPainter.Remove(control) 'Remove current ColorPainter from the Control
            _highlightOnMouseEnterManagedControls.Remove(control)

            If _keepHighlightedOnTabStopChildEntryManagedControls.Contains(control) Then 'Control that has 'HighlightOnMouseEnter = False' requires it to have 'KeepHighlightedOnTabStopChildEntry = False'
                _keepHighlightedOnTabStopChildEntryManagedControls.Remove(control)
                NotifyPropertyChanged(control, "KeepHighlightedOnTabStopChildEntry", True, False)
            End If
        End If
    End Sub
    '''<summary>Determines if the <see cref="Control"/> should stay highlight upon the mouse entering a child control's bounds with TabStop = True.</summary>
    '''<returns><see langword="True"/> if the <see cref="Control"/> should stay highlight upon the mouse entering a child control's bounds with TabStop = True; otherwise <see langword="False"/>.</returns>
    '''<param name="control">The <see cref="Control"/> to get the KeepHighlightedOnTabStopChildEntry value of.</param>
    <Category("Behavior")>
    <DefaultValue(False)>
    <Description("Determines if the Control should stay highlight upon the mouse entering a child control's bounds with TabStop = True.")>
    Public Function GetKeepHighlightedOnTabStopChildEntry(control As Control) As Boolean
        Return _keepHighlightedOnTabStopChildEntryManagedControls.Contains(control)
    End Function
    '''<summary>Determines if the <see cref="Control"/> should stay highlight upon the mouse entering a child control's bounds with TabStop = True.</summary>
    '''<param name="control">The <see cref="Control"/> to set the KeepHighlightedOnTabStopChildEntry value of.</param>
    '''<param name="value"><see langword="True"/> if the <see cref="Control"/> should stay highlight upon the mouse entering a child control's bounds with TabStop = True.</param>
    <Category("Behavior")>
    <DefaultValue(False)>
    <Description("Determines if the Control should stay highlight upon the mouse entering a child control's bounds with TabStop = True.")>
    Public Sub SetKeepHighlightedOnTabStopChildEntry(control As Control, value As Boolean)
        Dim bKeepHIghlightedOnTabStopChildEntry As Boolean = _keepHighlightedOnTabStopChildEntryManagedControls.Contains(control)
        If bKeepHIghlightedOnTabStopChildEntry = value Then Return
        If value Then 'Setting KeepHighlightedOnTabStopChildEntry to True
            _keepHighlightedOnTabStopChildEntryManagedControls.Add(control)

            Dim bControlHasPainter As Boolean = _controlPainter.TryGetValue(control, Nothing) 'Control that has 'KeepHighlightedOnTabStopChildEntry = True' requires it to have 'HighlightOnMouseEnter = True'
            If Not bControlHasPainter Then
                _controlPainter.Add(control, New ColorPainter(control))
                _highlightOnMouseEnterManagedControls.Add(control)
                NotifyPropertyChanged(control, "HighlightOnMouseEnter", False, True)
            End If

        Else 'Setting KeepHighlightedOnTabStopChildEntry to False
            _keepHighlightedOnTabStopChildEntryManagedControls.Remove(control)
        End If
    End Sub

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
