Appropriately schedule async tasks
==================================


For UnconfiguredProject scoped MEF parts:

    [Import(ExportContractNames.Scopes.UnconfiguredProject)]
    
    IProjectAsynchronousTasksService AsyncTasksService { get; set; }
    

For ConfiguredProject scoped MEF parts:

    [Import(ExportContractNames.Scopes.ConfiguredProject)]
    
    IProjectAsynchronousTasksService AsyncTasksService { get; set; }
    

### Ensure your async work doesn't hold up solution close too long



AsyncTasksService

