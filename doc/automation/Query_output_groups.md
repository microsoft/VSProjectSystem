Query output groups
===================

Having [obtained a `ConfiguredProject`](Finding_CPS_in_a_VS_project.md),
obtain the reference to the `IOutputGroupsService`:

    IOutputGroupsService outputGroupsService = configuredProject.Services.OutputGroups;
    var outputGroup = await outputGroupsService.GetOutputGroupsAsync("Built");

Remember that if you need to synchronously block on the result
instead of `await`ing it, that you follow [the threading rules][1].

 [1]: http://blogs.msdn.com/b/andrewarnottms/archive/2014/05/07/asynchronous-and-multithreaded-programming-within-vs-using-the-joinabletaskfactory.aspx