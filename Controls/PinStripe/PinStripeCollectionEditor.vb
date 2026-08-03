Imports System.ComponentModel
Imports System.ComponentModel.Design

'''<summary>Provides a user interface that can edit pin stripe collections at design time.</summary>
Friend Class PinStripeCollectionEditor
    Inherits CollectionEditor

    '''<summary> Initializes a new instance of the <see cref="PinStripeCollectionEditor"/> class using the specified collection type.</summary>
    '''<param name="type">The type of the collection for this editor to edit.</param>
    Public Sub New(type As Type)
        MyBase.New(type)
    End Sub

    Protected Overrides Function CreateCollectionItemType() As Type
        Return GetType(PinStripe)
    End Function
    Protected Overrides Function CreateInstance(itemType As Type) As Object
        Dim psmManager As PinStripeList = TryCast(Context.Instance, PinStripeList)
        If psmManager IsNot Nothing Then
            If itemType Is GetType(BrokenPinStripe) Then Return New BrokenPinStripe(psmManager)
            If itemType Is GetType(PinStripe) Then Return New PinStripe(psmManager)
        End If
        If itemType Is GetType(BrokenPinStripe) Then Return BrokenPinStripe.Empty
        If itemType Is GetType(PinStripe) Then Return PinStripe.Empty
        Return Nothing
    End Function
    Protected Overrides Function CreateNewItemTypes() As Type()
        Return {GetType(PinStripe), GetType(BrokenPinStripe)}
    End Function
    Public Overrides Function EditValue(context As ITypeDescriptorContext, provider As IServiceProvider, value As Object) As Object
        Dim oResult As Object = MyBase.EditValue(context, provider, value)
        context?.PropertyDescriptor.SetValue(context.Instance, oResult)
        Return oResult
    End Function

End Class
