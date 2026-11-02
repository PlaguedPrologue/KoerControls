Imports System.ComponentModel
Imports System.Globalization

'''<summary>Provides a type converter to convert a <see cref="PinStripe"/> to and from various other representations.</summary>
Public Class PinStripeConverter
    Inherits TypeConverter

    Public Overrides Function CanConvertFrom(context As ITypeDescriptorContext, sourceType As Type) As Boolean
        If sourceType Is GetType(String) Then Return True
        Return MyBase.CanConvertFrom(context, sourceType)
    End Function
    Public Overrides Function CanConvertTo(context As ITypeDescriptorContext, destinationType As Type) As Boolean
        If destinationType Is GetType(String) Then Return True
        Return MyBase.CanConvertTo(context, destinationType)
    End Function
    Public Overrides Function ConvertFrom(context As ITypeDescriptorContext, culture As CultureInfo, value As Object) As Object
        If TypeOf value Is String Then
            Dim sValue As String = CStr(value)
            If sValue = "(none)" Then Return Nothing
            Dim pslPinStripeList As PinStripeList = TryCast(TryCast(context?.PropertyDescriptor.Attributes(GetType(ExtenderProvidedPropertyAttribute)), ExtenderProvidedPropertyAttribute)?.Provider, PinStripeList)
            If pslPinStripeList Is Nothing Then pslPinStripeList = TryCast(context?.Instance, PinStripeList)
            If pslPinStripeList IsNot Nothing Then
                Dim psPinStripe As PinStripe = pslPinStripeList.PinStripes.FirstOrDefault(Function(ps) ps.ToString = sValue)
                If psPinStripe IsNot Nothing Then Return psPinStripe
                Return New PinStripe(pslPinStripeList)
            End If
        End If
        Return MyBase.ConvertFrom(context, culture, value)
    End Function
    Public Overrides Function ConvertTo(context As ITypeDescriptorContext, culture As CultureInfo, value As Object, destinationType As Type) As Object
        If destinationType Is GetType(String) Then
            If value Is Nothing Then Return "(none)"
            Return value.ToString
        End If
        Return MyBase.ConvertTo(context, culture, value, destinationType)
    End Function
    Public Overrides Function GetStandardValues(context As ITypeDescriptorContext) As StandardValuesCollection
        If context IsNot Nothing Then
            Dim eppaProperty As ExtenderProvidedPropertyAttribute = TryCast(context.PropertyDescriptor.Attributes(GetType(ExtenderProvidedPropertyAttribute)), ExtenderProvidedPropertyAttribute)
            If eppaProperty IsNot Nothing Then
                Dim pslPinStripeList As PinStripeList = TryCast(eppaProperty.Provider, PinStripeList)
                If pslPinStripeList IsNot Nothing Then
                    Dim psPinStripes As New List(Of Object) From {Nothing}
                    psPinStripes.AddRange(pslPinStripeList.PinStripes)
                    Return New StandardValuesCollection(psPinStripes)
                End If
            End If
        End If
        Return New StandardValuesCollection({Nothing})
    End Function
    Public Overrides Function GetStandardValuesExclusive(context As ITypeDescriptorContext) As Boolean
        Return True
    End Function
    Public Overrides Function GetStandardValuesSupported(context As ITypeDescriptorContext) As Boolean
        Return True
    End Function

End Class
