# `IPublishProvider`

The `IPublishProvider` interface can be exported to provide functionality
for the `IVsPublishableProjectCfg` interface in Visual Studio.

Make sure you have `[AppliesTo("xxx")]` attribute on your exported type where
`xxx` is your project type.

Sample code below:

```csharp
[Export(typeof(IPublishProvider))]
[AppliesTo("MyProjectType")]
internal class MyPublishProvider : IPublishProvider
{
    [Import]
    private ProjectProperties Properties { get; set; }

    public Task<bool> IsPublishSupportedAsync()
    {
        return Task.FromResult(true);
    }

    public async Task PublishAsync(CancellationToken cancellationToken, TextWriter outputPaneWriter)
    {
        await Task.Yield();
    }

    public Task<bool> ShowPublishPromptAsync()
    {
        return Task.FromResult(false);
    }
}
```
