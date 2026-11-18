Imports System.ComponentModel

Friend Class ConditionalReadonlyPropertyDescriptor
    Inherits PropertyDescriptor

    Private ReadOnly _source As PropertyDescriptor
    Private ReadOnly _isReadOnly As Func(Of Boolean)

    Public Sub New(descr As PropertyDescriptor, isReadOnly As Func(Of Boolean))
        MyBase.New(descr)
        _source = descr
        _isReadOnly = isReadOnly
    End Sub

    Public Overrides ReadOnly Property ComponentType As Type
        Get
            Return _source.ComponentType
        End Get
    End Property
    Public Overrides ReadOnly Property IsReadOnly As Boolean
        Get
            Return _isReadOnly()
        End Get
    End Property
    Public Overrides ReadOnly Property PropertyType As Type
        Get
            Return _source.PropertyType
        End Get
    End Property

    Public Overrides Function CanResetValue(component As Object) As Boolean
        Return Not IsReadOnly AndAlso _source.CanResetValue(component)
    End Function
    Public Overrides Function GetValue(component As Object) As Object
        Return _source.GetValue(component)
    End Function
    Public Overrides Sub ResetValue(component As Object)
        If Not IsReadOnly Then _source.ResetValue(component)
    End Sub
    Public Overrides Sub SetValue(component As Object, value As Object)
        If Not IsReadOnly Then _source.SetValue(component, value)
    End Sub
    Public Overrides Function ShouldSerializeValue(component As Object) As Boolean
        Return Not IsReadOnly AndAlso _source.ShouldSerializeValue(component)
    End Function

End Class
