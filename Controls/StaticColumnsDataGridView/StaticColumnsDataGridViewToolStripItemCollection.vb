Imports System.ComponentModel
Imports System.Reflection
Imports System.Runtime.CompilerServices

'''<summary>Represents a static collection of <see cref="ToolStripItem"/> objects asigned to the <see cref="WIndows.Forms.DataGridView.ContextMenuStrip"/> in a <see cref="StaticColumnsDataGridView"/>.</summary>
<DefaultMember("Item")>
<TypeConverter(GetType(StaticColumnsDataGridViewToolStripItemCollectionConverter))>
Public Class StaticColumnsDataGridViewToolStripItemCollection
    Implements ICollection, IEnumerable

#Region "PrivateItems"

    Private Function IsOwnerOfContextMenuStrip() As Boolean
        If DataGridView.ContextMenuStrip Is Nothing Then Return False
        For Each fiField As FieldInfo In DataGridView.GetType.GetFields(BindingFlags.Instance Or BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.DeclaredOnly)
            If GetType(ContextMenuStrip).IsAssignableFrom(fiField.FieldType) Then
                If ReferenceEquals(fiField.GetValue(DataGridView), DataGridView.ContextMenuStrip) Then
                    Return True
                End If
            End If
        Next
        Return False
    End Function

#End Region

#Region "Initializers"

    '''<summary>Initializes a new instance of the <see cref="StaticColumnsDataGridViewToolStripItemCollection"/> class for the given <see cref="StaticColumnsDataGridView"/>.</summary>
    '''<param name="dataGridView">The <see cref="StaticColumnsDataGridView"/> that created this collection.</param>
    Public Sub New(dataGridView As StaticColumnsDataGridView)
        Me.DataGridView = dataGridView
    End Sub

#End Region

#Region "Properties"

    Public ReadOnly Property Count As Integer Implements ICollection.Count
        Get
            If DataGridView.ContextMenuStrip Is Nothing OrElse Not IsOwnerOfContextMenuStrip() Then Return -1
            Return DataGridView.ContextMenuStrip.Items.Count
        End Get
    End Property
    '''<summary>Gets the <see cref="StaticColumnsDataGridView"/> upon which the collection performs column-related operations.</summary>
    Friend ReadOnly Property DataGridView As StaticColumnsDataGridView
    Private ReadOnly Property IsSynchronized As Boolean Implements ICollection.IsSynchronized
        Get
            Return False
        End Get
    End Property
    '''<summary>Gets or sets the tool strip item at the given index in the collection.</summary>
    '''<returns>The <see cref="ToolStripItem"/> at the given index.</returns>
    '''<param name="index">The zero-based index of the tool strip item to get or set.</param>
    '''<exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than zero or greater than the number of tool strip items in the collection minus one.</exception>
    Default Public ReadOnly Property Item(index As Integer) As ToolStripItem
        Get
            If DataGridView.ContextMenuStrip Is Nothing OrElse Not IsOwnerOfContextMenuStrip() Then Return Nothing
            Return DataGridView.ContextMenuStrip.Items(index)
        End Get
    End Property
    '''<summary>Gets or sets the tool strip item of the given name in the collection.</summary>
    '''<returns>The <see cref="ToolStripItem"/> identified by the toolStripItemName parameter.</returns>
    '''<param name="toolStripItemName">The name of the tool strip item to get or set.</param>
    '''<exception cref="ArgumentNullException"><paramref name="toolStripItemName"/> is null.</exception>
    <IndexerName("Item")>
    Default Public ReadOnly Property Item(toolStripItemName As String) As ToolStripItem
        Get
            If DataGridView.ContextMenuStrip Is Nothing OrElse Not IsOwnerOfContextMenuStrip() Then Return Nothing
            For Each tsiToolStripItem As ToolStripItem In DataGridView.ContextMenuStrip.Items
                If String.Equals(tsiToolStripItem.Name.TrimStart("_"c), toolStripItemName, StringComparison.OrdinalIgnoreCase) Then Return tsiToolStripItem
            Next
            Return Nothing
        End Get
    End Property
    Private ReadOnly Property SyncRoot As Object Implements ICollection.SyncRoot
        Get
            Return Nothing
        End Get
    End Property

#End Region

#Region "Functions"

    Public Sub CopyTo(array As Array, index As Integer) Implements ICollection.CopyTo
        If DataGridView.ContextMenuStrip Is Nothing OrElse Not IsOwnerOfContextMenuStrip() Then Return
        DataGridView.ContextMenuStrip.Items.CopyTo(array, index)
    End Sub
    Public Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        If DataGridView.ContextMenuStrip Is Nothing OrElse Not IsOwnerOfContextMenuStrip() Then Return Nothing
        Return DataGridView.ContextMenuStrip.Items.GetEnumerator
    End Function
    Public Overrides Function ToString() As String
        If DataGridView.ContextMenuStrip Is Nothing OrElse Not IsOwnerOfContextMenuStrip() Then Return "N/A"
        Return $"{DataGridView.ContextMenuStrip.Items.Count} Items"
    End Function

#End Region

End Class
