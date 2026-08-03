Imports System.ComponentModel

Public Class DataGridViewImageButtonCell
    Inherits DataGridViewButtonCell

#Region "Private Items"

    Private _buttonHighlighted As Boolean = False
    Private _cellBounds As Rectangle
    Private _contentBounds As Rectangle
    Private _image As Image
    Private _imageAlign As ContentAlignment = ContentAlignment.MiddleCenter
    Private _imageIndex As Integer = -1
    Private _imageKey As String = ""
    Private _imageList As ImageList
    Private _imageListUsesIntegerIndex As Boolean
    Private _maxImageSize As New Size(0, 0)
    Private _textAlign As ContentAlignment = ContentAlignment.MiddleCenter
    Private _textImageRelation As TextImageRelation = TextImageRelation.Overlay

    Private Sub ImageList_Disposed(sender As Object, e As EventArgs)
        ImageList = Nothing
    End Sub
    Private Sub ImageList_Recreated(sender As Object, e As EventArgs)
        ReCalculateContentBounds()
        DataGridView?.InvalidateCell(Me)
    End Sub
    Private Sub ReCalculateContentBounds()
        If DataGridView Is Nothing Then Return

        Dim iCellWidth As Integer = _cellBounds.Width
        If iCellWidth = 0 Then Return
        Dim iCellHeight As Integer = _cellBounds.Height

        Dim sImageSize As Size = If(Image?.Size, _cellBounds.Size)
        Dim iImageWidth As Integer = sImageSize.Width
        Dim iImageHeight As Integer = sImageSize.Height
        Dim dImageScale As Double = Math.Min(If(_maxImageSize.Width = 0, iCellWidth, Math.Min(_maxImageSize.Width, iCellWidth)) / iImageWidth, If(_maxImageSize.Height = 0, iCellHeight, Math.Min(_maxImageSize.Height, iCellHeight)) / iImageHeight)

        Dim iContentWidthOffset As Integer = iCellWidth - iImageWidth * dImageScale
        Dim iContentHeightOffset As Integer = iCellHeight - iImageHeight * dImageScale
        Dim iContentXPadding As Integer = iContentWidthOffset \ 2
        Dim iContentYPadding As Integer = iContentHeightOffset \ 2
        _contentBounds = New Rectangle(_cellBounds.X + iContentXPadding - 1, _cellBounds.Y + iContentYPadding - 1, iCellWidth - iContentWidthOffset, iCellHeight - iContentHeightOffset)
    End Sub

#End Region

