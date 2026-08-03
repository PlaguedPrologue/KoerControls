Imports System.ComponentModel
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices

Module Forms

#Region "Imports"

    <DllImport("user32.dll", SetLastError:=True)>
    Friend Sub ReleaseCapture()
    End Sub
    <DllImport("user32.dll", SetLastError:=True)>
    Friend Function SendMessage(hWnd As IntPtr, Msg As Integer, wParam As Integer, lParam As Integer) As Integer
    End Function

#End Region

#Region "Control"

    Private ReadOnly _doubleBufferValues As New ConditionalWeakTable(Of Control, HashSet(Of IDisposable))

    '''<summary>Adds a reason as to why a control is double buffered.</summary>
    '''<returns><see langword="True"/> if the <see cref="Control"/> as a double buffer property; otherwise <see langword="False"/>.</returns>
    '''<param name="control">The <see cref="Control"/> to double buffer.</param>
    '''<param name="reason">The reason to double buffer the <see cref="Control"/>.</param>
    <Extension>
    Public Function AddDoubleBufferReason(control As Control, reason As Object) As Boolean
        Dim piDoubleBuffer As PropertyInfo = control.GetType.GetProperty("DoubleBuffered", BindingFlags.Instance Or BindingFlags.NonPublic) 'Get the DoubleBuffered property for the control
        If piDoubleBuffer?.PropertyType IsNot GetType(Boolean) Then Return False 'Control does not have a DoubleBuffered property

        Dim dDoubleBufferSetters As HashSet(Of IDisposable) = Nothing 'Setup list of items causing the control to be double buffered
        If _doubleBufferValues.TryGetValue(control, dDoubleBufferSetters) Then 'Determine if a list is already set up for the control to have objects causing it to be double buffered
            If dDoubleBufferSetters.Count = 0 Then piDoubleBuffer.SetValue(control, True) 'Set the double buffered property if this is the first item causing the control to be double buffered
            dDoubleBufferSetters.Add(reason) 'Add the reason to the reasons the control is double buffered

        Else 'There are no previous records as to why the control is double buffered
            dDoubleBufferSetters = New HashSet(Of IDisposable)(ReferenceEqualityComparer.Instance) From {reason} 'Add reason to the reasons the control is double buffered
            If piDoubleBuffer.GetValue(control) Then 'Determine if the control is already double buffered
                dDoubleBufferSetters.Add(control) 'Add the control as its own reason as to why it is double buffered. Allow sthe control to remain double buffered once all double buffering objects are removed
            Else 'The control is not already double buffered
                piDoubleBuffer.SetValue(control, True) 'Set the control to be double buffered
            End If
            _doubleBufferValues.Add(control, dDoubleBufferSetters) 'Log reason as a reason as to why the control is double buffered
        End If

        Return True
    End Function
    '''<summary>Flattens the entire control heirarchy that is contained inside the specified control.</summary>
    '''<returns>The entire control heirarchy that is contained inside the specified control.</returns>
    '''<param name="control">The control on which to flatten its control heirarchy.</param>
    <Extension>
    Public Iterator Function GetAllControls(control As Control) As IEnumerable(Of Control)
        For Each cControl As Control In control.Controls
            For Each cChild As Control In cControl.GetAllControls
                Yield cChild
            Next
            Yield cControl
        Next
    End Function
    '''<summary>Flattens the entire control heirarchy of the specified type that is contained inside the specified control.</summary>
    '''<returns>The entire control heirarchy of the specified type that is contained inside the specified control.</returns>
    '''<typeparam name="T">The type of control to search for.</typeparam>
    '''<param name="control">The control on which to flatten its control heirarchy.</param>
    '''<param name="stopIterationAtTop"><see langword="True"/> if stopping search down a path at the first control of type <typeparamref name="T"/>; otherwise <see langword="False"/> if getting every child of type <typeparamref name="T"/>.</param>
    <Extension>
    Public Iterator Function GetAllControls(Of T)(control As Control, Optional stopIterationAtTop As Boolean = False) As IEnumerable(Of T)
        For Each cControl As Control In control.Controls
            Dim bIsOfTypeT As Boolean = TypeOf cControl Is T
            If Not bIsOfTypeT OrElse Not stopIterationAtTop Then
                For Each cChild As T In cControl.GetAllControls(Of T)(stopIterationAtTop)
                    Yield cChild
                Next
            End If
            If bIsOfTypeT Then
                Yield CObj(cControl)
                If stopIterationAtTop Then Return
            End If
        Next
    End Function
    '''<summary>Gets the displayed background color of the <see cref="Control"/> based on the transparency of itself, as well as all of its parent controls.</summary>
    '''<returns>The displayed background color of the <see cref="Control"/> based on the transparency of itself, as well as all of its parent controls
    '''<para/>Returns <see cref="Color.Transparent"/> if the <see cref="Control"/> is null or no parent control has been found, or if all of its own color as well as its parent colors are transparent..</returns>
    '''<param name="control">The <see cref="Control"/> to get the displayed background color of.</param>
    <Extension>
    Public Function GetDisplayedBackColor(control As Control) As Color
        Dim cColor As Color = control.BackColor
        Dim cParent As Control = control.Parent
        While cColor = Color.Transparent AndAlso cParent IsNot Nothing
            cColor = cParent.BackColor
            cParent = cParent.Parent
        End While
        Return cColor
    End Function
    '''<summary>Gets the displayed background color of the <see cref="Control"/> based on the transparency of itself, as well as all of its parent controls.</summary>
    '''<returns>The displayed background color of the <see cref="Control"/> based on the transparency of itself, as well as all of its parent controls
    '''<para/>Returns <see cref="Color.Transparent"/> if the <see cref="Control"/> is null or no parent control has been found, or if all of its own color as well as its parent colors are transparent..</returns>
    '''<param name="control">The <see cref="Control"/> to get the displayed background color of.</param>
    '''<param name="color">The expected background color of the <see cref="Control"/></param>
    <Extension>
    Public Function GetDisplayedBackColor(control As Control, color As Color) As Color
        Dim cColor As Color = color
        Dim cParent As Control = control.Parent
        While cColor = Color.Transparent AndAlso cParent IsNot Nothing
            cColor = cParent.BackColor
            cParent = cParent.Parent
        End While
        Return cColor
    End Function
    '''<summary>Gets the nearest parent control of type T.</summary>
    '''<returns>The nearset parent control of type T if found; othrwise, <see langword="Nothing"/>.</returns>
    '''<typeparam name="T">The type of the control to search for.</typeparam>
    '''<param name="this">The control to find its parent controls from.</param>
    <Extension>
    Public Function GetNearetParent(Of T)(this As Control) As T
        Dim cParent As Control = this.Parent
        While cParent IsNot Nothing
            If TypeOf cParent Is T Then
                Return CObj(cParent)
            End If
        End While
        Return Nothing
    End Function
    '''<summary>Gets the Z-index of a control.</summary>
    '''<returns>The Z-index of a control.</returns>
    '''<param name="control">The control for which the Z-index will be changed.</param>
    <Extension>
    Public Function GetZIndex(control As Control) As Integer
        Return If(control?.Parent?.Controls.IndexOf(control), 0)
    End Function
    '''<summary>Determines if a <see cref="IDisposable"/> object has been disposed.</summary>
    '''<returns><see langword="True"/> if the object is disposed; otherwise, <see langword="False"/>.</returns>
    '''<param name="obj">The <see cref="IDisposable"/> object to check.</param>
    <Extension>
    Public Function IsDisposed(obj As IDisposable) As Boolean
        If TypeOf obj Is Control Then Return CType(obj, Control).IsDisposed
        'If TypeOf obj Is Component Then Return CType(obj, Component).Site Is Nothing AndAlso CType(obj, Component).Container Is Nothing
        Dim tType As Type = obj.GetType
        Dim piDisposed As PropertyInfo = If(tType.GetProperty("IsDisposed"), tType.GetProperty("Disposed"))
        If piDisposed IsNot Nothing AndAlso piDisposed.PropertyType Is GetType(Boolean) Then Return piDisposed.GetValue(obj)
        Return False
    End Function
    '''<summary>Removes a reason as to why a control is double buffered.</summary>
    '''<returns><see langword="True"/> if the <see cref="Control"/> as a double buffer property; otherwise <see langword="False"/>.</returns>
    '''<param name="control">The <see cref="Control"/> to potentially stop double buffering.</param>
    '''<param name="reason">The reason to remove for double buffering the <see cref="Control"/>.</param>
    <Extension>
    Public Function RemoveDoubleBufferReason(control As Control, reason As Object) As Boolean
        Dim piDoubleBuffer As PropertyInfo = control.GetType.GetProperty("DoubleBuffered", BindingFlags.Instance Or BindingFlags.NonPublic) 'Get the DoubleBuffered property for the control
        If piDoubleBuffer?.PropertyType IsNot GetType(Boolean) Then Return False 'Control does not have a DoubleBuffered property

        Dim dDoubleBufferSetters As HashSet(Of IDisposable) = Nothing
        If Not _doubleBufferValues.TryGetValue(control, dDoubleBufferSetters) Then Return False 'Control does not have a double buffer reason, return false
        If dDoubleBufferSetters.Count = 1 Then 'Determine if removing the last double buffer reason for the Control
            _doubleBufferValues.Remove(control) 'Remove Control from the double buffer list
            piDoubleBuffer.SetValue(control, False) 'Set Control to no longer double buffer
            Return False
        Else 'Not removing the last double buffer reason for the Control
            dDoubleBufferSetters.Remove(reason) 'Remove reason from the reason for the Control to be double buffered
            Return True
        End If
    End Function
    '''<summary>Sets the Z-index of a control.</summary>
    '''<param name="control">The control for which the Z-index will be changed.</param>
    '''<param name="ZIndex">The new Z-index of the control.</param>
    <Extension>
    Public Sub SetZIndex(control As Control, ZIndex As Integer)
        control.Parent.Controls.SetChildIndex(control, ZIndex)
    End Sub

