# MEF

## Managed Extensibility Framework in the .NET Framework

The Managed Extensibility Framework (MEF) was introduced in .NET
Framework 4 as a library for creating extensible applications. It allows
application developers to discover and use extensions based on pre-defined
contracts regardless of where they are implemented. Since MEF provides the
technology to create an extensible project system, CPS utilizes it
extensively and uses it as its foundation; therefore, creating a CPS based
project system is basically providing a set of MEF components.

The concept of MEF can be found at [Managed Extensibility Framework (MEF)][mef].

[mef]:https://learn.microsoft.com/dotnet/framework/mef/

Through MEF, writing an extension to create a CPS based project system
becomes a straightforward task, which starts creating extension classes 
through templates provided in the CPS SDK.  A typical CPS component looks 
like this:

```csharp
[Export(typeof(IProjectTreePropertiesProvider))]
[AppliesTo("MyProjectType")]
internal class ProjectTreeIconProvider : IProjectTreePropertiesProvider
{
    /// <summary>
    /// Gets the tree factory.
    /// </summary>
    [Import]
    protected Lazy<IProjectTreeFactory> TreeFactory { get; private set; }

    /// <summary>
    /// Gets the unconfigured project.
    /// </summary>
    [Import]
    protected UnconfiguredProject UnconfiguredProject { get; set; }

    // ...
}
```

This extension implements the `IProjectTreePropertiesProvider` interface. Through the 
`Export` attribute, the class above implements `IProjectTreePropertiesProvider` contract 
so that the right component in CPS system can load the class when it is needed. 
Through the `Import` attribute, the class declares what it wants from the
system so that MEF will set the two properties with `Import` attribute
when it initializes the object.

There are a few important things to remember:

- The `AppliesTo` attribute declares that this component is only expected to be 
  used in a project with certain capabilities. In CPS, it is essential to 
  ensure that the component is only used by a targeted project. More 
  discussion on this topic will follow.
- One must only import a contract that is implemented by a MEF component. If a 
  random type not implemented by a MEF component is imported, the MEF engine 
  will not be able to resolve it, potentially bringing the entire project 
  system down. The VS MEF engine does provide some help in diagnosing the 
  problem in such situations.
- It is also important to import components in the correct 'scope,' a 'scope' 
  being a MEF v2 concept to control the lifespan of a component. Importing a 
  wrong scope could bring down the entire project system. A specific section 
  is devoted to the discussion of scopes inside this document.

## MEF inside Visual Studio

MEF is a powerful platform that supports a lot of advanced features and
dynamic compositions. Unfortunately, it is exactly these dynamic features
that will slow down the applications using them heavily. To improve 
performance, Dev 14 carries a different composition engine which preserves
computed composition data to speed up the loading of MEF components. 
Usually the new engineer maintains the preserved cache very well; however,
it could occasionally be out of sync with extensions. In this situation,
running the following commands inside command-line window will reset the
cache:

```
devenv /UpdateConfiguration
devenv /ClearCache
```

VS MEF provides a detailed error report when it finds errors inside MEF
compositions. It always tries to keep the rest of the components working by
removing incorrect components. This process may go further and remove
more components depending on components removed earlier. Reviewing the
error report file will help to diagnose issues such as the reason why a
certain component is never being loaded into VS. The error report file
can be found at 

```
%LOCALAPPDATA%\Microsoft\VisualStudio\[Version]\ComponentModelCache\Microsoft.VisualStudio.Default.err
```

Because a MEF error may cause chains of errors in other components, one
should always start with investigating level 1 composition errors.

Some common MEF errors include:

- Importing a contract that cannot be satisfied by any component in the system;
- Importing a single component, while there are multiple components satisfying 
  the contract;
- Implementing a contract that is not expected to be implemented by any 
  extension (E.g., lots of CPS services like `IProjectLockService` are 
  provided by the system, which should not be implemented again inside any 
  extension); 
- Importing a component in a wrong scope (e.g., `IProjectTreePropertiesProvider` 
  is expected to be in the `UnconfiguredProject` scope; therefore, the 
  implementation of the `IProjectTreePropertiesProvider` should never import a 
  `ConfiguredProject` directly).

In CPS, any contract that can be implemented by extensions is expected to
be implemented in multiple places; in this regard, one should always use
`ImportMany` to import components by using an `OrderPrecedenceImportCollection`.
In a developer's test environment, there might be only one component
to implement the contract and in this case, importing a single component
works. However, the same code may fail on a customer's machine, and therefore
should be carefully prevented in the first place.

VS MEF does not support certain advanced features such as dynamic composition
changes or generic type contracts, but most developers do not use them
anyway.

## CPS and MEF

