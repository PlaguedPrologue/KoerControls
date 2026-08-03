Imports System.ComponentModel
Imports System.ComponentModel.Design

Partial Friend Class StringCollectionEditor

    Protected Class StringCollectionForm
        Inherits CollectionForm

#Region "Private Items"

        Private _instruction As Label
        Private _overarchingLayoutPanel As TableLayoutPanel
        Private Shadows WithEvents CancelButton As Button
        Private WithEvents OkButton As Button
        Private WithEvents TextEntry As TextBox

        Private Sub CancelButton_Click(sender As Object, e As EventArgs) Handles CancelButton.Click
            DialogResult = DialogResult.Cancel
        End Sub
        Private Sub InitializeComponent()

            _overarchingLayoutPanel = New TableLayoutPanel()
            _instruction = New Label()
            TextEntry = New TextBox()
            OkButton = New Button()
            CancelButton = New Button()

            _overarchingLayoutPanel.SuspendLayout()
            SuspendLayout()

            '
            'overarchingLayoutPanel
            '
            _overarchingLayoutPanel.ColumnCount = 2
            _overarchingLayoutPanel.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0!))
            _overarchingLayoutPanel.ColumnStyles.Add(New ColumnStyle())

            _overarchingLayoutPanel.RowCount = 4
            _overarchingLayoutPanel.RowStyles.Add(New RowStyle())
            _overarchingLayoutPanel.RowStyles.Add(New RowStyle(SizeType.Absolute, 6.0!))
            _overarchingLayoutPanel.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0!))
            _overarchingLayoutPanel.RowStyles.Add(New RowStyle())

            _overarchingLayoutPanel.Dock = DockStyle.Fill
            _overarchingLayoutPanel.Location = New Point(12, 9)
            _overarchingLayoutPanel.Name = "overarchingLayoutPanel"
            _overarchingLayoutPanel.Size = New Size(451, 262)
            _overarchingLayoutPanel.TabIndex = 0

            '
            'instruction
            '
            _instruction.AutoSize = True
            _instruction.Location = New Point(12, 10)
            _instruction.Margin = New Padding(0)
            _instruction.Name = "instruction"
            _instruction.Size = New Size(227, 13)
            _instruction.TabIndex = 0
            _instruction.Text = "&Enter the strings in the collection (one per line):"

            '
            'textEntry
            '
            TextEntry.Dock = DockStyle.Fill
            TextEntry.Location = New Point(12, 27)
            TextEntry.Margin = New Padding(0)
            TextEntry.Multiline = True
            TextEntry.Name = "textEntry"
            TextEntry.ScrollBars = ScrollBars.Both
            TextEntry.Size = New Size(451, 213)
            TextEntry.TabIndex = 0
            TextEntry.WordWrap = False
            '
            'okButton
            '
            OkButton.Anchor = CType((AnchorStyles.Bottom Or AnchorStyles.Right), AnchorStyles)
            OkButton.AutoSize = True
            OkButton.Location = New Point(304, 248)
            OkButton.Margin = New Padding(0, 5, 3, 0)
            OkButton.Name = "okButton"
            OkButton.Size = New Size(75, 23)
            OkButton.TabIndex = 1
            OkButton.Text = "OK"
            OkButton.UseVisualStyleBackColor = True

            '
            'cancelButton
            '
            CancelButton.Anchor = CType((AnchorStyles.Bottom Or AnchorStyles.Right), AnchorStyles)
            CancelButton.AutoSize = True
            CancelButton.Location = New Point(385, 248)
            CancelButton.Margin = New Padding(3, 5, 0, 0)
            CancelButton.Name = "cancelButton"
            CancelButton.Size = New Size(75, 23)
            CancelButton.TabIndex = 2
            CancelButton.Text = "Cancel"
            CancelButton.UseVisualStyleBackColor = True

            '
            'Add controls to TableLayoutPanel
            '
            _overarchingLayoutPanel.Controls.Add(_instruction, 0, 0)
            _overarchingLayoutPanel.SetColumnSpan(_instruction, 2)

            _overarchingLayoutPanel.Controls.Add(TextEntry, 0, 2)
            _overarchingLayoutPanel.SetColumnSpan(TextEntry, 2)

            _overarchingLayoutPanel.Controls.Add(OkButton, 0, 3)
            _overarchingLayoutPanel.Controls.Add(CancelButton, 1, 3)

            '
            'StringCollectionEditor
            '
            AutoScaleDimensions = New SizeF(6.0!, 13.0!)
            AutoScaleMode = AutoScaleMode.Font
            ClientSize = New Size(475, 281)
            HelpButton = True
            MinimizeBox = False
            MaximizeBox = False
            Padding = New Padding(12, 9, 12, 10)
            ShowIcon = False
            ShowInTaskbar = False
            StartPosition = FormStartPosition.CenterScreen
            Text = "String Collection Editor"

            Controls.Add(_overarchingLayoutPanel)

            _overarchingLayoutPanel.ResumeLayout(False)
            _overarchingLayoutPanel.PerformLayout()

            ResumeLayout(False)
        End Sub
        Private Sub OKButton_Click(sender As Object, e As EventArgs) Handles OkButton.Click
            Dim eCancel As New CancelEventArgs
            OnAccepted(eCancel)
            If eCancel.Cancel Then Return
            DialogResult = DialogResult.OK
        End Sub
        Private Sub TextEntry_KeyDown(sender As Object, e As KeyEventArgs) Handles TextEntry.KeyDown
            If e.KeyCode <> Keys.Escape Then Return
            DialogResult = DialogResult.Cancel
            e.Handled = True
        End Sub

#End Region

#Region "Initializers"

        Public Sub New(editor As CollectionEditor)
            MyBase.New(editor)
            InitializeComponent()
        End Sub

#End Region

#Region "Functions"

        '''<summary>Gets each item currently in the textbox as a string.</summary>
        '''<return>Each item currently in the textbox as a string.</return>
        Public Function GetTextItems() As String()
            Dim sLines As New List(Of String)
            For Each sLine As String In TextEntry.Text.Split(Environment.NewLine)
                If Not String.IsNullOrWhiteSpace(sLine) Then sLines.Add(sLine.Trim)
            Next
            Return sLines.ToArray
        End Function
        '''<summary>PRovides the form the ability to set the the output items when accepted and before setting the dialog result and closing.</summary>
        Protected Overridable Sub OnAccepted(e As CancelEventArgs)
            Dim sValues As New List(Of String)
            For Each sLine As String In TextEntry.Text.Split(Environment.NewLine)
                If Not String.IsNullOrWhiteSpace(sLine) Then sValues.Add(sLine.Trim)
            Next
            Items = sValues.ToArray
        End Sub
        Protected Overrides Sub OnEditValueChanged()
            TextEntry.Text = $"{ String.Join(Environment.NewLine, Items)}{Environment.NewLine}"
        End Sub

#End Region

    End Class

End Class
