<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class CalendarLineView
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
        Me.MainPanel = New System.Windows.Forms.TableLayoutPanel()
        Me.BucketsView = New System.Windows.Forms.Panel()
        Me.BucketsGrid = New KoerControls.DateScroller()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.TasksGrid = New KoerControls.DateScroller()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.DataGridView2 = New System.Windows.Forms.DataGridView()
        Me.MainPanel.SuspendLayout()
        Me.BucketsView.SuspendLayout()
        Me.BucketsGrid.SuspendLayout()
        Me.TasksGrid.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MainPanel
        '
        Me.MainPanel.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.MainPanel.ColumnCount = 2
        Me.MainPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200.0!))
        Me.MainPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.MainPanel.Controls.Add(Me.BucketsView, 1, 0)
        Me.MainPanel.Controls.Add(Me.TasksGrid, 1, 1)
        Me.MainPanel.Controls.Add(Me.DataGridView1, 0, 0)
        Me.MainPanel.Controls.Add(Me.DataGridView2, 0, 1)
        Me.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.MainPanel.Location = New System.Drawing.Point(0, 0)
        Me.MainPanel.Name = "MainPanel"
        Me.MainPanel.RowCount = 2
        Me.MainPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.MainPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80.0!))
        Me.MainPanel.Size = New System.Drawing.Size(620, 448)
        Me.MainPanel.TabIndex = 0
        '
        'BucketsView
        '
        Me.BucketsView.Controls.Add(Me.BucketsGrid)
        Me.BucketsView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.BucketsView.Location = New System.Drawing.Point(200, 0)
        Me.BucketsView.Margin = New System.Windows.Forms.Padding(0)
        Me.BucketsView.Name = "BucketsView"
        Me.BucketsView.Size = New System.Drawing.Size(420, 89)
        Me.BucketsView.TabIndex = 2
        '
        'BucketsGrid
        '
        Me.BucketsGrid.AutoScrollMode = KoerControls.AutoScrollMode.Vertical
        Me.BucketsGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.BucketsGrid.Controls.Add(Me.FlowLayoutPanel1)
        Me.BucketsGrid.Dock = System.Windows.Forms.DockStyle.Fill
        '
        '
        '
        Me.BucketsGrid.HeaderOverlay.Host = Me.BucketsGrid
        Me.BucketsGrid.HeaderOverlay.Padding = New System.Windows.Forms.Padding(0, 4, 0, 4)
        Me.BucketsGrid.Location = New System.Drawing.Point(0, 0)
        Me.BucketsGrid.Margin = New System.Windows.Forms.Padding(0)
        Me.BucketsGrid.Name = "BucketsGrid"
        Me.BucketsGrid.Size = New System.Drawing.Size(420, 89)
        Me.BucketsGrid.TabIndex = 4
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(356, 41)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(200, 100)
        Me.FlowLayoutPanel1.TabIndex = 0
        '
        'TasksGrid
        '
        Me.TasksGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TasksGrid.Controls.Add(Me.Panel1)
        Me.TasksGrid.Dock = System.Windows.Forms.DockStyle.Fill
        '
        '
        '
        Me.TasksGrid.HeaderOverlay.BackColor = System.Drawing.SystemColors.Control
        Me.TasksGrid.HeaderOverlay.Host = Me.TasksGrid
        Me.TasksGrid.HeaderOverlay.Visible = False
        Me.TasksGrid.ItemPlacement = KoerControls.DateScrollerItemPlacement.GroupedWaterfall
        Me.TasksGrid.Location = New System.Drawing.Point(200, 89)
        Me.TasksGrid.Margin = New System.Windows.Forms.Padding(0)
        Me.TasksGrid.Name = "TasksGrid"
        Me.TasksGrid.Size = New System.Drawing.Size(420, 359)
        Me.TasksGrid.TabIndex = 3
        '
        'Panel1
        '
        Me.Panel1.Location = New System.Drawing.Point(304, 135)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(200, 274)
        Me.Panel1.TabIndex = 0
        '
        'DataGridView1
        '
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridView1.Location = New System.Drawing.Point(3, 3)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.Size = New System.Drawing.Size(194, 83)
        Me.DataGridView1.TabIndex = 4
        '
        'DataGridView2
        '
        Me.DataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridView2.Location = New System.Drawing.Point(3, 92)
        Me.DataGridView2.Name = "DataGridView2"
        Me.DataGridView2.Size = New System.Drawing.Size(194, 353)
        Me.DataGridView2.TabIndex = 5
        '
        'CalendarLineView
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.MainPanel)
        Me.Name = "CalendarLineView"
        Me.Size = New System.Drawing.Size(620, 448)
        Me.MainPanel.ResumeLayout(False)
        Me.BucketsView.ResumeLayout(False)
        Me.BucketsGrid.ResumeLayout(False)
        Me.TasksGrid.ResumeLayout(False)
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents MainPanel As Windows.Forms.TableLayoutPanel
    Friend WithEvents BucketsView As Windows.Forms.Panel
    Friend WithEvents TasksGrid As KoerControls.DateScroller
    Friend WithEvents BucketsGrid As KoerControls.DateScroller
    Friend WithEvents FlowLayoutPanel1 As Windows.Forms.FlowLayoutPanel
    Friend WithEvents Panel1 As Windows.Forms.Panel
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents DataGridView2 As DataGridView
End Class
