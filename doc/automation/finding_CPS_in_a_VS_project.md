Finding CPS in a VS project
===========================

[How to detect whether a project is a CPS project without risking being
broken in the next version of VS.](detect_whether_a_project_is_a_CPS_project.md)

Note: These instructions are intended for clients who are not MEF parts
themselves.  MEF parts that need access to CPS APIs should simply `[Import]`
the services they require.

```csharp
using Microsoft.VisualStudio.ProjectSystem.Properties;
using Microsoft.VisualStudio.Shell.Interop;

private static UnconfiguredProject? GetUnconfiguredProject(EnvDTE.Project project)
{
    if (project is IVsBrowseObjectContext context1)
    {
        return context1.UnconfiguredProject;
    }

    if (project.Object is IVsBrowseObjectContext context2)
    {
        // VC implements this on their DTE.Project.Object
        return context2.UnconfiguredProject;
    }

    return null;
}

private static UnconfiguredProject? GetUnconfiguredProject(IVsHierarchy hierarchy)
{
    if (ErrorHandler.Succeeded(hierarchy.GetProperty((uint)VSConstants.VSITEMID.Root, (int)__VSHPROPID.VSHPROPID_ExtObject, out object extObject)))
    {
        if (extObject is EnvDTE.Project dteProject)
        {
            return GetUnconfiguredProject(dteProject);
        }
    }

    return null;
}

private static UnconfiguredProject? GetUnconfiguredProject(IVsProject project)
{
    if (project is IVsBrowseObjectContext context)
    {
        return context.UnconfiguredProject;
    }

    if (project is IVsHierarchy hierarchy)
    {
        return GetUnconfiguredProject(hierarchy);
    }

    return null;
}
```

#### To obtain the `ConfiguredProject`

```csharp
UnconfiguredProject unconfiguredProject; // obtained from above

ConfiguredProject configuredProject = await unconfiguredProject.GetSuggestedConfiguredProjectAsync()
```

#### Obtain CPS services

The easiest way to obtain CPS services from an `UnconfiguredProject` or a
`ConfiguredProject` instance is to use either of these interfaces' `Services`
property, which provides access to the common CPS services for either of
these scopes. 

If the service you want isn't exposed on the `Services` property directly,
you can obtain arbitrary exports using the `Services.ExportProvider` property:

```csharp
ConfiguredProject configuredProject; // obtained from above

IOutputGroupsProvider ogp = configuredProject.Services.ExportProvider.GetExportedValue<IOutputGroupsProvider>();
```

#### Testing a project's type

Rather than using `ProjectTypeGuid`, please use the above method to detect
CPS, then check the `UnconfiguredProject.Capabilities` property for the
presence of specific capabilities.
