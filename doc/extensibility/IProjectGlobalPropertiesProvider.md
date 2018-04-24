`IProjectGlobalPropertiesProvider`
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
        internal BuildPropertiesProvider(IProjectCommonServices services)
            : base(services)
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
    internal class MyProjectGlobalPropertiesProvider :
        ProjectValueDataSourceBase<IImmutableDictionary<string, string>>,
        IProjectGlobalPropertiesProvider
    {
        private static readonly IImmutableDictionary<string, string> MyProperties
            = ImmutableDictionary<string, string>.Empty.Add("MyProperty", "MyValue");

        /// <summary>
        /// A value that increments with each new map of properties.
        /// </summary>
        private volatile IComparable version = 0L;

        /// <summary>
        /// The block to post to when publishing new values.
        /// </summary>
        private ITargetBlock<IProjectVersionedValue<IImmutableDictionary<string, string>>> targetBlock;

        /// <summary>
        /// The backing field for the <see cref="SourceBlock"/> property.
        /// </summary>
        private IReceivableSourceBlock<IProjectVersionedValue<IImmutableDictionary<string, string>>> publicBlock;

        /// <summary>
        /// Initializes a new instance of the <see cref="MyProjectGlobalPropertiesProvider"/> class.
        /// </summary>
        /// <param name="commonServices">The CPS common services.</param>
        protected MyProjectGlobalPropertiesProvider(IProjectCommonServices commonServices)
            : base(commonServices)
        {
        }

        /// <inheritdoc />
        public override NamedIdentity DataSourceKey { get; } =  new NamedIdentity("MyProperties");

        /// <inheritdoc />
        public override IComparable DataSourceVersion => this.version;

         /// <inheritdoc />
        public override IReceivableSourceBlock<IProjectVersionedValue<IImmutableDictionary<string, string>>> SourceBlock
        {
            get
            {
                this.EnsureInitialized();
                return this.publicBlock;
            }
        }

        /// <summary>
        /// See <see cref="IProjectGlobalPropertiesProvider"/>
        /// </summary>
        public Task<IImmutableDictionary<string, string>> GetGlobalPropertiesAsync(CancellationToken cancellationToken)
        {
            // Calculate the latest properties. This will be called when a user starts a build.
            return Task.FromResult(MyProperties);
        }

        /// <inheritdoc />
        protected override void Initialize()
        {
            base.Initialize();
            var broadcastBlock = new BroadcastBlock<IProjectVersionedValue<IImmutableDictionary<string, string>>>(
                null,
                new DataflowBlockOptions() { NameFormat = "MyGlobalProperties: {1}" });

            this.publicBlock = broadcastBlock.SafePublicize();
            this.targetBlock = broadcastBlock;

            // Hook up some events, or dependencies, that calculate new properties and post to the target block as needed.
            // Posting to the target block with an incremented DataSourceVersion will trigger a new project evaluation with
            // your new properties.
            this.targetBlock.Post(
                new ProjectVersionedValue<IImmutableDictionary<string, string>>(
                    MyProperties,
                    ImmutableDictionary<NamedIdentity, IComparable>.Empty.Add(this.DataSourceKey, this.DataSourceVersion)));
        }
    }
```
