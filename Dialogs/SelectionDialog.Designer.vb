<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class SelectionDialog
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
        Me.ResponseText = New KoerControls.MultiLineComboBox()
        Me.Cancel = New System.Windows.Forms.Button()
        Me.OK = New System.Windows.Forms.Button()
        Me.Prompt = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'Response
        '
        Me.ResponseText.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.ResponseText.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.ResponseText.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.ResponseText.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ResponseText.FormattingEnabled = True
        Me.ResponseText.Location = New System.Drawing.Point(4, 74)
        Me.ResponseText.Name = "ResponseText"
        Me.ResponseText.Size = New System.Drawing.Size(347, 21)
        Me.ResponseText.TabIndex = 2
        '
        'Cancel
        '
        Me.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel.Location = New System.Drawing.Point(293, 26)
        Me.Cancel.Name = "Cancel"
        Me.Cancel.Size = New System.Drawing.Size(58, 21)
        Me.Cancel.TabIndex = 4
        Me.Cancel.Text = "Cancel"
        Me.Cancel.UseVisualStyleBackColor = True
        '
        'OK
        '
        Me.OK.Location = New System.Drawing.Point(293, 2)
        Me.OK.Name = "OK"
        Me.OK.Size = New System.Drawing.Size(58, 22)
        Me.OK.TabIndex = 3
        Me.OK.Text = "OK"
        Me.OK.UseVisualStyleBackColor = True
        '
        'Prompt
        '
        Me.Prompt.BackColor = System.Drawing.SystemColors.Control
        Me.Prompt.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.Prompt.Location = New System.Drawing.Point(5, 6)
        Me.Prompt.Multiline = True
        Me.Prompt.Name = "Prompt"
        Me.Prompt.Size = New System.Drawing.Size(282, 62)
        Me.Prompt.TabIndex = 1
        Me.Prompt.TabStop = False
        '
        'SelectionDialog
        '
        Me.AcceptButton = Me.OK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel
        Me.ClientSize = New System.Drawing.Size(355, 99)
        Me.Controls.Add(Me.Prompt)
        Me.Controls.Add(Me.OK)
        Me.Controls.Add(Me.ResponseText)
        Me.Controls.Add(Me.Cancel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SelectionDialog"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ResponseText As MultiLineComboBox
    Friend WithEvents Cancel As Button
    Friend WithEvents OK As Button
    Friend WithEvents Prompt As TextBox
End Class
