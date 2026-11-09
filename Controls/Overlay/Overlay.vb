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

    Private _backColor As Color = Color.Transparent
    Private _displayHeight As Integer
    Private _displayWidth As Integer
    Private _dock As OverlayDockStyle = OverlayDockStyle.Top
    Private _expectedDisplayHeight As Integer
    Private _expectedDisplayWidth As Integer
    Private _font As Font
    Private _foreColor As Color = Color.Transparent
    Private _height As Integer = 0
    Private _hostBackColor As Color
    Private _hostBorderStyle As BorderStyle
    Private _hostClientHeight As Integer
    Private _hostClientWidth As Integer
    Private _hostFont As Font
    Private _hostForeColor As Color
    Private _hostPadding As Padding
    Private _location As Point
    Private _padding As Padding = Padding.Empty
    Private _visible As Boolean = True
    Private _width As Integer = 100

    Private Sub NotifyPropertyChanged(control As Control, propertyName As String, oldValue As Object, newValue As Object)
        Dim ccsChangeService As IComponentChangeService = TryCast(Host?.Site?.GetService(GetType(IComponentChangeService)), IComponentChangeService)
        If ccsChangeService Is Nothing Then Return
        Dim pdDescriptor As PropertyDescriptor = TypeDescriptor.GetProperties(control)(propertyName)
        If pdDescriptor Is Nothing Then Return
        ccsChangeService.OnComponentChanged(control, pdDescriptor, oldValue, newValue)
        TypeDescriptor.Refresh(control)
    End Sub
    Private Sub RefreshBounds(Optional forceRefreshBuffer As Boolean = False)
        If Handle = IntPtr.Zero OrElse Host Is Nothing Then Return

        Dim sHostClientSize As Size = Host.ClientSize
        Dim iHostClientHeight As Integer = sHostClientSize.Height
        Dim iHostClientWidth As Integer = sHostClientSize.Width
        Dim bLocationChanged As Boolean = False
        Dim bDisplayHeightChanged As Boolean
        Dim bDisplayWidthChanged As Boolean

        Select Case _dock
            Case OverlayDockStyle.Top
                If _height = 0 Then
                    _expectedDisplayHeight = _font.Height + _padding.Vertical
                Else
                    _expectedDisplayHeight = _height
                End If
                Dim iDisplayHeight As Integer = Math.Min(_expectedDisplayHeight, iHostClientHeight)
                bDisplayHeightChanged = iDisplayHeight <> _displayHeight
                If bDisplayHeightChanged Then _displayHeight = iDisplayHeight
                bDisplayWidthChanged = iHostClientWidth <> _displayWidth
                If bDisplayWidthChanged Then _displayWidth = iHostClientWidth

                Dim cParent As Control = Host.Parent
                If cParent IsNot Nothing Then
                    Dim pLocation As Point = cParent.PointToClient(Host.PointToScreen(Point.Empty))
                    bLocationChanged = pLocation <> _location
                    If bLocationChanged Then _location = pLocation
                End If

            Case OverlayDockStyle.Bottom
                If _height = 0 Then
                    _expectedDisplayHeight = _font.Height + _padding.Vertical
                Else
                    _expectedDisplayHeight = _height
                End If
                Dim iDisplayHeight As Integer = Math.Min(_expectedDisplayHeight, iHostClientHeight)
                bDisplayHeightChanged = iDisplayHeight <> _displayHeight
                If bDisplayHeightChanged Then _displayHeight = iDisplayHeight
                bDisplayWidthChanged = iHostClientWidth <> _displayWidth
                If bDisplayWidthChanged Then _displayWidth = iHostClientWidth

                Dim cParent As Control = Host.Parent
                If cParent IsNot Nothing Then
                    Dim pLocation As Point = cParent.PointToClient(Host.PointToScreen(Point.Empty))
                    If _displayHeight < iHostClientHeight Then pLocation.Y += iHostClientHeight - _displayHeight
                    bLocationChanged = pLocation <> _location
                    If bLocationChanged Then _location = pLocation
                End If

            Case OverlayDockStyle.Left
                bDisplayHeightChanged = iHostClientHeight <> _displayHeight
                If bDisplayHeightChanged Then _displayHeight = iHostClientHeight
                Dim iDisplayWidth As Integer = Math.Min(_width, iHostClientWidth)
                bDisplayWidthChanged = iDisplayWidth <> _displayWidth
                If bDisplayWidthChanged Then _displayWidth = iDisplayWidth

                Dim cParent As Control = Host.Parent
                If cParent IsNot Nothing Then
                    Dim pLocation As Point = cParent.PointToClient(Host.PointToScreen(Point.Empty))
                    bLocationChanged = pLocation <> _location
                    If bLocationChanged Then _location = pLocation
                End If

            Case OverlayDockStyle.Right
                bDisplayHeightChanged = iHostClientHeight <> _displayHeight
                If bDisplayHeightChanged Then _displayHeight = iHostClientHeight
                Dim iDisplayWidth As Integer = Math.Min(_width, iHostClientWidth)
                bDisplayWidthChanged = iDisplayWidth <> _displayWidth
                If bDisplayWidthChanged Then _displayWidth = iDisplayWidth

                Dim cParent As Control = Host.Parent
                If cParent IsNot Nothing Then
                    Dim pLocation As Point = cParent.PointToClient(Host.PointToScreen(Point.Empty))
                    If _displayWidth < _hostClientWidth Then pLocation.X += iHostClientWidth - _displayWidth
                    bLocationChanged = pLocation <> _location
                    If bLocationChanged Then _location = pLocation
                End If
        End Select

        If bDisplayHeightChanged OrElse bDisplayWidthChanged OrElse bLocationChanged Then
            _hostClientHeight = iHostClientHeight
            _hostClientWidth = iHostClientWidth
            PlaceOverlay()
            If bDisplayHeightChanged Then OnHeightChanged(EventArgs.Empty)
            If bLocationChanged Then OnLocationChanged(EventArgs.Empty)
            If bDisplayWidthChanged Then OnWidthChanged(EventArgs.Empty)
        ElseIf forceRefreshBuffer Then
            RefreshBuffer()
        End If
    End Sub

