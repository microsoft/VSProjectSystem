# `IProjectTreeProvider`

## Special nodes under projects in Solution Explorer

To add nodes directly under the project node in Solution Explorer of a
CPS-based project, you must export an `IProjectTreeProvider` with a
specific contract name, as shown here:

```csharp
[AppliesTo("Project capability expression here")]
[Export(ExportContractNames.ProjectTreeProviders.PhysicalViewRootGraft, typeof(IProjectTreeProvider))]
internal class YourSubtreeProvider : ProjectTreeProviderBase
{
    // implementation goes here
}
```

For the capability expression, please see [Extensibility Points](index.md).
