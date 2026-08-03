Imports System.Runtime.CompilerServices

'''<summary>Used to support comparisons of object reference equality.</summary>
Public NotInheritable Class ReferenceEqualityComparer
    Implements IEqualityComparer(Of Object)

    '''<summary>Creates a new instance of <see cref="ReferenceEqualityComparer"/>.</summary>
    Public Shared ReadOnly Property Instance As ReferenceEqualityComparer = New ReferenceEqualityComparer

    '''<summary>Determines whether the specified objects are equal.</summary>
    '''<returns><see langword="True"/> if the object are equal; otherwise, <see langword="False"/>.</returns>
    '''<param name="x">The first object to compare.</param>
    '''<param name="y">The second object to compare.</param>
    Public Overloads Function Equals(x As Object, y As Object) As Boolean Implements IEqualityComparer(Of Object).Equals
        Return ReferenceEquals(x, y)
    End Function
    '''<summary>Returns a hash code for the specified object.</summary>
    '''<returns>A hash code for the specified object.</returns>
    '''<param name="obj">The <see cref="Object"/> for which a hash code is to be returned.</param>
    Public Overloads Function GetHashCode(obj As Object) As Integer Implements IEqualityComparer(Of Object).GetHashCode
        Return RuntimeHelpers.GetHashCode(obj)
    End Function

End Class
