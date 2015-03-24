Obtaining the IProjectLockService
=================================

Please observe CPS [project locking
rules](onenote:Documentation.one#The%20Project%20Lock&section-id={768BD288-CDB5-4DCE-83D2-FC3994703CEA}&page-id={4FF0E6A5-AF0A-4490-8354-2AE8AB74EA96}&base-path=http://devdiv/sites/vspe/prjbld/OneNote/TeamInfo/CPS)
by not retaining any references to MSBuild objects beyond the scope of the
lock and only using these objects while not on the UI thread.  Violating
this exposes your code and other project-related code to the risk of
multithread-related IDE crashes, even if you're just reading the project.

### From MEF via import

Note that importing any CPS related service moves your MEF part from the
VS default MEF catalog into a CPS catalog "sub-scope". Import properties
are only 'satisfied' when MEF activated your type (not simply by "new"ing
up an instance of your object).

[Import]

IProjectLockService ProjectLockService { get; set; }


### From MEF via an imperative GetService query

ProjectService projectService; // [obtained as described
here](onenote:Documentation.one#Obtaining%20the%20ProjectService&section-id={768BD288-CDB5-4DCE-83D2-FC3994703CEA}&page-id={213C67CF-0707-470E-903D-1451517B2F73}&base-path=http://devdiv/sites/vspe/prjbld/OneNote/TeamInfo/CPS)

IProjectLockService projectLockService = projectService.Services.ProjectLockService;


### From a loaded project

IVsBrowseObjectContext context; // [previously
acquired](onenote:Design.one#Finding%20CPS%20in%20a%20VS%20project&section-id={89E1E997-B6E7-4F3E-A523-20563FE2C7D4}&page-id={8250FCD4-5FA4-42A7-9BEE-D0B8E6378092}&base-path=http://devdiv/sites/vspe/prjbld/OneNote/TeamInfo/CPS)

IProjectLockService projectLockService =
context.UnconfiguredProject.ProjectService.Services.ProjectLockService;

