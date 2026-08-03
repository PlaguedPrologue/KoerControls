Imports System.ComponentModel
Imports System.Drawing.Design

Public Class ImageIndexEditor
    Inherits UITypeEditor

#Region "Private Items"

    Private _currentImageList As ImageList
    Private _currentInstance As Object
    Private ReadOnly _imageEditor As UITypeEditor = TryCast(TypeDescriptor.GetEditor(GetType(Image), GetType(UITypeEditor)), UITypeEditor)

#End Region

#Region "Protected Items"

    '''<summary>Gets the <see cref="Image"/> from the specified index in the specified context.</summary>
    '''<returns>The <see cref="Image"/> from the specified index in the specified context..</returns>
    '''<param name="context">The <see cref="ITypeDescriptorContext"/> containing the <see cref="ImageList"/>.</param>
    '''<param name="index">The index of the <see cref="Image"/> in the <see cref="ImageList"/>.</param>
    '''<param name="key">The index of the <see cref="Image"/> in the <see cref="ImageList"/>.</param>
    '''<param name="useIntIndex"><see langword="True"/> if searching by <paramref name="index"/>; otherwise <see langword="False"/> if searching by <paramref name="key"/>.</param>
    Protected Overridable Function GetImage(context As ITypeDescriptorContext, index As Integer, key As String, useIntIndex As Boolean) As Image
        Dim oInstance As Object = context.Instance
        If TypeOf oInstance Is Object() OrElse (index < 0 AndAlso String.IsNullOrEmpty(key)) Then Return Nothing
        If _currentImageList Is Nothing OrElse Not ReferenceEquals(oInstance, _currentInstance) Then
            _currentInstance = oInstance
            Dim pdImageListProperty As PropertyDescriptor = GetImageListProperty(context.PropertyDescriptor, oInstance)
            While oInstance IsNot Nothing AndAlso pdImageListProperty Is Nothing
                Dim pdProperties As PropertyDescriptorCollection = TypeDescriptor.GetProperties(oInstance)
                For Each pdProperty As PropertyDescriptor In pdProperties
                    If GetType(ImageList).IsAssignableFrom(pdProperty.PropertyType) Then
                        _currentImageList = TryCast(pdProperty.GetValue(oInstance), ImageList)
                        Exit For
                    End If
                Next
                If pdImageListProperty Is Nothing Then oInstance = pdProperties("Parent")?.GetValue(oInstance)
            End While
        End If
        If _currentImageList IsNot Nothing Then
            If useIntIndex Then
                If index < _currentImageList.Images.Count Then Return _currentImageList.Images.Item(index)
            Else
                Return _currentImageList.Images.Item(key)
            End If
        End If
        Return Nothing
    End Function
    '''<summary>Gets the <see cref="PropertyDescriptor"/> of type <see cref="ImageList"/> for the specified property in the specified object.</summary>
    '''<returns>The <see cref="PropertyDescriptor"/> of type <see cref="ImageList"/> for the specified property in the specified object.</returns>
    '''<param name="currentComponent">The property to search for an <see cref="ImageList"/> object in.</param>
    '''<param name="instance">The object to look for an <see cref="ImageList"/> in.</param>
    Protected Shared Function GetImageListProperty(currentComponent As PropertyDescriptor, ByRef instance As Object) As PropertyDescriptor
        If TypeOf instance Is Object() Then Return Nothing
        Dim rilaImageListAttribute As RelatedImageListAttribute = Nothing
        Dim aAttributes As AttributeCollection = currentComponent.Attributes
        For Each aAttribute As Attribute In aAttributes
            If TypeOf aAttribute Is RelatedImageListAttribute Then
                rilaImageListAttribute = aAttribute
                Exit For
            End If
        Next
        If rilaImageListAttribute Is Nothing Then Return Nothing

        Dim oParentInstance As Object = instance
        Dim sPathInfo() As String = rilaImageListAttribute.RelatedImageList.Split(".")
        For i As Integer = 0 To sPathInfo.Length - 1
            If oParentInstance Is Nothing Then
                Debug.Fail("A property specified in the path is null or not yet instantiated at this time..")
                Return Nothing
            End If
            Dim pdProperty As PropertyDescriptor = TypeDescriptor.GetProperties(oParentInstance)(sPathInfo(i))
            If pdProperty Is Nothing Then
                Debug.Fail("The path specified to the property is wrong.")
                Return Nothing
            End If
            If i = sPathInfo.Length - 1 Then
                If GetType(ImageList).IsAssignableFrom(pdProperty.PropertyType) Then
                    instance = oParentInstance
                    Return pdProperty
                End If
            Else
                oParentInstance = pdProperty.GetValue(oParentInstance)
            End If
        Next
        Return Nothing
    End Function
    Public Overrides Function GetPaintValueSupported(context As ITypeDescriptorContext) As Boolean
        If _imageEditor Is Nothing Then Return False
        Return _imageEditor.GetPaintValueSupported(context)
    End Function
    Public Overrides Sub PaintValue(e As PaintValueEventArgs)
        If _imageEditor Is Nothing Then Return
        If TypeOf e.Value Is Integer Then
            _imageEditor.PaintValue(New PaintValueEventArgs(e.Context, GetImage(e.Context, e.Value, Nothing, True), e.Graphics, e.Bounds))
        ElseIf TypeOf e.Value Is String Then
            _imageEditor.PaintValue(New PaintValueEventArgs(e.Context, GetImage(e.Context, -1, e.Value, False), e.Graphics, e.Bounds))
        End If
    End Sub

#End Region

End Class
