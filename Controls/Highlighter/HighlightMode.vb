'''<summary>Determines how a highlightable control behaves when highlighting.</summary>
Public Enum HighlightMode
    '''<summary>Highilght control reacts to nothing.</summary>
    Never = 0
    '''<summary>Highlight control reacts only to itself.</summary>
    Self = 1
    '''<summary>Highlight control reacts to iteself and its children.</summary>
    SelfAndChildren = 2
End Enum
