'''<summary>Specifies whether an object or text is aligned to the left or right of of a reference point. Used for relative alignment between two objects looking at two respective points.</summary>
Public Enum LeftRightRelativeAlignment
    '''<summary>Both objects or texts are aligned to the left of their reference points.</summary>
    Left = 0
    '''<summary>Both objects or texts are aligned to the right of their reference points.</summary>
    Right = 1
    '''<summary>The objects or texts are aligned in a way that pushes them away from each other in accordance to their reference points.</summary>
    Outer = 2
    '''<summary>The objects or texts are aligned in a way that pulls them towards each other in accordance to their reference points.</summary>
    Inner = 3
End Enum
