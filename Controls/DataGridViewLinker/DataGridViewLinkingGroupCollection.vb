Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Drawing.Design

'''<summary>Repersents a collection of data grid view linking groups.</summary>
<Editor(GetType(DataGridViewLinkingGroupCollectionEditor), GetType(UITypeEditor))>
Public Class DataGridViewLinkingGroupCollection
    Inherits Collection(Of DataGridViewLinkingGroup)

#Region "Initalizers"

    '''<summary>Creates a new instance of the <see cref="DataGridViewLinkingGroupCollection"/> class.</summary>
    '''<param name="dataGridViewLinker">The <see cref="KoerControls.DataGridViewLinker"/> that owns the <see cref="DataGridViewLinkingGroupCollection"/>.</param>
    Public Sub New(dataGridViewLinker As DataGridViewLinker)
        Me.DataGridViewLinker = dataGridViewLinker
    End Sub

#End Region

#Region "Properties"

    '''<summary>Gets the <see cref="KoerControls.DataGridViewLinker"/> that owns the <see cref="DataGridViewLinkingGroupCollection"/>.</summary>
    '''<returns>The <see cref="KoerControls.DataGridViewLinker"/> that owns the <see cref="DataGridViewLinkingGroupCollection"/>.</returns>
    Public ReadOnly Property DataGridViewLinker As DataGridViewLinker

#End Region

#Region "Functions"

    Protected Overrides Sub ClearItems()
        For Each dgvlgLinkingGroup As DataGridViewLinkingGroup In Items
            If dgvlgLinkingGroup IsNot Nothing Then dgvlgLinkingGroup._dataGridViewLinker = Nothing
        Next
        MyBase.ClearItems()
    End Sub
    Protected Overrides Sub InsertItem(index As Integer, item As DataGridViewLinkingGroup)
        If item IsNot Nothing Then item._dataGridViewLinker = DataGridViewLinker
        MyBase.InsertItem(index, item)
    End Sub
    Protected Overrides Sub RemoveItem(index As Integer)
        Dim dgvlgCurrentItem As DataGridViewLinkingGroup = Items(index)
        If dgvlgCurrentItem IsNot Nothing Then dgvlgCurrentItem._dataGridViewLinker = Nothing
        MyBase.RemoveItem(index)
    End Sub
    Protected Overrides Sub SetItem(index As Integer, item As DataGridViewLinkingGroup)
        Dim dgvlgCurrentItem As DataGridViewLinkingGroup = Items(index)
        If dgvlgCurrentItem IsNot Nothing Then dgvlgCurrentItem._dataGridViewLinker = Nothing
        If item IsNot Nothing Then item._dataGridViewLinker = DataGridViewLinker
        MyBase.SetItem(index, item)
    End Sub

#End Region

End Class