#End Region

#Region "Events"

    '''<summary>Occurs when the height has changed</summary>
    Public event HeightChanged As EventHandler
    '''<summary>Occurs when the location has changed.</summary>
    Public Event LocationChanged As EventHandler
    '''<summary>Occurs when the visibility has changed.</summary>
    Public Event VisibleChanged As EventHandler
    '''<summary>Occurs when the width has changed.</summary>
    Public Event WidthChanged As EventHandler

#End Region

#Region "Initializers"

    Private Const WS_CHILD As Integer = &H40000000
    Private Const WS_CLIPSIBLINGS As Integer = &H4000000
    Private Const WS_EX_NOACTIVATE As Integer = &H8000000
    Private Const WS_VISIBLE As Integer = &H10000000

    '''<summary>Creates a new instance of <see cref="HeaderOverlay"/>.</summary>
    '''<param name="host">The hosting <see cref="Control"/> the <see cref="HeaderOverlay"/> is in.</param>
    '''<exception cref="ArgumentNullException"><paramref name="host"/> is <see langword="Nothing"/> is <see langword="Nothing"/>.</exception>
    Public Sub New(host As Control)
        MyBase.New
        If host Is Nothing Then Throw New ArgumentNullException(NameOf(host))

        Me.Host = host

        AddHandler host.BackColorChanged, AddressOf HostBackColorChanged
        AddHandler host.ClientSizeChanged, AddressOf HostClientSizeChanged
        AddHandler host.Disposed, AddressOf HostDisposed
        AddHandler host.ControlAdded, AddressOf HostControlAdded
        AddHandler host.FontChanged, AddressOf HostFontChanged
        AddHandler host.ForeColorChanged, AddressOf HostForeColorChanged
        AddHandler host.HandleCreated, AddressOf HostHandleCreated
        AddHandler host.Invalidated, AddressOf HostInvalidated
        AddHandler host.LocationChanged, AddressOf HostLocationChanged
        AddHandler host.PaddingChanged, AddressOf HostPaddingChanged
        Dim scHost As ScrollableControl = TryCast(host, ScrollableControl)
        If scHost IsNot Nothing Then AddHandler scHost.Scroll, AddressOf HostScroll
        If TryCast(host, Panel) IsNot Nothing Then AddHandler host.StyleChanged, AddressOf HostStyleChanged

        If host.IsHandleCreated Then
            CreateHandle()
            PlaceOverlay()
        End If
    End Sub
    '''<summary>Creates a new instance of <see cref="HeaderOverlay"/>.</summary>
    '''<param name="host">The hosting <see cref="Control"/> the <see cref="HeaderOverlay"/> is in.</param>
    '''<param name="paintAction">The <see cref="Action"/> used top paint the header overlay, defined by the host.</param>
    '''<exception cref="ArgumentNullException"><paramref name="host"/> is <see langword="Nothing"/> is <see langword="Nothing"/>.</exception>
    Public Sub New(host As Control, paintAction As Action(Of Graphics, Rectangle))
        MyBase.New
        If host Is Nothing Then Throw New ArgumentNullException(NameOf(host))

        Me.PaintAction = paintAction
        Me.Host = host

        AddHandler host.BackColorChanged, AddressOf HostBackColorChanged
        AddHandler host.ClientSizeChanged, AddressOf HostClientSizeChanged
        AddHandler host.ControlAdded, AddressOf HostControlAdded
        AddHandler host.Disposed, AddressOf HostDisposed
        AddHandler host.FontChanged, AddressOf HostFontChanged
        AddHandler host.ForeColorChanged, AddressOf HostForeColorChanged
        AddHandler host.HandleCreated, AddressOf HostHandleCreated
        AddHandler host.Invalidated, AddressOf HostInvalidated
        AddHandler host.LocationChanged, AddressOf HostLocationChanged
        AddHandler host.PaddingChanged, AddressOf HostPaddingChanged
        Dim scHost As ScrollableControl = TryCast(host, ScrollableControl)
        If scHost IsNot Nothing Then AddHandler scHost.Scroll, AddressOf HostScroll
        If TryCast(host, Panel) IsNot Nothing Then AddHandler host.StyleChanged, AddressOf HostStyleChanged

        If host.IsHandleCreated Then
            CreateHandle()
            PlaceOverlay()
        End If
    End Sub

    '''<summary>Creates a new window handle if one is not already assigned.</summary>
    Protected Overridable Shadows Sub CreateHandle()
        If Handle <> IntPtr.Zero OrElse Host Is Nothing OrElse Not _visible Then Return
        Dim cParent As Control = Host.Parent
        If cParent Is Nothing OrElse cParent.Handle = IntPtr.Zero Then Return

        Dim sHostClientSize As Size = Host.ClientSize
        Dim iHostClientHeight As Integer = sHostClientSize.Height
        Dim iHostClientWidth As Integer = sHostClientSize.Width
        Dim pLocation As Point = cParent.PointToClient(Host.PointToScreen(Point.Empty))
        Select Case _dock
            Case OverlayDockStyle.Bottom
                If _expectedDisplayHeight < iHostClientHeight Then pLocation.Y += iHostClientHeight - _expectedDisplayHeight
            Case OverlayDockStyle.Right
                If _expectedDisplayWidth < iHostClientWidth Then pLocation.X += iHostClientWidth - _expectedDisplayWidth
        End Select

        _expectedDisplayHeight = Host.Font.Height + _padding.Vertical
        _hostBackColor = Host.BackColor
        _hostBorderStyle = CType(Host, Panel).BorderStyle
        _hostClientHeight = iHostClientHeight
        _hostClientWidth = iHostClientWidth
        _hostPadding = Host.Padding
        _hostFont = Host.Font
        _hostForeColor = Host.ForeColor
        _location = pLocation
        Dim cpCreateParams = New CreateParams With {.ExStyle = WS_EX_NOACTIVATE, .Height = Math.Min(_expectedDisplayHeight, iHostClientHeight), .Parent = cParent.Handle, .Style = WS_CHILD Or WS_VISIBLE Or WS_CLIPSIBLINGS, .Width = Math.Min(_expectedDisplayWidth, iHostClientWidth), .X = pLocation.X, .Y = pLocation.Y}
        MyBase.CreateHandle(cpCreateParams)

        Dim scHost As ScrollableControl = TryCast(Host, ScrollableControl)
        If scHost IsNot Nothing Then MyHostScrollBarEventMonitor = New ScrollBarEventMonitor(scHost)

        InstallSelectionService()
        InstallZOrderWatch()
    End Sub

