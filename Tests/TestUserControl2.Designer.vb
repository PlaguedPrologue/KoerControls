<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TestUserControl2
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.CalendarLineView1 = New KoerControls.CalendarLineView()
        Me.SuspendLayout()
        '
        'CalendarLineView1
        '
        Me.CalendarLineView1.Location = New System.Drawing.Point(94, 32)
        Me.CalendarLineView1.Name = "CalendarLineView1"
        Me.CalendarLineView1.Size = New System.Drawing.Size(390, 250)
        Me.CalendarLineView1.TabIndex = 4
        '
        'TestUserControl2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.CalendarLineView1)
        Me.Name = "TestUserControl2"
        Me.Size = New System.Drawing.Size(578, 315)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents CalendarLineView1 As CalendarLineView
End Class
