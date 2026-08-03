Imports System.Reflection
Imports System.Runtime.CompilerServices

Partial Public Class Highlighter

    Friend Class ColorPainter
        Implements IDisposable

#Region "Private Items"

        Private WithEvents MyOwner As Control

        Private _backColor As HighlightableColor
        Private _childContainsCursor As Boolean
        Private ReadOnly _childColors As New ConditionalWeakTable(Of Control, HighlightableColorHandler)
        Private _containsCursor As Boolean
        Private _highlighted As Boolean
        Private Shared ReadOnly _ownerClickEvent As MethodInfo = GetType(Control).GetMethod("OnClick", BindingFlags.Instance Or BindingFlags.NonPublic)
        Private _settingBackgroundColor As Boolean

        Private Sub Child_BackColorChanged(sender As Object, e As EventArgs)
            Dim cControl As Control = TryCast(sender, Control)
            If cControl Is Nothing Then Return 'Determine if a Control is having its background color changed
            Dim hchColor As HighlightableColorHandler = Nothing
            If _childColors.TryGetValue(cControl, hchColor) AndAlso Not hchColor.SettingBackgroundColor Then 'Determine if Control has a highlightable background color and make sure it is not currently being set
                hchColor.SettingBackgroundColor = True 'State that the Control is having its background color set
                hchColor.Color = HighlightableColor.FromControl(cControl, _highlighted) 'Set the highlightable BackColor of Control
                hchColor.SettingBackgroundColor = False 'State that the Control is no longer having its background color set
            End If
        End Sub
        Private Sub Child_Clicked(sender As Object, e As EventArgs)
            Dim cClickedControl As Control = TryCast(sender, Control)
            While cClickedControl IsNot Nothing 'Go up the list of parents of the Control clicked
                If ReferenceEquals(cClickedControl, MyOwner) Then 'Determine if parent is the owning Control
                    _ownerClickEvent?.Invoke(MyOwner, {e}) 'Raise the owning Control's click event
                    Return 'Control clicked, exit
                End If
                If cClickedControl.TabStop OrElse _managedControls.Contains(cClickedControl) Then Return 'If the Control clicked can be selected or is in the list of managed Controls in the Highlighter (owning Control already checked), the owning Control cannot be clicked
                cClickedControl = cClickedControl.Parent 'Go up to the next parent
            End While
        End Sub
        Private Sub Child_MouseEnter(sender As Object, e As EventArgs)
            Dim cClickedControl As Control = TryCast(sender, Control)
            While cClickedControl IsNot Nothing 'Go up the list of parents of the Control clicked
                If ReferenceEquals(cClickedControl, MyOwner) Then 'Determine if parent is the owning Control
                    _childContainsCursor = True 'State that child Control contains the cursor
                    RefreshHighlighted() 'Referesh the highlight status of the Control
                    Return 'Control clicked, exit
                End If
                If cClickedControl.TabStop OrElse _managedControls.Contains(cClickedControl) Then Return 'If the Control clicked can be selected or is in the list of managed Controls in the Highlighter (owning Control already checked), the owning Control cannot be stated as entered
                cClickedControl = cClickedControl.Parent 'Go up to the next parent
            End While
        End Sub
        Private Sub Child_MouseLeave(sender As Object, e As EventArgs)
            Dim cClickedControl As Control = TryCast(sender, Control)
            While cClickedControl IsNot Nothing 'Go up the list of parents of the Control clicked
                If ReferenceEquals(cClickedControl, MyOwner) Then 'Determine if parent is the owning Control
                    _childContainsCursor = False 'State that child Control does not contain the cursor
                    RefreshHighlighted() 'Referesh the highlight status of the Control
                    Return 'Control clicked, exit
                End If
                If cClickedControl.TabStop OrElse _managedControls.Contains(cClickedControl) Then Return 'If the Control clicked can be selected or is in the list of managed Controls in the Highlighter (owning Control already checked), the owning Control cannot be stated as entered
                cClickedControl = cClickedControl.Parent 'Go up to the next parent
            End While
        End Sub
        Private Sub MyOwner_BackColorChanged(sender As Object, e As EventArgs) Handles MyOwner.BackColorChanged
            If _settingBackgroundColor Then Return 'Do nothing if background color is already being set
            _settingBackgroundColor = True 'State that the background color is being set
            MyOwner.SuspendLayout() 'Pause layout to help with UI updates
            _backColor = HighlightableColor.FromARGB(MyOwner.GetDisplayedBackColor) 'Set the highlight BackColor of the owning Control
            MyOwner.BackColor = _backColor.GetColor(_highlighted) 'Set the BackColor of the owning Control
            MyOwner.ResumeLayout() 'Resume layout to allow UI updates
            _settingBackgroundColor = False 'State that the background color is finished being set
        End Sub
        Private Sub MyOwner_ControlAdded(sender As Object, e As ControlEventArgs) Handles MyOwner.ControlAdded
            AddHandler e.Control.BackColorChanged, AddressOf Child_BackColorChanged
            AddHandler e.Control.Click, AddressOf Child_Clicked
            AddHandler e.Control.ControlAdded, AddressOf MyOwner_ControlAdded
            AddHandler e.Control.ControlRemoved, AddressOf MyOwner_ControlRemoved
            AddHandler e.Control.MouseEnter, AddressOf Child_MouseEnter
            AddHandler e.Control.MouseLeave, AddressOf Child_MouseLeave
            _childColors.Add(e.Control, New HighlightableColorHandler(HighlightableColor.FromControl(e.Control)))
            If _highlighted Then SetIndirectlyHighlighted(e.Control, _highlighted)
        End Sub
        Private Sub MyOwner_ControlRemoved(sender As Object, e As ControlEventArgs) Handles MyOwner.ControlRemoved
            RemoveHandler e.Control.BackColorChanged, AddressOf Child_BackColorChanged
            RemoveHandler e.Control.Click, AddressOf Child_Clicked
            RemoveHandler e.Control.ControlAdded, AddressOf MyOwner_ControlAdded
            RemoveHandler e.Control.ControlRemoved, AddressOf MyOwner_ControlRemoved
            RemoveHandler e.Control.MouseEnter, AddressOf Child_MouseEnter
            RemoveHandler e.Control.MouseLeave, AddressOf Child_MouseLeave
            Dim hchControlColor As HighlightableColorHandler = Nothing
            If _childColors.TryGetValue(e.Control, hchControlColor) Then
                _childColors.Remove(e.Control)
                If _highlighted Then e.Control.BackColor = hchControlColor.Color.UnhighlightedColor
            End If
        End Sub
        Private Sub MyOwner_GotFocus(sender As Object, e As EventArgs) Handles MyOwner.GotFocus
            RefreshHighlighted()
        End Sub
        Private Sub MyOwner_LostFocus(sender As Object, e As EventArgs) Handles MyOwner.LostFocus
            RefreshHighlighted()
        End Sub
        Private Sub MyOwner_MouseEnter(sender As Object, e As EventArgs) Handles MyOwner.MouseEnter
            _containsCursor = True
            RefreshHighlighted()
        End Sub
        Private Sub MyOwner_MouseLeave(sender As Object, e As EventArgs) Handles MyOwner.MouseLeave
            _containsCursor = False
            RefreshHighlighted()
        End Sub

