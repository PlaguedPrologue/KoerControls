Imports System.ComponentModel

Friend Class StaticColumnsDataGridViewContextMenuStripPropertyDescriptor
    Inherits PropertyDescriptor

    Private ReadOnly _baseDescriptor As PropertyDescriptor

    Public Sub New(baseDescriptor As PropertyDescriptor)
        MyBase.New(baseDescriptor.Name, baseDescriptor.Attributes.Cast(Of Attribute)().ToArray)
        _baseDescriptor = baseDescriptor
    End Sub

    Public Overrides ReadOnly Property ComponentType As Type
        Get
            Return _baseDescriptor.ComponentType
        End Get
    End Property
    Public Overrides ReadOnly Property Converter As TypeConverter
        Get
            Return New StaticColumnsDataGridViewContextMenuStripConverter(_baseDescriptor.Converter)
        End Get
    End Property
    Public Overrides ReadOnly Property IsReadOnly As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides ReadOnly Property PropertyType As Type
        Get
            Return GetType(ContextMenuStrip)
        End Get
    End Property

    Public Overrides Function CanResetValue(component As Object) As Boolean
        Dim scdgvComponent As StaticColumnsDataGridView = TryCast(component, StaticColumnsDataGridView)
        If scdgvComponent Is Nothing Then Return False
        Return Not ReferenceEquals(scdgvComponent.DefaultContextMenuStrip, scdgvComponent.ContextMenuStrip)
    End Function
    Public Overrides Function GetEditor(editorBaseType As Type) As Object
        Return _baseDescriptor.GetEditor(editorBaseType)
    End Function
    Public Overrides Function GetValue(component As Object) As Object
        Return _baseDescriptor.GetValue(component)
    End Function
    Public Overrides Sub ResetValue(component As Object)
        Dim scdgvComponent As StaticColumnsDataGridView = TryCast(component, StaticColumnsDataGridView)
        If scdgvComponent Is Nothing Then Return
        SetValue(component, scdgvComponent.DefaultContextMenuStrip)
    End Sub
    Public Overrides Sub SetValue(component As Object, value As Object)
        _baseDescriptor.SetValue(component, value)
        OnValueChanged(component, EventArgs.Empty)
    End Sub
    Public Overrides Function ShouldSerializeValue(component As Object) As Boolean
        Dim scdgvComponent As StaticColumnsDataGridView = TryCast(component, StaticColumnsDataGridView)
        If scdgvComponent Is Nothing Then Return False
        Return Not ReferenceEquals(scdgvComponent.DefaultContextMenuStrip, scdgvComponent.ContextMenuStrip)
    End Function

End Class
