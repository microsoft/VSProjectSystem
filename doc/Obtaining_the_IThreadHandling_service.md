Obtaining the IThreadHandling service
=====================================

### From MEF via import

Note that importing any CPS related service moves your MEF part from the
VS default MEF catalog into a CPS catalog "sub-scope". Import properties
are only 'satisfied' when MEF activated your type (not simply by "new"ing
up an instance of your object).

```csharp
    [Import]
    IThreadHandling ThreadHandling { get; set; }
```

### From MEF via an imperative GetService query

```csharp
    ProjectService projectService;
    IThreadHandling threadHandling = projectService.Services.ThreadingPolicy;
```

Where `projectService` is obtained as described in [Obtaining the ProjectService](Obtaining_the_ProjectService.md).

### From a loaded project

```csharp
    IVsBrowseObjectContext context;
    IThreadHandling threadHandling = context.UnconfiguredProject.ProjectService.Services.ThreadingPolicy;
```

Where `context` is obtained as described in [Finding CPS in a VS project](Finding_CPS_in_a_VS_project.md).