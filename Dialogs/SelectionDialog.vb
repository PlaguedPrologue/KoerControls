'''<summary>Displays a a prompt in a dialog box, waits for the user to select an item from a list and click a button, then returns a <see cref="Windows.Forms.DialogResult"/>.</summary>
Public Class SelectionDialog

#Region "Private Items"

    Private _result As DialogResult = DialogResult.Cancel
    Private _response As Object

    Private Sub OK_Click(sender As Object, e As EventArgs) Handles OK.Click
        _result = DialogResult.OK
        _response = If(ResponseText.SelectedItem, ResponseText.Text)
        Close()
    End Sub

#End Region

#Region "Properties"

    '''<summary>Gets the entered response of the dialog box.</summary>
    '''<returns>The entered response of the dialog box.</returns>
    Public ReadOnly Property Response As Object
        Get
            Return _response
        End Get
    End Property

#End Region

#Region "Functions"

    '''<summary>Shows the <see cref="SelectionDialog"/> as a modal dialog box.</summary>
    '''<returns>One of the <see cref="Windows.Forms.DialogResult"/> values.</returns>
    '''<param name="prompt">The main text that appears inside the form.</param>
    '''<param name="title">The text that appears on the header of the form.</param>
    '''<param name="items">the items to select from in the response.</param> 
    '''<param name="linesPerItem">The total lines to display in the selection.</param>
    Public Overloads Function ShowDialog(items As IEnumerable, prompt As String, title As String, Optional linesPerItem As Integer = 1) As DialogResult
        Return ShowDialog(items, prompt, title, Nothing, ComboBoxStyle.DropDownList, linesPerItem)
    End Function
    '''<summary>Shows the <see cref="SelectionDialog"/> as a modal dialog box.</summary>
    '''<returns>One of the <see cref="Windows.Forms.DialogResult"/> values.</returns>
    '''<param name="prompt">The main text that appears inside the form.</param>
    '''<param name="title">The text that appears on the header of the form.</param>
    '''<param name="items">the items to select from in the response.</param> 
    '''<param name="defaultResponse">The default text in the response field.</param>
    '''<param name="linesPerItem">The total lines to display in the selection.</param>
    Public Overloads Function ShowDialog(items As IEnumerable, prompt As String, title As String, defaultResponse As Object, Optional linesPerItem As Integer = 1) As DialogResult
        Return ShowDialog(items, prompt, title, defaultResponse, ComboBoxStyle.DropDownList, linesPerItem)
    End Function
    '''<summary>Shows the <see cref="SelectionDialog"/> as a modal dialog box.</summary>
    '''<returns>One of the <see cref="Windows.Forms.DialogResult"/> values.</returns>
    '''<param name="prompt">The main text that appears inside the form.</param>
    '''<param name="title">The text that appears on the header of the form.</param>
    '''<param name="items">the items to select from in the response.</param> 
    '''<param name="dropDownStyle">The <see cref="ComboBoxStyle"/> to apply to the selection.</param>
    '''<param name="linesPerItem">The total lines to display in the selection.</param>
    Public Overloads Function ShowDialog(items As IEnumerable, prompt As String, title As String, dropDownStyle As ComboBoxStyle, Optional linesPerItem As Integer = 1) As DialogResult
        Return ShowDialog(items, prompt, title, Nothing, dropDownStyle, linesPerItem)
    End Function
    '''<summary>Shows the <see cref="SelectionDialog"/> as a modal dialog box.</summary>
    '''<returns>One of the <see cref="Windows.Forms.DialogResult"/> values.</returns>
    '''<param name="prompt">The main text that appears inside the form.</param>
    '''<param name="title">The text that appears on the header of the form.</param>
    '''<param name="items">the items to select from in the response.</param> 
    '''<param name="defaultResponse">The default text in the response field.</param>
    '''<param name="dropDownStyle">The <see cref="ComboBoxStyle"/> to apply to the drop down style of the selection.</param>
    '''<param name="linesPerItem">The total lines to display in the selection.</param>
    Public Overloads Function ShowDialog(items As IEnumerable, prompt As String, title As String, defaultResponse As Object, dropDownStyle As ComboBoxStyle, Optional linesPerItem As Integer = 1) As DialogResult
        Height += (linesPerItem - 1) * ResponseText.Font.Height
        Me.Prompt.Text = prompt
        ResponseText.DropDownStyle = dropDownStyle
        ResponseText.Items.AddRange(items)
        ResponseText.LinesPerItem = linesPerItem
        ResponseText.SelectedItem = defaultResponse
        Text = title

        ShowDialog()
        Return _result
    End Function

#End Region

End Class
