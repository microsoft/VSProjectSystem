# CPS Globbing Behavior
In the new version of CPS support for MSBuild globbing is being added. This document demonstrates the
behavior CPS will have as a user interacts with a globbing-enabled project.

# Use Case - ASP.net Core

This use case describes what happens to the users project file as it is interacted with via
Solution and File Explorer.

    * The example project of ASP.NET core is a rough guess of what it may look like, the final globs
    are still being determined.
    * CPS will try to group up Items and Actions together. Blank lines in the project file are used
    to show separation of item groups.

## 1: Including/Excluding files and folders
---------------------------------------------------

1. User creates a new ASP.net core, which includes these default globs.

   ```xml
   <Compile Include="**\*.cs" />

   <Content Include="wwwroot\**" />

   <Compile Remove="wwwroot\**\*.cs" />
   <Compile Remove="obj\**" />
   <Compile Remove="bin\**" />
   ```

2. User adds a *class.cs* at the project root and *script.js* file under wwwroot, via VS or File Explorer.
    * There is no change to the project file, the file automatically appears in solution Explorer.

   ```xml
   <Compile Include="**\*.cs" />

   <Content Include="wwwroot\**" />

   <Compile Remove="wwwroot\**\*.cs" />
   <Compile Remove="obj\**" />
   <Compile Remove="bin\**" />
   <!-- No project file change as all added items are covered by globs -->
   ```

3. User excludes *class.cs*

   ```xml
   <Compile Include="**\*.cs" />

   <Content Include="wwwroot\**" />

   <Compile Remove="wwwroot\**\*.cs" />
   <Compile Remove="obj\**" />
   <Compile Remove="bin\**" />

   <Compile Remove="class.cs" /> <!-- explicit removal added -->
   ```

4. User includes *class.cs*

   ```xml
   <Compile Include="**\*.cs" />

   <Content Include="wwwroot\**" />

   <Compile Remove="wwwroot\**\*.cs" />
   <Compile Remove="obj\**" />
   <Compile Remove="bin\**" />

   <!-- explicit removal deleted to accommodate inclusion via glob -->
   ```

5. User adds *script.js* file at project root.
    * CPS adds an explicit include for that js file.

   ```xml
   <Compile Include="**\*.cs" />

   <Content Include="wwwroot\**" />

   <Compile Remove="wwwroot\**\*.cs" />
   <Compile Remove="obj\**" />
   <Compile Remove="bin\**" />

   <Content Include="script.js" /> <!-- explicit include added because no glob covers it -->
   ```

6. User drags a new folder (described below) under project root in file explorerer,
then Includes the folder through solution explorer.
    * CPS adds globs based on each extension found.

   Folder Contents:
   ```
   New Folder
   |   1.txt
   |   2.txt
   |__ Sub Folder
       | script.js
   ```

   ```xml
   <Compile Include="**\*.cs" />

   <Content Include="New Folder\**\*.txt" /> <!-- Glob includes added based on the contents of the folder -->
   <Content Include="New Folder\**\*.js" />
   <Content Include="wwwroot\**" />

   <Compile Remove="wwwroot\**\*.cs" />
   <Compile Remove="obj\**" />
   <Compile Remove="bin\**" />

   <Content Include="script.js" />
   ```

7. User excludes *New Folder*.
    * Includes are deleted, removals are added.
    * CPS determines which globs potentially cover the folder to be excluded, and adds a removal for each one.

   ```xml
   <Compile Include="**\*.cs" />

   <Content Include="wwwroot\**" />

   <Compile Remove="New Folder\**" /> <!-- CPS will add a glob remove, Compile is the only glob covering this folder -->
   <Compile Remove="wwwroot\**\*.cs" />
   <Compile Remove="obj\**" />
   <Compile Remove="bin\**" />

   <Content Include="script.js" />
   ```

8. User deletes *New Folder*.
    * New Folder is **already excluded**, so CPS does not edit the project file when deleting it
    * This means the Remove item will be left behind when deleting this folder.

   ```xml
   <Compile Include="**\*.cs" />

   <Content Include="wwwroot\**" />

   <Compile Remove="New Folder\**" /> <!-- CPS leaves this behind -->
   <Compile Remove="wwwroot\**\*.cs" />
   <Compile Remove="obj\**" />
   <Compile Remove="bin\**" />

   <Content Include="script.js" />
   ```

9. User adds *New Folder* through solution explorer.
    * New Folder removals are deleted, folder include is added.
    * Adding through file explorer, then selecting *Include* behaves the same.

   ```xml
   <Compile Include="**\*.cs" />

   <Content Include="wwwroot\**" />

   <Compile Remove="wwwroot\**\*.cs" />
   <Compile Remove="obj\**" />
   <Compile Remove="bin\**" />

   <Content Include="script.js" />

   <Folder Include="New Folder\" /> <!-- Remove deleted, Include added -->
   ```

