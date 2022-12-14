Query output groups
===================

Having [obtained a `ConfiguredProject`](finding_CPS_in_a_VS_project.md),
obtain the reference to the `IOutputGroupsService`:

```csharp
IOutputGroupsService outputGroupsService = configuredProject.Services.OutputGroups;
var outputGroup = await outputGroupsService.GetOutputGroupsAsync("Built");
```

Remember that if you need to synchronously block on the result
instead of `await`ing it, that you follow [the threading rules][1].

 [1]: https://devblogs.microsoft.com/premier-developer/asynchronous-and-multithreaded-programming-within-vs-using-the-joinabletaskfactory/