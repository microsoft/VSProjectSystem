Obtaining the `IProjectThreadingService`/`IThreadHandling` service
=====================================
**Visual Studio 2017:** IProjectThreadingService

**Visual Studio 2015:** IThreadHandling


### From MEF via import

Note that importing any CPS related service moves your MEF part from the
VS default MEF catalog into a CPS catalog "sub-scope". Import properties
are only 'satisfied' when MEF activated your type (not simply by `new`ing
up an instance of your object).

**Visual Studio 2017:**
```csharp
    [Import]
    IProjectThreadingService ProjectThreadingService { get; set; }
```

**Visual Studio 2015:**
```csharp
    [Import]
    IThreadHandling ThreadHandling { get; set; }
```

### From MEF via an imperative `GetService` query

**Visual Studio 2017:**
```csharp
    IProjectService projectService;
    IProjectThreadingService projectThreadingService = projectService.Services.ThreadingPolicy;
```

**Visual Studio 2015:**
```csharp
    ProjectService projectService;
    IThreadHandling threadHandling = projectService.Services.ThreadingPolicy;
```

Where `projectService` is obtained as described in 
[Obtaining the `ProjectService`](obtaining_the_ProjectService.md).

### From a loaded project

**Visual Studio 2017:**
```csharp
    IVsBrowseObjectContext context;
    IProjectThreadingService projectThreadingService = context.UnconfiguredProject.ProjectService.Services.ThreadingPolicy;
```

**Visual Studio 2015:**
```csharp
    IVsBrowseObjectContext context;
    IThreadHandling threadHandling = context.UnconfiguredProject.ProjectService.Services.ThreadingPolicy;
```

Where `context` is obtained as described in [Finding CPS in a VS 
project](finding_CPS_in_a_VS_project.md).
