Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Drawing.Text
Imports System.Runtime.InteropServices
Imports System.Windows.Forms.Design.Behavior

Public Class Overlay
    Inherits NativeWindow

#Region "Private Items"

    Private _height As Integer
    'Private _hostBorderStyle As BorderStyle
    Private _left As Integer
    Private _minimumHeight As Integer
    Private _minimumWidth As Integer
    Private _top As Integer
    Private _width As Integer

#End Region

#Region "Properties"

    '''<summary>Gets the handle for this window.</summary>
    '''<returns>If successful, an <see cref="IntPtr"/> representing the handle to the associated <see cref="Overlay"/>; otherwise, 0 if no handle is associated with the overlay.</returns>
    <Browsable(False)>
    Public Overloads ReadOnly Property Handle As IntPtr
        Get
            Return MyBase.Handle
        End Get
    End Property
    '''<summary>The hosting <see cref="Control"/> the <see cref="Overlay"/> is contained within.</summary>
    <Browsable(False)>
    Public ReadOnly Property Host As Control
    '''<summary>Gets or sets the coordinates of the upper-left corner of the overlay relative to the upper-left corner of its host.</summary>
    '''<returns>The <see cref="Point"/> that represents the upper-left corner of the overlay relative to the upper-left corner of its host.</returns>
    Public Property Location As Point
        Get
            Return New Point(_left, _top)
        End Get
        Set(value As Point)
            Dim sHostClientSize As Size = Host.Size
            Dim iHostClientWidth As Integer = sHostClientSize.Width
            Dim iLeft As Integer = Math.Max(0, Math.Min(value.X, iHostClientWidth)) '0 <= value.X <= HostWidth
            Dim iHostClientHeight As Integer = sHostClientSize.Height
            Dim iTop As Integer = Math.Max(0, Math.Min(value.Y, iHostClientHeight)) '0 <= value.Y <= HostHeight
            If iLeft = _left AndAlso iTop = _top Then Return

            _left = iLeft
            _top = iTop
            PlaceOverlay()
        End Set
    End Property
    '''<summary>Gets or sets the size that is the lower limit of the <see cref="Overlay"/> can specify so long as it fits within the bounds of the host.</summary>
    '''<returns>An ordered pair of <see cref="System.Drawing.Size"/> that representing the width and height of a rectangle.</returns>
    Public Property MinimumSize As Size
        Get
            Return New Size(_minimumWidth, _minimumHeight)
        End Get
        Set(value As Size)
            Dim iMinimumWidth As Integer = value.Width
            Dim iMinimumHeight As Integer = value.Height
            If iMinimumWidth = _minimumWidth AndAlso iMinimumHeight = _minimumHeight Then Return

            _minimumWidth = iMinimumWidth
            _minimumHeight = iMinimumHeight

            If _width < iMinimumWidth OrElse _height < iMinimumHeight Then
                _width = Math.Max(_width, iMinimumWidth)
                _height = Math.Max(_height, iMinimumHeight)
                PlaceOverlay()
            End If
        End Set
    End Property
    '''<summary>Gets or sets the height and width of the overlay.</summary>
    '''<returns>The <see cref="System.Drawing.Size"/> that represents the height and width of the overlay in pixels.</returns>
    Public Property Size As Size
        Get
            Return New Size(_width, _height)
        End Get
        Set(value As Size)
            Dim sHostClientSize As Size = Host.ClientSize
            Dim iWidth As Integer = Math.Max(value.Width, _minimumWidth)
            Dim iHeight As Integer = Math.Max(value.Height, _minimumHeight)
            If iWidth = _width AndAlso iHeight = _height Then Return

            _width = iWidth
            _height = iHeight
            PlaceOverlay()
        End Set
    End Property

#End Region

#Region "Initializers"

    Private Const WS_CHILD As Integer = &H40000000
    Private Const WS_CLIPSIBLINGS As Integer = &H4000000
    Private Const WS_EX_NOACTIVATE As Integer = &H8000000
    Private Const WS_VISIBLE As Integer = &H10000000

    '''<summary>Creates a new instance of <see cref="Overlay"/>.</summary>
    '''<param name="host">The hosting <see cref="Control"/> the <see cref="Overlay"/> is in.</param>
    '''<param name="paintAction">The <see cref="Action"/> used top paint the header overlay, defined by the host.</param>
    '''<exception cref="ArgumentNullException"><paramref name="host"/> is <see langword="Nothing"/> -OR- <paramref name="paintAction"/> is <see langword="Nothing"/>.</exception>
    Public Sub New(host As Control, paintAction As Action(Of Graphics, Rectangle))
        MyBase.New

        If host Is Nothing Then Throw New ArgumentNullException(NameOf(host))
        If paintAction Is Nothing Then Throw New ArgumentNullException(NameOf(paintAction))

        '_hostBorderStyle = If(TryCast(host, Panel)?.BorderStyle, BorderStyle.None)
        _paintAction = paintAction
        Me.Host = host

        'AddHandler host.ClientSizeChanged, AddressOf HostClientSizeChanged
        AddHandler host.ControlAdded, AddressOf HostControlAdded
        AddHandler host.Disposed, AddressOf HostDisposed
        AddHandler host.LocationChanged, AddressOf HostLocationChanged
        'If TypeOf host Is Panel Then AddHandler host.StyleChanged, AddressOf HostStyleChanged
    End Sub

    '''<summary>Attaches the <see cref="Overlay"/> to its  host <see cref="Control"/>.</summary>
    '''<exception cref="InvalidOperationException">The host is not hosted on a <see cref="Form"/> while attaching.</exception>
    Public Sub Attach()
        If Host.FindForm Is Nothing Then Throw New InvalidOperationException("Overlay parent Control must be hosted on a Form before the overlay can be attached.")
        CreateHandle()
    End Sub
    '''<summary>Attaches a new <see cref="Overlay"/> to the specified host <see cref="Control"/>.</summary>
    '''<remarks>Place in the <see cref="Control.OnHandleCreated"/> override, after the handle has been created for the host.</remarks>
    '''<returns>The new <see cref="Overlay"/>.</returns>
    '''<param name="host">The <see cref="Control"/> to attach the new <see cref="Overlay"/> to.</param>
    '''<param name="paintAction">The paint <see cref="Action"/> that will paint the header on the <see cref="Control"/>.</param>
    '''<exception cref="ArgumentNullException"><paramref name="host"/> is <see langword="Nothing"/> -OR- <paramref name="paintAction"/> is <see langword="Nothing"/>.</exception>
    '''<exception cref="InvalidOperationException">The <paramref name="host"/> is not hosted on a <see cref="Form"/> while attaching.</exception>
    Public Shared Function Attach(host As Control, paintAction As Action(Of Graphics, Rectangle)) As Overlay
        If host Is Nothing Then Throw New ArgumentNullException(NameOf(host))
        If paintAction Is Nothing Then Throw New ArgumentNullException(NameOf(paintAction))
        If host.FindForm Is Nothing Then Throw New InvalidOperationException("Overlay parent Control must be hosted on a Form before the overlay can be attached.")

        Dim oOverlay As New Overlay(host, paintAction)
        oOverlay.CreateHandle()
        Return oOverlay
    End Function
    '''<summary>Creates a new window handle if one is not already assigned.</summary>
    Protected Overridable Shadows Sub CreateHandle()
        If Handle <> IntPtr.Zero OrElse Host Is Nothing Then Return
        Dim cOwner As Control = Host.FindForm
        If cOwner Is Nothing Then Return

        Dim cpCreateParams = New CreateParams With {.ExStyle = WS_EX_NOACTIVATE, .Height = _height, .Parent = cOwner.Handle, .Style = WS_CHILD Or WS_VISIBLE Or WS_CLIPSIBLINGS, .Width = _width, .X = _left, .Y = _top}
        MyBase.CreateHandle(cpCreateParams)

        InstallSelectionService()
        PlaceOverlay()
    End Sub

#End Region

#Region "Designer Movement"

    Private _behaviorService As BehaviorService
    Private _selectionService As ISelectionService

    Private Sub BehaviorServiceBeginDrag(sender As Object, e As BehaviorDragDropEventArgs)
        For Each oDragComponent As Object In e.DragComponents
            If ReferenceEquals(oDragComponent, Host) Then
                If Handle <> IntPtr.Zero Then DestroyHandle()
                Exit For
            End If
        Next
    End Sub
    Private Sub BehaviorServiceEndDrag(sender As Object, e As BehaviorDragDropEventArgs)
        CreateHandle()
        _behaviorService?.SyncSelection()
        _behaviorService?.Invalidate()
    End Sub

    Private Sub InstallDesignerBehavior()
        If _behaviorService Is Nothing Then
            _behaviorService = TryCast(Host.Site.GetService(GetType(BehaviorService)), BehaviorService)
            If _behaviorService Is Nothing Then Return
        End If

        If Not _selectionService.GetComponentSelected(Host) Then Return
        AddHandler _behaviorService.BeginDrag, AddressOf BehaviorServiceBeginDrag
        AddHandler _behaviorService.EndDrag, AddressOf BehaviorServiceEndDrag
    End Sub
    Private Sub RemoveDesignerBehavior()
        If _behaviorService Is Nothing Then Return
        RemoveHandler _behaviorService.BeginDrag, AddressOf BehaviorServiceBeginDrag
        RemoveHandler _behaviorService.EndDrag, AddressOf BehaviorServiceEndDrag
        _behaviorService = Nothing
    End Sub
    Private Sub SelectionServiceSelectionChanged(sender As Object, e As EventArgs)
        If _selectionService Is Nothing Then Return
        If _selectionService.GetComponentSelected(Host) Then
            InstallDesignerBehavior()
        Else
            RemoveDesignerBehavior()
        End If
    End Sub
    Private Sub InstallSelectionService()
        If LicenseManager.UsageMode <> LicenseUsageMode.Designtime AndAlso (Host?.Site Is Nothing OrElse Not Host.Site.DesignMode) Then Return

        If _selectionService Is Nothing Then
            _selectionService = TryCast(Host.Site.GetService(GetType(ISelectionService)), ISelectionService)
            If _selectionService Is Nothing Then Return
            AddHandler _selectionService.SelectionChanged, AddressOf SelectionServiceSelectionChanged
        End If

        InstallDesignerBehavior()
    End Sub
    Private Sub RemoveSelectionService()
        RemoveDesignerBehavior()
        If _selectionService Is Nothing Then Return
        RemoveHandler _selectionService.SelectionChanged, AddressOf SelectionServiceSelectionChanged
        _selectionService = Nothing
    End Sub

#End Region

#Region "Movements"

    Private HWND_PREVIOUS As IntPtr = IntPtr.Zero

    Private Const SWP_NOACTIVATE As UInteger = &H10
    Private Const SWP_SHOWWINDOW As UInteger = &H40

    Private Const GW_HWNDPREV As UInteger = 3

    <DllImport("user32.dll", SetLastError:=True)>
    Private Shared Function GetWindow(hWnd As IntPtr, uCmd As UInteger) As IntPtr
    End Function
    <DllImport("user32.dll", SetLastError:=True)>
    Private Shared Function IsWindow(hWnd As IntPtr) As Boolean
    End Function
    <DllImport("user32.dll", SetLastError:=True)>
    Private Shared Function SetWindowPos(hWnd As IntPtr, hWndInsertAfter As IntPtr, x As Integer, y As Integer, cx As Integer, cy As Integer, uFlags As UInteger) As Boolean
    End Function

    'Protected Overridable Sub HostClientSizeChanged(sender As Object, e As EventArgs)
    '    If Handle = IntPtr.Zero OrElse Host Is Nothing Then Return

    '    Dim sHostClientSize As Size = Host.ClientSize
    '    Dim iWidth As Integer = Math.Min(sHostClientSize.Width - _left, Math.Max(_width, _minimumWidth)) 'HostWidth-Left >= value.Width >= MinWidth
    '    Dim iHeight As Integer = Math.Min(sHostClientSize.Height - _top, Math.Max(_height, _minimumHeight)) 'HostHeight-Top >= value.Width >= MinHeight
    '    If iWidth = _width AndAlso iHeight = _height Then Return

    '    _height = iHeight
    '    _width = iWidth
    '    PlaceOverlay()
    'End Sub
    Private Sub HostControlAdded(sender As Object, e As ControlEventArgs)
        If Handle = IntPtr.Zero Then Return

        PlaceOverlay()
    End Sub
    Private Sub HostDisposed(sender As Object, e As EventArgs)
        RemoveSelectionService()
        _backBufferGraphics?.Dispose()
        _backBufferGraphics = Nothing
        _backBuffer?.Dispose()
        _backBuffer = Nothing
        If Handle <> IntPtr.Zero Then DestroyHandle()
        _paintAction = Nothing
    End Sub
    Private Sub HostLocationChanged(sender As Object, e As EventArgs)
        If Handle = IntPtr.Zero OrElse Host Is Nothing Then Return

        PlaceOverlay()
    End Sub
    'Protected Overridable Sub HostStyleChanged(sender As Object, e As EventArgs)
    '    If Handle = IntPtr.Zero OrElse Host Is Nothing Then Return

    '    Dim pHost As Panel = TryCast(Host, Panel)
    '    If pHost Is Nothing Then Return

    '    Dim bsHostBorderStyle As BorderStyle = pHost.BorderStyle
    '    If bsHostBorderStyle = _hostBorderStyle Then Return
    '    _hostBorderStyle = bsHostBorderStyle

    '    Dim cOwner As Control = Host.FindForm
    '    If cOwner Is Nothing Then Return

    '    Dim sHostClientSize As Size = Host.ClientSize
    '    Dim pLocation As Point = cOwner.PointToClient(Host.PointToScreen(New Point(_left, _top)))
    '    Dim iLeft As Integer = pLocation.X
    '    Dim iWidth As Integer = Math.Min(sHostClientSize.Width - _left, Math.Max(_width, _minimumWidth)) 'HostWidth-Left >= value.Width >= MinWidth
    '    Dim iTop As Integer = pLocation.Y
    '    Dim iHeight As Integer = Math.Min(sHostClientSize.Height - _top, Math.Max(_height, _minimumHeight)) 'HostHeight-Top >= value.Width >= MinHeight
    '    If iHeight = _height AndAlso iLeft = _left AndAlso iTop = _top AndAlso iWidth = _width Then Return

    '    _height = iHeight
    '    _left = iLeft
    '    _top = iTop
    '    _width = iWidth
    '    PlaceOverlay()
    'End Sub

    Protected Overridable Sub PlaceOverlay()
        If Handle = IntPtr.Zero OrElse Host Is Nothing OrElse Host.Handle = IntPtr.Zero Then Return

        Dim cOwner As Control = _Host.FindForm
        If cOwner Is Nothing Then Return

        If Not IsWindow(HWND_PREVIOUS) Then
            Dim hwndPrevious As IntPtr = GetWindow(Host.Handle, GW_HWNDPREV)
            If hwndPrevious <> IntPtr.Zero Then HWND_PREVIOUS = hwndPrevious
        End If

        Dim pLocation As Point = cOwner.PointToClient(Host.PointToScreen(New Point(_left, _top)))
        SetWindowPos(Handle, HWND_PREVIOUS, pLocation.X, pLocation.Y, Math.Min(_width, Host.Width - _left), Math.Min(_height, Host.Height - _top), SWP_NOACTIVATE Or SWP_SHOWWINDOW)
        RefreshBuffer()
    End Sub

#End Region

#Region "Paint Actions"

    <StructLayout(LayoutKind.Sequential)>
    Private Structure PAINTSTRUCT
        Public hdc As IntPtr
        Public fErase As Boolean
        Public rcPaint As RECT
        Public fRestore As Boolean
        Public fIncUpdate As Boolean

        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=32)>
        Public rgbReserved As Byte()
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Private Structure RECT
        Public Left As Integer
        Public Top As Integer
        Public Right As Integer
        Public Bottom As Integer
    End Structure

    Private Const HTTRANSPARENT As Integer = -1
    Private Const WM_ERASEBKGND As Integer = &H14
    Private Const WM_NCHITTEST As Integer = &H84
    Private Const WM_PAINT As Integer = &HF

    <DllImport("User32.dll")>
    Private Shared Function BeginPaint(hWnd As IntPtr, ByRef lpPaint As PAINTSTRUCT) As IntPtr
    End Function
    <DllImport("user32.dll")>
    Private Shared Function EndPaint(hWnd As IntPtr, ByRef lpPaint As PAINTSTRUCT) As Boolean
    End Function
    <DllImport("user32.dll")>
    Private Shared Function InvalidateRect(hWnd As IntPtr, lpRect As IntPtr, bErase As Boolean) As Boolean
    End Function

    Private _backBuffer As Bitmap
    Private _backBufferGraphics As Graphics
    Private _paintAction As Action(Of Graphics, Rectangle)

    Private Sub RebuildBackBuffer()
        If Host Is Nothing OrElse _width <= 0 OrElse _height <= 0 Then Return
        If _backBuffer Is Nothing OrElse _backBuffer.Width <> _width OrElse _backBuffer.Height <> _height Then
            _backBufferGraphics?.Dispose()
            _backBuffer?.Dispose()
            _backBuffer = New Bitmap(_width, _height, PixelFormat.Format32bppPArgb)
            _backBufferGraphics = Graphics.FromImage(_backBuffer)
            _backBufferGraphics.TextRenderingHint = TextRenderingHint.SystemDefault
            _backBufferGraphics.SmoothingMode = SmoothingMode.None
        End If

        _backBufferGraphics.Clear(Host.BackColor)
        _paintAction?.Invoke(_backBufferGraphics, New Rectangle(0, 0, Math.Min(_width, Host.Width - _left), Math.Min(_height, Host.Height - _top)))
    End Sub
    Private Sub RefreshBuffer()
        If Handle = IntPtr.Zero Then Return
        RebuildBackBuffer()
        InvalidateRect(Handle, IntPtr.Zero, False)
    End Sub

    Protected Overrides Sub WndProc(ByRef m As Message)
        Select Case m.Msg
            Case WM_ERASEBKGND
                m.Result = New IntPtr(1)
                Return
            Case WM_NCHITTEST
                m.Result = New IntPtr(HTTRANSPARENT)
                Return
            Case WM_PAINT
                Dim psPaintStruct As PAINTSTRUCT = Nothing
                Dim hdc As IntPtr = BeginPaint(Handle, psPaintStruct)
                If hdc = IntPtr.Zero Then Return
                Try
                    If _backBuffer Is Nothing Then RebuildBackBuffer()
                    If _backBuffer IsNot Nothing Then
                        Using gGraphics As Graphics = Graphics.FromHdc(hdc)
                            gGraphics.DrawImageUnscaled(_backBuffer, 0, 0)
                        End Using
                    End If
                Finally
                    EndPaint(Handle, psPaintStruct)
                End Try
                Return
        End Select
        MyBase.WndProc(m)
    End Sub

#End Region

End Class
