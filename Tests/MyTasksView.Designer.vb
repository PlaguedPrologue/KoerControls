<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MyTasksView
    Inherits System.Windows.Forms.UserControl

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
        Me.components = New System.ComponentModel.Container()
        Me.MainPanel = New System.Windows.Forms.TableLayoutPanel()
        Me.NotesInfo = New System.Windows.Forms.Label()
        Me.CompletedInfo = New System.Windows.Forms.Label()
        Me.UpcomingInfo = New System.Windows.Forms.Label()
        Me.CurrentInfo = New System.Windows.Forms.Label()
        Me.LinePanel4 = New KoerControls.LinePanel()
        Me.LinePanel3 = New KoerControls.LinePanel()
        Me.LinePanel1 = New KoerControls.LinePanel()
        Me.LinePanel2 = New KoerControls.LinePanel()
        Me.OverdueInfo = New System.Windows.Forms.Label()
        Me.CompletedTasksDateRangePicker = New KoerControls.DateRangePicker()
        Me.TaskRefresher = New System.Windows.Forms.Timer(Me.components)
        Me.MainPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'MainPanel
        '
        Me.MainPanel.ColumnCount = 3
        Me.MainPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.MainPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 1.0!))
        Me.MainPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.MainPanel.Controls.Add(Me.NotesInfo, 0, 7)
        Me.MainPanel.Controls.Add(Me.CompletedInfo, 2, 3)
        Me.MainPanel.Controls.Add(Me.UpcomingInfo, 0, 3)
        Me.MainPanel.Controls.Add(Me.CurrentInfo, 2, 0)
        Me.MainPanel.Controls.Add(Me.LinePanel4)
        Me.MainPanel.Controls.Add(Me.LinePanel3)
        Me.MainPanel.Controls.Add(Me.LinePanel1)
        Me.MainPanel.Controls.Add(Me.LinePanel2)
        Me.MainPanel.Controls.Add(Me.OverdueInfo, 0, 0)
        Me.MainPanel.Controls.Add(Me.CompletedTasksDateRangePicker, 2, 4)
        Me.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.MainPanel.Location = New System.Drawing.Point(0, 0)
        Me.MainPanel.Name = "MainPanel"
        Me.MainPanel.RowCount = 9
        Me.MainPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
        Me.MainPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40.0!))
        Me.MainPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 1.0!))
        Me.MainPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
        Me.MainPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.MainPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40.0!))
        Me.MainPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 1.0!))
        Me.MainPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
        Me.MainPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.MainPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.MainPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.MainPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.MainPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.MainPanel.Size = New System.Drawing.Size(620, 426)
        Me.MainPanel.TabIndex = 1
        '
        'NotesInfo
        '
        Me.NotesInfo.AutoSize = True
        Me.MainPanel.SetColumnSpan(Me.NotesInfo, 3)
        Me.NotesInfo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.NotesInfo.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.NotesInfo.Location = New System.Drawing.Point(3, 361)
        Me.NotesInfo.Name = "NotesInfo"
        Me.NotesInfo.Size = New System.Drawing.Size(614, 65)
        Me.NotesInfo.TabIndex = 10
        Me.NotesInfo.Text = "Notes"
        '
        'CompletedInfo
        '
        Me.CompletedInfo.AutoSize = True
        Me.CompletedInfo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CompletedInfo.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CompletedInfo.Location = New System.Drawing.Point(313, 155)
        Me.CompletedInfo.Name = "CompletedInfo"
        Me.CompletedInfo.Size = New System.Drawing.Size(304, 28)
        Me.CompletedInfo.TabIndex = 7
        Me.CompletedInfo.Text = "Completed"
        '
        'UpcomingInfo
        '
        Me.UpcomingInfo.AutoSize = True
        Me.UpcomingInfo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.UpcomingInfo.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UpcomingInfo.Location = New System.Drawing.Point(3, 155)
        Me.UpcomingInfo.Name = "UpcomingInfo"
        Me.UpcomingInfo.Size = New System.Drawing.Size(303, 28)
        Me.UpcomingInfo.TabIndex = 5
        Me.UpcomingInfo.Text = "Upcoming"
        '
        'CurrentInfo
        '
        Me.CurrentInfo.AutoSize = True
        Me.CurrentInfo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CurrentInfo.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CurrentInfo.Location = New System.Drawing.Point(313, 0)
        Me.CurrentInfo.Name = "CurrentInfo"
        Me.CurrentInfo.Size = New System.Drawing.Size(304, 25)
        Me.CurrentInfo.TabIndex = 3
        Me.CurrentInfo.Text = "Current"
        '
        'LinePanel4
        '
        Me.LinePanel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.MainPanel.SetColumnSpan(Me.LinePanel4, 3)
        Me.LinePanel4.Length = 311
        Me.LinePanel4.Location = New System.Drawing.Point(0, 25)
        Me.LinePanel4.Margin = New System.Windows.Forms.Padding(0)
        Me.LinePanel4.Name = "LinePanel4"
        Me.LinePanel4.Size = New System.Drawing.Size(311, 1)
        Me.LinePanel4.TabIndex = 15
        Me.LinePanel4.Thickness = 1
        '
        'LinePanel3
        '
        Me.LinePanel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LinePanel3.Length = 0
        Me.LinePanel3.Location = New System.Drawing.Point(0, 154)
        Me.LinePanel3.Margin = New System.Windows.Forms.Padding(0)
        Me.LinePanel3.Name = "LinePanel3"
        Me.LinePanel3.Size = New System.Drawing.Size(0, 1)
        Me.LinePanel3.TabIndex = 14
        Me.LinePanel3.Thickness = 1
        '
        'LinePanel1
        '
        Me.LinePanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LinePanel1.Length = 204
        Me.LinePanel1.Location = New System.Drawing.Point(309, 0)
        Me.LinePanel1.Margin = New System.Windows.Forms.Padding(0)
        Me.LinePanel1.Name = "LinePanel1"
        Me.LinePanel1.Orientation = System.Windows.Forms.Orientation.Vertical
        Me.MainPanel.SetRowSpan(Me.LinePanel1, 6)
        Me.LinePanel1.Size = New System.Drawing.Size(1, 204)
        Me.LinePanel1.TabIndex = 12
        Me.LinePanel1.Thickness = 1
        '
        'LinePanel2
        '
        Me.LinePanel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LinePanel2.Length = 0
        Me.LinePanel2.Location = New System.Drawing.Point(0, 154)
        Me.LinePanel2.Margin = New System.Windows.Forms.Padding(0)
        Me.LinePanel2.Name = "LinePanel2"
        Me.LinePanel2.Size = New System.Drawing.Size(0, 1)
        Me.LinePanel2.TabIndex = 13
        Me.LinePanel2.Thickness = 1
        '
        'OverdueInfo
        '
        Me.OverdueInfo.AutoSize = True
        Me.OverdueInfo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.OverdueInfo.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OverdueInfo.Location = New System.Drawing.Point(3, 0)
        Me.OverdueInfo.Name = "OverdueInfo"
        Me.OverdueInfo.Size = New System.Drawing.Size(303, 25)
        Me.OverdueInfo.TabIndex = 1
        Me.OverdueInfo.Text = "Overdue"
        '
        'CompletedTasksDateRangePicker
        '
        Me.CompletedTasksDateRangePicker.CustomFormat = Nothing
        Me.CompletedTasksDateRangePicker.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.CompletedTasksDateRangePicker.Location = New System.Drawing.Point(310, 183)
        Me.CompletedTasksDateRangePicker.Margin = New System.Windows.Forms.Padding(0)
        Me.CompletedTasksDateRangePicker.Name = "CompletedTasksDateRangePicker"
        Me.CompletedTasksDateRangePicker.Size = New System.Drawing.Size(225, 20)
        Me.CompletedTasksDateRangePicker.TabIndex = 8
        '
        'TaskRefresher
        '
        Me.TaskRefresher.Interval = 300000
        '
        'MyTasksView
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.Controls.Add(Me.MainPanel)
        Me.Name = "MyTasksView"
        Me.Size = New System.Drawing.Size(620, 426)
        Me.MainPanel.ResumeLayout(False)
        Me.MainPanel.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents MainPanel As Windows.Forms.TableLayoutPanel
    Friend WithEvents LinePanel1 As KoerControls.LinePanel
    Friend WithEvents LinePanel2 As KoerControls.LinePanel
    Friend WithEvents LinePanel4 As KoerControls.LinePanel
    Friend WithEvents LinePanel3 As KoerControls.LinePanel
    Friend WithEvents OverdueInfo As Windows.Forms.Label
    Friend WithEvents CompletedInfo As Windows.Forms.Label
    Friend WithEvents UpcomingInfo As Windows.Forms.Label
    Friend WithEvents CurrentInfo As Windows.Forms.Label
    Friend WithEvents CompletedTasksDateRangePicker As KoerControls.DateRangePicker
    Friend WithEvents NotesInfo As Windows.Forms.Label
    Friend WithEvents TaskRefresher As Windows.Forms.Timer
End Class