#End Region

#Region "ComboBox"

    '''<summary>Sets the specified item to a different value.</summary>
    '''<param name="comboBox">The <see cref="ComboBox"/> updating.</param>
    '''<param name="item">The Item to change.</param>
    '''<param name="value">The new value.</param>
    <Extension>
    Public Sub SetItem(comboBox As ComboBox, item As String, value As String)
        comboBox.SuspendLayout()
        Dim iSelectedIndex As Integer = comboBox.SelectedIndex
        For iIndex = 0 To comboBox.Items.Count - 1
            If comboBox.Items(iIndex) = item Then
                comboBox.Items(iIndex) = value
                Exit For
            End If
        Next
        comboBox.SelectedIndex = iSelectedIndex
        comboBox.ResumeLayout()
    End Sub
    '''<summary>Sets the specified item to a different value.</summary>
    '''<param name="comboBox">The <see cref="ComboBox"/> updating.</param>
    '''<param name="index">The index of the Item to change.</param>
    '''<param name="value">The new value.</param>
    '''<exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is greater than the number of items in the <see cref="ComboBox"/>.</exception>
    Public Sub SetItem(comboBox As ComboBox, index As Integer, value As String)
        If index >= comboBox.Items.Count Then Throw New ArgumentOutOfRangeException
        comboBox.SuspendLayout()
        Dim iSelectedIndex As Integer = comboBox.SelectedIndex
        comboBox.Items(index) = value
        comboBox.SelectedIndex = iSelectedIndex
        comboBox.ResumeLayout()
    End Sub

#End Region

#Region "DataGridViewCell"

    '''<summary>Gets the displayed background color of the <see cref="DataGridViewCell"/> based on the transparency of itself, its owning row, its owning column and its owning data grid view in that respective order.</summary>
    '''<returns>The displayed background color of the <see cref="DataGridViewCell"/> based on the transparency of itself, its owning row, its owning column and its owning data grid view in that respective order.
    '''<para/>Returns <see cref="Color.Transparent"/> if the <see cref="DataGridViewCell"/> is null or no owning row, column, or data grid view has been found, or if all of its own color as well as the row, column, and data grid view colors are transparent..</returns>
    '''<param name="cell">The <see cref="DataGridViewCell"/> to get the displayed background color of.</param>
    <Extension>
    Public Function GetDisplayedBackColor(cell As DataGridViewCell)
        If cell Is Nothing Then Return Color.Transparent
        If ReferenceEquals(cell.DataGridView?.CurrentCell, cell) Then
            If cell.Style.SelectionBackColor.ToArgb <> 0 Then Return cell?.Style.SelectionBackColor
            If cell.OwningRow IsNot Nothing AndAlso cell.OwningRow.DefaultCellStyle.SelectionBackColor.ToArgb <> 0 Then Return cell.OwningRow.DefaultCellStyle.SelectionBackColor
            If cell.OwningColumn IsNot Nothing AndAlso cell.OwningColumn.DefaultCellStyle.SelectionBackColor.ToArgb <> 0 Then Return cell.OwningColumn.DefaultCellStyle.SelectionBackColor
            If cell.DataGridView IsNot Nothing Then Return cell.DataGridView.DefaultCellStyle.SelectionBackColor
            Return Color.Transparent
        End If
        If cell.Style.BackColor.ToArgb <> 0 Then Return cell.Style.BackColor
        If cell.OwningRow IsNot Nothing AndAlso cell.OwningRow.DefaultCellStyle.BackColor.ToArgb <> 0 Then Return cell.OwningRow.DefaultCellStyle.BackColor
        If cell.OwningColumn IsNot Nothing AndAlso cell.OwningColumn.DefaultCellStyle.BackColor.ToArgb <> 0 Then Return cell.OwningColumn.DefaultCellStyle.BackColor
        If cell.DataGridView IsNot Nothing Then Return cell.DataGridView.DefaultCellStyle.BackColor
        Return Color.Transparent
    End Function

#End Region

#Region "TabPage"

    '''<summary>Gets the bounding rectangle for the tab of the control.</summary>
    '''<returns>A <see cref="Rectangle"/> that represents the bounds for the tab of the control.</returns>
    '''<param name="tabPage">The <see cref="TabPage"/> to get the tab rectangle from.</param>
    <Extension>
    Public Function GetTabRect(tabPage As TabPage) As Rectangle
        If tabPage Is Nothing Then Return Rectangle.Empty
        Dim tcParent As TabControl = tabPage.Parent
        If tcParent Is Nothing Then Return Rectangle.Empty
        Dim rValue As Rectangle = tcParent.GetTabRect(tcParent.TabPages.IndexOf(tabPage))
        rValue.Inflate(0, 6)
        Return rValue
    End Function

#End Region

End Module
