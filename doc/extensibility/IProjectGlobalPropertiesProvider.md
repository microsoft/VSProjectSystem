`IProjectCapabilitiesProvider`
============================

Caution: Do **NOT** ever set global properties directly on the MSBuild Project
object or its `ProjectCollection`.

To add global properties to a project, export an 
`IProjectGlobalPropertiesProvider` to the `UnconfiguredProject` scope. 
There are a few distinct contract names by which this interface may be 
exported in order to apply to different scenarios. These distinct contracts
are described below in terms of how they should be exported:

| Contract name    | Applies to MSBuild evaluation? | Applies to design-time builds? | Applies to full builds? |
|---|:---:|:---:|:---:|
| `[Export(typeof(IProjectGlobalPropertiesProvider))]` | Yes | Yes | Yes |
| `[ExportBuildGlobalPropertiesProvider(designTimeBuildProperties: false)]` | No | No | Yes |
| `[ExportBuildGlobalPropertiesProvider(designTimeBuildProperties: true)]` | No | Yes | No |

If the global properties you want to set on projects never changes after your
initial properties are determined within the scope of your exported MEF part,
the simplest way to export global properties is by deriving from
`StaticGlobalPropertiesProviderBase`. Note that each instance of your
class may return different properties even with this base class. By importing
`UnconfiguredProject` into your class, for example, you'll have a unique
instance per-project and each instance can return a different set of properties.

```csharp
    [Export(typeof(IProjectGlobalPropertiesProvider))]
    [AppliesTo("YourUniqueCapability")]
    internal class BuildPropertiesProvider : StaticGlobalPropertiesProviderBase
    {
        [ImportingConstructor]
        internal BuildPropertiesProvider(IThreadHandling threadHandling)
            : base(threadHandling.JoinableTaskContext)
        {
        }
 
        public override Task<IImmutableDictionary<string, string>> GetGlobalPropertiesAsync(CancellationToken cancellationToken)
        {
            IImmutableDictionary<string, string> properties = Empty.PropertiesMap
                .SetItem("a", "b");

            return Task.FromResult<IImmutableDictionary<string, string>>(properties);
        }
     }
```

Otherwise if your global properties may change over time, you'll need to 
derive from a different base class and implement more members.

```csharp
    [Export(typeof(IProjectGlobalPropertiesProvider))]
    [AppliesTo("YourUniqueCapability")]
    internal class GlobalProjectCollectionWatcher :
        ProjectValueDataSourceBase<IImmutableDictionary<string, string>>,
        IProjectGlobalPropertiesProvider
```  

