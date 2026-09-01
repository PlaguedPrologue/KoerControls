The StaticColumnsDataGridView class is meant to be inherited by user controls.

To create a new StaticColumnsDataGridView, start by createing a new User Control(Window Forms) item.
In the Designer.vb class of that item, replace the 'Inherits System.Windows.Forms.UserControl' line at the top with 'Inherits KoerControls.StaticColumnsDataGridView'
Remove the 'Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font' line in the 'InitilizeComponent()' function.
Add <ToolboxItem(True)> to the code base of that new User Control.
Start editing as normal. When the project containing the new User Control is rebuilt, it can be added to a new form and will automatically populate with the pre-defined columns.