Imports System.Runtime.InteropServices

'''<summary>Monitors accesibility events associated with the scroll values being programatically set in a <see cref="ScrollableControl"/>.</summary>
Public Class ScrollBarEventMonitor
    Implements IDisposable

#Region "Private Items"

    Private Const EO_VALUECHANGE As UInteger = &H800E
    Private Const OID_HSCROLL As Integer = -6
    Private Const OID_VSCROLL As Integer = -5
    Private Const WE_OUTOFCONTEXT As UInteger = 0

    <DllImport("user32.dll", SetLastError:=True)>
    Private Shared Function SetWinEventHook(eventMin As UInteger, eventMax As UInteger, hmodWinEventProc As IntPtr, lpfnWinEventProc As WinEventDelegate, idProcess As UInteger, idThread As UInteger, dwFlags As UInteger) As IntPtr
    End Function
    <DllImport("user32.dll")>
    Private Shared Function UnhookWinEvent(hWinEventHook As IntPtr) As Boolean
    End Function

    Private Delegate Sub WinEventDelegate(hWinEventHook As IntPtr, eventType As UInteger, hwnd As IntPtr, idObject As Integer, idChild As Integer, idEventThread As UInteger, dwmsEventTime As UInteger)

    Private WithEvents MyHost As ScrollableControl

    Private ReadOnly _callback As WinEventDelegate
    Private _hook As IntPtr
    Private _oldHorizontalScrollValue As Integer = -1
    Private _oldVerticalScrollValue As Integer = -1

    Private Sub MyHost_Disposed(sender As Object, e As EventArgs) Handles MyHost.Disposed
        Dispose()
    End Sub
    Private Sub MyHost_Scroll(sender As Object, e As ScrollEventArgs) Handles MyHost.Scroll
        Select Case e.ScrollOrientation
            Case ScrollOrientation.HorizontalScroll
                _oldHorizontalScrollValue = MyHost.HorizontalScroll.Value
            Case ScrollOrientation.VerticalScroll
                _oldVerticalScrollValue = MyHost.VerticalScroll.Value
        End Select
    End Sub

#End Region

#Region "Events"

    '''<summary>Occurs when the the value of a scrollbar is programatically set in a <see cref="ScrollableControl"/>.</summary>
    Public Event ScrollValueSet As ScrollEventHandler

#End Region

#Region "Initializers"

    '''<summary>Initializers a new instance of the <see cref="ScrollBarEventMonitor"/> class.</summary>
    '''<param name="host">The <see cref="ScrollableControl"/> whose scrollbars will be monitored.</param>
    Public Sub New(host As ScrollableControl)
        MyHost = host
        _callback = AddressOf WinEventProc
        _hook = SetWinEventHook(EO_VALUECHANGE, EO_VALUECHANGE, IntPtr.Zero, _callback, 0, 0, WE_OUTOFCONTEXT)
    End Sub

#End Region

#Region "Functions"

    Public Overridable Sub Dispose() Implements IDisposable.Dispose
        If _hook <> IntPtr.Zero Then
            UnhookWinEvent(_hook)
            _hook = IntPtr.Zero
            MyHost = Nothing
        End If
    End Sub
    '''<summary>Processes Windows accesibility events received by the event hook.</summary>
    '''<param name="hWinEventHook">The handle to the registered Windows event hook.</param>
    '''<param name="eventType">The type of accessibility event that occurred.</param>
    '''<param name="hwnd">The handle to the window associated with the event.</param>
    '''<param name="idObject">The identifier of the accessible object associated with the event.</param>
    '''<param name="idChild">The identifier of the child element associated with the event.</param>
    '''<param name="idEventThread">The identifier of the thread that generated the event.</param>
    '''<param name="dwmsEventTime">The timestamp of the event, in milliseconds.</param>
    Protected Overridable Sub WinEventProc(hWinEventHook As IntPtr, eventType As UInteger, hwnd As IntPtr, idObject As Integer, idChild As Integer, idEventThread As UInteger, dwmsEventTime As UInteger)
        If eventType <> EO_VALUECHANGE OrElse hwnd <> MyHost.Handle OrElse idChild <> 0 Then Return
        Select Case idObject
            Case OID_HSCROLL
                Dim iNewHorizontalScrollValue As Integer = MyHost.HorizontalScroll.Value
                If iNewHorizontalScrollValue <> _oldHorizontalScrollValue Then
                    RaiseEvent ScrollValueSet(MyHost, New ScrollEventArgs(ScrollEventType.ThumbPosition, _oldHorizontalScrollValue, iNewHorizontalScrollValue, ScrollOrientation.HorizontalScroll))
                    _oldHorizontalScrollValue = iNewHorizontalScrollValue
                End If
            Case OID_VSCROLL
                Dim iNewVerticalScrollValue As Integer = MyHost.VerticalScroll.Value
                If iNewVerticalScrollValue <> _oldVerticalScrollValue Then
                    RaiseEvent ScrollValueSet(MyHost, New ScrollEventArgs(ScrollEventType.ThumbPosition, _oldVerticalScrollValue, iNewVerticalScrollValue, ScrollOrientation.VerticalScroll))
                    _oldVerticalScrollValue = iNewVerticalScrollValue
                End If
        End Select
    End Sub

#End Region

End Class