10. User deletes *wwwroot* from solution explorer.
    * Includes and Removes that start with *wwwroot* are removed.
    * *wwwroot* is special to ASP.NET core, so it would be up to their extensions to
    prevent CPS from deleting these globs, *or* to recreate the specific globs when
    the user re-adds *wwwroot*.

   ```xml
   <Compile Include="**\*.cs" />
   <!-- wwwroot include is deleted -->

   <!-- wwwroot remove is deleted because it was included at the time of deletion -->
   <Compile Remove="obj\**" />
   <Compile Remove="bin\**" />

   <Content Include="script.js" />

   <Folder Include="New Folder\" />
   ```

11. User excludes *New Folder* and adds *class.cs* to that folder via File Explorer,
then includes the folder.
    * No Compile include is added because an existing glob already covers it

   ```xml
   <Compile Include="**\*.cs" />
   <!-- This compile glob already covers class.cs, so a new one is not added -->

   <!-- The Compile Remove="New Folder\**" is deleted to accomadate the include -->
   <Compile Remove="obj\**" />
   <Compile Remove="bin\**" />

   <Content Include="script.js" />
   ```

## 2: Renaming and Changing ItemTypes of  Files and Folders
-------------------------------------------------------------------

1. Again, start with a new ASP.NET Core project.

   ```xml
   <Compile Include="**\*.cs" />

   <Content Include="wwwroot\**" />

   <Compile Remove="wwwroot\**\*.cs" />
   <Compile Remove="obj\**" />
   <Compile Remove="bin\**" />
   ```

2. User renames *wwwroot* to *root*.
    * Includes and Removes are renamed.

   ```xml
   <Compile Include="**\*.cs" />

   <Content Include="root\**" /> <!-- renamed -->

   <Compile Remove="root\**\*.cs" /> <!-- renamed -->
   <Compile Remove="obj\**" />
   <Compile Remove="bin\**" />
   ```

3. User has added *class.txt* to the project.

   ```xml
   <Compile Include="**\*.cs" />

   <Content Include="root\**" />

   <Compile Remove="root\**\*.cs" />
   <Compile Remove="obj\**" />
   <Compile Remove="bin\**" />

   <Content Include="class.txt" /> <!-- explicit include added -->
   ```

4. User renames *class.txt* to *class.cs* and changes item type to *Compile*.
    * The compile glob now covers this item, so we remove the include.

   ```xml
   <Compile Include="**\*.cs" />

   <Content Include="root\**" />

   <!-- explicit include removed -->

   <Compile Remove="root\**\*.cs" />
   <Compile Remove="obj\**" />
   <Compile Remove="bin\**" />
   ```

5. User has added, excluded, and deleted a folder *MyFolder*.
    * This results in the lingering remove item.

   ```xml
   <Compile Include="**\*.cs" />

   <Content Include="root\**" />

   <Compile Remove="MyFolder\**" /> <!-- lingering remove due to exclusion -->
   <Compile Remove="root\**\*.cs" />
   <Compile Remove="obj\**" />
   <Compile Remove="bin\**" />
   ```

6. User adds a new folder *New Folder* and adds *class.cs* to it.
    * A folder item is added then removed as *class.cs* is covered by a glob

   ```xml
   <Compile Include="**\*.cs" />

   <Content Include="root\**" />

   <!-- no change -->
   <Compile Remove="MyFolder\**" />
   <Compile Remove="root\**\*.cs" />
   <Compile Remove="obj\**" />
   <Compile Remove="bin\**" />
   ```

7. User renames *New Folder* to *MyFolder*
    * *MyFolder* remove is deleted.

   ```xml
   <Compile Include="**\*.cs" />

   <Content Include="root\**" />

   <!-- Remove deleted to prevent contents of the folder being excluded -->
   <Compile Remove="root\**\*.cs" />
   <Compile Remove="obj\**" />
   <Compile Remove="bin\**" />
   ```

## 3: Metadata with Globbing
------------------------------------

1. Again, start with a new ASP.NET Core project.

   ```xml
   <Compile Include="**\*.cs" />

   <Content Include="wwwroot\**" />

   <Compile Remove="wwwroot\**\*.cs" />
   <Compile Remove="obj\**" />
   <Compile Remove="bin\**" />
   ```

2. User adds *class.cs* and changes *Copy To Output Directory* to *Copy Always*
    * CPS adds an Update item for class.cs since it is from a glob

   ```xml
   <Compile Include="**\*.cs" />

   <Content Include="wwwroot\**" />

   <Compile Remove="wwwroot\**\*.cs" />
   <Compile Remove="obj\**" />
   <Compile Remove="bin\**" />

   <Compile Update="class.cs"> <!-- Update item added -->
       <CopyToOutputDirectory>Always</CopyToOutputDirectory>
   </Compile>
   ```

