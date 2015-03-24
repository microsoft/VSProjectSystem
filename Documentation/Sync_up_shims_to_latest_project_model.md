Sync up shims to latest project model
=====================================

After using the new CPS APIs to update a project, you may at times need to
know when (and perhaps expedite)  the IVsHierarchy, DTE, and other legacy
shims have been updated with the latest project state. 


To accomplish this, you must [acquire](Finding_CPS_in_a_VS_project.md) the
[IProjectTreeService](http://index/#Microsoft.VisualStudio.ProjectSystem.V14Only/Designers/IProjectTreeService.cs,3b84de0f37919a5c,references)
export and call its 

PublishLatestTreeAsync method. That method returns a Task that completes
when the IVsHierarchy object model has been updated. 

