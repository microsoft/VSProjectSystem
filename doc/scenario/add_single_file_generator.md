How to add a single file generator
==================================

Single File Generators allow generating content based on initial file. For example, here are some popular single file generators:
  - [T4 Text Templates](https://msdn.microsoft.com/en-us/library/bb166817.aspx)
  - ResX Generator
  
More information about Single Files Generators: https://msdn.microsoft.com/en-us/library/bb166817.aspx

The following steps explain how to create and use such a Single File Generator in a CPS based project type.

**Warning:** If you have multiple project types using the same project file extension you may hit a problem where the ProjectTypeGuid gets picked up from a different project type. If that's the case, the single file generators will fail to be found. That can happen if you create multiple project types using the default "myproj" extension.

1. Implement the single file generator
  - Add a new class to the project type (e.g. FooGenerator). Provide an implementation for it. Here is a simplified version, where we replace all "a" with "A".

  ```csharp
  [Guid("Your GUID here")]
  public class FooGenerator : IVsSingleFileGenerator
  {
      public int DefaultExtension(out string pbstrDefaultExtension)
      {
          pbstrDefaultExtension = ".txt";
          return 0;
      }

      public int Generate(string wszInputFilePath, string bstrInputFileContents, string wszDefaultNamespace, IntPtr[] rgbOutputFileContents, out uint pcbOutput, IVsGeneratorProgress pGenerateProgress)
      {
          string output = bstrInputFileContents.Replace("a", "A");
          byte[] bytes = Encoding.UTF8.GetBytes(output);

          rgbOutputFileContents[0] = Marshal.AllocCoTaskMem(bytes.Length);
          Marshal.Copy(bytes, 0, rgbOutputFileContents[0], bytes.Length);

          pcbOutput = (uint)bytes.Length;
          return 0;
      }
  }
  ```
2. Register the code generator for your project type
    - Add the following property to the VsPackage class
  ```csharp
  public const string GeneratorProjectTypeGuid = "{" + ProjectTypeGuid + "}";
  ```
    - Add the following attributes to the VsPackage class
  ```csharp
    [CodeGeneratorRegistration(typeof(FooGenerator), "FooGenerator", GeneratorProjectTypeGuid, GeneratesDesignTimeSource = true)]
    [ProvideObject(typeof(FooGenerator))]
    public sealed class VsPackage : Package
    {
      // ...
    }
  ```
3. Add a new item type using the Project Item Type
  - Right click on `Rules`, Add New Item -> Extensibility -> Project System -> Project Item Type
  - Name it "Foo.xaml" - this will add support for a new ".foo" item type
  - Follow the additional instructions in the template
  - Edit "Foo.xaml" and add a new StringProperty named Generator
  ```Xml
    <StringProperty Name="Generator" Visible="True"/>
  ```
4. Define the `SingleFileGenerators` capability (Note this is not needed for the default project type because it is already defined in `Microsoft.Common.CurrentVersion.targets` but will be needed if your project type uses a different set of targets)
  - Insert the following line in `CustomProject.targets` (next to the other ProjectCapability items)
  ```Xml
  <ProjectCapability Include="SingleFileGenerators" />
  ```
5. Test the generator
  - Build and Run
  - Create a new project of your project type
  - Because we don't have an item template for ".foo" files, we need to create one in File Explorer and include it in the project
    - Right click on the Project -> Open Folder in File Explorer
    - Create a new text file
    - Open it in Notepad, type some text - make sure you include some lower case "a" characters
    - Save
    - Rename it to "aaa.foo"
    - Switch back to VS
    - Click Show all files -> you should see aaa.foo
    - Right click on aaa.foo -> Include
  - Select aaa.foo in solution explorer
  - You should see the Generator property - specify `FooGenerator`
  - Once you specify the generator -> generator will be automatically invoked
  - If you expand Aaa.foo you will notice a new file aaa.txt that was generated from aaa.foo by the generator
  
    
    
