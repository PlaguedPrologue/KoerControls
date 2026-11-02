Imports System.Runtime.CompilerServices

Public Module ExtendedMath

    '''<summary>Iterates a value through a function a specified number of times.</summary>
    '''<returns>The value returned after running it through a function a specified number of times.</returns>
    '''<param name="value">The value to iterate.</param>
    '''<param name="func">The function to pass the value through.</param>
    '''<param name="n">The total number of times to iterate.</param>
    <Extension>
    Public Function Iterate(value As Double, func As Func(Of Double, Double), n As Integer)
        For i = 1 To n
            value = func(value)
        Next
        Return value
    End Function
    '''<summary>Rounds an integer rounded to the nearest multiple of a specified integer.</summary>
    '''<returns>An integer rounded to the nearest multiple of a specified integer.</returns>
    '''<param name="value">The integer to round.</param>
    '''<param name="roundTo">The rounding value.</param>
    <Extension>
    Public Function RoundToFloor(value As Integer, roundTo As Integer) As Integer
        Return (value \ roundTo) * roundTo
    End Function

End Module
