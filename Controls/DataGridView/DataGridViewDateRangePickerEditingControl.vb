Imports System.ComponentModel

'''<summary>Represents and editing control for a <see cref="DateRangePicker"/>.</summary>
<ToolboxItem(False)>
Public Class DataGridViewDateRangePickerEditingControl
    Inherits DateRangePicker
    Implements IDataGridViewEditingControl

#Region "Properties"

    Public Property EditingControlDataGridView As DataGridView Implements IDataGridViewEditingControl.EditingControlDataGridView
    Public Property EditingControlFormattedValue As Object Implements IDataGridViewEditingControl.EditingControlFormattedValue
        Get
            Return Value
        End Get
        Set(value As Object)
            If TypeOf value IsNot DateRange Then Return
            Me.Value = value
        End Set
    End Property
    Public Property EditingControlRowIndex As Integer Implements IDataGridViewEditingControl.EditingControlRowIndex
    Public Property EditingControlValueChanged As Boolean Implements IDataGridViewEditingControl.EditingControlValueChanged
    Public ReadOnly Property EditingPanelCursor As Cursor Implements IDataGridViewEditingControl.EditingPanelCursor
        Get
            Return MyBase.Cursor
        End Get
    End Property
    Public ReadOnly Property RepositionEditingControlOnValueChange As Boolean = False Implements IDataGridViewEditingControl.RepositionEditingControlOnValueChange

#End Region

#Region "Functions"

    Public Sub ApplyCellStyleToEditingControl(dataGridViewCellStyle As DataGridViewCellStyle) Implements IDataGridViewEditingControl.ApplyCellStyleToEditingControl
        Font = dataGridViewCellStyle.Font
        ForeColor = dataGridViewCellStyle.ForeColor
        BackColor = dataGridViewCellStyle.BackColor
    End Sub
    Public Function EditingControlWantsInputKey(keyData As Keys, dataGridViewWantsInputKey As Boolean) As Boolean Implements IDataGridViewEditingControl.EditingControlWantsInputKey
        Select Case Keys.KeyCode
            Case Keys.Left, Keys.Right, Keys.Up, Keys.Down, Keys.Home, Keys.End
                Return True
            Case Else
                Return Not dataGridViewWantsInputKey
        End Select
    End Function
    Public Function GetEditingControlFormattedValue(context As DataGridViewDataErrorContexts) As Object Implements IDataGridViewEditingControl.GetEditingControlFormattedValue
        Return Value
    End Function
    Protected Overrides Sub OnValueChanged(eventargs As EventArgs)
        If EditingControlDataGridView IsNot Nothing Then
            EditingControlValueChanged = True
            EditingControlDataGridView.NotifyCurrentCellDirty(True)
        End If
        MyBase.OnValueChanged(eventargs)
    End Sub
    Public Sub PrepareEditingControlForEdit(selectAll As Boolean) Implements IDataGridViewEditingControl.PrepareEditingControlForEdit
    End Sub

#End Region

End Class
