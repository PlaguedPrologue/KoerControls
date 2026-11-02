Imports System.ComponentModel
Imports System.Globalization

'''<summary>Provides a type converter to convert a DataGridView LinkingGroup to and from various other representations.</summary>
Public Class DataGridViewLinkingGroupConverter
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
            Dim dgvlLinker As DataGridViewLinker = TryCast(context?.Instance, DataGridViewLinker)
            If dgvlLinker Is Nothing Then dgvlLinker = TryCast(TryCast(context?.PropertyDescriptor.Attributes(GetType(ExtenderProvidedPropertyAttribute)), ExtenderProvidedPropertyAttribute)?.Provider, DataGridViewLinker)
            If dgvlLinker IsNot Nothing Then
                Dim dgvlgLinkingGroup As DataGridViewLinkingGroup = dgvlLinker.LinkingGroups.FirstOrDefault(Function(dgvlg) dgvlg.Value = sValue)
                If dgvlgLinkingGroup IsNot Nothing Then Return dgvlgLinkingGroup
                Return New DataGridViewLinkingGroup(sValue, dgvlLinker.Container)
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
            Dim dgvlLinker As DataGridViewLinker = TryCast(context?.Instance, DataGridViewLinker)
            If dgvlLinker Is Nothing Then dgvlLinker = TryCast(TryCast(context?.PropertyDescriptor.Attributes(GetType(ExtenderProvidedPropertyAttribute)), ExtenderProvidedPropertyAttribute)?.Provider, DataGridViewLinker)
            If dgvlLinker IsNot Nothing Then
                Dim dgvlgLinkingGroups As New List(Of Object) From {Nothing}
                dgvlgLinkingGroups.AddRange(dgvlLinker.LinkingGroups)
                Return New StandardValuesCollection(dgvlgLinkingGroups)
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
