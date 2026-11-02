'''<summary>Determines the way date scroller items are placed in a date scroller.</summary>
Public Enum DateScrollerItemPlacement
    '''<summary>Items are placed flowing downard so that no two items take up the same column.</summary>
    Waterfall = 0
    '''<summary>Items are placed by group, where the groups are in a waterfall and each group is placed in a waterfall.</summary>
    GroupedWaterfall = 1
    '''<summary>items are placed as compact as possible.</summary>
    Compact = 2
    '''<summary>Items are placed by group, where the groups are in a waterfall and each group is placed in a compact way.</summary>
    GroupedCompact = 3
End Enum
