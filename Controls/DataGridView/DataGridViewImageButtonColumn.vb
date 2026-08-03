Imports System.ComponentModel
Imports System.Drawing.Design

Public Class DataGridViewImageButtonColumn
    Inherits DataGridViewColumn

#Region "Private Items"

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
        Dim dgvibcTemplate As DataGridViewImageButtonCell = TryCast(CellTemplate, DataGridViewImageButtonCell)
        If dgvibcTemplate IsNot Nothing Then dgvibcTemplate.ImageList = Nothing
        If DataGridView Is Nothing Then Return
        For Each dgvrRow As DataGridViewRow In DataGridView.Rows
            Dim dgvibcCell As DataGridViewImageButtonCell = TryCast(dgvrRow.Cells(Index), DataGridViewImageButtonCell)
            If dgvibcCell IsNot Nothing Then dgvibcCell.ImageList = Nothing
        Next
    End Sub
    Private Sub ImageList_Recreated(sender As Object, e As EventArgs)
        Dim dgvibcTemplate As DataGridViewImageButtonCell = TryCast(CellTemplate, DataGridViewImageButtonCell)
        If dgvibcTemplate IsNot Nothing Then dgvibcTemplate.ImageList = ImageList
        If DataGridView Is Nothing Then Return
        For Each dgvrRow As DataGridViewRow In DataGridView.Rows
            Dim dgvibcCell As DataGridViewImageButtonCell = TryCast(dgvrRow.Cells(Index), DataGridViewImageButtonCell)
            If dgvibcCell IsNot Nothing Then dgvibcTemplate.ImageList = ImageList
        Next
    End Sub

#End Region

