Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Drawing.Design
Imports System.Reflection
Imports System.Runtime.CompilerServices

'''<summary>A component that can allow any Control to have a PinStripe painted in its background.</summary>
<Designer(GetType(ComponentDesigner))>
<ProvideProperty("PinStripe", GetType(Control))>
<ToolboxItem(True)>
Public Class PinStripeList
    Inherits Component
    Implements IExtenderProvider

#Region "Private Items"

    Private Shared ReadOnly _controlPainters As New ConditionalWeakTable(Of Control, PinStripe.PainterCollection)
    Private ReadOnly _localControlPainter As New ConditionalWeakTable(Of Control, PinStripe.Painter)
    Private ReadOnly _managedControls As New WeakCollection(Of Control)
    Private ReadOnly _pinStripes As New PinStripeCollection

    Private Function CanExtend(extendee As Object) As Boolean Implements IExtenderProvider.CanExtend
        Return TypeOf extendee Is Control AndAlso extendee.GetType.GetProperty("BackColor", BindingFlags.Instance Or BindingFlags.Public)?.PropertyType Is GetType(Color)
    End Function

#End Region

#Region "Initializers"

    '''<summary>Creates a new instance of the <see cref="PinStripeList"/> class.</summary>
    '''<param name="container">The <see cref="IContainer"/> that will contain the <see cref="PinStripeList"/>.</param>
    Public Sub New(container As IContainer)
        container?.Add(Me)
    End Sub

#End Region

#Region "Properties"

    '''<summary>Gets each <see cref="PinStripe"/> contained in the <see cref="PinStripeList"/>.</summary>
    '''<returns>Each <see cref="PinStripe"/> contained in the <see cref="PinStripeList"/>.</returns>
    <Category("Data")>
    <Description("The pin stripes available for the owning form.")>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)>
    <Editor(GetType(PinStripeCollectionEditor), GetType(UITypeEditor))>
    Public ReadOnly Property PinStripes As PinStripeCollection
        Get
            Return _pinStripes
        End Get
    End Property
    Private Sub ResetPinStripes()
        For Each cControl As Control In _managedControls 'Go through each Control the PinStripeLIst has PinStripes assigned to
            Dim pPainter As PinStripe.Painter = Nothing
            Dim pControlPainters As PinStripe.PainterCollection = Nothing
            If _localControlPainter.TryGetValue(cControl, pPainter) AndAlso _controlPainters.TryGetValue(cControl, pControlPainters) Then  'Determine if the current PinStripeList contains a Painter assigned to the Control
                If pControlPainters.Count = 1 Then  'Determine if PainterCollection will be empty upon Painter removal
                    _controlPainters.Remove(cControl) 'Remove PainterCollection from the Control
                    pControlPainters.Dispose() 'Dispose the PainterCollection
                Else 'PainterCollection will not be empty upon Painter removal
                    pControlPainters.Remove(pPainter) 'Remove Painter from PainterCollection
                End If
            End If
            _localControlPainter.Remove(cControl) 'Remove the Painter from the Control
        Next
        _managedControls.Clear() 'Clear all managed Controls
        _pinStripes.Clear() 'Clear out each PinStripe in the PinStripeList
    End Sub
    Private Function ShouldSerializePinStripes() As Boolean
        Return _pinStripes.Count > 0
    End Function
    '''<summary>Gets the <see cref="PinStripe"/> assigned to the specified <see cref="Control"/>.</summary>
    '''<returns>The <see cref="PinStripe"/> assigned to the specified <see cref="Control"/>.</returns>
    '''<param name="control">The <see cref="Control"/> to get the <see cref="PinStripe"/> of.</param>
    <Category("Appearance")>
    <Description("The PinStripe assigned to the Control for painting the background.")>
    <TypeConverter(GetType(PinStripeConverter))>
    Public Function GetPinStripe(control As Control) As PinStripe
        Dim pPainter As PinStripe.Painter = Nothing
        If _localControlPainter.TryGetValue(control, pPainter) Then Return pPainter.PinStripe
        Return Nothing
    End Function
    '''<summary>Assigns the specified <see cref="PinStripe"/> to the specified <see cref="Control"/>.</summary>
    '''<param name="control">The <see cref="Control"/> to set the <see cref="PinStripe"/> of.</param>
    '''<param name="value">The <see cref="PinStripe"/> to assign to the specified <see cref="Control"/>.</param>
    <Category("Appearance")>
    <Description("The PinStripe assigned to the Control for painting the background.")>
    <TypeConverter(GetType(PinStripeConverter))>
    Public Sub SetPinStripe(control As Control, value As PinStripe)
        Dim pPainter As PinStripe.Painter = Nothing
        Dim bControlHasLocalPainter As Boolean = _localControlPainter.TryGetValue(control, pPainter) 'Get the Painter from the current PinStripeList assigned to the Control
        If bControlHasLocalPainter AndAlso ReferenceEquals(pPainter.PinStripe, value) Then Return 'Control has a Painter from the current PinStripeList assigned and PinStripe does not change for the Control, exit
        Dim bValueIsNull As Boolean = value Is Nothing 'Determine if new value is null
        Dim pControlPainters As PinStripe.PainterCollection = Nothing
        Dim bControlHasPainters As Boolean = _controlPainters.TryGetValue(control, pControlPainters) 'Get the PainterCollection currently assigned to the Control

        'Remove current PinStripe from the Control
        If bControlHasLocalPainter AndAlso bControlHasPainters Then 'Determine if the current PinStripeList contains a Painter assigned to the Control
            If pControlPainters.Count = 1 AndAlso bValueIsNull Then 'Determine if PainterCollection will be empty upon Painter removal and if no new Painter will be assigned
                _controlPainters.Remove(control) 'Remove PainterCollection from the Control
                pControlPainters.Dispose() 'Dispose the PainterCollection
            Else 'PainterCollection will not be empty OR a new Painter will be assigned
                pControlPainters.Remove(pPainter) 'Remove Painter from PainterCollection
            End If
            _localControlPainter.Remove(control) 'Remove Painter from the Control
        End If

        'Add the specified PinStripe to the Control
        If bValueIsNull Then 'Determine if new PinStripe is null
            _managedControls.Remove(control) 'Remove Control from list of managed Controls
        Else 'New PinStripe is not null
            _managedControls.Add(control) 'Add Control to list of managed Controls
            pPainter = New PinStripe.Painter(control, value) 'Create new Painter for Control and PinStripe
            If Not bControlHasPainters Then 'Control does not already have a PainterCollection assigned to it
                pControlPainters = New PinStripe.PainterCollection(control) 'Create new PainterCollection for the Control
                _controlPainters.Add(control, pControlPainters) 'Add PainterCollection to the Control
            End If
            pControlPainters.Add(pPainter) 'Add the Painter to the PainterCollection assigned to the Control
            _localControlPainter.Add(control, pPainter) 'Add Painter to the Control
        End If

        control.Invalidate()
    End Sub
    Friend Sub ResetPinStripe(control As Control)
        Dim pPainter As PinStripe.Painter = Nothing
        Dim pControlPainters As PinStripe.PainterCollection = Nothing
        If _localControlPainter.TryGetValue(control, pPainter) AndAlso _controlPainters.TryGetValue(control, pControlPainters) Then 'Determine if the current PinStripeList contains a Painter assigned to the Control
            If pControlPainters.Count = 1 Then 'Determine if PainterCollection will be empty upon Painter removal
                _controlPainters.Remove(control) 'Remove PainterCollection from the Control
                pControlPainters.Dispose() 'Dispose the PainterCollection
            Else 'PainterCollection will not be empty upon Painter removal
                pControlPainters.Remove(pPainter) 'Remove Painter from PainterCollection
            End If
        End If
        _localControlPainter.Remove(control) 'Remove the Painter from the Control
        _managedControls.Remove(control) 'Remove the Control from the managed Controls
    End Sub
    Friend Function ShouldSerializePinStripe(control As Control) As Boolean
        Return _managedControls.Contains(control)
    End Function

#End Region

#Region "Functions"

    Protected Overrides Sub Dispose(disposing As Boolean)
        For Each cControl As Control In _managedControls 'Go through each Control the PinStripeLIst has PinStripes assigned to
            Dim pPainter As PinStripe.Painter = Nothing
            Dim pControlPainters As PinStripe.PainterCollection = Nothing
            If _localControlPainter.TryGetValue(cControl, pPainter) AndAlso _controlPainters.TryGetValue(cControl, pControlPainters) Then  'Determine if the current PinStripeList contains a Painter assigned to the Control
                If pControlPainters.Count = 1 Then  'Determine if PainterCollection will be empty upon Painter removal
                    _controlPainters.Remove(cControl) 'Remove PainterCollection from the Control
                    pControlPainters.Dispose() 'Dispose the PainterCollection
                Else 'PainterCollection will not be empty upon Painter removal
                    pControlPainters.Remove(pPainter) 'Remove Painter from PainterCollection
                End If
            End If
        Next
        MyBase.Dispose(disposing)
    End Sub
    '''<summary>Iterates through each <see cref="PinStripe"/> assigned to the specified <see cref="Control"/>.</summary>
    '''<returns>Each <see cref="PinStripe"/> assigned to the specified <see cref="Control"/>.</returns>
    '''<param name="control">The <see cref="Control"/> that can a <see cref="PinStripe"/>.</param>
    Public Shared Iterator Function GetPinStripes(control As Control) As IEnumerable(Of PinStripe)
        Dim pPainters As PinStripe.PainterCollection = Nothing
        If Not _controlPainters.TryGetValue(control, pPainters) Then Return
        For Each pPainter As PinStripe.Painter In pPainters
            Yield pPainter.PinStripe
        Next
    End Function

#End Region

End Class