#End Region

#Region "Properties"

    '''<summary>Gets or sets the background color for the overlay.</summary>
    '''<returns>The <see cref="Color"/> that represents the background color of the overlay. The default is the host's background color.</returns>
    <Category("Appearance")>
    <Description("The background color of the overlay.")>
    Public Property BackColor As Color
        Get
            If _backColor = Color.Transparent AndAlso Host IsNot Nothing Then _backColor = Host.BackColor
            Return _backColor
        End Get
        Set(value As Color)
            If value = _backColor Then Return
            Dim cOldBackColor As Color = _backColor
            _backColor = value

            RefreshBuffer()

            NotifyPropertyChanged(Host, NameOf(BackColor), cOldBackColor, value)
        End Set
    End Property
    Private Sub ResetBackColor()
        If Host IsNot Nothing Then BackColor = Host.BackColor
    End Sub
    Private Function ShouldSerializeBackColor() As Boolean
        If Host Is Nothing Then Return False
        Return _backColor <> Host.BackColor
    End Function
    '''<summary>Gets the displayed height of the overlay in pixels.</summary>
    '''<return>An <see cref="Integer"/> that represents the displayed height of the overlay in pixels.</return>
    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public ReadOnly Property DisplayedHeight As Integer
        Get
            If _visible Then Return _displayHeight
            Return 0
        End Get
    End Property
    '''<summary>Gets the displayed width of the overlay in pixels.</summary>
    '''<return>An <see cref="Integer"/> that represents the displayed width of the overlay in pixels.</return>
    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public ReadOnly Property DisplayedWidth As Integer
        Get
            If _visible Then Return _displayWidth
            Return 0
        End Get
    End Property
    '''<summary>Gets or sets which control borders are docked to its parent control and determines how a control is resized with its parent.</summary>
    '''<returns>One of the <see cref="OverlayDockStyle"/> values. The default is <see cref="OverlayDockStyle.Top"/>.</returns>
    '''<exception cref="InvalidEnumArgumentException">The value assigned is not one of the <see cref="OverlayDockStyle"/> values.</exception>
    <Category("Appearance")>
    <DefaultValue(OverlayDockStyle.Top)>
    <Description("Defines which borders of the control are bound to the container.")>
    Public Property Dock As OverlayDockStyle
        Get
            Return _dock
        End Get
        Set(value As OverlayDockStyle)
            If value = _dock Then Return
            Dim dsOldDock As OverlayDockStyle = _dock
            _dock = value

            RefreshBounds()

            NotifyPropertyChanged(Host, NameOf(Dock), dsOldDock, value)
        End Set
    End Property
    '''<summary>Gets or sets the font of the text displayed in the overlay.</summary>
    '''<returns>The <see cref="System.Drawing.Font"/> to apply to the text displayed by the overlay. The default is the host's font.</returns>
    <Category("Appearance")>
    <Description("The font used to display text in the overlay.")>
    Public Property Font As Font
        Get
            If _font Is Nothing AndAlso Host IsNot Nothing Then _font = Host.Font.Clone
            Return _font
        End Get
        Set(value As Font)
            If value.Equals(_font) Then Return
            Dim fOldFont As Font = _font
            _font = value

            RefreshBounds(True)

            NotifyPropertyChanged(Host, NameOf(Font), fOldFont, value)
        End Set
    End Property
    Private Sub ResetFont()
        If Host IsNot Nothing Then Font = Host.Font.Clone
    End Sub
    Private Function ShouldSerializeFont() As Boolean
        If Host Is Nothing Then Return False
        Return Not _font.Equals(Host.Font)
    End Function
    '''<summary>Gets or sets the foreground color of the overlay.</summary>
    '''<returns>The foreground <see cref="Color"/> of the overlay. The default is the host's foreground color.</returns>
    <Category("Appearance")>
    <Description("The foreground color of this overlay, which is used to display text.")>
    Public Property ForeColor As Color
        Get
            If _foreColor = Color.Transparent AndAlso Host IsNot Nothing Then _foreColor = Host.ForeColor
            Return _foreColor
        End Get
        Set(value As Color)
            If value = _foreColor Then Return
            Dim cOldForeColor As Color = _foreColor
            _foreColor = value

            RefreshBuffer()

            NotifyPropertyChanged(Host, NameOf(ForeColor), cOldForeColor, value)
        End Set
    End Property
    Private Sub ResetForeColor()
        If Host IsNot Nothing Then ForeColor = Host.ForeColor
    End Sub
    Private Function ShouldSerializeForeColor() As Boolean
        If Host Is Nothing Then Return False
        Return _foreColor <> Host.ForeColor
    End Function
    '''<summary>Gets the handle for this window.</summary>
    '''<returns>If successful, an <see cref="IntPtr"/> representing the handle to the associated <see cref="Overlay"/>; otherwise, 0 if no handle is associated with the overlay.</returns>
    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Overloads ReadOnly Property Handle As IntPtr
        Get
            Return MyBase.Handle
        End Get
    End Property
    '''<summary>Gets or sets the width of the control when its dock is <see cref="OverlayDockStyle.Top"/> or <see cref="OverlayDockStyle.Bottom"/>.</summary>
    '''<remarks>When set to 0, the height will automatically fit the text height and the internal padding for one line of text.</remarks>
    '''<returns>An <see cref="Integer"/> that represents the width of the overlay when its dock is <see cref="OverlayDockStyle.Top"/> or <see cref="OverlayDockStyle.Bottom"/>.</returns>
    <Category("Appearance")>
    <DefaultValue(0)>
    <Description("The height of the overlay when dock is Left or Right. A value of 0 will cause the height to be autmoatically generated.")>
    Public Property Height As Integer
        Get
            Return _height
        End Get
        Set(value As Integer)
            If value = _height Then Return
            Dim iOldHeight As Integer = _height
            _height = value

            Select Case _dock
                Case OverlayDockStyle.Top, OverlayDockStyle.Bottom
                    RefreshBounds()
            End Select

            NotifyPropertyChanged(Host, NameOf(Height), iOldHeight, value)
        End Set
    End Property
    '''<summary>Gets the hosting <see cref="Control"/> the <see cref="Overlay"/> is contained within.</summary>
    '''<returns>The hosting <see cref="Control"/> the <see cref="Overlay"/> is contained within.</returns>
    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public ReadOnly Property Host As Control
    '''<summary>Gets or sets padding within the control.</summary>
    '''<returns>A <see cref="Windows.Forms.Padding"/> representing the control's internal spacing characteristics.</returns>
    <Category("Layout")>
    <Description("Specifies the interior spacing of a control.")>
    Public Property Padding As Padding
        Get
            Return _padding
        End Get
        Set(value As Padding)
            If value = _padding Then Return
            Dim pOldPadding As Padding = _padding
            _padding = value

            RefreshBounds()

            NotifyPropertyChanged(Host, NameOf(Padding), pOldPadding, value)
        End Set
    End Property
    Private Sub ResetPadding()
        Padding = Padding.Empty
    End Sub
    Private Function ShouldSerializePadding() As Boolean
        Return Padding <> Padding.Empty
    End Function
    '''<summary>Gets the action used to paint the overlay.</summary>
    '''<returns>The action used to paint the overlay.</returns>
    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property PaintAction As Action(Of Graphics, Rectangle)
    '''<summary>Determines if the overlay will be drawn on its host.</summary>
    '''<returns><see langword="True"/> if the overlay will be drawn on its host; otherwise, <see langword="False"/>.</returns>
    <Category("Appearance")>
    <DefaultValue(True)>
    <Description("Determines if the overlay will be drawn on its host.")>
    Public Property Visible As Boolean
        Get
            Return _visible
        End Get
        Set(value As Boolean)
            If value = _visible Then Return
            Dim bOldVisible As Boolean = _visible
            _visible = value

            If value Then
                CreateHandle()
                PlaceOverlay()
            Else
                DestroyHandle()
            End If
            OnVisibleChanged(EventArgs.Empty)

            NotifyPropertyChanged(Host, NameOf(Visible), bOldVisible, value)
        End Set
    End Property
    '''<summary>Gets or sets the width of the control when its dock is <see cref="OverlayDockStyle.Left"/> or <see cref="OverlayDockStyle.Right"/>.</summary>
    '''<returns>An <see cref="Integer"/> that represents the width of the overlay when its dock is <see cref="OverlayDockStyle.Left"/> or <see cref="OverlayDockStyle.Right"/>.</returns>
    <Category("Appearance")>
    <DefaultValue(100)>
    <Description("The width of the overlay when dock is Left or Right.")>
    Public Property Width As Integer
        Get
            Return _width
        End Get
        Set(value As Integer)
            If value = _width Then Return
            Dim iOldWidth As Integer = _width
            _width = value

            Select Case _dock
                Case OverlayDockStyle.Left, OverlayDockStyle.Right
                    RefreshBounds()
            End Select

            NotifyPropertyChanged(Host, NameOf(Width), iOldWidth, value)
        End Set
    End Property

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
        PlaceOverlay()
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

