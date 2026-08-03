Imports System.Runtime.CompilerServices

Module ExtendedMath

    '''<summary>Rounds an integer rounded to the nearest multiple of a specified integer.</summary>
    '''<returns>An integer rounded to the nearest multiple of a specified integer.</returns>
    '''<param name="value">The integer to round.</param>
    '''<param name="roundTo">The rounding value.</param>
    <Extension>
    Public Function RoundToFloor(value As Integer, roundTo As Integer) As Integer
        Return (value \ roundTo) * roundTo
    End Function

End Module
