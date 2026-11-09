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
        Me.DateScroller1 = New KoerControls.DateScroller()
        Me.DateScrollerItem1 = New KoerControls.DateScrollerItem()
        Me.Group1 = New KoerControls.DateScrollerGroup()
        Me.DateScrollerItem2 = New KoerControls.DateScrollerItem()
        Me.Group2 = New KoerControls.DateScrollerGroup()
        Me.DateScrollerItem3 = New KoerControls.DateScrollerItem()
        Me.DateScrollerItem4 = New KoerControls.DateScrollerItem()
        Me.DateScrollerItem5 = New KoerControls.DateScrollerItem()
        Me.DateScroller1.SuspendLayout()
        Me.SuspendLayout()
        '
        'DateScroller1
        '
        Me.DateScroller1.AutoScrollMargin = New System.Drawing.Size(3, 3)
        Me.DateScroller1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.DateScroller1.Controls.Add(Me.DateScrollerItem1)
        Me.DateScroller1.Controls.Add(Me.DateScrollerItem2)
        Me.DateScroller1.Controls.Add(Me.DateScrollerItem3)
        Me.DateScroller1.Controls.Add(Me.DateScrollerItem4)
        Me.DateScroller1.Controls.Add(Me.DateScrollerItem5)
        Me.DateScroller1.DateRange = New KoerControls.DateRange(2026, 10, 18, 0, 0, 0, 0, 2026, 10, 18, 0, 0, 0, 0)
        Me.DateScroller1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateScroller1.HeaderOverlay.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.DateScroller1.ItemGroups.Add(Me.Group1)
        Me.DateScroller1.ItemGroups.Add(Me.Group2)
        Me.DateScroller1.Items.Add(Me.DateScrollerItem1)
        Me.DateScroller1.Items.Add(Me.DateScrollerItem2)
        Me.DateScroller1.Items.Add(Me.DateScrollerItem3)
        Me.DateScroller1.Items.Add(Me.DateScrollerItem4)
        Me.DateScroller1.Items.Add(Me.DateScrollerItem5)
        Me.DateScroller1.Location = New System.Drawing.Point(232, 149)
        Me.DateScroller1.Name = "DateScroller1"
        Me.DateScroller1.Padding = New System.Windows.Forms.Padding(5)
        Me.DateScroller1.Size = New System.Drawing.Size(468, 169)
        Me.DateScroller1.TabIndex = 0
        '
        'DateScrollerItem1
        '
        Me.DateScrollerItem1.BackColor = System.Drawing.Color.Gray
        Me.DateScrollerItem1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.DateScrollerItem1.DateRange = New KoerControls.DateRange(2026, 10, 19, 0, 0, 0, 0, 2026, 10, 20, 0, 0, 0, 0)
        Me.DateScrollerItem1.ItemGroup = Me.Group1
        Me.DateScrollerItem1.Name = "DateScrollerItem1"
        Me.DateScrollerItem1.TabIndex = 0
        '
        'Group1
        '
        Me.Group1.Name = "Group1"
        Me.Group1.Value = "Group1"
        '
        'DateScrollerItem2
        '
        Me.DateScrollerItem2.BackColor = System.Drawing.Color.DarkGray
        Me.DateScrollerItem2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.DateScrollerItem2.DateRange = New KoerControls.DateRange(2026, 10, 19, 0, 0, 0, 0, 2026, 10, 21, 0, 0, 0, 0)
        Me.DateScrollerItem2.ItemGroup = Me.Group2
        Me.DateScrollerItem2.Name = "DateScrollerItem2"
        Me.DateScrollerItem2.TabIndex = 1
        '
        'Group2
        '
        Me.Group2.Name = "Group2"
        Me.Group2.Value = "Group2"
        '
        'DateScrollerItem3
        '
        Me.DateScrollerItem3.BackColor = System.Drawing.Color.Gainsboro
        Me.DateScrollerItem3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.DateScrollerItem3.DateRange = New KoerControls.DateRange(2026, 10, 22, 0, 0, 0, 0, 2026, 10, 22, 0, 0, 0, 0)
        Me.DateScrollerItem3.ItemGroup = Me.Group1
        Me.DateScrollerItem3.Name = "DateScrollerItem3"
        Me.DateScrollerItem3.TabIndex = 4
        '
        'DateScrollerItem4
        '
        Me.DateScrollerItem4.BackColor = System.Drawing.Color.Silver
        Me.DateScrollerItem4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.DateScrollerItem4.DateRange = New KoerControls.DateRange(2026, 10, 20, 0, 0, 0, 0, 2026, 10, 22, 0, 0, 0, 0)
        Me.DateScrollerItem4.ItemGroup = Me.Group2
        Me.DateScrollerItem4.Name = "DateScrollerItem4"
        Me.DateScrollerItem4.TabIndex = 2
        '
        'DateScrollerItem5
        '
        Me.DateScrollerItem5.BackColor = System.Drawing.Color.LightGray
        Me.DateScrollerItem5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.DateScrollerItem5.DateRange = New KoerControls.DateRange(2026, 10, 21, 0, 0, 0, 0, 2026, 10, 22, 0, 0, 0, 0)
        Me.DateScrollerItem5.ItemGroup = Me.Group1
        Me.DateScrollerItem5.Name = "DateScrollerItem5"
        Me.DateScrollerItem5.TabIndex = 3
        '
        'TestForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(1082, 392)
        Me.Controls.Add(Me.DateScroller1)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "TestForm"
        Me.Text = "TestForm"
        Me.DateScroller1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents DateScroller1 As DateScroller
    Friend WithEvents DateScrollerItem2 As DateScrollerItem
    Friend WithEvents DateScrollerItem1 As DateScrollerItem
    Friend WithEvents DateScrollerItem3 As DateScrollerItem
    Friend WithEvents DateScrollerItem4 As DateScrollerItem
    Friend WithEvents Group1 As DateScrollerGroup
    Friend WithEvents DateScrollerItem5 As DateScrollerItem
    Friend WithEvents Group2 As DateScrollerGroup
End Class
