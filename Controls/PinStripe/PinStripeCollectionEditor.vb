Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Reflection

'''<summary>Provides a user interface that can edit pin stripe collections at design time.</summary>
Friend Class PinStripeCollectionEditor
    Inherits CollectionEditor

    Private _createTypes As Type() = Nothing

    '''<summary> Initializes a new instance of the <see cref="PinStripeCollectionEditor"/> class using the specified collection type.</summary>
    '''<param name="type">The type of the collection for this editor to edit.</param>
    Public Sub New(type As Type)
        MyBase.New(type)
    End Sub

    Protected Overrides Function CreateCollectionItemType() As Type
        Return GetType(PinStripe)
    End Function
    Protected Overrides Function CreateInstance(itemType As Type) As Object
        If Not GetType(PinStripe).IsAssignableFrom(itemType) Then Return Nothing

        Dim dhHost As IDesignerHost = TryCast(GetService(GetType(IDesignerHost)), IDesignerHost)
        Dim pslPinStripeList As PinStripeList = TryCast(Context.Instance, PinStripeList)

        If dhHost IsNot Nothing Then
            Dim psPinStripe As PinStripe = dhHost.CreateComponent(itemType)
            If pslPinStripeList IsNot Nothing Then psPinStripe.PinStripeList = pslPinStripeList
            Return psPinStripe
        End If

        If pslPinStripeList IsNot Nothing Then Return Activator.CreateInstance(itemType, pslPinStripeList)
        Return Activator.CreateInstance(itemType)
    End Function
    Protected Overrides Function CreateNewItemTypes() As Type()
        If _createTypes Is Nothing Then
            Dim aBaseAssembly As Assembly = GetType(PinStripe).Assembly
            _createTypes = AppDomain.CurrentDomain.GetAssemblies.Where(Function(a) a Is aBaseAssembly OrElse a.GetReferencedAssemblies.Any(Function(an) an.FullName = aBaseAssembly.FullName)).SelectMany(Function(a) a.GetTypes).Where(Function(t) t.IsClass AndAlso Not t.IsAbstract AndAlso GetType(PinStripe).IsAssignableFrom(t)).ToArray
        End If
        Return _createTypes
    End Function
    Public Overrides Function EditValue(context As ITypeDescriptorContext, provider As IServiceProvider, value As Object) As Object
        Dim oResult As Object = MyBase.EditValue(context, provider, value)
        context?.PropertyDescriptor.SetValue(context.Instance, oResult)
        Return oResult
    End Function

End Class
