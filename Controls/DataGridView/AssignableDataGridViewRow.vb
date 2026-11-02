'''<summary>Represents a <see cref="DataGridViewRow"/> that can be reassigned from one <see cref="Windows.Forms.DataGridView"/> to another.</summary>
Public Class AssignableDataGridViewRow
    Inherits DataGridViewRow

#Region "Functions"

    '''<summary>Changes the <see cref="Windows.Forms.DataGridView"/> the <see cref="AssignableDataGridViewRow"/> is contained within.</summary>
    '''<remarks>If the <see cref="Windows.Forms.DataGridView"/> does not support the current <see cref="AssignableDataGridViewRow"/> format, the row will be rebuilt.
    '''<para/>If the row should not be rebuild, use <see cref="TryAssignToDataGridView(DataGridView, Integer)"/>.</remarks>
    '''<returns><see langword="True"/> if the dataGridViewRow did not need to be rebuilt to fit in the <see cref="Windows.Forms.DataGridView"/>; otherwise <see langword="False"/>.</returns>
    '''<param name="dataGridView">The new <see cref="Windows.Forms.DataGridView"/> of the <see cref="AssignableDataGridViewRow"/>.</param>
    '''<param name="rowIndex">The position at which to insert the row. Enter '-1' to add to the end of the data grid view.</param>
    '''    '''<exception cref="ArgumentNullException"><paramref name="dataGridView"/> is null.</exception>
    '''<exception cref="IndexOutOfRangeException"><paramref name="rowIndex"/> is less than zero or greater than the number of rows in the collection.</exception>
    Public Overridable Function AssignToDataGridView(dataGridView As DataGridView, Optional rowIndex As Integer = -1) As Boolean
        If dataGridView Is Nothing Then Throw New ArgumentNullException(NameOf(dataGridView))
        If rowIndex < -1 OrElse rowIndex > dataGridView.RowCount Then Throw New IndexOutOfRangeException("Row index is less than zero or greater than the number of rows in the collection.")
        If ReferenceEquals(dataGridView, Me.DataGridView) Then Return True
        Dim bRebuilt As Boolean = Cells.Count > dataGridView.Columns.Count
        If bRebuilt Then
            RebuildOn(dataGridView)
        End If
        Dim dgvcDataGridViewCurrentCell = Me.DataGridView?.CurrentCell
        Dim iSelectedCellIndex As Integer = If(Cells.Contains(dgvcDataGridViewCurrentCell), dgvcDataGridViewCurrentCell.ColumnIndex, -1)
        Me.DataGridView.Rows.Remove(Me)
        If rowIndex = -1 Then
            rowIndex = dataGridView.RowCount
            dataGridView.Rows.Add(Me)
        Else
            dataGridView.Rows.Insert(rowIndex, Me)
        End If
        If iSelectedCellIndex <> -1 Then
            dataGridView.CurrentCell = dataGridView.Rows(rowIndex).Cells(iSelectedCellIndex)
        End If
        Return Not bRebuilt
    End Function
    '''<summary>Removews the last set of cells to make the <see cref="AssignableDataGridViewRow"/> cells fit into the <paramref name="dataGridView"/>.</summary>
    '''<param name="dataGridView">The <see cref="Windows.Forms.DataGridView"/> the <see cref="AssignableDataGridViewRow"/> is required to fit into.</param>
    '''<exception cref="ArgumentNullException"><paramref name="dataGridView"/> is null.</exception>
    Public Overridable Sub RebuildOn(dataGridView As DataGridView)
        If dataGridView Is Nothing Then Throw New ArgumentNullException(NameOf(dataGridView))
        While Cells.Count > dataGridView.Columns.Count
            Cells.RemoveAt(Cells.Count - 1)
        End While
    End Sub
    '''<summary>Attempts to change the <see cref="Windows.Forms.DataGridView"/> the <see cref="AssignableDataGridViewRow"/> is contained within.</summary>
    '''<remarks>If the <see cref="AssignableDataGridViewRow"/> should be rebuilt upon formats not matching, use <see cref="AssignToDataGridView(DataGridView, Integer)"/>.</remarks>
    '''<returns><see langword="True"/> if the <see cref="AssignableDataGridViewRow"/> was able to be reassigned; otherwise, <see langword="False"/>.</returns>
    '''<param name="dataGridView">The new <see cref="Windows.Forms.DataGridView"/> of the <see cref="AssignableDataGridViewRow"/>.</param>
    '''<param name="rowIndex">The position at which to insert the row. Enter '-1' to add to the end of the DataGridView.</param>
    '''<exception cref="ArgumentException">dataGridViewRow has more cells than there are columns in the DataGridView.</exception>
    '''    '''<exception cref="ArgumentNullException"><paramref name="dataGridView"/> is null.</exception>
    '''<exception cref="IndexOutOfRangeException"><paramref name="rowIndex"/> is less than zero or greater than the number of rows in the collection.</exception>
    Public Overridable Function TryAssignToDataGridView(dataGridView As DataGridView, Optional rowIndex As Integer = -1) As Boolean
        If dataGridView Is Nothing Then Throw New ArgumentNullException(NameOf(dataGridView))
        If Cells.Count > dataGridView.Columns.Count Then Throw New ArgumentException("DataGridViewRow contains more cells than there are columns in the DataGridView.")
        If rowIndex < -1 OrElse rowIndex > dataGridView.RowCount Then Throw New IndexOutOfRangeException("Row index must fall within the number of rows in the collection.")
        If ReferenceEquals(dataGridView, Me.DataGridView) Then Return True
        Try
            Dim dgvcDataGridViewCurrentCell = Me.DataGridView?.CurrentCell
            Dim iSelectedCellIndex As Integer = If(Cells.Contains(dgvcDataGridViewCurrentCell), dgvcDataGridViewCurrentCell.ColumnIndex, -1)
            Me.DataGridView.Rows.Remove(Me)
            If rowIndex = -1 Then
                rowIndex = dataGridView.RowCount
                dataGridView.Rows.Add(Me)
            Else
                dataGridView.Rows.Insert(rowIndex, Me)
            End If
            If iSelectedCellIndex <> -1 Then
                dataGridView.CurrentCell = dataGridView.Rows(rowIndex).Cells(iSelectedCellIndex)
            End If
            Return True
        Catch
            Return False
        End Try
    End Function

#End Region

End Class
