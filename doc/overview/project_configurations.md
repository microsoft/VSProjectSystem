# Project configurations

Project configuration is the key to hint the project system how the project
will be built, deployed, debugged, etc. The project configuration represents
as a combination of 'Configuration' and 'Platform', i.e., 'Debug|AnyCPU',
'Release|x86', etc. In general a project might have multiple project
configurations, and only one project configuration could be active. The
active project configuration is set by the user via the Configuration
Manager dialog. 

## How does CPS know the project configurations?

CPS has two built-in strategies to figure out the project configurations.
The activated strategy is determined by project capability.

If the project has the capability named `ProjectConfigurationsDeclaredAsItems`,
then CPS will crawl the project file and the imports and gather the
`ProjectConfiguration` items. By convention, the `ProjectConfiguration`
items should be grouped and put into one `ItemGroup` element with the label
`ProjectConfigurations`, and they should be saved in the project file.

For example:

```xml
<ItemGroup Label="ProjectConfigurations">
  <ProjectConfiguration Include="Debug|AnyCPU">
    <Configuration>Debug</Configuration>
    <Platform>AnyCPU</Platform>
  </ProjectConfiguration>
  <ProjectConfiguration Include="Debug|ARM">
    <Configuration>Debug</Configuration>
    <Platform>ARM</Platform>
  </ProjectConfiguration>
</ItemGroup>
```

If the project has the capability named `ProjectConfigurationsInferredFromUsage`,
CPS will crawl the project file and imports and gather the project
configuration through the usages in conditions.

For example:

```xml
<PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU'">
  <DebugSymbols>true</DebugSymbols>
  <DebugType>full</DebugType>
  <Optimize>false</Optimize>
  <OutputPath>bin\Debug\</OutputPath>
  <DefineConstants>DEBUG;TRACE</DefineConstants>
  <ErrorReport>prompt</ErrorReport>
  <WarningLevel>4</WarningLevel>
</PropertyGroup>

<PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Release|AnyCPU'">
  <DebugType>pdbonly</DebugType>
  <Optimize>true</Optimize>
  <OutputPath>bin\Release\</OutputPath>
  <DefineConstants>TRACE</DefineConstants>
  <ErrorReport>prompt</ErrorReport>
  <WarningLevel>4</WarningLevel>
</PropertyGroup>
```

Surely a new project type based on CPS could define a 3rd capability to
implement yet another strategy to figure out the project configurations.
For example, the ASP.NET 5 project is based on CPS and it implements a different
strategy to read the project configurations from the project.json file.

## How do I implement my own strategy for the project configurations?

- Choose a distinguished capability name for the new strategy. e.g.,
  `ProjectConfigurationsFromProjectJson`
- Implement and export `IProjectConfigurationsServiceInternal` with the
  `AppliesTo()` being set to that capability. For example:

```csharp    
[Export(typeof(IProjectConfigurationsService))]
[AppliesTo("ProjectConfigurationsFromProjectJson")]
internal class ProjectJsonConfigurationsService : IProjectConfigurationsServiceInternal
{
}
```
        
- Include that capability in the common targets file. For example:

```xml
<ItemGroup>
  <ProjectCapability Include="ProjectConfigurationsFromProjectJson" />
</ItemGroup>
```
    
- Ensure the two built-in capabilities `ProjectConfigurationsDeclaredAsItems` 
  and `ProjectConfigurationsInferredFromUsage` are not defined in the common 
  targets file.
