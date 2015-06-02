Obtaining the IProjectLockService
=================================

Please observe CPS [project locking rules](The_Project_Lock.md)
by not retaining any references to MSBuild objects beyond the scope of the
lock and only using these objects while not on the UI thread.  Violating
this exposes your code and other project-related code to the risk of
multithread-related IDE crashes, even if you're just reading the project.

### From MEF via import

Note that importing any CPS related service moves your MEF part from the
VS default MEF catalog into a CPS catalog "sub-scope". Import properties
are only 'satisfied' when MEF activated your type (not simply by `new`ing
up an instance of your object).

    [Import]
    IProjectLockService ProjectLockService { get; set; }

### From MEF via an imperative `GetService` query

    ProjectService projectService;
    IProjectLockService projectLockService = projectService.Services.ProjectLockService;

Initialize the `projectService` variable [as described here](obtaining_the_ProjectService.md).

### From a loaded project

    IVsBrowseObjectContext context;
    IProjectLockService projectLockService = context.UnconfiguredProject.ProjectService.Services.ProjectLockService;

Initialize the `context` variable [as described here](finding_CPS_in_a_VS_project.md).

