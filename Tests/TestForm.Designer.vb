<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class TestForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        Me.TestStaticColumnsDataGridView1 = New KoerControls.TestStaticColumnsDataGridView()
        CType(Me.TestStaticColumnsDataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TestStaticColumnsDataGridView1
        '
        CType(Me.TestStaticColumnsDataGridView1.GetStaticColumn("Column1"), System.Windows.Forms.DataGridViewTextBoxColumn).Visible = False
        Me.TestStaticColumnsDataGridView1.Location = New System.Drawing.Point(441, 205)
        Me.TestStaticColumnsDataGridView1.Name = "TestStaticColumnsDataGridView1"
        Me.TestStaticColumnsDataGridView1.Size = New System.Drawing.Size(240, 150)
        Me.TestStaticColumnsDataGridView1.TabIndex = 0
        CType(Me.TestStaticColumnsDataGridView1.GetDefaultContextMenuStripItem("ToolStripMenuItem2"), System.Windows.Forms.ToolStripMenuItem).DoubleClickEnabled = True
        '
        'TestForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.TestStaticColumnsDataGridView1)
        Me.Name = "TestForm"
        Me.Text = "TestForm"
        CType(Me.TestStaticColumnsDataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TestStaticColumnsDataGridView1 As TestStaticColumnsDataGridView
End Class
