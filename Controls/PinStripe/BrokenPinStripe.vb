Imports System.Collections.Specialized
Imports System.ComponentModel
Imports System.Drawing.Design

'''<summary>Represents a pin stripe that can be painted onto the background of a control. Can be broken up along the length of a Control.</summary>
<DesignTimeVisible(False)>
<ToolboxItem(False)>
<TypeConverter(GetType(PinStripeConverter))>
Public Class BrokenPinStripe
    Inherits PinStripe

#Region "Private Items"

    Private WithEvents MyPinStripeLocations As New IntegerRangeCollection

    Private _joinedPinStripeLocations As IEnumerable(Of IntegerRange) = IntegerRange.Join(MyPinStripeLocations)

    Private Sub MyPinStripeLocations_CollectionChanged(sender As Object, e As NotifyCollectionChangedEventArgs) Handles MyPinStripeLocations.CollectionChanged
        _joinedPinStripeLocations = IntegerRange.Join(MyPinStripeLocations)
        OnPinStripeLocationsChanged(e)
    End Sub

#End Region

#Region "Initializers"

    Public Sub New()
        MyBase.New
    End Sub
    '''<summary>Creates a new instance of the <see cref="BrokenPinStripe"/> class.</summary>
    '''<param name="pinStripeList">The <see cref="KoerControls.PinStripeList"/> that will contain the <see cref="BrokenPinStripe"/>.</param>
    Public Sub New(pinStripeList As PinStripeList)
        MyBase.New(pinStripeList)
    End Sub

#End Region

#Region "Events"

    '''<summary>Occurs when the pin stripe locations has changed.</summary>
    Public Event PinStripeLocationsChanged As NotifyCollectionChangedEventHandler

#End Region

#Region "Properties"

    '''<summary>Gets the bounds of each section the <see cref="BrokenPinStripe"/> should occupy.</summary>
    '''<returns>The bounds of each section the <see cref="BrokenPinStripe"/> should occupy</returns>
    <Category("Appearance")>
    <Description("The bounds of each section the broken pin stripe should occupy.")>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)>
    <Editor(GetType(IntegerRangeCollectionEditor), GetType(UITypeEditor))>
    Public ReadOnly Property PinStripeLocations As IntegerRangeCollection
        Get
            Return MyPinStripeLocations
        End Get
    End Property
    '''<summary>Represents the bounds of each section the <see cref="BrokenPinStripe"/> should occupy when each section that is overlapping is joined together.</summary>
    '''<returns>The bounds of each section the <see cref="BrokenPinStripe"/> should occupy when each section that is overlapping is joined together.</returns>
    <Browsable(False)>
    Public ReadOnly Property JoinedPinStripeLocations As IEnumerable(Of IntegerRange)
        Get
            Return _joinedPinStripeLocations
        End Get
    End Property
    Private Sub ResetPinStripeLocations()
        MyPinStripeLocations.Clear()
        _joinedPinStripeLocations = IntegerRange.Join(MyPinStripeLocations)
    End Sub
    Private Function ShouldSerializePinStripeLocations() As Boolean
        Return MyPinStripeLocations.Any
    End Function

#End Region

#Region "Functions"

    Public Overrides Function IsDefault() As Boolean
        Return MyBase.IsDefault AndAlso MyPinStripeLocations.Count = 0
    End Function
    '''<summary>Raises the <see cref="PinStripeLocationsChanged"/> event.</summary>
    '''<param name="e">The <see cref="ListChangedEventArgs"/> containing the event data.</param>
    Protected Overridable Sub OnPinStripeLocationsChanged(e As NotifyCollectionChangedEventArgs)
        RaiseEvent PinStripeLocationsChanged(Me, e)
    End Sub
    Public Overrides Function ToString() As String
        Return $"Broken {MyBase.ToString}"
    End Function

#End Region

End Class
