Provide custom icons for the Project Type/Item type
===================================================
**[Item template:](../extensibility/project_item_templates.md)** Project Tree Modifier extension

**[Item template:](../extensibility/project_item_templates.md)** Custom Icons

##Tutorial
In order to add custom icons to your project, you need to follow these steps:

1. Add New Item
  - **Visual Studio 2017**: `Project Tree Properties Provider extension`
  - **Visual Studio 2015**: `Project Tree Modifier extension`
  - This will add an [IProjectTreePropertiesProvider](../extensibility/IProjectTreePropertiesProvider)/[IProjectTreeModifier](../extensibility/IProjectTreeModifier.md) export to your project type that will replace the project icon with a JavaScript Icon
2. Add New Item - `Custom Icons`. This template does a few things:
  - Generates a new .imagemanifest file, that defines one image with 2 sources:
    - .png file that will be used for 100 % dpi
    - .xaml file that will be used for everything else
    - Note that `Include in VSIX` is set to true
  - Generates a new class (`Images1Monikers.cs`) that exposes a property to easily access the new image
3. Update the file generated at step 2 to consume the ImageMoniker exposed at step 1

**Visual Studio 2017:**
  ```csharp
  propertyValues.Icon = Images1Monikers.ProjectIconImageMoniker.ToProjectSystemType();
  ```

**Visual Studio 2015:**
  ```csharp
  tree = tree.SetIcon(Images1Monikers.ProjectIconImageMoniker.ToProjectSystemType());
  ```

##Adding an image manifest manually
1. Image manifest
  1. Create a new xml file, name it to .imagemanifest (e.g. `Images1.imagemanifest`)
  
    ```xml
    <?xml version="1.0" encoding="utf-8"?>
    <ImageManifest xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
                               xmlns:xsd="http://www.w3.org/2001/XMLSchema"
                               xmlns="http://schemas.microsoft.com/VisualStudio/ImageManifestSchema/2014">
      <Symbols>
        <String Name="Resources" Value="/<AssemblyName>;Component/" />
        <Guid Name="Images1Guid" Value="<GUID>" />
        <ID Name="ProjectIcon" Value="0" />
      </Symbols>
      <Images>
        <Image Guid="$(Images1Guid)" ID="$(ProjectIcon)">
          <Source Uri="$(Resources)/Images/Images1ProjectIcon.xaml" />
          <Source Uri="$(Resources)/Images/Images1ProjectIcon.png" >
            <Size Value="16" />
          </Source>  
        </Image>
      </Images>
      <ImageLists />
    </ImageManifest>
  ```
  2. Replace `<AssemblyName>` with your assembly name 
  3. Replace `<GUID>`with a guid
  3. In the Properties page, set the following properties:
    - `Build Action` to `Content`
    - `Include in VSIX` to `True`
2. Image Monikers
  1. Create a new `Images1Monikers.cs` file
    ```csharp
    using System;
    using Microsoft.VisualStudio.Imaging.Interop;
    
    namespace <namespace>
    {
        public static class Images1Monikers
        {
            private static readonly Guid ManifestGuid = new Guid("<GUID>");
    
            private const int ProjectIcon = 0;
    
            public static ImageMoniker ProjectIconImageMoniker
            {
                get
                {
                    return new ImageMoniker { Guid = ManifestGuid, Id = ProjectIcon };
                }
            }
        }
    }
    ```
  2. Replace `<GUID>` with the same value used in the image manifest
  3. Replace `<namespace>` with your namespace
3. Add images
  1. Add the the following images to your project under an `Images` folder
    - Images/Images1ProjectIcon.png
    - Images/Images1ProjectIcon.xaml
  2. Set the `Build Action` for both images to `Resource`
4. Consume the new Image Moniker from your code (see the Tutorial above)
