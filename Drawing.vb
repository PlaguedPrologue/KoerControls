Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices

Public Module Drawing

    '''<summary>Gets each line of text based on the <see cref="Font"/> and width available.</summary>
    '''<returns>Each line of the text based on the <see cref="Font"/> and width to fill.</returns>
    '''<param name="font">The <see cref="Font"/> of the text.</param>
    '''<param name="text">The text to reformat.</param>
    '''<param name="width">The total width available.</param>
    <Extension>
    Public Iterator Function GetLinesForWidth(font As Font, text As String, width As Integer) As IEnumerable(Of String)
        If String.IsNullOrEmpty(text) Then 'Determine if text is empty
            Yield text 'Return text
            Return 'Exit early
        End If
        Dim iSingleSpaceWidth As Integer = TextRenderer.MeasureText(" ", font).Width 'Get the width of a single space character
        Dim iSpaceWidth As Integer = TextRenderer.MeasureText("  ", font).Width - iSingleSpaceWidth 'For some reason, measuring the text adds more width
        Dim iWidthAdder As Integer = iSingleSpaceWidth - iSpaceWidth 'Get the additional width the text renderer adds
        Dim sNewLine As String = "" 'Setup first new line
        Dim sCurrentLines As String() = text.Split({vbNewLine, vbLf, vbCr}, StringSplitOptions.None) 'Get each current line
        For Each sCurrentLine As String In sCurrentLines 'Go through each line of the text
            Dim iNewLineWidth As Integer = 0 'Setup new line width
            For Each sWord As String In sCurrentLine.Split({" "}, StringSplitOptions.None) 'Go through each word in the current line
                Dim iWordWidth As Integer = TextRenderer.MeasureText(sWord, font).Width 'Get width of word
                If iNewLineWidth = 0 Then 'Determine if starting new line
                    sNewLine = sWord 'Set new line to word
                    iNewLineWidth = iWordWidth 'Set new line width to word width
                ElseIf iNewLineWidth + iSpaceWidth + iWordWidth + iWidthAdder > width Then 'Determine if adding word to new line will exceed bounds
                    Yield sNewLine 'Return new line
                    sNewLine = sWord 'Set new line to word
                    iNewLineWidth = iWordWidth 'Set new line width to word width  
                Else
                    sNewLine = $"{sNewLine} {sWord}" 'Add word to new line 
                    iNewLineWidth += iSpaceWidth + iWordWidth 'Add space width and word width to new line width
                End If
            Next
            Yield sNewLine 'Return last new line of current line
        Next
    End Function
    '''<summary>Gets each line of text based on the <see cref="Font"/> and height available.</summary>
    '''<remarks>Current version DOES NOT respect new line characters. More work required to make that work, and I just don't want to deal with it.</remarks>
    '''<returns>Each line of the text based on the <see cref="Font"/> and height to fill.</returns>
    '''<param name="font">The <see cref="Font"/> of the text.</param>
    '''<param name="text">The text to reformat.</param>
    '''<param name="height">The total height available.</param>
    '''<param name="width">The width of the new text. This parameter is passed uninitialized; any value originally supplied in width will be overwritten.</param>
    <Extension>
    Public Function GetLinesForHeight(font As Font, text As String, height As Integer, <Out> ByRef width As Integer) As String
        Dim sCurrentLines As IEnumerable(Of String) = text.Split({vbCrLf, vbLf, vbCr}, StringSplitOptions.None) 'Get current lines
        Dim iFontHeight As Integer = font.Height
        If sCurrentLines.Count * (iFontHeight + 1) < height Then 'Determine if no new lines can be added
            Return text 'Return current lines

        Else 'New lines can be added
            'Get initial info
            Dim iLineCount = height \ iFontHeight 'Get line count
            Dim tliLines(iLineCount) As TextLineIndexer 'Setup lines
            Dim sWords() As String = text.Split({" ", vbNewLine, vbLf, vbCr}, StringSplitOptions.RemoveEmptyEntries) 'Get each word
            Dim iWordCount As Integer = sWords.Length 'Get word count
            Dim iSpaceWidth As Integer = TextRenderer.MeasureText(" ", font).Width - TextRenderer.MeasureText(" ", font).Width 'Get width of the [space] character

            'Create initial lines
            Dim iPreviousLineIndex As Integer = 0 'Setup indexer for previous line in loop
            tliLines(0) = New TextLineIndexer(iSpaceWidth) 'Setup first line
            For iWordIndex As Integer = 0 To iWordCount - 1 'Go through each word
                Dim wiWord As New WordIndexer(sWords(iWordIndex), font, iWordIndex) 'Get word from index
                Dim iLineIndex As Integer = (iWordIndex * iLineCount) \ iWordCount 'Get initial line index of word

                If iLineIndex = iPreviousLineIndex Then 'Determine if new line was not started
                    tliLines(iLineIndex).AddWordToEnd(wiWord) 'Add word to line
                Else 'New line was started
                    tliLines(iLineIndex) = New TextLineIndexer(iSpaceWidth) 'Create new line
                    tliLines(iLineIndex).AddWordToEnd(wiWord) 'Add word to line
                End If
                iPreviousLineIndex = iLineIndex
            Next

            'Rearrange words
            Dim iMaxLineIndex As Integer = iLineCount - 1
            While True 'Continue to rearrange
                'Get the longest line
                Dim iLongestLineIndex As Integer 'Setup longest line index
                Dim iCurrentWidth As Integer = 0 'Setup current width
                For iLineIndex As Integer = 0 To iMaxLineIndex
                    Dim tliLine As TextLineIndexer = tliLines(iLineIndex)
                    If tliLine.Width > iCurrentWidth Then
                        iCurrentWidth = tliLine.Width
                        iLongestLineIndex = iLineIndex
                    End If
                Next

                'Determine if rearrange is useful
                Dim iBestLineIndexRemoveFrom As Integer 'Set up the best line index to remove from
                Dim iBestLineIndexAddTo As Integer 'Set up the best line index to add to
                Dim iLargestDeltaWidth As Integer = 0 'Get largest change in width upon rearrangin
                If iLongestLineIndex > 0 Then 'Determine if longest line is not the first
                    Dim iDeltaWidth As Integer 'Set up width change
                    Dim iWordSwitchComparison As Integer = tliLines(iLongestLineIndex - 1).CompareWordSwitch(tliLines(iLongestLineIndex), iDeltaWidth) 'Get comparison and width change
                    If iWordSwitchComparison = 1 Then 'Should remove from previous line and add to longest line
                        iBestLineIndexRemoveFrom = iLongestLineIndex - 1
                        iBestLineIndexAddTo = iLongestLineIndex
                        iLargestDeltaWidth = iDeltaWidth
                    ElseIf iWordSwitchComparison = -1 Then 'Should remove from longest line and add to previous line
                        iBestLineIndexRemoveFrom = iLongestLineIndex
                        iBestLineIndexAddTo = iLongestLineIndex - 1
                        iLargestDeltaWidth = iDeltaWidth
                    End If
                End If
                If iLongestLineIndex < iMaxLineIndex Then 'Determine if longest line is not the last line
                    Dim iDeltaWidth As Integer 'Set up width change
                    Dim iWordSwitchComparison As Integer = tliLines(iLongestLineIndex).CompareWordSwitch(tliLines(iLongestLineIndex + 1), iDeltaWidth) 'Get comparison and width change
                    If iDeltaWidth > iLargestDeltaWidth Then 'Determine if transferring with next line is better than transferring with previous line
                        If iWordSwitchComparison = 1 Then 'Should remove from longest line and add to next line
                            iBestLineIndexRemoveFrom = iLongestLineIndex
                            iBestLineIndexAddTo = iLongestLineIndex + 1
                        ElseIf iWordSwitchComparison = -1 Then 'Should remove from next line and add to longest line
                            iBestLineIndexRemoveFrom = iLongestLineIndex + 1
                            iBestLineIndexAddTo = iLongestLineIndex
                        End If
                    End If
                End If

                If iLargestDeltaWidth > 0 Then 'Rearranging was deemed useful
                    If iBestLineIndexRemoveFrom > iBestLineIndexAddTo Then 'Remove from start and add to end
                        tliLines(iBestLineIndexRemoveFrom).MoveFirstWordToEndOf(tliLines(iBestLineIndexAddTo))
                    Else 'Remove from end and add to start
                        tliLines(iBestLineIndexRemoveFrom).MoveLastWordToStartOf(tliLines(iBestLineIndexAddTo))
                    End If
                Else 'Rearranging was not deemed useful
                    Exit While
                End If
            End While

            Return String.Join(vbNewLine, tliLines.Select(Function(tli) tli.ToString))



            'While True 'Continue to rearrange
            '    Dim iBestLineIndexRemoveFrom As Integer 'Set up the best line index to remove from
            '    Dim iBestLineIndexAddTo As Integer 'Set up the best line index to add to
            '    Dim iLargestDeltaWidth As Integer = 0 'Get largest change in width upon rearrangin

            '    For iLineIndex As Integer = 0 To iMaxLineIndex
            '        If iLineIndex > 0 Then 'Current line is not first line
            '            Dim iDeltaWidth As Integer 'Set up width change
            '            Dim iWordSwitchComparison As Integer = tliLines(iMaxLineIndex - 1).CompareWordSwitch(tliLines(iMaxLineIndex), iDeltaWidth) 'Get comparison and width change
            '            If iWordSwitchComparison = 1 Then 'Should remove from previous line and add to current
            '                If iDeltaWidth > iLargestDeltaWidth Then 'Determine if word switch is better than best word switch found
            '                    If iWordSwitchComparison = 1 Then 'Should remove from previous line and add to current line
            '                        iBestLineIndexRemoveFrom = iMaxLineIndex - 1
            '                        iBestLineIndexAddTo = iMaxLineIndex
            '                        iLargestDeltaWidth = iDeltaWidth
            '                    Else 'Should remove from current line and add to previous line
            '                        iBestLineIndexRemoveFrom = iMaxLineIndex
            '                        iBestLineIndexAddTo = iMaxLineIndex - 1
            '                        iLargestDeltaWidth = iDeltaWidth
            '                    End If
            '                End If
            '            End If
            '        End If

            '        If iLineIndex < iMaxLineIndex Then 'Current line is not last line
            '            Dim iDeltaWidth As Integer 'Set up width change
            '            Dim iWordSwitchComparison As Integer = tliLines(0).CompareWordSwitch(tliLines(1), iDeltaWidth) 'Get comparison and width change
            '            If iDeltaWidth > iLargestDeltaWidth Then 'Determine if word switch is better than best word switch found
            '                If iWordSwitchComparison = 1 Then 'Should remove from current line and add to next line
            '                    iBestLineIndexRemoveFrom = 0
            '                    iBestLineIndexAddTo = 1
            '                    iLargestDeltaWidth = iDeltaWidth
            '                Else 'Should remove from next lien and add to current line
            '                    iBestLineIndexRemoveFrom = 1
            '                    iBestLineIndexAddTo = 0
            '                    iLargestDeltaWidth = iDeltaWidth
            '                End If
            '            End If
            '        End If
            '    Next

            '    'Transfer best word if found
            '    If iLargestDeltaWidth > 0 Then 'Determine if words should rearrange
            '        If iBestLineIndexRemoveFrom > iBestLineIndexAddTo Then 'Remove from start, add to end
            '            tliLines(iBestLineIndexRemoveFrom).MoveFirstWordToEndOf(tliLines(iBestLineIndexAddTo))
            '        Else 'Remove from end, add to start
            '            tliLines(iBestLineIndexRemoveFrom).MoveLastWordToStartOf(tliLines(iBestLineIndexAddTo))
            '        End If
            '    Else 'Best solution found
            '        Exit While
            '    End If
            'End While



            'Return String.Join(vbNewLine, sNewLines)
        End If
    End Function
    '''<summary>Gets the <see cref="IntegerRange"/> that spans the direction of the <see cref="RectangleF"/>.</summary>
    '''<returns>The <see cref="IntegerRange"/> that spans the direction of the <see cref="RectangleF"/> if a direction is set; otherwise, and empty <see cref="IntegerRange"/>.</returns>
    <Extension>
    Public Function ToIntegerRange(bounds As RectangleF, direction As Direction) As IntegerRange
        Select Case direction
            Case Direction.Left, Direction.Right
                Return New IntegerRange(bounds.Left, bounds.Right)
            Case Direction.Up, Direction.Down
                Return New IntegerRange(bounds.Top, bounds.Bottom)
            Case Else
                Return IntegerRange.Empty
        End Select
    End Function

    Private Structure WordIndexer

        Public ReadOnly TextIndex As Integer
        Public ReadOnly Width As Integer
        Public ReadOnly Word As String

        Public Sub New(word As String, font As Font, textIndex As Integer)
            Me.TextIndex = textIndex
            Width = TextRenderer.MeasureText(word, font).Width
            Me.Word = word
        End Sub

    End Structure

    Private Class TextLineIndexer

        Private Property BaseWidth As Integer
        Private ReadOnly Words As List(Of WordIndexer)

        Private ReadOnly SpaceWidth As Integer
        Public ReadOnly Property Width As Integer
            Get
                Return BaseWidth
            End Get
        End Property


        Public Sub New(spaceWidth As Integer)
            Me.SpaceWidth = spaceWidth
        End Sub

        Public Sub AddWordToEnd(word As WordIndexer)
            If Words.Count = 0 Then
                BaseWidth = word.Width
            Else
                BaseWidth += SpaceWidth + word.Width
            End If
            Words.Add(word)
        End Sub
        Public Sub AddWordToStart(word As WordIndexer)
            If Words.Count = 0 Then
                BaseWidth = word.Width
            Else
                BaseWidth += SpaceWidth + word.Width
            End If
            Words.Insert(0, word)
        End Sub
        '''<returns>1 if removing from Me and adding to otherLine is best, -1 if adding to Me and removing from otherLine is best, 0 if no change should happen.</returns>
        Public Function CompareWordSwitch(otherLine As TextLineIndexer, <Out> ByRef newWidth As Integer) As Integer
            If Width > otherLine.Width Then 'My width is larger. Transfer word from Me to otherLine
                If Words.Count > 1 Then 'Check if word can be removed. Removing only word would make width = 0. Do not do that.
                    Dim iOtherLineNewWidth As Integer = otherLine.Width + Words(Words.Count - 1).Width + SpaceWidth 'Get the new width of the next line after word transfers
                    If Width > iOtherLineNewWidth Then 'Transferring reduces the width of the bounding box containing both lines
                        newWidth = iOtherLineNewWidth 'Set new width
                        'deltaWidth = Width - iOtherLineNewWidth 'Set delta width
                        Return 1 'State that removing from Me and adding to otherLine is a good option
                    End If
                    newWidth = Width 'Do not change, new width is My width
                End If

            ElseIf otherLine.Width > Width Then 'otherLine's width is larger. Transfer word from otherLine to Me
                If otherLine.Words.Count > 1 Then 'Check if word can be removed. Removing only word would make width = 0. Do not do that.
                    Dim iNewWidth As Integer = Width + otherLine.Words(0).Width + SpaceWidth 'Get the new width of Me after word transfers
                    If otherLine.Width > iNewWidth Then 'Transferring reduces the width of the bounding box containing both lines
                        newWidth = iNewWidth 'Set new width
                        'deltaWidth = otherLine.Width - iNewWidth 'Set delta width
                        Return 0 'Stat that removing from otherLine and adding to Me is a good option
                    End If
                End If
                newWidth = otherLine.Width 'Do not change, new width is otherLine width
            End If

            'Do not change
            Return 0
        End Function
        Public Sub MoveFirstWordToEndOf(other As TextLineIndexer)
            other.AddWordToEnd(Words(0))
            RemoveWordFromStart()
        End Sub
        Public Sub MoveLastWordToStartOf(other As TextLineIndexer)
            other.AddWordToStart(Words(Words.Count - 1))
            RemoveWordFromEnd()
        End Sub
        Public Sub RemoveWordFromEnd()
            If Words.Count = 1 Then
                BaseWidth = 0
            Else
                BaseWidth -= SpaceWidth + Words(Words.Count - 1).Width
            End If
            Words.RemoveAt(Words.Count - 1)
        End Sub
        Public Sub RemoveWordFromStart()
            If Words.Count = 1 Then
                BaseWidth = 0
            Else
                BaseWidth -= SpaceWidth + Words(0).Width
            End If
            Words.RemoveAt(0)
        End Sub
        Public Overrides Function ToString() As String
            Return String.Join(" ", Words.Select(Function(wi) wi.Word))
        End Function

    End Class

End Module
