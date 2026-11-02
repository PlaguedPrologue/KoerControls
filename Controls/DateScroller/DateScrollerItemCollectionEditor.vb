Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Reflection

'''<summary>Provides a user interface that can edit date scroller item collections at design time.</summary>
Friend Class DateScrollerItemCollectionEditor
    Inherits CollectionEditor

    Private _createTypes As Type() = Nothing

    '''<summary> Initializes a new instance of the <see cref="DateScrollerItemCollectionEditor"/> class using the specified collection type.</summary>
    '''<param name="type">The type of the collection for this editor to edit.</param>
    Public Sub New(type As Type)
        MyBase.New(type)
    End Sub

    Protected Overrides Function CreateCollectionItemType() As Type
        Return GetType(DateScrollerItem)
    End Function
    Protected Overrides Function CreateInstance(itemType As Type) As Object
        If Not GetType(DateScrollerItem).IsAssignableFrom(itemType) Then Return Nothing

        Dim dhHost As IDesignerHost = TryCast(GetService(GetType(IDesignerHost)), IDesignerHost)
        Dim dsDateScroller As DateScroller = TryCast(Context.Instance, DateScroller)

        If dhHost IsNot Nothing Then
            Dim dsiDateScrollerItem As DateScrollerItem = dhHost.CreateComponent(itemType)
            If dsDateScroller IsNot Nothing Then
                dsDateScroller.Items.Add(dsiDateScrollerItem)
                dsDateScroller.Controls.Add(dsiDateScrollerItem)
                dsDateScroller.PlaceItems()
            End If
            Return dsiDateScrollerItem
        End If

        If dsDateScroller IsNot Nothing Then Return Activator.CreateInstance(itemType, dsDateScroller)
        Return Activator.CreateInstance(itemType)
    End Function
    Protected Overrides Function CreateNewItemTypes() As Type()
        If _createTypes Is Nothing Then
            Dim aBaseAssembly As Assembly = GetType(DateScrollerItem).Assembly
            _createTypes = AppDomain.CurrentDomain.GetAssemblies.Where(Function(a) a Is aBaseAssembly OrElse a.GetReferencedAssemblies.Any(Function(an) an.FullName = aBaseAssembly.FullName)).SelectMany(Function(a) a.GetTypes).Where(Function(t) t.IsClass AndAlso Not t.IsAbstract AndAlso GetType(DateScrollerItem).IsAssignableFrom(t)).ToArray
        End If
        Return _createTypes
    End Function
    Public Overrides Function EditValue(context As ITypeDescriptorContext, provider As IServiceProvider, value As Object) As Object
        Dim oResult As Object = MyBase.EditValue(context, provider, value)
        context?.PropertyDescriptor.SetValue(context.Instance, oResult)
        Return oResult
    End Function

End Class
