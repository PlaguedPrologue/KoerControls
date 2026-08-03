<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EditingInputBox
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Prompt = New System.Windows.Forms.TextBox()
        Me.ResponseText = New System.Windows.Forms.TextBox()
        Me.TopButton = New System.Windows.Forms.Button()
        Me.MiddleButton = New System.Windows.Forms.Button()
        Me.BottomButton = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Prompt
        '
        Me.Prompt.BackColor = System.Drawing.SystemColors.Control
        Me.Prompt.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.Prompt.Location = New System.Drawing.Point(5, 5)
        Me.Prompt.Multiline = True
        Me.Prompt.Name = "Prompt"
        Me.Prompt.Size = New System.Drawing.Size(282, 66)
        Me.Prompt.TabIndex = 7
        Me.Prompt.TabStop = False
        '
        'ResponseText
        '
        Me.ResponseText.Location = New System.Drawing.Point(4, 74)
        Me.ResponseText.Multiline = True
        Me.ResponseText.Name = "ResponseText"
        Me.ResponseText.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.ResponseText.Size = New System.Drawing.Size(347, 72)
        Me.ResponseText.TabIndex = 8
        '
        'TopButton
        '
        Me.TopButton.Location = New System.Drawing.Point(293, 2)
        Me.TopButton.Name = "TopButton"
        Me.TopButton.Size = New System.Drawing.Size(58, 22)
        Me.TopButton.TabIndex = 11
        Me.TopButton.UseVisualStyleBackColor = True
        '
        'MiddleButton
        '
        Me.MiddleButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.MiddleButton.Location = New System.Drawing.Point(293, 26)
        Me.MiddleButton.Name = "MiddleButton"
        Me.MiddleButton.Size = New System.Drawing.Size(58, 22)
        Me.MiddleButton.TabIndex = 12
        Me.MiddleButton.UseVisualStyleBackColor = True
        '
        'BottomButton
        '
        Me.BottomButton.Location = New System.Drawing.Point(293, 50)
        Me.BottomButton.Name = "BottomButton"
        Me.BottomButton.Size = New System.Drawing.Size(58, 22)
        Me.BottomButton.TabIndex = 13
        Me.BottomButton.UseVisualStyleBackColor = True
        '
        'EditingInputBox
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(355, 151)
        Me.Controls.Add(Me.BottomButton)
        Me.Controls.Add(Me.MiddleButton)
        Me.Controls.Add(Me.TopButton)
        Me.Controls.Add(Me.Prompt)
        Me.Controls.Add(Me.ResponseText)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "EditingInputBox"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Prompt As Windows.Forms.TextBox
    Friend WithEvents ResponseText As Windows.Forms.TextBox
    Friend WithEvents TopButton As Windows.Forms.Button
    Friend WithEvents MiddleButton As Windows.Forms.Button
    Friend WithEvents BottomButton As Windows.Forms.Button
End Class