3. User sets *Copy To Output Directory* back to default
    * CPS deletes update item

   ```xml
   <Compile Include="**\*.cs" />

   <Content Include="wwwroot\**" />

   <Compile Remove="wwwroot\**\*.cs" />
   <Compile Remove="obj\**" />
   <Compile Remove="bin\**" />
   <!-- update item deleted -->
   ```

4. User sets *Copy To Output Directory* to *Copy Always* again, then excludes class.cs
    * Update item is deleted, removal is added

   ```xml
   <Compile Include="**\*.cs" />

   <Content Include="wwwroot\**" />

   <Compile Remove="wwwroot\**\*.cs" />
   <Compile Remove="obj\**" />
   <Compile Remove="bin\**" />
   <!-- no update item -->

   <Compile Remove="class.cs" />
   ```

# General Behavior

## Behavior at a glimpse
------------------------
1. The focus of CPS is that actions via Solution Explorer will edit the project file, even if it
ends up removing/changing some special globs. If a user wants to manipulate the folder structure
without editing globs, then File Explorer can be used.
    * The exception is excluded files, that will never edit the project.
2. Renaming or moving an included folder or file will always result in those item(s) remaining 
included.
    * However, extra items may end up included.
3. Operations on excluded items will never edit the project file.
4. When including a folder, CPS will try to add items via globs.
5. There is no memory of former globs in the project, so excluding then including a folder may not
result in the same globs being added back.

## Exclude vs Remove
--------------------
In MSBuild there is two ways to exclude an item from a glob. An *Exclude* is declared along with an
include and will exclude it from only that include. A *Remove* will remove from all items logically
declared before the remove. CPS will primarily use removes, but will respect excludes. CPS will only
delete parts of excludes and never add to them. Any new exclude/remove writes to the project file by
CPS will be done via removes.

\* if it later becomes that Exclude and Remove is interchangeable (and *Exclude* can be independent
of *Include*) then we can revisit which label CPS uses.

## Files
--------
### Including a File
CPS scans the directory automatically, so if a file appears under the project directory it will be
automatically included if it is covered by a glob. When manually including an excluded item CPS
will first check for removals and exclusions, deleting those if possible. If that does not work an
explicit include will be added for the item.

### Excluding a File
To remove a single file from a project, CPS will delete any explicit includes and then add explicit
removes for any globs that cover the file.

### Deleting a File
If a file is included via a glob, then CPS will only delete the file off disk.

### Renaming a File
A rename will consider globs. Renaming to something under globs will allow the item to be included
via the glob. If the source include is an explicit include, then it will be removed in this case.

## Folders
----------
CPS will try to perform as many folder operations as possible via globbing. CPS will only edit globs
that are determined to belong directly to that folder cone. IE: `<Content Include="Content\**"/>`
belongs directly to the folder *Content* as it starts with that exact string and then a path separator.

### Including a Folder
CPS will first delete removals and excludes that belong directly to the folder being including. Then
globs for that folder will be added on a per-extension bases as needed. If an item is already covered
by a glob, a new one will not be added.

\* Note for empty folders: MSBuild globbing evaluation matches only files and not folders. This means
an empty folder will never be included as part of a glob. Even using `<Folder Include="**" />` will only
match files and not folders.

### Excluding a Folder
CPS will delete any includes, literal or glob, that belong directly to the folder being excluded. Then 
glob removals will be added for any remaining globs that cover the folder.

Performing operations on an excluded folder will not edit the project file. Even if the user renames
or deletes folder *A* and `<Content Remove="A\**"/>` exists, that glob will not be renamed or deleted.
This means that a rename of a removed folder may include it back in the project. 

\* Excludes/Removes *may* be edited during operations on included items 

### Deleting a Folder
Deleting an included folder will delete all globs directly belonging to that folder, and then any
string literal items that fall under the folder are also deleted.

### Renaming a Folder
Renaming a folder is perhaps the most complex scenario CPS can handle in regards to globs. This is
because both the source name and target name can be a part of globs through a variety of ways. As a
result CPS will have fairly limited logic in this scenario. A rename will rename any globs that
directly belong to the folder, regardless if there is already existing globs for the target name. 
A rename of an included folder will also delete any target-name removal globs. This is to allow the
folder to remain included without needing to add more includes to the project file.

CPS will ensure that what was included before remains included during a rename. However, with 
globbing a rename may cause duplicate includes or new items being included. Renaming an excluded 
folder may cause the folder to become included.

### Drag/Drop & Copy/Cut/Paste
A drag/drop and cut/paste, or a *move* operation will rename the sources when possible. This will 
behave similarly to a rename in most common scenarios. When a rename is not possible it will perform
a remove/include operation. A copy/paste, or *copy* operation will always behave as an add operation.
This means copying a folder will add globs, but copying all the files from a folder will add explicit
includes (where not already covered by globs).