#Region "Properties"

    '''<summary>Gets the bounding rectangle that encloses the cell's content area.</summary>
    '''<returns>The <see cref="Rectangle"/> that bounds the cell's contents.</returns>
    <Browsable(False)>
    Public Overloads ReadOnly Property ContentBounds As Rectangle
        Get
            Return _contentBounds
        End Get
    End Property
    '''<summary>Gets or sets the image that is displayed on a button control.</summary>
    '''<returns>The <see cref="System.Drawing.Image"/> displayed on a button control.</returns>
    Public Property Image As Image
        Get
            If _image IsNot Nothing OrElse _imageList Is Nothing Then Return _image
            If _imageListUsesIntegerIndex Then
                If _imageIndex < 0 Then Return _image
                If _imageIndex < _imageList.Images.Count Then Return _imageList.Images(_imageIndex)
                Return _imageList.Images(_imageList.Images.Count - 1)
            End If
            If _imageList.Images.IndexOfKey(_imageKey) < 0 Then Return Nothing
            Return _imageList.Images.Item(_imageKey)
        End Get
        Set(value As Image)
            If Equals(value, _image) Then Return
            _image = value
            If value IsNot Nothing Then
                _imageIndex = -1
                If _imageList IsNot Nothing Then
                    RemoveHandler _imageList.Disposed, AddressOf ImageList_Disposed
                    RemoveHandler _imageList.RecreateHandle, AddressOf ImageList_Recreated
                    _imageList = Nothing
                End If
            End If
            ReCalculateContentBounds()
            DataGridView?.InvalidateCell(Me)
        End Set
    End Property
    '''<summary>Gets or sets the alignment of the image on the button control.</summary>
    '''<returns>One of the <see cref="ContentAlignment"/> values. The default value is <see cref="ContentAlignment.MiddleCenter"/>.</returns>
    '''<exception cref="InvalidEnumArgumentException">The value assigned is not one of the <see cref="ContentAlignment"/> values.</exception>
    Public Property ImageAlign As ContentAlignment
        Get
            Return _imageAlign
        End Get
        Set(value As ContentAlignment)
            If value = _imageAlign Then Return
            _imageAlign = value
            DataGridView?.InvalidateCell(Me)
        End Set
    End Property
    '''<summary>Gets or sets the image list index value of the image displayed on the button control.</summary>
    '''<returns>A zero-based index, which represents the image position in an <see cref="Windows.Forms.ImageList"/>. The default is -1.</returns>
    '''<exception cref="ArgumentOutOfRangeException">The assigned value is less than the lower bounds of the <see cref="ImageList"/>.</exception>
    Public Property ImageIndex As Integer
        Get
            'If returning default image, return default image. Otherwise, if ImageLIst is not defined, return the expected ImageIndex. Otherwise, if ImageList contains the ImageIndex, return the ImageIndex
            If _imageIndex = -1 OrElse _imageList Is Nothing OrElse _imageIndex < _imageList.Images.Count Then Return _imageIndex
            Return _imageList?.Images.Count - 1 'ImageIndex was not valid for the given ImageList, so return the last Image in the ImageList
        End Get
        Set(value As Integer)
            If value < -1 Then Throw New ArgumentOutOfRangeException()
            If value = _imageIndex AndAlso value <> -1 Then Return
            _imageIndex = value
            _imageKey = ""
            _imageListUsesIntegerIndex = True
            If value <> -1 Then _image = Nothing
            ReCalculateContentBounds()
            DataGridView?.InvalidateCell(Me)
        End Set
    End Property
    '''<summary>Gets or sets the key accessor for the image in the <see cref="ImageList"/>.</summary>
    '''<returns>A string representing the key of the image.</returns>
    Public Property ImageKey As String
        Get
            Return _imageKey
        End Get
        Set(value As String)
            If value = _imageKey AndAlso Not String.IsNullOrEmpty(value) Then Return
            _imageKey = value
            _imageIndex = -1
            _imageListUsesIntegerIndex = False
            If value IsNot Nothing Then _image = Nothing
            ReCalculateContentBounds()
            DataGridView?.InvalidateCell(Me)
        End Set
    End Property
    '''<summary>Gets or sets the <see cref="Windows.Forms.ImageList"/> that contains the <see cref="System.Drawing.Image"/> displayed on a button control.</summary>
    '''<returns>A <see cref="Windows.Forms.ImageList"/>. The default value is null.</returns>
    Public Property ImageList As ImageList
        Get
            Return _imageList
        End Get
        Set(value As ImageList)
            If ReferenceEquals(value, _imageList) Then Return
            If _imageList IsNot Nothing Then
                RemoveHandler _imageList.Disposed, AddressOf ImageList_Disposed
                RemoveHandler _imageList.RecreateHandle, AddressOf ImageList_Recreated
            End If
            _imageList = value
            _imageIndex = -1
            _imageKey = ""
            If value IsNot Nothing Then
                _image = Nothing
                AddHandler value.Disposed, AddressOf ImageList_Disposed
                AddHandler value.RecreateHandle, AddressOf ImageList_Recreated
            End If
            ReCalculateContentBounds()
            DataGridView?.InvalidateCell(Me)
        End Set
    End Property
    '''<summary>Gets or sets the maximum <see cref="System.Drawing.Size"/> of the button control.</summary>
    '''<returns>The maximum <see cref="System.Drawing.Size"/> of the button control.
    '''<para/>When set to (0, 0), the image will always fill the cell.</returns>
    Public Property MaxImageSize As Size
        Get
            Return _maxImageSize
        End Get
        Set(value As Size)
            If value = _maxImageSize Then Return
            _maxImageSize = value
            ReCalculateContentBounds()
            DataGridView?.InvalidateCell(Me)
        End Set
    End Property
    '''<summary>Gets or sets the alignment of the text on the button control.</summary>
    '''<returns>One of the <see cref="ContentAlignment"/> values. The default is <see cref="ContentAlignment.MiddleCenter"/>.</returns>
    '''<exception cref="InvalidEnumArgumentException">The value assigned is not one of the <see cref="ContentAlignment"/> values.</exception>
    Public Property TextAlign As ContentAlignment
        Get
            Return _textAlign
        End Get
        Set(value As ContentAlignment)
            If value = _textAlign Then Return
            _textAlign = value
            DataGridView?.InvalidateCell(Me)
        End Set
    End Property
    '''<summary>Gets or sets the position of text and image relative to each other.</summary>
    '''<returns>One of the values of <see cref="Windows.Forms.TextImageRelation"/>. The default is <see cref="Windows.Forms.TextImageRelation.Overlay"/>.</returns>
    Public Property TextImageRelation As TextImageRelation
        Get
            Return _textImageRelation
        End Get
        Set(value As TextImageRelation)
            If value = _textImageRelation Then Return
            _textImageRelation = value
            DataGridView?.InvalidateCell(Me)
        End Set
    End Property
    '''<summary>Gets or sets the value associated with this cell.</summary>
    '''<returns>Gets or sets the data to be displayed by the cell. The default is -1/>.</returns>
    '''<exception cref="ArgumentOutOfRangeException"><see cref="RowIndex"/> is outside the valid range of 0 to the number of rows in the control minus 1.</exception>
    '''<exception cref="InvalidOperationException"><see cref="ColumnIndex"/> is less than 0, indicating that the cell is a row header cell.</exception>
    Public Overloads Property Value As Object
        Get
            'If returning default image, return default image. Otherwise, if ImageLIst is not defined, return the expected ImageIndex. Otherwise, if ImageList contains the ImageIndex, return the ImageIndex
            If _imageIndex = -1 OrElse _imageList Is Nothing OrElse _imageIndex < _imageList.Images.Count Then Return _imageIndex
            Return _imageList?.Images.Count - 1 'ImageIndex was not valid for the given ImageList, so return the last Image in the ImageList
        End Get
        Set(value As Object)
            If value < -1 Then Throw New ArgumentOutOfRangeException()
            If value = _imageIndex AndAlso value <> -1 Then Return
            _imageIndex = value
            _imageKey = ""
            _imageListUsesIntegerIndex = True
            If value <> -1 Then _image = Nothing
            ReCalculateContentBounds()
            DataGridView?.InvalidateCell(Me)
        End Set
    End Property
    '''<summary>Gets or sets the <see cref="Type"/> of value assigned to the control.</summary>
    '''<returns>The <see cref="Type"/> of value assigned to the control.</returns>
    Public Overrides ReadOnly Property ValueType As Type
        Get
            Return GetType(Integer)
        End Get
    End Property

