Obtaining the IThreadHandling service
=====================================

### From MEF via import

Note that importing any CPS related service moves your MEF part from the
VS default MEF catalog into a CPS catalog "sub-scope". Import properties
are only 'satisfied' when MEF activated your type (not simply by "new"ing
up an instance of your object).

[Import]

IThreadHandling ThreadHandling { get; set; }


### From MEF via an imperative GetService query

ProjectService projectService; // [obtained as described
here](onenote:Documentation.one#Obtaining%20the%20ProjectService&section-id={768BD288-CDB5-4DCE-83D2-FC3994703CEA}&page-id={213C67CF-0707-470E-903D-1451517B2F73}&base-path=http://devdiv/sites/vspe/prjbld/OneNote/TeamInfo/CPS)

IThreadHandling threadHandling = projectService.Services.ThreadingPolicy;


### From a loaded project

IVsBrowseObjectContext context; // [previously
acquired](onenote:Design.one#Finding%20CPS%20in%20a%20VS%20project&section-id={89E1E997-B6E7-4F3E-A523-20563FE2C7D4}&page-id={8250FCD4-5FA4-42A7-9BEE-D0B8E6378092}&base-path=http://devdiv/sites/vspe/prjbld/OneNote/TeamInfo/CPS)

IThreadHandling threadHandling = context.UnconfiguredProject.ProjectService.Services.ThreadingPolicy;

