Scopes
======

### Introduction to scopes

In Visual Studio there are three "scopes" of context related to project
systems:

- IVsSolution, or the global context. There is exactly one of these in the process.
- IVsProject, or the project context. There is exactly one of these per project in the solution.
- IVsProjectCfg, or the project configuration context. There is exactly one of these for each build configuration, for each project, in the solution.

These contexts can be represented in a hierarchy:

- Visual Studio process
  - IVsSolution 
    - IVsProject (a.csproj)
      - IVsProjectCfg (Debug|AnyCPU)
      - IVsProjectCfg (Release|AnyCPU)
      - IVsProjectCfg (Debug|x86)
      - IVsProjectCfg (Release|x86)
    - IVsProject (b.vcxproj)
      - IVsProjectCfg (Debug|Win32)
      - IVsProjectCfg (Release|Win32)
                
CPS has these three context scopes as well. But they are known by the CPS
concept names rather than their VS-specific equivalents. Here they are
with their equivalents:

| VS term       | CPS term            | MSBuild term                            |
|---------------|---------------------|-----------------------------------------|
| IVsSolution   | ProjectService      | ProjectCollection                       |
| IVsProject    | UnconfiguredProject | ProjectRootElement (construction model) |
| IVsProjectCfg | ConfiguredProject   | Project (evaluation model)              |

Any code in VS may obtain the ProjectService or IVsSolution because there
is just one in the process. 

Code that wants an UnconfiguredProject (or IVsProject) must either already
be operating at that context to obtain it, or must ask its parent context
for the project-specific context by naming the project to be obtained.

Code that wants a ConfiguredProject (or IVsProjectCfg) must similarly
belong to that context or first retrieve an UnconfiguredProject and then
request the ConfiguredProject by configuration name.


Note that ProjectService scope is not equivalent to the VS default MEF container.
ProjectService is a scope beneath the default container and [must be obtained from the
IProjectServiceAccessor](onenote:Documentation.one#Obtaining%20the%20ProjectService&section-id={768BD288-CDB5-4DCE-83D2-FC3994703CEA}&page-id={213C67CF-0707-470E-903D-1451517B2F73}&base-path=http://devdiv/sites/vspe/prjbld/OneNote/TeamInfo/CPS),
which itself is defined in the VS default container.

### Scopes at odds

Because CPS projects are MSBuild based which has a project file format
in which nearly everything can be an expression and therefore may vary by
project configuration, most of CPS services for querying or manipulating
project data are implemented in the ConfiguredProject scope. This is
at odds with VS which expects most project data to be exposed at the
IVsProject context. For example, the IVsProject object is expected to
be able to enumerate items in the project, but items in MSBuild are the
result of MSBuild evaluation (since they may include MSBuild expressions or
conditions) so items can only be obtained from the IVsProjectCfg context. 

To rectify this disparity, CPS can 'lift' services that are defined in the
ConfiguredProject scope into the UnconfiguredProject scope. As there are
many ConfiguredProject scopes that are children of an UnconfiguredProject
scope, CPS picks the ConfiguredProject associated with the active project
configuration. The active project configuration is a VS concept that dictates
that for a given project, at most one configuration can be 'active'. CPS
synchronizes with the VS concept of active project configuration so that
CPS always uses the active ConfiguredProject as its source for data when
it is queried for it at the UnconfiguredProject scope.

### Which scope should I operate at?

The short answer is that if your code operates at the project level (that
is, you want one instance of your service per project) you should export
it to the UnconfiguredProject scope. Otherwise if you want your service
to have an instance for each individual configuration you should export
it to the ConfiguredProject scope.


### Controlling the MEF scope your MEF part is exported to

MEF parts belong to the scope necessary to satisfy all of its imports. A MEF
part that imports nothing belongs to the 'default' or global scope. A MEF part
that imports anything from the ConfiguredProject scope belongs to the
ConfiguredProject scope as well (unless it does so via the
ActiveConfiguredProject<T> wrapper). A MEF part that imports anything from
the ConfiguredProject.

| A part belongs to, the scope below, when it imports MEF parts in the columns to the right | VS default container | ProjectService | UnconfiguredProject | ConfiguredProject |
|-------------------------------------------------------------------------------------------|----------------------|----------------|---------------------|-------------------|
| VS default container                                                                      | Y/N                  | No             | No                  | No                |
| CPS ProjectService                                                                        | Y/N                  | Yes            | No                  | No                |
| CPS UnconfiguredProject                                                                   | Y/N                  | Y/N            | Yes                 | No                |
| CPS ConfiguredProject                                                                     | Y/N                  | Y/N            | Y/N                 | Yes               |

### Importing ConfiguredProject MEF exports at the UnconfiguredProject level

This ability to 'lift' data upward from ConfiguredProject to UnconfiguredProject
scope is exposed to you in at least two ways. If you simply want to access
ConfiguredProject data that CPS exposes while you're at the UnconfiguredProject
context the easiest way is often to query for the service stub that CPS often
exposes at the UnconfiguredProject scope. For example, if you want to get
the IProjectSubscriptionService, which is exported to the ConfiguredProject
scope, you can instead get the IActiveConfiguredProjectSubscriptionService,
which is available at the UnconfiguredProject scope.

When you are defining a MEF part that is exported to the UnconfiguredProject
scope, you may import ConfiguredProject-scoped services using the
ActiveConfiguredProject<T> wrapper. This is a MEF open-generic export
that you can import, providing your own T, where T is an export from the
ConfiguredProject scope. Note this technique only works when T is exported
with the default MEF contract name for T.  So for example, if you want to
import the IBuildProject service from the active configuration from your
UnconfiguredProject MEF part, you can use this syntax:

```csharp
    [Import]
    ActiveConfiguredProject<IBuildProject> BuildProject { get; set; }
```

Note that the generic type argument can even be ConfiguredProject itself:

```csharp
    [Import]
    ActiveConfiguredProject<ConfiguredProject> ActiveConfiguredProject { get; set; }
```

Most commonly, it's useful to define your own private nested class that
imports everything you need from the ConfiguredProject scope, and then
import that:

```csharp
    [Export]
    class MyUnconfiguredProjectPart {
        [Import]
        UnconfiguredProject Project { get; set; }

        [Import]
        ActiveConfiguredProject<ConfiguredProjectHelper> ActiveConfigurationExports { get; set; }

        [Export]
        class ConfiguredProjectHelper {
            [Import]
            internal ConfiguredProject ConfiguredProject { get; set; }

            [Import]
            internal IBuildProject BuildProject { get; set; }
        }
    }
```

Q: Is there a way to know which services belong to which scope (e.g. naming
convention)?

A: No. Just documentation. But I hope we have a better answer in VNext

