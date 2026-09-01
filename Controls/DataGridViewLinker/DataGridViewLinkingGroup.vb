Imports System.ComponentModel

'''<summary>Represents a group that links DataGridViews together.</summary>
Public Class DataGridViewLinkingGroup
    Inherits NamedComponent
    Implements IComponent, IDisposable

#Region "Private Items"

    Friend _dataGridViewLinker As DataGridViewLinker

#End Region

#Region "Initializers"

    '''<summary>Create a new empty instance of the <see cref="DataGridViewLinkingGroup"/> clas.</summary>
    Public Sub New()
        MyBase.New
    End Sub
    '''<summary>Creates a new instance of the <see cref="DataGridViewLinkingGroup"/> class.</summary>
    '''<param name="value">The name value of the <see cref="DataGridViewLinkingGroup"/>.</param>
    Public Sub New(value As String)
        MyBase.New(value)
    End Sub
    '''<summary>Creates a new instance of the <see cref="DataGridViewLinkingGroup"/> class.</summary>
    '''<param name="value">The name value of the <see cref="DataGridViewLinkingGroup"/>.</param>
    '''<param name="container">The owning <see cref="IContainer"/> of the <see cref="DataGridViewLinkingGroup"/>.</param>
    Public Sub New(value As String, container As IContainer)
        MyBase.New(value, container)
    End Sub

#End Region

#Region "Properties"

    '''<summary>Gets or sets the currently active cell.</summary>
    '''<returns>The <see cref="DataGridViewCell"/> that represents the current cell, or null if there is no current cell. The default is the first cell in the first column or null if there are no cells in the control.</returns>
    '''<exception cref="InvalidOperationException">The value of this property cannot be set because changes to the current cell cannot be committed or canceled.-or-The specified cell when setting this property is in a hidden row or column. Re-entrant calling is only allowed when the <see cref="DataGridView"/> is bound to a <see cref="DataGridView.DataSource"/>. Re-entrant calling results from a change to the underlying data.</exception>
    Public Property CurrentCell As DataGridViewCell
        Get
            If _dataGridViewLinker Is Nothing Then Return Nothing
            For Each dgvDataGridView As DataGridView In _dataGridViewLinker.GetDataGridViews(Me)
                Dim dgvcCurrentCell As DataGridViewCell = dgvDataGridView.CurrentCell
                If dgvcCurrentCell IsNot Nothing Then Return dgvcCurrentCell
            Next
            Return Nothing
        End Get
        Set(value As DataGridViewCell)
            If _dataGridViewLinker Is Nothing Then Return
            If value Is Nothing Then
                For Each dgvDataGridView As DataGridView In _dataGridViewLinker.GetDataGridViews(Me)
                    dgvDataGridView.CurrentCell = Nothing
                Next
            Else
                Dim dgvValueDataGridView As DataGridView = value?.DataGridView
                If _dataGridViewLinker.GetDataGridViews(Me).Any(Function(dgv) ReferenceEquals(dgv, dgvValueDataGridView)) Then dgvValueDataGridView.CurrentCell = value
            End If
        End Set
    End Property
    '''<summary>Gets the row containing the current cell.</summary>
    '''<returns>The <see cref="DataGridViewRow"/> that represents the row containing the current cell, or <see langword="Nothing"/> if there is no current cell.</returns>
    Public ReadOnly Property CurrentRow As DataGridViewRow
        Get
            If _dataGridViewLinker Is Nothing Then Return Nothing
            For Each dgvDataGridView As DataGridView In _dataGridViewLinker.GetDataGridViews(Me)
                Dim dgvrCurrentRow As DataGridViewRow = dgvDataGridView.CurrentRow
                If dgvrCurrentRow IsNot Nothing Then Return dgvrCurrentRow
            Next
            Return Nothing
        End Get
    End Property
    '''<summary>Gets the <see cref="KoerControls.DataGridViewLinker"/> that owns the <see cref="DataGridViewLinkingGroup"/>.</summary>
    '''<returns>The <see cref="KoerControls.DataGridViewLinker"/> that owns the <see cref="DataGridViewLinkingGroup"/>.</returns>
    Public ReadOnly Property DataGridViewLinker As DataGridViewLinker
        Get
            Return _dataGridViewLinker
        End Get
    End Property

#End Region

End Class
