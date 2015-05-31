Custom reference types
======================

If your project has a new kind of reference that is "resolved" during the
build (e.g., converted from some simple form that the user may recognize to
an absolute path on the machine running the build), CPS may support such
references by exporting a few interfaces, as described below.

But consider first whether existing reference types may accommodate your
needs. 

### Step 1. Read, write, resolve your reference type

First it will be useful to write a component that knows how to read, write,
and resolve your references by implementing the `IResolvableReferencesService` 
interface.

As a part of this step, you should also define a new `.xaml` rule file
for the item type representing your custom reference. You can follow the
steps in [Custom item types](Custom_item_types.md) except do not add your
new rule to the `ProjectItemsSchema.xaml` file as your reference does not
represent a source item.

### Step 2. Add a tab in Reference Manager for your reference type

To support the Reference Manager dialog, you will need to export the 
`IVsReferenceManagerUserAsync` interface.

### Step 3. Make your custom reference type appear in Solution Explorer's References node

Currently the References node in Solution Explorer is not extensible to
new types of references. But if your custom project type has only your
custom references, this isn't a problem as you can supply an entirely new
References node.

You can define a References node by exporting 
[`IProjectTreeProvider`](IProjectTreeProvider.md). Implementations of this 
interface should derive from `ProjectTreeProviderBase`.

### TODO

Cover DTE, vslangproj access to references. Any other places in CPS where
the types of references are hard-coded?
