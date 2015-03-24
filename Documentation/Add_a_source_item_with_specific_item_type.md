Add a source item with specific item type
=========================================

Visual Studio project systems like an IVs* interface for adding items
with explicitly specified item types. Normally this is sufficient because
the project system can pick reasonable defaults for item types based
on file extension or expected project behavior. These defaults [can be
augmented](onenote:Documentation.one#Custom%20item%20types&section-id={949F487D-4BC2-4B7C-B889-A62D8BB6120F}&page-id={853099AB-5896-4EDB-8F48-D07E64BB2000}&base-path=http://devdiv/sites/vspe/prjbld/OneNote/TeamInfo/CPS).
If you still need to explicitly specify an item type for a project item,
follow these steps:


ConfiguredProject configuredProject; // [obtain a
ConfiguredProject](onenote:Documentation.one#Obtaining%20ConfiguredProject&section-id={949F487D-4BC2-4B7C-B889-A62D8BB6120F}&page-id={7072BCCB-F3BF-4944-93A3-9D720B68F518}&base-path=http://devdiv/sites/vspe/prjbld/OneNote/TeamInfo/CPS)

IProjectItemProvider sourceItems = configuredProject.Services.SourceItems;

await sourceItems.AddAsync("CustomItemType", "projectRelativePath\ToYourFile.xpp");


When using this approach to add source files to the project, CPS will:

- Add the source file to source control, if it is within the project directory.
- Automatically add the item to Solution Explorer, DTE, and raise other appropriate events if indeed the item you're adding has an item type that belongs to [the set of source item types](onenote:Documentation.one#Custom%20item%20types&section-id={949F487D-4BC2-4B7C-B889-A62D8BB6120F}&page-id={853099AB-5896-4EDB-8F48-D07E64BB2000}&base-path=http://devdiv/sites/vspe/prjbld/OneNote/TeamInfo/CPS).
CPS will not:

- Copy the file into the project directory if it is not already there.
- Expand an item template
    
