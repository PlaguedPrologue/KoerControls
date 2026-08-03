'''<summary>Displayts a prompt in a dialog box, waits for the user to enter text and click a button, then returns an <see cref="EditingDialogResult"/>.</summary>
Public Class EditingInputBox

#Region "Private Items"

    Private _acceptButtonType As EditingDialogResult
    Private _bottomButtonType As EditingDialogResult
    Private _middleButtonType As EditingDialogResult
    Private _result As EditingDialogResult
    Private _response As String
    Private _topButtonType As EditingDialogResult

    Private Sub BottomButton_Click(sender As Object, e As EventArgs) Handles BottomButton.Click
        _result = _bottomButtonType
        Close()
    End Sub
    Private Sub MiddleButton_Click(sender As Object, e As EventArgs) Handles MiddleButton.Click
        _result = _middleButtonType
        Close()
    End Sub
    Private Sub TopButton_Click(sender As Object, e As EventArgs) Handles TopButton.Click
        _result = _topButtonType
        Close()
    End Sub
    Private Sub ResponseText_KeyDown(sender As Object, e As KeyEventArgs) Handles ResponseText.KeyDown
        Select Case e.KeyCode
            Case Keys.Enter
                _result = _acceptButtonType
                Close()
            Case Keys.Escape
                Close()
        End Select
    End Sub
    Private Sub ResponseText_TextChanged(sender As Object, e As EventArgs) Handles ResponseText.TextChanged
        _response = ResponseText.Text
    End Sub

#End Region

#Region "Properties"

    '''<summary>Gets the entered response of the dialog box.</summary>
    '''<returns>The entered response of the dialog box.</returns>
    Public ReadOnly Property Response As String
        Get
            Return _response
        End Get
    End Property

#End Region

#Region "Functions"

    '''<summary>Shows the <see cref="EditingInputBox"/> as a modal dialog box.</summary>
    '''<returns>One of the <see cref="EditingDialogResult"/> values.</returns>
    '''<param name="prompt">The main text that appears inside the form.</param>
    '''<param name="title">The text that appears on the header of the form.</param>
    '''<param name="buttons">The <see cref="EditingMessageBoxButtons"/> to display.</param>
    Public Overloads Function ShowDialog(prompt As String, title As String, Optional buttons As EditingMessageBoxButtons = EditingMessageBoxButtons.OKCancel) As EditingDialogResult
        Return ShowDialog(prompt, title, "", buttons)
    End Function
    '''<summary>Shows the <see cref="EditingInputBox"/> as a modal dialog box.</summary>
    '''<returns>One of the <see cref="EditingDialogResult"/> values.</returns>
    '''<param name="prompt">The main text that appears inside the form.</param>
    '''<param name="title">The text that appears on the header of the form.</param>
    '''<param name="defaultResponse">The default text in the response field.</param>
    '''<param name="buttons">The <see cref="EditingMessageBoxButtons"/> to display.</param>
    Public Overloads Function ShowDialog(prompt As String, title As String, defaultResponse As String, Optional buttons As EditingMessageBoxButtons = EditingMessageBoxButtons.OKCancel) As EditingDialogResult
        _response = defaultResponse
        Me.Prompt.Text = prompt
        ResponseText.Text = defaultResponse
        Text = title
        Select Case buttons
            Case EditingMessageBoxButtons.OK
                _acceptButtonType = EditingDialogResult.OK
                _topButtonType = EditingDialogResult.OK
                _result = EditingDialogResult.OK
                AcceptButton = TopButton
                TopButton.Text = "OK"
                MiddleButton.Visible = False
                BottomButton.Visible = False
            Case EditingMessageBoxButtons.OKCancel
                _acceptButtonType = EditingDialogResult.OK
                _topButtonType = EditingDialogResult.OK
                _middleButtonType = EditingDialogResult.Cancel
                _result = EditingDialogResult.Cancel
                AcceptButton = TopButton
                CancelButton = MiddleButton
                TopButton.Text = "OK"
                MiddleButton.Text = "Cancel"
                BottomButton.Visible = False
            Case EditingMessageBoxButtons.AbortRetryIgnore
                _acceptButtonType = EditingDialogResult.Retry
                _topButtonType = EditingDialogResult.Abort
                _middleButtonType = EditingDialogResult.Retry
                _bottomButtonType = EditingDialogResult.Ignore
                _result = EditingDialogResult.Ignore
                AcceptButton = MiddleButton
                CancelButton = BottomButton
                TopButton.Text = "Abort"
                MiddleButton.Text = "Retry"
                BottomButton.Text = "Ignore"
            Case EditingMessageBoxButtons.YesNoCancel
                _acceptButtonType = EditingDialogResult.Yes
                _topButtonType = EditingDialogResult.Yes
                _middleButtonType = EditingDialogResult.No
                _bottomButtonType = EditingDialogResult.Cancel
                _result = EditingDialogResult.Cancel
                AcceptButton = TopButton
                CancelButton = BottomButton
                TopButton.Text = "Yes"
                MiddleButton.Text = "No"
                BottomButton.Text = "Cancel"
            Case EditingMessageBoxButtons.YesNo
                _acceptButtonType = EditingDialogResult.Yes
                _topButtonType = EditingDialogResult.Yes
                _middleButtonType = EditingDialogResult.No
                _result = EditingDialogResult.No
                AcceptButton = TopButton
                CancelButton = MiddleButton
                TopButton.Text = "Yes"
                MiddleButton.Text = "No"
                BottomButton.Visible = False
            Case EditingMessageBoxButtons.RetryCancel
                _acceptButtonType = EditingDialogResult.Retry
                _topButtonType = EditingDialogResult.Retry
                _middleButtonType = EditingDialogResult.Cancel
                _result = EditingDialogResult.Cancel
                AcceptButton = TopButton
                CancelButton = MiddleButton
                TopButton.Text = "Retry"
                MiddleButton.Text = "Cancel"
                BottomButton.Visible = False
            Case EditingMessageBoxButtons.OKCancelDelete
                _acceptButtonType = EditingDialogResult.OK
                _topButtonType = EditingDialogResult.OK
                _middleButtonType = EditingDialogResult.Cancel
                _bottomButtonType = EditingDialogResult.Delete
                _result = EditingDialogResult.Cancel
                AcceptButton = TopButton
                CancelButton = MiddleButton
                TopButton.Text = "OK"
                MiddleButton.Text = "Cancel"
                BottomButton.Text = "Delete"
        End Select

        ShowDialog()
        Return _result
    End Function

#End Region

End Class
