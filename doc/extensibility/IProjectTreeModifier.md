`IProjectTreeModifier` and `IProjectTreeModifier2`
======================

**[Item template:](project_item_templates.md)** Project Tree Modifier extension

The `IProjectTreeModifier` interface can be exported into the 
`UnconfiguredProject` scope to apply cosmetic changes to a project tree 
or one of its nodes. `IProjectTreeModifier.ApplyModifications` is called back
for every visible and non-visible node in the tree.

This allows you to, for example, set the icon on 
your project's root node to be consistent with your branding. You can 
also set the icons on specific source items that are may be unique to 
your project type.

Do NOT use this to modify the structure of the tree. Adding, removing, or
moving nodes in the tree in a modifier will most likely cause malfunctions
in the project system. 

To change a node based on its logical position 
within the tree (for example, the special `My Project` folder within Visual 
Basic projects), your modifier should act on the children of the project root 
(ie when the specified `tree` has a capability of `ProjectTreeCapabilities.ProjectRoot`).
This is because tree modifiers are called to update existing and to initialize new nodes. 
For new nodes, there is way to know where it will fit logically into the tree, until it's been
actually added to the tree.

You can create an `IProjectTreeModifier` export using the "Project Tree
Modifier extension" item template included in the CPS SDK.

IProjectTreeModifier implementations can also implement (but not export) 
`IProjectTreeModifier2` to be passed the original tree before the latest 
mutation, so that it can determine what a previous tree modifier changed.
This can prevent multiple modifiers from updating the same
properties repeatedly. If a tree modifier implements this interface, 
`IProjectTreeModifier2.ApplyModifications` will be called instead of 
`IProjectTreeModifier.ApplyModifications`.
