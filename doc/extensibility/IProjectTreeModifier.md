`IProjectTreeModifier`
======================

The `IProjectTreeModifier` interface can be exported into the 
`ConfiguredProject` scope to apply cosmetic changes to a project tree 
or one of its nodes. This allows you to, for example, set the icon on 
your project's root node to be consistent with your branding. You can 
also set the icons on specific source items that are may be unique to 
your project type.

Do NOT use this to modify the structure of the tree. Adding, removing, or
moving nodes in the tree in a modifier will most likely cause malfunctions
in the project system.

You can create an `IProjectTreeModifier` export using the "Project Tree
Modifier extension" item template included in the CPS SDK.
