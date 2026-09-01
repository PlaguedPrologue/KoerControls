Imports System.ComponentModel
Imports System.Globalization

Friend Class StaticDataGridViewColumnCollectionConverter
    Inherits TypeConverter

    Public Overrides Function CanConvertTo(context As ITypeDescriptorContext, destinationType As Type) As Boolean
        If destinationType = GetType(String) Then Return True
        Return MyBase.CanConvertTo(context, destinationType)
    End Function
    Public Overrides Function ConvertTo(context As ITypeDescriptorContext, culture As CultureInfo, value As Object, destinationType As Type) As Object
        If destinationType = GetType(String) AndAlso TypeOf value Is StaticDataGridViewColumnCollection Then Return DirectCast(value, StaticDataGridViewColumnCollection).ToString
        Return MyBase.ConvertTo(context, culture, value, destinationType)
    End Function
    Public Overrides Function GetStandardValuesSupported(context As ITypeDescriptorContext) As Boolean
        Return False
    End Function

End Class
