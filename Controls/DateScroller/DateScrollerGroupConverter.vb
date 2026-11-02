Imports System.ComponentModel
Imports System.Globalization

'''<summary>Provides a type converter to convert a DateScrollerGroup to and from various other representations.</summary>
Public Class DateScrollerGroupConverter
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
            Dim sValue As String = value
            If sValue = "(none)" OrElse String.IsNullOrWhiteSpace(sValue) Then Return Nothing
            Dim dsDateScroller As DateScroller = TryCast(context?.Instance, DateScrollerItem)?.DateScroller
            If dsDateScroller IsNot Nothing Then
                Dim dsgDateScrollerGroup As DateScrollerGroup = dsDateScroller.ItemGroups.FirstOrDefault(Function(tg) tg.Value = sValue)
                If dsgDateScrollerGroup IsNot Nothing Then Return dsgDateScrollerGroup
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
        Dim dsDateScroller As DateScroller = TryCast(context?.Instance, DateScrollerItem)?.DateScroller
        If dsDateScroller IsNot Nothing Then
            Dim dgsDateScrollerGroups As New List(Of Object) From {Nothing}
            dgsDateScrollerGroups.AddRange(dsDateScroller.ItemGroups)
            Return New StandardValuesCollection(dgsDateScrollerGroups)
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
