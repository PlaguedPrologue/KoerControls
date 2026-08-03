Imports System.ComponentModel
Imports System.Globalization

'''<summary>Provides a type converter to convert a <see cref="DateRange"/> to and from various other representations.</summary>
Public Class DateRangeConverter
    Inherits ExpandableObjectConverter

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
            Dim drValue As DateRange = Nothing
            If DateRange.TryParse(value, drValue) Then Return drValue
        End If
        Return MyBase.ConvertFrom(context, culture, value)
    End Function
    Public Overrides Function ConvertTo(context As ITypeDescriptorContext, culture As CultureInfo, value As Object, destinationType As Type) As Object
        If TypeOf value Is DateRange AndAlso destinationType Is GetType(String) Then
            Dim drValue As DateRange = value
            If culture Is Nothing Then culture = CultureInfo.CurrentCulture
            Return $"{drValue.StartDate.ToString("d", culture)}, {drValue.EndDate.ToString("d", culture)}"
        End If
        Return MyBase.ConvertTo(context, culture, value, destinationType)
    End Function

End Class
