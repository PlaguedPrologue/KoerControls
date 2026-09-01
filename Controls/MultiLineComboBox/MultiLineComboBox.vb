Imports System.ComponentModel

Public Class MultiLineComboBox
    Inherits ComboBox

#Region "Private Items"

    Private _drawMode As DrawMode = DrawMode.Normal
    Private _linesPerItem As Integer = 1
    Private ReadOnly _textBox As New TextBox With {.Multiline = True}
    Private _zIndex As Integer

#End Region

#Region "Properties"

    '''<summary>Gets or sets a value indicating whether your code or the operating system will handle drawing of elements in the list.</summary>
    '''<returns>One of the <see cref="Windows.Forms.DrawMode"/> enumeration values. The default is <see cref="DrawMode.Normal"/>.</returns>
    <Category("Behavior")>
    <DefaultValue(DrawMode.Normal)>
    <Description("Indicates whether the code or the operating system will handle drawing of elements in the list.")>
    Public Overloads Property DrawMode As DrawMode
        Get
            Return _drawMode
        End Get
        Set(value As DrawMode)
            If _linesPerItem = 1 OrElse value <> DrawMode.Normal Then MyBase.DrawMode = value 'Multiple lines cannot have a normal draw mode
            _drawMode = value
        End Set
    End Property
    '''<summary>Gets or sets the font of the displayed control.</summary>
    '''<returns>The <see cref="System.Drawing.Font"/> to applly to the text displayed by the control. The default is the value of the <see cref="Control.DefaultFont"/> property.</returns>
    <Category("Appearance")>
    <Description("The font used to display text in the control.")>
    Public Overloads Property Font As Font
        Get
            Return MyBase.Font
        End Get
        Set(value As Font)
            MyBase.Font = value
            If DrawMode <> DrawMode.Normal Then Height = LinesPerItem * value.Height + 2
        End Set
    End Property
    Private Shadows Sub ResetFont()
        If Parent Is Nothing Then
            Font = DefaultFont
        Else
            Font = Parent.Font
        End If
    End Sub
    Private Function ShouldSerializeFont() As Boolean
        If Parent Is Nothing Then
            Return Not MyBase.Font.Equals(DefaultFont)
        Else
            Return Not MyBase.Font.Equals(Parent.Font)
        End If
    End Function
    '''<summary>Gets or sets the lines per item of the control.</summary>
    '''<returns>The lines per item of the control. The default is 1</returns>
    '''<exception cref="ArgumentOutOfRangeException">value is less than 1.</exception>
    <Category("Appearance")>
    <DefaultValue(1)>
    <Description("The lines per item in the combo box. The default is 1.")>
    Public Property LinesPerItem As Integer
        Get
            Return _linesPerItem
        End Get
        Set(value As Integer)
            If value = _linesPerItem Then Return
            If value <= 0 Then Throw New ArgumentOutOfRangeException
            _linesPerItem = value
            If value > 1 AndAlso MyBase.DrawMode = DrawMode.Normal Then MyBase.DrawMode = DrawMode.OwnerDrawFixed 'Multiple lines cannot have a normal draw mode
            ItemHeight = value * Font.Height + 2
        End Set
    End Property

#End Region

#Region "Functions"

    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle()
        Controls.Add(_textBox)
    End Sub
    Protected Overrides Sub OnDrawItem(e As DrawItemEventArgs)
        MyBase.OnDrawItem(e)
        If MyBase.DrawMode = DrawMode.OwnerDrawFixed AndAlso e.Index >= 0 Then
            Dim bInEditControl As Boolean = (e.State And DrawItemState.ComboBoxEdit) = DrawItemState.ComboBoxEdit
            Dim bSelected As Boolean = (e.State And DrawItemState.Selected) = DrawItemState.Selected

            If bSelected AndAlso Not bInEditControl Then
                Using bBush As New SolidBrush(SystemColors.Highlight)
                    e.Graphics.FillRectangle(bBush, e.Bounds)
                End Using
                Using bBrush As New SolidBrush(SystemColors.HighlightText)
                    Dim iItemLines As String() = DirectCast(Items(e.Index), String).Split({"\n", vbNewLine}, StringSplitOptions.None)
                    e.Graphics.DrawString(String.Join(vbNewLine, iItemLines.Take(Math.Min(iItemLines.Length, LinesPerItem))), e.Font, bBrush, New Rectangle(e.Bounds.X, e.Bounds.Y + 1, e.Bounds.Width, e.Bounds.Height))
                End Using
            Else
                Using bBush As New SolidBrush(BackColor)
                    e.Graphics.FillRectangle(bBush, e.Bounds)
                End Using
                Using bBrush As New SolidBrush(ForeColor)
                    Dim iItemLines As String() = DirectCast(Items(e.Index), String).Split({"\n", vbNewLine}, StringSplitOptions.None)
                    e.Graphics.DrawString(String.Join(vbNewLine, iItemLines.Take(Math.Min(iItemLines.Length, LinesPerItem))), e.Font, bBrush, New Rectangle(e.Bounds.X, e.Bounds.Y + 1, e.Bounds.Width, e.Bounds.Height))
                End Using
            End If

            If Not bInEditControl Then
                e.DrawFocusRectangle()
                If e.Index <> Items.Count - 1 AndAlso ItemHeight <> Font.Height + 2 Then
                    Using pPen As New Pen(SystemColors.WindowFrame)
                        e.Graphics.DrawLine(pPen, 0, e.Bounds.Y + e.Bounds.Height - 1, e.Bounds.Width, e.Bounds.Y + e.Bounds.Height - 1)
                    End Using
                End If
            End If
        End If
    End Sub
    Protected Overrides Sub OnDropDown(e As EventArgs)
        MyBase.OnDropDown(e)
        _zIndex = Parent?.Controls.IndexOf(Me)
        BringToFront()
    End Sub
    Protected Overrides Sub OnDropDownClosed(e As EventArgs)
        MyBase.OnDropDownClosed(e)
        If Parent IsNot Nothing Then Parent.Controls.SetChildIndex(Me, _zIndex)
        'Entier invoke required to make sure textbox is on top and the control is visually updated correctly.
        'I don't understand why it need to be done this exact way, so please don't change this unless you know what you are doing.
        BeginInvoke(Sub()
                        If Focused Then _textBox.Focus()
                        Text = _textBox.Text
                        _textBox.SelectionLength = _textBox.TextLength
                        SelectionLength = 0
                        Update()
                        OnTextChanged(e)
                    End Sub)
    End Sub
    Protected Overrides Sub OnFontChanged(e As EventArgs)
        MyBase.OnFontChanged(e)
        ItemHeight = _linesPerItem * Font.Height + 2
    End Sub
    Protected Overrides Sub OnGotFocus(e As EventArgs)
        MyBase.OnGotFocus(e)
        If Not DroppedDown Then _textBox.Select()
    End Sub
    Protected Overrides Sub OnSizeChanged(e As EventArgs)
        MyBase.OnSizeChanged(e)
        _textBox.Size = New Size(ClientSize.Width - SystemInformation.VerticalScrollBarWidth, ClientSize.Height)
    End Sub
    Protected Overrides Sub OnTextChanged(e As EventArgs)
        MyBase.OnTextChanged(e)
        _textBox.Text = Text
        _textBox.SelectionStart = SelectionStart
        _textBox.SelectionLength = SelectionLength
    End Sub

#End Region

End Class
