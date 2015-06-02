Sync up shims to latest project model
=====================================

After using the new CPS APIs to update a project, you may at times need to
know when (and perhaps expedite)  the `IVsHierarchy`, DTE, and other legacy
shims have been updated with the latest project state. 

To accomplish this, you must [acquire](finding_CPS_in_a_VS_project.md) the
`IProjectTreeService` export and call its `PublishLatestTreeAsync` method. 
That method returns a Task that completes when the `IVsHierarchy` object 
model has been updated. 
