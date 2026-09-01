Imports System.ComponentModel
Imports System.Reflection
Imports System.Runtime.CompilerServices

'''<summary>Represents a static collection of <see cref="DataGridViewColumn"/> objects in a <see cref="StaticColumnsDataGridView"/>.</summary>
<DefaultMember("Item")>
<TypeConverter(GetType(StaticDataGridViewColumnCollectionConverter))>
Public Class StaticDataGridViewColumnCollection
    Implements ICollection, IEnumerable

#Region "Initializers"

    '''<summary>Initializes a new instance of the <see cref="StaticDataGridViewColumnCollection"/> class for the given <see cref="StaticColumnsDataGridView"/>.</summary>
    '''<param name="dataGridView">The <see cref="StaticColumnsDataGridView"/> that created this collection.</param>
    Public Sub New(dataGridView As StaticColumnsDataGridView)
        Me.DataGridView = dataGridView
    End Sub

#End Region

#Region "Properties"

    Public ReadOnly Property Count As Integer Implements ICollection.Count
        Get
            Return DataGridView.ColumnCount
        End Get
    End Property
    '''<summary>Gets the <see cref="StaticColumnsDataGridView"/> upon which the collection performs column-related operations.</summary>
    Friend ReadOnly Property DataGridView As StaticColumnsDataGridView
    Public ReadOnly Property IsSynchronized As Boolean Implements ICollection.IsSynchronized
        Get
            Return DataGridView.Columns.IsSynchronized
        End Get
    End Property
    '''<summary>Gets or sets the column at the given index in the collection.</summary>
    '''<returns>The <see cref="DataGridViewColumn"/> at the given index.</returns>
    '''<param name="index">The zero-based index of the column to get or set.</param>
    '''<exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than zero or greater than the number of columns in the collection minus one.</exception>
    Default Public ReadOnly Property Item(index As Integer) As DataGridViewColumn
        Get
            Return DataGridView.Columns(index)
        End Get
    End Property
    '''<summary>Gets or sets the column of the given name in the collection.</summary>
    '''<returns>The <see cref="DataGridViewColumn"/> identified by the columnName parameter.</returns>
    '''<param name="columnName">The name of the column to get or set.</param>
    '''<exception cref="ArgumentNullException"><paramref name="columnName"/> is null.</exception>
    <IndexerName("Item")>
    Default Public ReadOnly Property Item(columnName As String) As DataGridViewColumn
        Get
            For Each dgvcColumn As DataGridViewColumn In DataGridView.Columns
                If String.Equals(dgvcColumn.Name.TrimStart("_"c), columnName, StringComparison.OrdinalIgnoreCase) Then Return dgvcColumn
            Next
            Return Nothing
        End Get
    End Property
    Public ReadOnly Property SyncRoot As Object Implements ICollection.SyncRoot
        Get
            Return DataGridView.Columns.SyncRoot
        End Get
    End Property

#End Region

#Region "Functions"

    Public Sub CopyTo(array As Array, index As Integer) Implements ICollection.CopyTo
        DataGridView.Columns.CopyTo(array, index)
    End Sub
    Public Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return DataGridView.Columns.GetEnumerator
    End Function
    Public Overrides Function ToString() As String
        Return $"{DataGridView.ColumnCount} Columns"
    End Function

#End Region

End Class
