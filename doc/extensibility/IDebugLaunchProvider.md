IDebugLaunchProvider
===============

**[Item template:](project_item_templates.md)** Custom Debugger extension

## Tutorial
To add a debugger to a project you should export an `IDebugLaunchProvider`.
The simplest way to do this for Visual Studio 2015 is by using the [item template](project_item_templates.md)
for that purpose:

1. Project -> Add New Item
2. C# -> Extensibility -> Project System
3. Pick the "Custom Debugger Extension" template
4. Follow the instructions on screen.

## Example
In this example, a simple debugger is added to the project. It will show up in the drop down with the name "MyDebugger"
when the appropriate capabilities are matched (in this case `MyUnconfiguredProject.UniqueCapability`). The XAML rule
is completely optional, but it is a convenient way for allowing the user to optionally configure your debugger before
launching it. The configuration is done by the user setting properties on the user file, then the debugger reads those
properties while launching. If your debugger has no configuration necessary, then the XAML rule is not needed.

For another example see [ScriptDebuggerLaunchProvider.cs](../../samples/WindowsScript/WindowsScript/WindowsScript.ProjectType/ScriptDebuggerLaunchProvider.cs)
and [ScriptDebugger.xaml](../../samples/WindowsScript/WindowsScript/WindowsScript.ProjectType/BuildSystem/Rules/ScriptDebugger.xaml).

### The xaml rule for your debugger.
See [adding xaml rules](adding_xaml_rules.md)

``` XML
<?xml version="1.0" encoding="utf-8"?>
<Rule
	Name="MyDebugger"
	DisplayName="My Debugger"
	PageTemplate="debugger"
	Description="My debugger options"
	xmlns="http://schemas.microsoft.com/build/2009/properties">
    <Rule.DataSource>
        <!-- Store debugger properties in the user file, as they are design-time -->
        <DataSource Persistence="UserFileWithXamlDefaults" HasConfigurationCondition="True"/>
        <!-- Configuration conditions is optional. Depends on if you want these to be set per configuration. -->
    </Rule.DataSource>

    <!-- Add properties as needed to configure your debugger. These will appear on project properties. -->
    <StringProperty Name="MyProperty" DisplayName="My Property" Default="something" Description="Some property for configuring the debugger" />
</Rule>
```

If you use Microsoft.VisualStudio.ProjectSystem.SDK.Tools to compile the above XAML rule into your project, you can access it strongly-typed below.


### The debugger launch provider
``` CSharp
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.ProjectSystem.Debug;
using Microsoft.VisualStudio.ProjectSystem.Properties;
using Microsoft.VisualStudio.ProjectSystem.VS.Debug;

namespace MyPackage
{
    [ExportDebugger(MyDebugger.SchemaName)] // name of the schema from above
    [AppliesTo(MyUnconfiguredProject.UniqueCapability)]
    public class MyDebugLaunchProvider : DebugLaunchProviderBase
    {
        // Code-generated type from compiling "XamlPropertyRule"
        private readonly ProjectProperties projectProperties;

        [ImportingConstructor]
        public MyDebugLaunchProvider(ConfiguredProject configuredProject, ProjectProperties projectProperties)
            : base(configuredProject)
        {
            this.projectProperties = projectProperties;
        }

        // This is one of the methods of injecting rule xaml files into the project system.
        [ExportPropertyXamlRuleDefinition("MyPackage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9be6e469bc4921f1", "XamlRuleToCode:MyDebugger.xaml", "Project")]
        [AppliesTo(MyUnconfiguredProject.UniqueCapability)]
        private object DebuggerXaml { get { throw new NotImplementedException(); } }

        public override Task<bool> CanLaunchAsync(DebugLaunchOptions launchOptions)
        {
            // perform any necessary logic to determine if the debugger can launch
            return Task.FromResult(true);
        }

        public override Task<IReadOnlyList<IDebugLaunchSettings>> QueryDebugTargetsAsync(DebugLaunchOptions launchOptions)
        {
            var settings = new DebugLaunchSettings(launchOptions)
            {
                // configure settings as appropriate.
                LaunchDebugEngineGuid = MyDebuggerEngineGuid, // Microsoft.VisualStudio.ProjectSystem.Debug.DebuggerEngines has some well known engines
            };

            // you can get your xaml properties via:
            var debuggerProperties = await this.projectProperties.GetMyDebuggerPropertiesAsync();

            return Task.FromResult<IReadOnlyList<IDebugLaunchSettings>>(new IDebugLaunchSettings[] { settings });
        }
    }
}

```


## Support for older versions of Visual Studio

This particular extensibility point also exists as far back as Visual C++ 2010.

There are different extensions for each version of VS. Downloading the 
extension and creating a project from its project template will pop up 
a README file that explains how to use it.

- [Visual C++ 2015 Debugger Launch Extension][1]
- [Visual C++ 2013 Debugger Launch Extension][2]
- [Visual C++ 2012 Debugger Launch Extension][3]
- [Visual C++ 2010 Debugger Launch Extension][4]

Consider renaming debugger class (or rule file) so they don't collide
(namespace is the only distinguisher)
    
 [1]: https://visualstudiogallery.msdn.microsoft.com/7fe7f19f-ceb9-47e3-b440-c62df2b85281
 [2]: http://visualstudiogallery.msdn.microsoft.com/e831676e-9510-4651-b724-cf4299b220b5
 [3]: http://visualstudiogallery.msdn.microsoft.com/8d2faf2c-3937-489a-9e0a-c43ff26ca427
 [4]: http://visualstudiogallery.msdn.microsoft.com/f1e9c8b5-134e-4bb1-bd0e-37a220dae99e
