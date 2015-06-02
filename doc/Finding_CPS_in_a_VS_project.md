Finding CPS in a VS project
===========================

**Caution: CPS has no stable public API of its own. Any references to CPS
assemblies or types therein can be counted on to break with each successive
release of Visual Studio until we can take time in our schedule to stabilize
our API. Sorry.**

[How to detect whether a project is a CPS project without risking being
broken in the next version of VS.](How_to_detect_whether_a_project_is_a_CPS_project.md)

Note: These instructions are intended for clients who are not MEF parts
themselves.  MEF parts that need access to CPS APIs should simply `[Import]`
the services they require.

```csharp
    #ref Microsoft.VisualStudio.ProjectSystem.v14only.dll
    #ref Microsoft.VisualStudio.ProjectSystem.VS.v14only.dll  // may be useful after acquiring CPS
    #ref Microsoft.VisualStudio.ProjectSystem.Utilities.v14.0.dll // may be useful after acquiring CPS

    using Microsoft.VisualStudio.ProjectSystem.Designers;

    private UnconfiguredProject GetUnconfiguredProject(IVsProject project) {
        IVsBrowseObjectContext context = project as IVsBrowseObjectContext;
        if (context == null) { // VC implements this on their DTE.Project.Object
            IVsHierarchy hierarchy = project as IVsHierarchy;
            if (hierarchy != null) {
                object extObject;
                if (ErrorHandler.Succeeded(hierarchy.GetProperty((uint)VSConstants.VSITEMID.Root, (int)__VSHPROPID.VSHPROPID_ExtObject, out extObject))) {
                    EnvDTE.Project dteProject = extObject as EnvDTE.Project;
                    if (dteProject != null) {
                        context = dteProject.Object as IVsBrowseObjectContext;
                    }
                }
            }
        }

        return context != null ? context.UnconfiguredProject : null;
    }

    private UnconfiguredProject GetUnconfiguredProject(EnvDTE.Project project)
    {
        IVsBrowseObjectContext context = project as IVsBrowseObjectContext;
        if (context == null && project != null) { // VC implements this on their DTE.Project.Object
            context = project.Object as IVsBrowseObjectContext;
        }

        return context != null ? context.UnconfiguredProject : null;
    }
```

#### To obtain the ConfiguredProject

```csharp
    UnconfiguredProject unconfiguredProject; // obtained from above
    var configuredProject = await unconfiguredProject.GetSuggestedConfiguredProjectAsync()
```

#### Obtain CPS services

The easiest way to obtain CPS services from an UnconfiguredProject or a
ConfiguredProject instance is to use either of these interfaces' Services
property, which provides access to the common CPS services for either of
these scopes. 

If the service you want isn't exposed on the Services property directly,
you can obtain arbitrary exports using the Services.ExportProvider property:

```csharp
    ConfiguredProject cp;
    IOutputGroupsProvider ogp = cp.Services.ExportProvider.GetExportedValue<IOutputGroupsProvider>();
```

To test for whether a given project is a WWA project

Rather than using ProjectTypeGuid, please use the above method to detect
CPS, then check the UnconfiguredProject.Capabilities property for the
presence of the "Javascript" and "WindowsAppContainer" capabilities.