#End Region

#Region "Initializers"

        Public Sub New(owner As Control)
            MyOwner = owner

            _backColor = HighlightableColor.FromARGB(owner.GetDisplayedBackColor) 'Set the highlight BackColor of the owning Control
            For Each cControl As Control In owner.GetAllControls 'Go through each Control contained in the owning Control
                _childColors.Add(cControl, New HighlightableColorHandler(HighlightableColor.FromControl(cControl))) 'Set the highlight BackColor of the child Control
                AddHandler cControl.BackColorChanged, AddressOf Child_BackColorChanged
                AddHandler cControl.Click, AddressOf Child_Clicked
                AddHandler cControl.ControlAdded, AddressOf MyOwner_ControlAdded
                AddHandler cControl.ControlRemoved, AddressOf MyOwner_ControlRemoved
                AddHandler cControl.MouseEnter, AddressOf Child_MouseEnter
                AddHandler cControl.MouseLeave, AddressOf Child_MouseLeave
            Next
        End Sub

#End Region

#Region "Properties"

        '''<summary>Determines if the <see cref="Control"/> is currently highlighted.</summary>
        '''<returns><see langword="True"/> if the <see cref="Control"/> is currently highlighted; otherwise <see langword="False"/>.</returns>
        Public ReadOnly Property Highlighted As Boolean
            Get
                Return _highlighted
            End Get
        End Property

