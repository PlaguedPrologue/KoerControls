Imports System.ComponentModel
Imports System.Globalization

Public Class StaticColumnsDataGridViewContextMenuStripConverter
    Inherits TypeConverter

    Private ReadOnly _baseConverter As TypeConverter

    Public Sub New(baseConverter As TypeConverter)
        _baseConverter = baseConverter
    End Sub

    Public Overrides Function CanConvertFrom(context As ITypeDescriptorContext, sourceType As Type) As Boolean
        If sourceType Is GetType(String) Then Return True
        Return _baseConverter.CanConvertFrom(context, sourceType)
    End Function
    Public Overrides Function CanConvertTo(context As ITypeDescriptorContext, destinationType As Type) As Boolean
        Return _baseConverter.CanConvertTo(context, destinationType)
    End Function
    Public Overrides Function ConvertFrom(context As ITypeDescriptorContext, culture As CultureInfo, value As Object) As Object
        If TypeOf value Is String AndAlso DirectCast(value, String) = "(default)" Then
            Dim dgvStaticColumnsDataGridView As StaticColumnsDataGridView = TryCast(context?.Instance, StaticColumnsDataGridView)
            If dgvStaticColumnsDataGridView IsNot Nothing Then Return dgvStaticColumnsDataGridView.DefaultContextMenuStrip
        End If
        Return _baseConverter.ConvertFrom(context, culture, value)
    End Function
    Public Overrides Function ConvertTo(context As ITypeDescriptorContext, culture As CultureInfo, value As Object, destinationType As Type) As Object
        Dim dgvStaticColumnsDataGridView As StaticColumnsDataGridView = TryCast(context?.Instance, StaticColumnsDataGridView) 'Get the owning StaticColumnsDataGridView
        If dgvStaticColumnsDataGridView Is Nothing Then Return _baseConverter.ConvertTo(context, culture, value, destinationType) 'Return the default ConvertTo value if StaticColumnsDataGridView is nothing
        If value IsNot Nothing AndAlso ReferenceEquals(dgvStaticColumnsDataGridView.DefaultContextMenuStrip, value) Then Return "(default)" 'Return '(default)' if the ContextMenuStrip is the default ContextMenuStrip
        Return _baseConverter.ConvertTo(context, culture, value, destinationType) 'Return the default ConvertTo value
    End Function
    Public Overrides Function GetStandardValues(context As ITypeDescriptorContext) As StandardValuesCollection
        Dim svCurrentStandardValues As StandardValuesCollection = _baseConverter.GetStandardValues(context) 'Get the current standard values
        Dim dgvStaticColumnsDataGridView As StaticColumnsDataGridView = TryCast(context?.Instance, StaticColumnsDataGridView) 'Get the owning StaticColumnsDataGridView
        If dgvStaticColumnsDataGridView Is Nothing Then Return svCurrentStandardValues 'Return the current standard values if the owning StaticColumnsDataGridView does not exits
        Dim svStandardValues As New List(Of Object) 'Setup new list of standard values
        If svCurrentStandardValues IsNot Nothing Then svStandardValues.AddRange(svCurrentStandardValues.Cast(Of Object)()) 'Add current list of standard values to new list of standard values
        Dim cmsDefaultContextMenuStrip As ContextMenuStrip = dgvStaticColumnsDataGridView.DefaultContextMenuStrip 'Get the default ContextMenuStrip of the StaticColumnsDataGridView
        If cmsDefaultContextMenuStrip IsNot Nothing AndAlso Not svStandardValues.Contains(cmsDefaultContextMenuStrip) Then svStandardValues.Insert(0, cmsDefaultContextMenuStrip) 'Insert default ContextMenuStrip to the start of the list if one is set
        Return New StandardValuesCollection(svStandardValues) 'Return the new standard values
    End Function
    Public Overrides Function GetStandardValuesExclusive(context As ITypeDescriptorContext) As Boolean
        Return _baseConverter.GetStandardValuesExclusive(context)
    End Function
    Public Overrides Function GetStandardValuesSupported(context As ITypeDescriptorContext) As Boolean
        Return True
    End Function

End Class
