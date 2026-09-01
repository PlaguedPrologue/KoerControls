Imports System.ComponentModel
Imports System.Reflection
Imports System.Runtime.CompilerServices

'''<summary>Represents a static collection of <see cref="ToolStripItem"/> objects asigned to the <see cref="WIndows.Forms.DataGridView.ContextMenuStrip"/> in a <see cref="StaticColumnsDataGridView"/>.</summary>
<DefaultMember("Item")>
<TypeConverter(GetType(StaticColumnsDataGridViewDefaultContextMenuStripItemCollectionConverter))>
Public Class StaticColumnsDataGridViewDefaultContextMenuStripItemCollection
    Implements ICollection, IEnumerable

#Region "Initializers"

    '''<summary>Initializes a new instance of the <see cref="StaticColumnsDataGridViewDefaultContextMenuStripItemCollection"/> class for the given <see cref="StaticColumnsDataGridView"/>.</summary>
    '''<param name="contextMenuStrip">The <see cref="ContextMenuStrip"/> that created this collection.</param>
    Public Sub New(contextMenuStrip As ContextMenuStrip)
        Me.ContextMenuStrip = contextMenuStrip
    End Sub

#End Region

#Region "Properties"

    '''<summary>Gets the <see cref="ContextMenuStrip"/> upon which the collection performs item-related operations.</summary>
    Public ReadOnly Property ContextMenuStrip As ContextMenuStrip
    Public ReadOnly Property Count As Integer Implements ICollection.Count
        Get
            If ContextMenuStrip Is Nothing Then Return -1
            Return ContextMenuStrip.Items.Count
        End Get
    End Property
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
            If ContextMenuStrip Is Nothing Then Throw New ArgumentOutOfRangeException(NameOf(StaticColumnsDataGridViewDefaultContextMenuStripItemCollection.Item))
            Return ContextMenuStrip.Items(index)
        End Get
    End Property
    '''<summary>Gets or sets the tool strip item of the given name in the collection.</summary>
    '''<returns>The <see cref="ToolStripItem"/> identified by the toolStripItemName parameter.</returns>
    '''<param name="toolStripItemName">The name of the tool strip item to get or set.</param>
    '''<exception cref="ArgumentNullException"><paramref name="toolStripItemName"/> is null.</exception>
    <IndexerName("Item")>
    Default Public ReadOnly Property Item(toolStripItemName As String) As ToolStripItem
        Get
            If ContextMenuStrip Is Nothing Then Return Nothing
            For Each tsiToolStripItem As ToolStripItem In ContextMenuStrip.Items
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
        If ContextMenuStrip Is Nothing Then Return
        ContextMenuStrip.Items.CopyTo(array, index)
    End Sub
    Public Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        If ContextMenuStrip Is Nothing Then Return Nothing
        Return ContextMenuStrip.Items.GetEnumerator
    End Function
    Public Overrides Function ToString() As String
        If ContextMenuStrip Is Nothing Then Return "N/A"
        Return $"{ContextMenuStrip.Items.Count} Items"
    End Function

#End Region

End Class