#Region "Initializers"

    '''<summary>Initializes a new instance of the <see cref="DataGridViewImageButtonColumn"/> class using an existing <see cref="DataGridViewImageButtonCell"/> as a template.</summary>
    Public Sub New()
        MyBase.New(New DataGridViewImageButtonCell)
        MyBase.SortMode = DataGridViewColumnSortMode.Automatic
    End Sub

#End Region

#Region "Properties"

    '''<summary>Gets or sets the image that is displayed on a button control.</summary>
    '''<returns>The <see cref="System.Drawing.Image"/> displayed on a button control.</returns>
    <Category("Appearance")>
    <Description("The image that will be displayed on the control.")>
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
            Dim dgvibcTemplate As DataGridViewImageButtonCell = TryCast(CellTemplate, DataGridViewImageButtonCell)
            If dgvibcTemplate IsNot Nothing Then dgvibcTemplate.Image = value
            If DataGridView Is Nothing Then Return
            For Each dgvrRow As DataGridViewRow In DataGridView.Rows
                Dim dgvibcCell As DataGridViewImageButtonCell = TryCast(dgvrRow.Cells(Index), DataGridViewImageButtonCell)
                If dgvibcCell IsNot Nothing Then dgvibcCell.Image = value
            Next
        End Set
    End Property
    Private Sub ResetImage()
        Image = Nothing
    End Sub
    Private Function ShouldSerializeImage() As Boolean
        Return Image IsNot Nothing
    End Function
    '''<summary>Gets or sets the alignment of the image on the button control.</summary>
    '''<returns>One of the <see cref="ContentAlignment"/> values. The default value is <see cref="ContentAlignment.MiddleCenter"/>.</returns>
    '''<exception cref="InvalidEnumArgumentException">The value assigned is not one of the <see cref="ContentAlignment"/> values.</exception>
    <Category("Appearance")>
    <DefaultValue(ContentAlignment.MiddleCenter)>
    <Description("The alignment of the image that will be displayed on the control.")>
    Public Property ImageAlign As ContentAlignment
        Get
            Return _imageAlign
        End Get
        Set(value As ContentAlignment)
            If value = _imageAlign Then Return
            _imageAlign = value
            Dim dgvibcTemplate As DataGridViewImageButtonCell = TryCast(CellTemplate, DataGridViewImageButtonCell)
            If dgvibcTemplate IsNot Nothing Then dgvibcTemplate.ImageAlign = value
            If DataGridView Is Nothing Then Return
            For Each dgvrRow As DataGridViewRow In DataGridView.Rows
                Dim dgvibcCell As DataGridViewImageButtonCell = TryCast(dgvrRow.Cells(Index), DataGridViewImageButtonCell)
                If dgvibcCell IsNot Nothing Then dgvibcCell.ImageAlign = value
            Next
        End Set
    End Property
    '''<summary>Gets or sets the image list index value of the image displayed on the button control.</summary>
    '''<returns>A zero-based index, which represents the image position in an <see cref="Windows.Forms.ImageList"/>. The default is -1.</returns>
    '''<exception cref="ArgumentOutOfRangeException">The assigned value is less than the lower bounds of the <see cref="ImageList"/>.</exception>
    <Category("Appearance")>
    <DefaultValue(-1)>
    <Description("The index of the image in the ImageList to display on the control.")>
    <Editor(GetType(ImageIndexEditor), GetType(UITypeEditor))>
    <TypeConverter(GetType(ImageIndexConverter))>
    Public Property ImageIndex As Integer
        Get
            'If returning default image, return default image. Otherwise, if ImageLIst is not defined, return the expected ImageIndex. Otherwise, if ImageList contains the ImageIndex, return the ImageIndex
            If _imageIndex = -1 OrElse _imageList Is Nothing OrElse _imageIndex < _imageList.Images.Count Then Return _imageIndex
            Return _imageList.Images.Count 'ImageIndex was not valid for the given ImageList, so return the last Image in the ImageList
        End Get
        Set(value As Integer)
            If value < -1 Then Throw New ArgumentOutOfRangeException()
            If value = _imageIndex AndAlso value <> -1 Then Return
            _imageIndex = value
            _imageKey = ""
            _imageListUsesIntegerIndex = True
            If value <> -1 Then _image = Nothing
            Dim dgvibcTemplate As DataGridViewImageButtonCell = TryCast(CellTemplate, DataGridViewImageButtonCell)
            If dgvibcTemplate IsNot Nothing Then dgvibcTemplate.ImageIndex = value
            If DataGridView Is Nothing Then Return
            For Each dgvrRow As DataGridViewRow In DataGridView.Rows
                Dim dgvibcCell As DataGridViewImageButtonCell = TryCast(dgvrRow.Cells(Index), DataGridViewImageButtonCell)
                If dgvibcCell IsNot Nothing Then dgvibcCell.ImageIndex = value
            Next
        End Set
    End Property
    '''<summary>Gets or sets the key accessor for the image in the <see cref="ImageList"/>.</summary>
    '''<returns>A string representing the key of the image.</returns>
    <Category("Appearance")>
    <Description("The index of the image in the ImageList to display on the control.")>
    <Editor(GetType(ImageIndexEditor), GetType(UITypeEditor))>
    <TypeConverter(GetType(ImageKeyConverter))>
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
            Dim dgvibcTemplate As DataGridViewImageButtonCell = TryCast(CellTemplate, DataGridViewImageButtonCell)
            If dgvibcTemplate IsNot Nothing Then dgvibcTemplate.ImageKey = value
            If DataGridView Is Nothing Then Return
            For Each dgvrRow As DataGridViewRow In DataGridView.Rows
                Dim dgvibcCell As DataGridViewImageButtonCell = TryCast(dgvrRow.Cells(Index), DataGridViewImageButtonCell)
                If dgvibcCell IsNot Nothing Then dgvibcCell.ImageKey = value
            Next
        End Set
    End Property
    Private Sub ResetImageKey()
        ImageKey = ""
    End Sub
    Private Function ShouldSerializeImageKey() As Boolean
        Return Not String.IsNullOrEmpty(_imageKey) AndAlso _imageKey <> "(none)"
    End Function
    '''<summary>Gets or sets the <see cref="Windows.Forms.ImageList"/> that contains the <see cref="System.Drawing.Image"/> displayed on a button control.</summary>
    '''<returns>A <see cref="Windows.Forms.ImageList"/>. The default value is null.</returns>
    <Category("Appearance")>
    <Description("The ImageList to get the image to display on the control.")>
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
            Dim dgvibcTemplate As DataGridViewImageButtonCell = TryCast(CellTemplate, DataGridViewImageButtonCell)
            If dgvibcTemplate IsNot Nothing Then dgvibcTemplate.ImageList = value
            If DataGridView Is Nothing Then Return
            For Each dgvrRow As DataGridViewRow In DataGridView.Rows
                Dim dgvibcCell As DataGridViewImageButtonCell = TryCast(dgvrRow.Cells(Index), DataGridViewImageButtonCell)
                If dgvibcCell IsNot Nothing Then dgvibcCell.ImageList = value
            Next
        End Set
    End Property
    Private Sub ResetImageList()
        ImageList = Nothing
    End Sub
    Private Function ShouldSerializeImageList() As Boolean
        Return _imageList IsNot Nothing
    End Function
    '''<summary>Gets or sets the maximum <see cref="Size"/> of the button control.</summary>
    '''<returns>The maximum <see cref="Size"/> of the button control.
    '''<para/>When set to (0, 0), the image will always fill the cell.</returns>
    <Category("Appearance")>
    <Description("The maximum size of the image that will be displayed on the control. When set to (0, 0), the image will always fill the cell.")>
    Public Property MaxImageSize As Size
        Get
            Return _maxImageSize
        End Get
        Set(value As Size)
            If value = _maxImageSize Then Return
            _maxImageSize = value
            Dim dgvibcTemplate As DataGridViewImageButtonCell = TryCast(CellTemplate, DataGridViewImageButtonCell)
            If dgvibcTemplate IsNot Nothing Then dgvibcTemplate.MaxImageSize = value
            If DataGridView Is Nothing Then Return
            For Each dgvrRow As DataGridViewRow In DataGridView.Rows
                Dim dgvibcCell As DataGridViewImageButtonCell = TryCast(dgvrRow.Cells(Index), DataGridViewImageButtonCell)
                If dgvibcCell IsNot Nothing Then dgvibcCell.MaxImageSize = value
            Next
        End Set
    End Property
    Private Sub ResetMaxImageSize()
        MaxImageSize = New Size(0, 0)
    End Sub
    Private Function ShouldSerializeMaxImageSize() As Boolean
        Return MaxImageSize.Width <> 0 OrElse MaxImageSize.Height <> 0
    End Function
    '''<summary>Gets or sets the sort mode for this column.</summary>
    '''<returns>A <see cref="DataGridViewColumnSortMode"/> that specifieds the criteria used to order the rows based on the cell values in a column.</returns>
    '''<exception cref="InvalidOperationException">The value assigned to the property conflicts with <see cref="DataGridViewSelectionMode"/>.</exception>
    <Category("Behavior")>
    <DefaultValue(DataGridViewColumnSortMode.Automatic)>
    <Description("The sort mode for the column.")>
    Public Overloads Property SortMode As DataGridViewColumnSortMode
        Get
            Return MyBase.SortMode
        End Get
        Set(value As DataGridViewColumnSortMode)
            MyBase.SortMode = value
        End Set
    End Property
    '''<summary>Gets or sets the alignment of the text on the button control.</summary>
    '''<returns>One of the <see cref="ContentAlignment"/> values. The default is <see cref="ContentAlignment.MiddleCenter"/>.</returns>
    '''<exception cref="InvalidEnumArgumentException">The value assigned is not one of the <see cref="ContentAlignment"/> values.</exception>
    <Category("Appearance")>
    <DefaultValue(ContentAlignment.MiddleCenter)>
    <Description("The alignment of the text that will be displayed on the control.")>
    Public Property TextAlign As ContentAlignment
        Get
            Return _textAlign
        End Get
        Set(value As ContentAlignment)
            If value = _textAlign Then Return
            _textAlign = value
            Dim dgvibcTemplate As DataGridViewImageButtonCell = TryCast(CellTemplate, DataGridViewImageButtonCell)
            If dgvibcTemplate IsNot Nothing Then dgvibcTemplate.TextAlign = value
            If DataGridView Is Nothing Then Return
            For Each dgvrRow As DataGridViewRow In DataGridView.Rows
                Dim dgvibcCell As DataGridViewImageButtonCell = TryCast(dgvrRow.Cells(Index), DataGridViewImageButtonCell)
                If dgvibcCell IsNot Nothing Then dgvibcCell.TextAlign = value
            Next
        End Set
    End Property
    '''<summary>Gets or sets the position of text and image relative to each other.</summary>
    '''<returns>One of the values of <see cref="Windows.Forms.TextImageRelation"/>. The default is <see cref="Windows.Forms.TextImageRelation.Overlay"/>.</returns>
    <Category("Apperance")>
    <DefaultValue(TextImageRelation.Overlay)>
    <Description("Specifies the relative location of the image to the text on the button.")>
    Public Property TextImageRelation As TextImageRelation
        Get
            Return _textImageRelation
        End Get
        Set(value As TextImageRelation)
            If value = _textImageRelation Then Return
            _textImageRelation = value
            Dim dgvibcTemplate As DataGridViewImageButtonCell = TryCast(CellTemplate, DataGridViewImageButtonCell)
            If dgvibcTemplate IsNot Nothing Then dgvibcTemplate.TextImageRelation = value
            If DataGridView Is Nothing Then Return
            For Each dgvrRow As DataGridViewRow In DataGridView.Rows
                Dim dgvibcCell As DataGridViewImageButtonCell = TryCast(dgvrRow.Cells(Index), DataGridViewImageButtonCell)
                If dgvibcCell IsNot Nothing Then dgvibcCell.TextImageRelation = value
            Next
        End Set
    End Property

#End Region

#Region "Functions"

    Public Overrides Function Clone() As Object
        Dim dgvibcClone As DataGridViewImageButtonColumn = MyBase.Clone
        dgvibcClone._image = _image
        dgvibcClone._imageAlign = _imageAlign
        dgvibcClone._imageIndex = _imageIndex
        dgvibcClone._imageKey = _imageKey
        dgvibcClone._imageList = _imageList
        dgvibcClone._imageListUsesIntegerIndex = _imageListUsesIntegerIndex
        dgvibcClone._maxImageSize = _maxImageSize
        dgvibcClone._textAlign = _textAlign
        dgvibcClone._textImageRelation = _textImageRelation
        dgvibcClone.CellTemplate = CType(CellTemplate, DataGridViewImageButtonCell).Clone
        dgvibcClone.SortMode = MyBase.SortMode
        Return dgvibcClone
    End Function
    Public Overrides Function ToString() As String
        Return $"DataGridViewImageButtonColumn {{ Name={Name}, Index={Index} }}"
    End Function

#End Region

End Class
