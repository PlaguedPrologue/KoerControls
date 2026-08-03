Imports System.ComponentModel

'''<summary>Represents a standard Windows label. Can be set to automatically resize the hieght and change new line characters to fit the available width.</summary>
<Designer(GetType(AutoSizeLabelDesigner))>
Public Class AutoSizeLabel
    Inherits Label

#Region "Private Items"

    Private _autoSize As Boolean = True
    Private _text As String

#End Region

#Region "Properties"

    '''<summary>Determines if this control auto sizes to allow its text to fill the available width.</summary>
    '''<returns><see langword="True"/> if this control auto sizes to allow its text to fill the available width; otherwise, <see langword="False"/>.</returns>
    <DefaultValue(True)>
    Public Overloads Property AutoSize As Boolean 'Hide the base AutoSize property and leave it as 'False'. This allows the designer to always be able to adjust the size as the AutoResize allows.
        Get
            Return _autoSize
        End Get
        Set(value As Boolean)
            If value = _autoSize Then Return
            _autoSize = value
            If value Then Resize()
        End Set
    End Property
    '''<summary>Gets or sets the text associated with this control.</summary>
    '''<returns>The text associated with this control.</returns>
    <Category("Appearance")>
    <DefaultValue("")>
    <Description("The text associated with this control.")>
    Public Overrides Property Text As String
        Get
            Return _text
        End Get
        Set(value As String)
            If value = _text Then Return
            _text = value
            If _autoSize Then
                Resize()
            Else
                MyBase.Text = value
            End If
        End Set
    End Property

#End Region

#Region "Functions"

    Protected Overrides Sub OnHandleCreated(e As EventArgs)
        MyBase.OnHandleCreated(e)
        MyBase.AutoSize = False
        If _autoSize Then Resize()
    End Sub
    Protected Overrides Sub OnSizeChanged(e As EventArgs)
        MyBase.OnSizeChanged(e)
        If _autoSize Then Resize()
    End Sub
    '''<summary>Reiszes the control auto resizes to allow its text to fill the available width.</summary>
    Protected Overridable Shadows Sub Resize()
        Try
            Dim sNewLines As IEnumerable(Of String)
            Dim sNewText As String
            Dim iNewHeight As Integer

            'Resize when parent is available and dock set
            If Parent IsNot Nothing Then
                Select Case Dock
                    Case DockStyle.Top, DockStyle.Bottom
                        sNewLines = Font.GetLinesForWidth(_text, ClientSize.Width - Padding.Horizontal) 'Get new lines for the text 
                        sNewText = String.Join(vbNewLine, sNewLines) 'Get new text 
                        If MyBase.Text <> sNewText Then MyBase.Text = sNewText 'Replace text if necessary
                        iNewHeight = Font.Height * sNewLines.Count + Padding.Vertical 'Get new height
                        Dim iNewWidth As Integer = Parent.ClientSize.Width - Parent.Padding.Horizontal
                        If Height <> iNewHeight OrElse Width <> iNewWidth Then Size = New Size(iNewWidth, iNewHeight) 'Set size if necessary
                        Return
                    Case DockStyle.Left, DockStyle.Right
                        sNewText = String.Join(vbNewLine, Font.GetLinesForWidth(_text, ClientSize.Width)) 'Get new text
                        If MyBase.Text <> sNewText Then MyBase.Text = sNewText 'Replace text if necessary
                        Return
                End Select
            End If

            'Resize when parent not available or dock not set
            sNewLines = Font.GetLinesForWidth(_text, ClientSize.Width) 'Get new lines for the text
            sNewText = String.Join(vbNewLine, sNewLines) 'Get new text
            If MyBase.Text <> sNewText Then MyBase.Text = sNewText 'Replace text if necessary
            iNewHeight = Font.Height * sNewLines.Count + Padding.Vertical 'Get new height 
            If Height <> iNewHeight Then Height = iNewHeight 'Set height if necessary
        Catch ex As Exception
            Throw New Exception(ex.Message, ex)
        End Try
    End Sub

#End Region

End Class
