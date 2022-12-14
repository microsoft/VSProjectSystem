# Content/item types

**[Item template:](../extensibility/project_item_templates.md)** Project Item Type

## Introduction

Currently, only items in your project with recognized item types will appear
in Solution Explorer or be available as project items via automation.

This page describes how to add support for a new item type using the
built-in "Project Item Type" project [item template](../extensibility/project_item_templates.md).

## Tutorial

Let's suppose that you would like to add support to your project type for
a new file that has the extension ".foo".

### Adding support for a new item type

- In the Solution Explorer, Right click on the "Rules" subfolder of your 
  ProjectType project (typically located under [ProjectType]\BuildSystem\Rules)
- From the context menu, select Add -> New Item to invoke the Add New Item 
  dialog (see screenshot below)
  - Select Visual C# Items\Extensibility\Project Item Type
  - Provide the following name "Foo.xaml" - the name of the file ("Foo") 
    represents the file extension of the item type we are adding (".foo")
  - Press Add
- Important: Follow the additional instructions included in the file to update 
  ProjectItemsSchema.xaml and CustomProjectCs.targets with the provided code 
  snippets
- Done!
    
![](../Images/Fig_2.png)
    
### Using the new item type

Now let's see the new item type in action - because there is no item
template yet for a file ".foo" we need to create it and include it in the
project manually.

- Build and run VS with your Project Type (by default ProjectType1)
- Create a new Project of type Project Type (by default ProjectType11)
- Right click on the project node in solution explorer -> Open Folder in File 
  Explorer
  - In File Explorer, create a new Text Document - name it "Test.foo"
- Switch back to Visual Studio
- In Solution Explorer, right click on the project node -> Add Existing Item 
  and select the file you created above ("Test.foo")

#### Observations

Here are some things to notice about the newly added file:

- Select "Test.foo" in the Solution Explorer
- Look at the Properties Window -> you should notice that a custom property 
  "My Property" gets displayed for "Test.foo". That property was defined in 
  the Rules file that was included above
- Set some value for "My Property" -- e.g., "abc" and press enter
![](../Images/Fig_3.png)
- In a similar way, press the "Property Pages" button in the Properties 
  Window - this will open the Property Pages dialog, that should show the 
  property and its value
![](../Images/Fig_4.png)
- If you open the project file, you will notice that the file was added to 
  the project according to our definition, and that the custom property 
  "MyProperty" was set for the current configuration to the value specified 
  above. Here is how to do that:
  - Right click on the project node in Solution Explorer -> Unload Project 
    (save changes)
  - Right click again on the project in Solution Explorer -> Edit ProjectType11.myproj
     ```xml
     <FooCompile Include="Test.foo">
         <MyProperty Condition="'$(Configuration)|$(Platform)'=='Debug|AnyCPU'">abc</MyProperty>
     </FooCompile>
     ```

#### Making the new item type available in the *add new item* dialog

In order to and add new items of the newly created *item type* using the "Add -> New item..." dialog, you need to create a new item template.

The following steps describe how to add a new item template for an item template with extension ".foo" to a ProjectType1

1. Add a new item template project to your solution:
  - Right click on the solution node
  - Add new project
  - Select "Visual C#"->"Extensibility"->"C# Item Template"
  - Provide a name for the new project (e.g. `FooItemTemplate`)
2. Add a project reference from your project type to this new project:
  - right click on `ProjectType1.ProjectType`
  - select "Add Reference"
  - in the "Reference Manager" dialog select "Projects" on the left side and check `FooItemTemplate`
3. Add the new item template as an asset to the vsix manifest (located under the .ProjectType project)
  - Open `source.extension.vsixmanifest` from `ProjectType1.ProjectType`
  - Click "Assets" on the left side
  - Click the "New" button and provide the following values in the Add New Asset dialog:
    - Type: select "Microsoft.VisualStudio.ItemTemplate"
    - Source: select "A project in the current solution"
    - Project: Select `FooItemTemplate`
4. Customize the new item template
  - Rename the `Class.cs` to something that applies to your project (e.g. `Class.foo`)
  - Update the .vstemplate file `FooItemTemplate.vstemplate`
    - Update the 2 places where the old file `Class.cs` is specified (`DefaultName` and `ProjectItem`) to `Class.foo`
    - Add an `AppliesTo` element under `TemplateData` that points to your project's unique capability. E.g:
       ```xml
       <TemplateData>
           ...
           <AppliesTo>ProjectType1</AppliesTo>
       </TemplateData>
       ```

See https://msdn.microsoft.com/en-us/library/vstudio/tsyyf0yh.aspx for more information.
For more information about what can be done through the *vsix manifest designer*, see:
https://msdn.microsoft.com/en-us/library/vstudio/ee943167(v=vs.140).aspx

More details
------------
- See also [Custom item types](../extensibility/custom_item_types.md)
- More documentation about defining a .xaml rule file for your item type:
  - [Platform Extensibility (Part 1)](https://learn.microsoft.com/visualstudio/extensibility/creating-a-basic-project-system-part-1)
  - [Platform Extensibility (Part 2)](https://learn.microsoft.com/visualstudio/extensibility/creating-a-basic-project-system-part-2)