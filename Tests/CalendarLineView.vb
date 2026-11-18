Imports System.ComponentModel
Imports System.Windows.Forms

<ToolboxItem(True)>
Public Class CalendarLineView

#Region "Private Items"

    Private Sub TasksGrid_Scroll(sender As Object, e As ScrollEventArgs) Handles TasksGrid.Scroll
        If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
            BucketsGrid.HorizontalScroll.Value = e.NewValue
            BucketsGrid.HorizontalScroll.Value = e.NewValue
        End If
    End Sub
    'Private Sub BucketsGrid_Scroll(sender As Object, e As ScrollEventArgs) Handles BucketsGrid.Scroll
    '    If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
    '        TasksGrid.HorizontalScroll.Value = e.NewValue
    '        TasksGrid.HorizontalScroll.Value = e.NewValue
    '    End If
    'End Sub

#End Region

End Class
