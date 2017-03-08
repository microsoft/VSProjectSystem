Obtaining the ProjectService
============================

**Visual Studio 2017:** IProjectService

**Visual Studio 2015:** ProjectService

### From MEF via import

Note that importing any CPS related service moves your MEF part from the
VS default MEF catalog into a CPS catalog "sub-scope". Import properties
are only 'satisfied' when MEF activated your type (not simply by `new`ing
up an instance of your object).

**Visual Studio 2017:**
```csharp
    [Import]
    IProjectService ProjectService { get; set; }
```

**Visual Studio 2015:**
```csharp
    [Import]
    ProjectService ProjectService { get; set; }
```

### From MEF via an imperative `GetService` query

**Visual Studio 2017:**
```csharp
    IServiceProvider site; // the VS global service provider
    var componentModel = site.GetService(typeof(SComponentModel)) as IComponentModel;
    var projectServiceAccessor = componentModel.GetService<IProjectServiceAccessor>();
    IProjectService projectService = projectServiceAccessor.GetProjectService();
```

**Visual Studio 2015:**
```csharp
    IServiceProvider site; // the VS global service provider
    var componentModel = site.GetService(typeof(SComponentModel)) as IComponentModel;
    var projectServiceAccessor = componentModel.GetService<IProjectServiceAccessor>();
    ProjectService projectService = projectServiceAccessor.GetProjectService();
```