#Region "Host Actions"

    Private Const GW_HWNDPREV As UInteger = 3
    Private Const SWP_NOACTIVATE As UInteger = &H10
    Private Const SWP_NOMOVE As UInteger = &H2
    Private Const SWP_NOSIZE As UInteger = &H1
    Private Const SWP_NOZORDER As UInteger = &H4
    Private Const SWP_SHOWWINDOW As UInteger = &H40

    <DllImport("user32.dll", SetLastError:=True)>
    Private Shared Function GetWindow(hWnd As IntPtr, uCmd As UInteger) As IntPtr
    End Function
    <DllImport("user32.dll", SetLastError:=True)>
    Private Shared Function SetWindowPos(hWnd As IntPtr, hWndInsertAfter As IntPtr, x As Integer, y As Integer, cx As Integer, cy As Integer, uFlags As UInteger) As Boolean
    End Function

    Private WithEvents MyHostScrollBarEventMonitor As ScrollBarEventMonitor

    Private Sub HostBackColorChanged(sender As Object, e As EventArgs)
        If Handle = IntPtr.Zero OrElse Host Is Nothing Then Return

        Dim cPreviousHostBackColor As Color = _hostBackColor
        _hostBackColor = Host.BackColor
        If _backColor <> cPreviousHostBackColor Then Return

        BackColor = _hostBackColor
    End Sub
    Private Sub HostClientSizeChanged(sender As Object, e As EventArgs)
        RefreshBounds()
    End Sub
    Private Sub HostControlAdded(sender As Object, e As ControlEventArgs)
        If Handle = IntPtr.Zero Then Return

        PlaceOverlay()
    End Sub
    Private Sub HostDisposed(sender As Object, e As EventArgs)
        RemoveSelectionService()
        RemoveZOrderWatcher()
        _backBufferGraphics?.Dispose()
        _backBufferGraphics = Nothing
        _backBuffer?.Dispose()
        _backBuffer = Nothing
        If Handle <> IntPtr.Zero Then DestroyHandle()
        PaintAction = Nothing
    End Sub
    Private Sub HostFontChanged(sender As Object, e As EventArgs)
        If Handle = IntPtr.Zero OrElse Host Is Nothing Then Return

        Dim fPreviousHostFont As Font = _hostFont
        _hostFont = Host.Font
        If Not _font.Equals(fPreviousHostFont) Then Return

        Font = _hostFont
    End Sub
    Private Sub HostForeColorChanged(sender As Object, e As EventArgs)
        If Handle = IntPtr.Zero OrElse Host Is Nothing Then Return

        Dim cPreviousHostBackColor As Color = _hostForeColor
        _hostForeColor = Host.ForeColor
        If _foreColor <> cPreviousHostBackColor Then Return

        ForeColor = _hostForeColor
    End Sub
    Private Sub HostHandleCreated(sender As Object, e As EventArgs)
        CreateHandle()
        PlaceOverlay()
    End Sub
    Private Sub HostInvalidated(sender As Object, e As InvalidateEventArgs)
        RefreshBuffer()
    End Sub
    Private Sub HostLocationChanged(sender As Object, e As EventArgs)
        RefreshBounds()
    End Sub
    Private Sub HostPaddingChanged(sender As Object, e As EventArgs)
        If Handle = IntPtr.Zero OrElse Host Is Nothing Then Return

        Dim pPreviousHostPadding As Padding = _hostPadding
        _hostPadding = Host.Padding
        If _padding <> pPreviousHostPadding Then Return

        Padding = _hostPadding
    End Sub
    Private Sub HostScroll(sender As Object, e As ScrollEventArgs)
        If e.ScrollOrientation <> ScrollOrientation.HorizontalScroll Then Return
        Dim scHost As ScrollableControl = TryCast(Host, ScrollableControl)
        If scHost Is Nothing Then Return
        RefreshBuffer()
    End Sub
    Private Sub HostStyleChanged(sender As Object, e As EventArgs)
        If Handle = IntPtr.Zero OrElse Host Is Nothing Then Return

        Dim pHost As Panel = TryCast(Host, Panel)
        If pHost Is Nothing Then Return

        Dim bsHostBorderStyle As BorderStyle = pHost.BorderStyle
        If bsHostBorderStyle = _hostBorderStyle Then Return
        _hostBorderStyle = bsHostBorderStyle

        RefreshBounds()
    End Sub
    Private Sub MyHostScrollBarEventMonitor_ScrollValueSet(sender As Object, e As ScrollEventArgs) Handles MyHostScrollBarEventMonitor.ScrollValueSet
        If e.ScrollOrientation <> ScrollOrientation.HorizontalScroll Then Return
        Dim scHost As ScrollableControl = TryCast(Host, ScrollableControl)
        If scHost Is Nothing Then Return
        RefreshBuffer()
    End Sub