#End Region

#Region "Functions"

        Public Overridable Sub Dispose() Implements IDisposable.Dispose
            If _highlighted Then
                For Each cControl As Control In MyOwner.GetAllControls
                    RemoveHandler cControl.Click, AddressOf Child_Clicked
                    RemoveHandler cControl.ControlAdded, AddressOf MyOwner_ControlAdded
                    RemoveHandler cControl.ControlRemoved, AddressOf MyOwner_ControlRemoved
                    RemoveHandler cControl.MouseEnter, AddressOf Child_MouseEnter
                    RemoveHandler cControl.MouseLeave, AddressOf Child_MouseLeave
                    Dim hchControlColor As HighlightableColorHandler = Nothing
                    If _childColors.TryGetValue(cControl, hchControlColor) Then
                        _childColors.Remove(cControl) 'Remove Control from list before setting color to help short-circuit the Child_BackColorChanged function
                        cControl.BackColor = hchControlColor.Color.UnhighlightedColor
                    End If
                Next
                _settingBackgroundColor = True
                MyOwner.BackColor = _backColor.UnhighlightedColor

            Else
                For Each cControl As Control In MyOwner.GetAllControls
                    RemoveHandler cControl.Click, AddressOf Child_Clicked
                    RemoveHandler cControl.ControlAdded, AddressOf MyOwner_ControlAdded
                    RemoveHandler cControl.ControlRemoved, AddressOf MyOwner_ControlRemoved
                    RemoveHandler cControl.MouseEnter, AddressOf Child_MouseEnter
                    RemoveHandler cControl.MouseLeave, AddressOf Child_MouseLeave
                    Dim hchControlColor As HighlightableColorHandler = Nothing
                Next
            End If
        End Sub
        Protected Overridable Sub RefreshHighlighted()
            If _settingBackgroundColor Then Return 'Do nothing if background color is already being set
            Dim bHighlighted As Boolean = _childContainsCursor OrElse _containsCursor 'Determine if the owning Control should be highlighted
            If bHighlighted = _highlighted Then Return 'Highlight status does not change
            _settingBackgroundColor = True 'State that the background color is being set
            _highlighted = bHighlighted 'Update the highlight status
            MyOwner.SuspendLayout() 'Suspend layout to help with UI updates
            MyOwner.BackColor = _backColor.GetColor(bHighlighted) 'Set the background color of the owning Control
            For Each cChild As Control In MyOwner.Controls 'Go through each child Control
                SetIndirectlyHighlighted(cChild, bHighlighted) 'Refresh the background color of the child Control
            Next
            MyOwner.ResumeLayout() 'Resume layout to allow UI updates
            _settingBackgroundColor = False 'State that the background color is finished being set
        End Sub
        Protected Overridable Sub SetIndirectlyHighlighted(control As Control, highlighted As Boolean)
            If _managedControls.Contains(control) Then Return 'Do nothing if Control is in the list of managed Controls for the HIghlighter
            If control.BackColor <> Color.Transparent Then 'If the BackColor of the Control is transparent, there is no need to change the color
                Dim hchControlColor As HighlightableColorHandler = Nothing
                If _childColors.TryGetValue(control, hchControlColor) AndAlso Not hchControlColor.SettingBackgroundColor Then 'Determine if Control has a highlightable background color and make sure it is not currently being set
                    hchControlColor.SettingBackgroundColor = True 'State that the Control is having its background color set
                    control.BackColor = hchControlColor.Color.GetColor(highlighted) 'Set BackColor of Control
                    hchControlColor.SettingBackgroundColor = False 'State that the Control is no longer having its background color set
                End If
            End If
            For Each cChild As Control In control.Controls 'Go through each child Control
                SetIndirectlyHighlighted(cChild, highlighted) 'Refresh the background color of the child Control
            Next
        End Sub

#End Region

    End Class

End Class
