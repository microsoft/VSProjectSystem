Add a source item with specific item type
=========================================

Visual Studio project systems like an `IVs`* interface for adding items
with explicitly specified item types. Normally this is sufficient because
the project system can pick reasonable defaults for item types based
on file extension or expected project behavior. These defaults [can be
augmented](custom_item_types.md). If you still need to explicitly 
specify an item type for a project item, follow these steps:

    ConfiguredProject configuredProject;
    IProjectItemProvider sourceItems = configuredProject.Services.SourceItems;
    await sourceItems.AddAsync("CustomItemType", "projectRelativePath\ToYourFile.xpp");

When using this approach to add source files to the project, CPS *will*:

1. Add the source file to source control, if it is within the project 
   directory.
2. Automatically add the item to Solution Explorer, DTE, and raise other
   appropriate events if indeed the item you're adding has an item type that
   belongs to [the set of source item types](custom_item_types.md).

CPS will *not*:

1. Copy the file into the project directory if it is not already there.
2. Expand an item template