CPS, which uses MEF to construct project systems, is like a big cooperation
system that combines MEF components implemented in both the core CPS and
all extensions together to make all different kinds of projects work. The
health of the system depends on whether every contributor follows basic
rules. One of the key designs of CPS is the function of a project being 
controlled by its *capabilities*. A section of this document is devoted
to discussing how a project declares its capabilities. It is through its
capabilities that a JavaScript project becomes a JavaScript project and
a device project becomes a device project.

Most CPS extensions or its MEF components are designed to support certain
types of projects. For both function and performance, a component written
for a certain project system (such as for a C++ project) should not be
loaded into another project system (such as a JavaScript project). To
determine the proper environment to use the component, CPS relies on the
fact that the component carries the correct `AppliesTo` metadata.  It is
important to use the correct `AppliesTo` metadata when defining a component.
Normally, the `AppliesTo` metadata is the new project type the component
supports; in the advanced scenario, `AppliesTo` metadata can be an expression
like this:

```csharp
[AppliesTo("MyLanaguageProject & DeviceProject")]
```

Also, when a component exports additional properties or methods, the
metadata should be declared wherever the export attribute is used.

The `AppliesTo` metadata is not directly supported by MEF; therefore, when
components are imported from CPS extensions, `OrderPrecedenceImportCollection`
should be used to filter out those components which don't match the
context. For example, a component in the `unconfiguredProject` that imports
`IVsHierarchy` looks like this:

```csharp
[Export]
public class MyClass
{
    [ImportMany(ExportContractNames.VsTypes.IVsHierarchy)]
    private OrderPrecedenceImportCollection<IVsHierarchy> vsHierarchies;

    private IVsHierarchy VsHierarchy
    {
        get { return this.vsHierarchies.First().Value; }
    }

    [ImportingConstructor]
    internal MyClass(UnconfiguredProject unconfiguredProject)
    {
        // MEF does not know how to construct one of these custom collection types,
        // so we construct it here for MEF. After the MyClass constructor completes,
        // MEF will proceed to call Add on this.vsHierarchies to fill it with exports.
        this.vsHierarchies = new OrderPrecedenceImportCollection<IVsHierarchy>(
            projectCapabilityCheckProvider: unconfiguredProject);
    }

    // ...
}
```

In this case, the code expects that one and only one `IVsHierarchy` is
implemented for the project. In other scenarios, the code might expect
multiple implementations (such as a debugger provider) or no implementation
at all in the collection. `OrderPrecedenceImportCollection` uses the project
capabilities in the project to filter the components found in MEF, keeping
only those matching the capabilities of the project.

For a component within the configured project scope, or in the project
service scope, the `UnconfiguredProject` in the sample above should be
replaced by `ConfiguredProject` or `IProjectService`/`ProjectService`.

Components in the extensions can import a singleton service implemented by CPS
without using the `OrderPrecedenceImportCollection`. These singleton services
are provided by CPS and cannot be replaced by extensions. Such services
include the `ConfiguredProject`, `UnconfiguredProject`, `IProjectLockService`,
etc.

### Obtaining MEF components in project scopes

As described in [scopes](scopes.md), MEF project-specific components instances
can exist in the scope of an `UnconfiguredProject` or a `ConfiguredProject`.

If your component is a MEF part and in a compatible scope, then the best way
to obtain an instance is just to import it using a standard MEF import.

However, if your component is not participating in MEF, or is in the global
or `ProjectService` scopes, then you can obtain project-scoped components
via the relevant instance of `UnconfiguredProject` or `ConfiguredProject`.

Note that several key CPS services are available directly on via the project's
`Services` property. However if you need a MEF component that is not already
exposed this way, you can use the project's `ExportProvider` as follows.

For example:

```csharp
UnconfiguredProject unconfiguredProject = ...;
ConfiguredProject configuredProject = ...;

IMyUnconfiguredComponent c1 = unconfiguredProject.Services.ExportProvider.GetExportedValue<IMyUnconfiguredComponent>();

IMyConfiguredComponent c2 = configuredProject.Services.ExportProvider.GetExportedValue<IMyConfiguredComponent>();
```

For details on obtaining an instance of `UnconfiguredProject` or `ConfiguredProject`,
see [Finding CPS in a VS project](../automation/finding_CPS_in_a_VS_project.md)

## MEF and C# Nullable Reference Types

When using the C# nullable feature in your code, you may need to tweak the above
examples slightly.

For example, NRT requires all non-nullable fields/properties be assigned by the end of the
constructor, however MEF assigns the value of imports via reflection. To get around
this you can use:

```csharp
[Import]
public MyComponent Foo { get; private set; } = null!;
```

The `= null!` tells the compiler that this is initialized to a value, and the `!`
suppresses a warning about it being assigned to null. This change has no runtime
impact.

Note that imports that allow defaults should be marked nullable, as MEF might not
initialize them.

For example:

```csharp
[Import(AllowDefault = true)]
public MyComponent? Foo { get; private set; };

[ImportingConstructor]
public Bar([Import(AllowDefault = true) Baz? baz])
{
  // ...
}
```
