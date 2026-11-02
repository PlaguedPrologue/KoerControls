Imports System.CodeDom
Imports System.ComponentModel.Design.Serialization

Public Class IntegerRangeCodeDomSerializer
    Inherits CodeDomSerializer

    Public Overrides Function Deserialize(manager As IDesignerSerializationManager, codeObject As Object) As Object
        Return MyBase.Deserialize(manager, codeObject)
    End Function
    Public Overrides Function Serialize(manager As IDesignerSerializationManager, value As Object) As Object
        If TypeOf value IsNot IntegerRange Then Return MyBase.Serialize(manager, value)
        Dim irIntegerRange As IntegerRange = value
        Return New CodeObjectCreateExpression(GetType(IntegerRange), New CodePrimitiveExpression(irIntegerRange.StartValue), New CodePrimitiveExpression(irIntegerRange.EndValue))
    End Function

End Class