#End Region

#Region "Z-Order Watch"

    Private Delegate Sub WinEventDelegate(hWinEventHook As IntPtr, eventType As UInteger, hwnd As IntPtr, idObject As Integer, idChild As Integer, idEventThread As UInteger, msEventTime As UInteger)

    Private Const SW_HIDE As Integer = 0
    Private Const SW_SHOWNA As Integer = 8
    Private Const WE_OBJECTREORDER As UInteger = &H8004
    Private Const WE_OUTOFCONTEXT As UInteger = &H0

    <DllImport("kernel32.dll", SetLastError:=True)>
    Private Shared Function GetCurrentProcessId() As UInteger
    End Function
    <DllImport("user32.dll", SetLastError:=True)>
    Private Shared Function SetWinEventHook(eventMin As UInteger, eventMax As UInteger, hmodWinEventProc As IntPtr, lpfnWinEventProc As WinEventDelegate, idProcess As UInteger, idThread As UInteger, flags As UInteger) As IntPtr
    End Function
    <DllImport("user32.dll", SetLastError:=True)>
    Private Shared Function ShowWindow(hWnd As IntPtr, nCmdShow As Integer) As Boolean
    End Function
    <DllImport("user32.dll", SetLastError:=True)>
    Private Shared Function UnhookWinEvent(hWinEventHook As IntPtr) As Boolean
    End Function

    Private _ownerHandle As IntPtr
    Private _winEventHook As IntPtr
    Private _winEventProc As WinEventDelegate

    Private Sub InstallZOrderWatch()
        If _winEventHook <> IntPtr.Zero OrElse Host Is Nothing Then Return

        Dim cParent As Control = Host.Parent
        If cParent Is Nothing OrElse cParent.Handle = IntPtr.Zero Then Return

        _ownerHandle = cParent.Handle
        _winEventProc = AddressOf WinEventProc
        _winEventHook = SetWinEventHook(WE_OBJECTREORDER, WE_OBJECTREORDER, IntPtr.Zero, _winEventProc, GetCurrentProcessId, 0, WE_OUTOFCONTEXT)
    End Sub
    Private Sub RemoveZOrderWatcher()
        If _winEventHook <> IntPtr.Zero Then
            UnhookWinEvent(_winEventHook)
            _winEventHook = IntPtr.Zero
        End If
        _winEventProc = Nothing
        _ownerHandle = IntPtr.Zero
    End Sub
    Private Sub WinEventProc(hWinEventHook As IntPtr, eventType As UInteger, hwnd As IntPtr, idObject As Integer, idChild As Integer, idEventThread As UInteger, msEventTime As UInteger)
        If eventType <> WE_OBJECTREORDER OrElse hwnd = IntPtr.Zero Then Return
        If hwnd = Host.Handle OrElse hwnd = _ownerHandle Then
            If Handle <> IntPtr.Zero AndAlso Host IsNot Nothing AndAlso Host.Handle <> IntPtr.Zero Then
                PlaceOverlay()
                'Show/Hide helps deal with movement glyphs of other controls partially overlapping the overlay when the other control's z-order is changed
                ShowWindow(Handle, SW_HIDE)
                ShowWindow(Handle, SW_SHOWNA)
            End If
        End If
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
    Private Const WM_HSCROLL As Integer = &H114
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

    Private Sub RebuildBackBuffer()
        If Handle = IntPtr.Zero OrElse Host Is Nothing OrElse _displayHeight <= 0 OrElse _displayWidth <= 0 Then Return

        If _backBuffer Is Nothing OrElse _backBuffer.Height <> _displayHeight OrElse _backBuffer.Width <> _displayWidth Then
            _backBufferGraphics?.Dispose()
            _backBuffer?.Dispose()
            _backBuffer = New Bitmap(_displayWidth, _displayHeight, PixelFormat.Format32bppPArgb)
            _backBufferGraphics = Graphics.FromImage(_backBuffer)
            _backBufferGraphics.TextRenderingHint = TextRenderingHint.SystemDefault
            _backBufferGraphics.SmoothingMode = SmoothingMode.None
        End If

        _backBufferGraphics.Clear(_backColor)
        PaintAction?.Invoke(_backBufferGraphics, New Rectangle(0, 0, _displayWidth, _displayHeight))
    End Sub
    Private Sub RefreshBuffer()
        If Handle = IntPtr.Zero OrElse Not _visible Then Return
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

