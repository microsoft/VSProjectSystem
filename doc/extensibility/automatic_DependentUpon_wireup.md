# Automatic `DependentUpon` wire-up

Project systems can specify dependencies between items. For example, a `.xaml.cs` 
file can be represented as dependent upon its `.xaml` file.
In Solution Explorer, dependent items get displayed as child items under the item they depend upon. 

## Static dependency

The dependent information gets stored in the project file, using
evaluation-time `<DependentUpon>` metadata.

```xml
<None Include="foo.xaml" />

<Compile Include="foo.xaml.cs">
  <DependentUpon>foo.xaml</DependentUpon>
</Compile>
```

CPS can automatically add the necessary `<DependentUpon>` metadata 
project items when using _Add New Item_ and _Add Existing Item_.

In addition, it inherits a behavior from VB/C# that supports
adding the child files automatically when user only selects and adds the
parent file.

## Dynamic file dependency

Available since Visual Studio 2017.

Allows dynamic calculation of file dependencies without storing them in the project file.

This feature, in combination with the new [file globbing](../overview/globbing_behavior.md) feature
(that enables not listing each included item in the project file),
allows keeping the size of the project file to a minimum.

Dynamic file dependency is enabled by the `DynamicDependentFile` [capability](../overview/about_project_capabilities.md).
```xml
<ProjectCapability Include="DynamicDependentFile" />
```

## How to define new dependencies

There are two different ways to define new dependencies:
- Add rule data for the built-in calculation provided by CPS
- Implement `IDependentFilesProvider` if you need a more advanced logic than the built-in implementation supports (Visual Studio 2017 or later)

### Using the built-in item dependency calculation

CPS's built-in dependency calculation is extended by authoring XAML rules.

In the following example we will make files ending with `.xaml.cs`
dependent upon (and appear under) their parent `.xaml` files.

Firstly, we need to define a content type and a mapping from ‘file
extension’ to ‘content type’. They could be added into the existing
`ProjectItemsSchema.xaml` file, or a new XAML file being included in project
system specific `.targets` file.

```xml
<ProjectSchemaDefinitions xmlns="http://schemas.microsoft.com/build/2009/properties">
  <ContentType Name="PageXaml" DisplayName="XAML Page" ItemType="Page" />
  <FileExtension Name=".xaml" ContentType="PageXaml" />
</ProjectSchemaDefinitions>
```

Then when CPS handles a `.xaml` file, it can map it to a content type
and retrieve more metadata from the content type.

Next, we need to specify the ‘dependent file extensions’ of `.xaml` via adding metadata
`DependentExtensions` (`DependentFileExtensions` in Visual Studio 2015) into the content type.

Multiple dependent file extensions are allowed and must be separated by `;` (semicolon).

Allows defining dependencies for files that have the same file name. For example:
*filename*.*ext2* depends on *filename*.*ext1*

The following snippent defines *filename*.xaml.cs as dependent on *filename*.xaml:

```xml
<ContentType Name="PageXaml" DisplayName="XAML Page" ItemType="Page">
  <NameValuePair Name="DependentExtensions" Value=".xaml.cs" />
</ContentType>
```
