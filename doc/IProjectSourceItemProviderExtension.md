IProjectSourceItemProviderExtension
===================================

This interface may be exported at the ConfiguredProject scope to intercept
project item manipulations. For example, if you want items to be added,
renamed, or removed from a custom imported project file instead of the
root project file (the one the user opened in VS), you should export this
interface to do so.

Note that Shared Projects already have this behavior by exporting this
interface under an AppliesTo expression that evaluates to true when the
"SourceItemsFromImports" project capability is present.

For example:

```csharp
    [Export(typeof(IProjectSourceItemProviderExtension))]
    [AppliesTo("YourUniqueCapability")]
    public class YourProjectSourceItemProviderExtension : IProjectSourceItemProviderExtension
    {
        // implement interface here
    }
```