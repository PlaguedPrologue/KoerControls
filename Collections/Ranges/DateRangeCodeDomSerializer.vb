Imports System.CodeDom
Imports System.ComponentModel.Design.Serialization

Public Class DateRangeCodeDomSerializer
    Inherits CodeDomSerializer

    Public Overrides Function Deserialize(manager As IDesignerSerializationManager, codeObject As Object) As Object
        Return MyBase.Deserialize(manager, codeObject)
    End Function
    Public Overrides Function Serialize(manager As IDesignerSerializationManager, value As Object) As Object
        If TypeOf value IsNot DateRange Then Return MyBase.Serialize(manager, value)
        Dim drDateRange As DateRange = value
        Dim dStartDate As Date = drDateRange.StartDate
        Dim dEndDate As Date = drDateRange.EndDate
        Return New CodeObjectCreateExpression(GetType(DateRange), New CodePrimitiveExpression(dStartDate.Year), New CodePrimitiveExpression(dStartDate.Month), New CodePrimitiveExpression(dStartDate.Day), New CodePrimitiveExpression(dStartDate.Hour), New CodePrimitiveExpression(dStartDate.Minute), New CodePrimitiveExpression(dStartDate.Second), New CodePrimitiveExpression(dStartDate.Millisecond), New CodePrimitiveExpression(dEndDate.Year), New CodePrimitiveExpression(dEndDate.Month), New CodePrimitiveExpression(dEndDate.Day), New CodePrimitiveExpression(dEndDate.Hour), New CodePrimitiveExpression(dEndDate.Minute), New CodePrimitiveExpression(dEndDate.Second), New CodePrimitiveExpression(dEndDate.Millisecond))
    End Function

End Class