#End Region

#Region "Functions"

    Public Overrides Function Clone() As Object
        Dim dgvibcClone As DataGridViewImageButtonCell = MyBase.Clone
        dgvibcClone._cellBounds = _cellBounds
        dgvibcClone._contentBounds = _contentBounds
        dgvibcClone._image = _image
        dgvibcClone._imageAlign = _imageAlign
        dgvibcClone._imageIndex = _imageIndex
        dgvibcClone._imageKey = _imageKey
        dgvibcClone._imageList = _imageList
        dgvibcClone._imageListUsesIntegerIndex = _imageListUsesIntegerIndex
        dgvibcClone._maxImageSize = _maxImageSize
        dgvibcClone._textAlign = _textAlign
        dgvibcClone._textImageRelation = _textImageRelation
        Return dgvibcClone
    End Function
    Protected Overrides Function GetValue(rowIndex As Integer) As Object
        'If returning default image, return default image. Otherwise, if ImageLIst is not defined, return the expected ImageIndex. Otherwise, if ImageList contains the ImageIndex, return the ImageIndex
        If _imageIndex = -1 OrElse _imageList Is Nothing OrElse _imageIndex < _imageList.Images.Count Then Return _imageIndex
        Return _imageList?.Images.Count - 1 'ImageIndex was not valid for the given ImageList, so return the last Image in the ImageList
    End Function
    Protected Overrides Sub OnContentDoubleClick(e As DataGridViewCellEventArgs)
        If Not _buttonHighlighted Then Return
        MyBase.OnContentDoubleClick(e)
    End Sub
    Protected Overrides Sub OnMouseEnter(rowIndex As Integer)
        MyBase.OnMouseEnter(rowIndex)
        'Highlight button if entering on button
        If Not _contentBounds.Contains(DataGridView.PointToClient(Cursor.Position)) Then Return
        _buttonHighlighted = True
        DataGridView.InvalidateCell(Me)
    End Sub
    Protected Overrides Sub OnMouseLeave(rowIndex As Integer)
        MyBase.OnMouseLeave(rowIndex)
        'Unhighlight button if leaving from button
        If Not _buttonHighlighted Then Return
        _buttonHighlighted = False
        DataGridView.InvalidateCell(Me)
    End Sub
    Protected Overrides Sub OnMouseMove(e As DataGridViewCellMouseEventArgs)
        MyBase.OnMouseMove(e)
        'Switch button highlight state if entering or leaving button while still in cell
        If _contentBounds.Contains(DataGridView.PointToClient(Cursor.Position)) = _buttonHighlighted Then Return
        _buttonHighlighted = Not _buttonHighlighted
        DataGridView.InvalidateCell(Me)
    End Sub
    Protected Overrides Sub Paint(graphics As Graphics, clipBounds As Rectangle, cellBounds As Rectangle, rowIndex As Integer, elementState As DataGridViewElementStates, value As Object, formattedValue As Object, errorText As String, cellStyle As DataGridViewCellStyle, advancedBorderStyle As DataGridViewAdvancedBorderStyle, paintParts As DataGridViewPaintParts)
        MyBase.Paint(graphics, clipBounds, cellBounds, rowIndex, elementState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts)
        If OwningColumn?.Site IsNot Nothing AndAlso OwningColumn.Site.DesignMode Then Return
        If _cellBounds <> cellBounds Then
            _cellBounds = cellBounds
            ReCalculateContentBounds()
        End If

        Using bBrush As New SolidBrush(GetDisplayedBackColor)
            graphics.FillRectangle(bBrush, _cellBounds)
        End Using
        graphics.DrawRectangle(SystemPens.ActiveBorder, _cellBounds.X - 1, _cellBounds.Y - 1, _cellBounds.Width, _cellBounds.Height)
        If DataGridView.AllowUserToAddRows AndAlso rowIndex = DataGridView.NewRowIndex Then Return 'Don't paint button if button is on new 'adding' row in data grid view
        If _buttonHighlighted Then
            graphics.FillRectangle(SystemBrushes.GradientActiveCaption, _contentBounds)
        Else
            graphics.FillRectangle(SystemBrushes.ButtonFace, _contentBounds)
        End If
        Dim iWidth As Integer = _contentBounds.Width
        If iWidth > 1 Then
            Dim iHeight As Integer = _contentBounds.Height
            If iHeight > 1 Then
                Dim iImage As Image = Image
                If iImage IsNot Nothing Then
                    graphics.DrawImage(iImage, New Rectangle(_contentBounds.X + 1, _contentBounds.Y + 1, iWidth - 1, iHeight - 1))
                End If
            End If
        End If
        If _buttonHighlighted Then
            graphics.DrawRectangle(SystemPens.ControlDarkDark, _contentBounds)
        Else
            graphics.DrawRectangle(SystemPens.ActiveBorder, _contentBounds)
        End If
    End Sub

#End Region

End Class