#Region "Functions"

    '''<summary>Raises the <see cref="HeightChanged"/> event.</summary>
    '''<param name="e">The <see cref="EventArgs"/> that contains the event data.</param>
    Protected Overridable Sub OnHeightChanged(e As EventArgs)
        RaiseEvent HeightChanged(Me, e)
    End Sub
    '''<summary>Raises the <see cref="LocationChanged"/> event.</summary>
    '''<param name="e">The <see cref="EventArgs"/> that contains the event data.</param>
    Protected Overridable Sub OnLocationChanged(e As EventArgs)
        RaiseEvent LocationChanged(Me, e)
    End Sub
    '''<summary>Raises the <see cref="VisibleChanged"/> event.</summary>
    '''<param name="e">The <see cref="EventArgs"/> that contains the event data.</param>
    Protected Overridable Sub OnVisibleChanged(e As EventArgs)
        RaiseEvent VisibleChanged(Me, e)
    End Sub
    '''<summary>Raises the <see cref="WidthChanged"/> event.</summary>
    '''<param name="e">The <see cref="EventArgs"/> that contains the event data.</param>
    Protected Overridable Sub OnWidthChanged(e As EventArgs)
        RaiseEvent WidthChanged(Me, e)
    End Sub
    '''<summary>Places the overlay based on the expected size and position.</summary>
    Protected Overridable Sub PlaceOverlay()
        If Handle = IntPtr.Zero OrElse Host Is Nothing OrElse Host.Handle = IntPtr.Zero Then Return

        SetWindowPos(Handle, GetWindow(Host.Handle, GW_HWNDPREV), 0, 0, 0, 0, SWP_NOSIZE Or SWP_NOMOVE Or SWP_NOACTIVATE) 'Place in Z-plane
        SetWindowPos(Handle, IntPtr.Zero, _location.X, _location.Y, _displayWidth, _displayHeight, SWP_NOZORDER Or SWP_NOACTIVATE Or SWP_SHOWWINDOW) 'Place with X/Y plane

        RefreshBuffer()
    End Sub

#End Region

End Class
