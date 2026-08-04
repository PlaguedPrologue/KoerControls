<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class DateRangePicker
    Inherits System.Windows.Forms.Control

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.EndDatePicker = New System.Windows.Forms.DateTimePicker()
        Me.StartDatePicker = New System.Windows.Forms.DateTimePicker()
        Me.DelimiterText = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'EndDatePicker
        '
        Me.EndDatePicker.Dock = System.Windows.Forms.DockStyle.Right
        Me.EndDatePicker.Location = New System.Drawing.Point(-200, 0)
        Me.EndDatePicker.Name = "EndDatePicker"
        Me.EndDatePicker.Size = New System.Drawing.Size(200, 20)
        Me.EndDatePicker.TabIndex = 0
        '
        'StartDatePicker
        '
        Me.StartDatePicker.Dock = System.Windows.Forms.DockStyle.Left
        Me.StartDatePicker.Location = New System.Drawing.Point(0, 0)
        Me.StartDatePicker.Name = "StartDatePicker"
        Me.StartDatePicker.Size = New System.Drawing.Size(200, 20)
        Me.StartDatePicker.TabIndex = 0
        '
        'DelimiterText
        '
        Me.DelimiterText.AutoSize = True
        Me.DelimiterText.Dock = System.Windows.Forms.DockStyle.Left
        Me.DelimiterText.Location = New System.Drawing.Point(200, 0)
        Me.DelimiterText.Name = "DelimiterText"
        Me.DelimiterText.Padding = New System.Windows.Forms.Padding(3)
        Me.DelimiterText.Size = New System.Drawing.Size(26, 19)
        Me.DelimiterText.TabIndex = 0
        Me.DelimiterText.Text = "To"
        '
        'DateRangePicker
        '
        Me.Controls.Add(Me.EndDatePicker)
        Me.Controls.Add(Me.DelimiterText)
        Me.Controls.Add(Me.StartDatePicker)
        Me.Size = New System.Drawing.Size(426, 20)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents EndDatePicker As DateTimePicker
    Friend WithEvents StartDatePicker As DateTimePicker
    Friend WithEvents DelimiterText As Label

End Class
