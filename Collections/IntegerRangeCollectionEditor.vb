Imports System.ComponentModel
Imports System.ComponentModel.Design

'''<summary>Provides a user interface that can edit integer range collections at design time.</summary>
Public Class IntegerRangeCollectionEditor
    Inherits CollectionEditor

#Region "Protected Items"

    Protected Overrides Function CreateCollectionItemType() As Type
        Return GetType(IntegerRange)
    End Function
    Protected Overrides Function CreateInstance(itemType As Type) As Object
        Return IntegerRange.Empty
    End Function
    Public Overrides Function EditValue(context As ITypeDescriptorContext, provider As IServiceProvider, value As Object) As Object
        Dim oResult As Object = MyBase.EditValue(context, provider, value)
        context?.PropertyDescriptor.SetValue(context.Instance, oResult)
        Return oResult
    End Function

#End Region

#Region "Initializers"

    '''<summary> Initializes a new instance of the <see cref="IntegerRangeCollectionEditor"/> class using the specified collection type.</summary>
    '''<param name="type">The type of the collection for this editor to edit.</param>
    Public Sub New(type As Type)
        MyBase.New(type)
    End Sub

#End Region

End Class
