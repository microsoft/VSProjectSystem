# DataFlow Examples

## New Slim DataFlow blocks

In Visual Studio 2019, in order to reduce the memory usage, we introduced slim implementation for some of the data flow blocks.

Before:
```csharp
var actionBlock = new ActionBlock<IProjectVersionedValue<IProjectSubscriptionUpdate>>(
    this.OnSubscriptionUpdate,
    new ExecutionDataflowBlockOptions { NameFormat = "My Block {1}"});
```

After:
```csharp
var actionBlock = DataflowBlockSlim.CreateActionBlock<IProjectVersionedValue<IProjectSubscriptionUpdate>>(
    this.OnSubscriptionUpdate,
    nameFormat: "My Block {1}");
```

## Original Data Source

``` Csharp
/// <summary>
/// An example of an original datasource.
/// The goal is to listen to an original source and publish a new value whenever it changes.
/// </summary>
[Export(typeof(IMyOriginalDataSourceContract))]
private class MyOriginalDataSource : ProjectValueDataSourceBase<string>, IMyOriginalDataSourceContract
{
    private int sourceVersion;

    private BroadcastBlock<IProjectVersionedValue<string> broadcastBlock;

    private IReceivableSourceBlock<IProjectVersionedValue<string> publicBlock;

    private string lastPublishedValue;

    [ImportingConstructor]
    protected MockOriginalDataSource(UnconfiguredProject unconfiguredProject)
        : base(unconfiguredProject.Services)
    {
    }

    public override NamedIdentity DataSourceKey { get; } = new NamedIdentity(nameof(MyOriginalDataSource));

    public override IComparable DataSourceVersion => this.sourceVersion;

    public override IReceivableSourceBlock<IProjectVersionedValue<string> SourceBlock
    {
        get
        {
            this.EnsureInitialized();
            return this.publicBlock;
        }
    }

    /// <summary>
    /// Dataflow is lazy-initialized in CPS. Only when a downstream source actually listens to your SourceBlock
    /// do we want to actually start everything up.
    /// </summary>
    protected override void Initialize()
    {
        base.Initialize();
        this.broadcastBlock = new BroadcastBlock<string>(
            null,
            new DataflowBlockOptions() { NameFormat = "MyOriginalDataSource: {1}" }); // this is recommended to identify your block
        
        this.publicBlock = broadcastBlock.SafePublicize(); // CPS extension method to seal off Post
        this.PostNewValue(this.GetNewValue()); // always publish an initial value!

        // subscribe this.OnChange() to be notified when the original source changes
        // Could be something like watching for file change events of a config file
    }

    /// <summary>
    /// This is called whenever the original source changes
    /// </summary>
    private void OnChange()
    {
        this.PostNewValue(this.GetNewValue());
    }

    /// <summary>
    /// Gets the new value from your original source you are publishing values for
    /// </summary>
    private string GetNewValue()
    {
        // implementation. Could be reading a file for example.
    }

    private string PostNewValue(string newValue)
    {
        // Add thread safety as needed. Make sure to never regress the data source version published
        if (newValue != this.lastPublishedValue) // only publish if you have to
        {
            this.lastPublishedValue = newValue;
            this.broadcastBlock.Post(
                new ProjectVersionedValue<string>(
                    newValue,
                    ImmutableDictionary.Create<NamedIdentity, IComparable>().Add(
                        this.DataSourceKey,
                        this.sourceVersion++))); // increment your version either before or after publishing. So long as it increments!
        }
    }
}
```

## Chained Data Source single source

``` Csharp
/// <summary>
/// Links to a single upstream source and produces values based on it.
/// Since this is chained, its versions are derived from the source versions
/// </summary>
[Export(typeof(IMyChainedDataSourceContract))]
private class MyChainedDataSource : ChainedProjectValueDataSourceBase<string>, IMyChainedDataSourceContract
{

    [ImportingConstructor]
    protected MyChainedDataSource(UnconfiguredProject project)
        : base(project.Services)
    {
    }

    [Import]
    private MyOriginalDataSource DataSource { get; set; }

    protected override IDisposable LinkExternalInput(ITargetBlock<IProjectVersionedValue<string>> targetBlock)
    {
        // The datasource version is derived from the upstream version,
        // we do not need a version of our own. However, we must produce a value for every supplied value.
        // Even if our value does not change.
        this.JoinUpstreamDataSources(this.DataSource); // Must join upstream to avoid deadlocks!
        var transformBlock = new TransformBlock<IProjectVersionedValue<string>, IProjectVersionedValue<string>>(
            i => i.Derive(v => v + "New Value!"),
            new ExecutionDataFlowBlockOptions() { NameFormat = "MyChainedDataSource Input: {1}" }); // do some processing with this value and produce a new value
        var firstLink = this.DataSource.SourceBlock.LinkTo(transformBlock, new DataflowLinkOptions { PropagateCompletion = true });
        transformBlock.LinkTo(targetBlock, new DataflowLinkOptions { PropagateCompletion = true });
        return firstLink; // disposing first link will cause everything linked to it to dispose
    }
}
```

## Chained Data Source multiple sources

``` Csharp
/// <summary>
/// Links to multiple upstream data sources and produces a single value from them.
/// This is done using <see cref="ProjectDataSources.SyncLinkTo" />.
/// </summary>
[Export(typeof(IMyChainedDataSourceContract2))]
private class MyChainedDataSource2 : ChainedProjectValueDataSourceBase<string>, IMyChainedDataSourceContract2
{

    [ImportingConstructor]
    protected MyChainedDataSource2(UnconfiguredProject project)
        : base(project.Services)
    {
    }

    [Import]
    private MyOriginalDataSource DataSource1 { get; set; }

    [Import]
    private MyOtherOriginalDataSource DataSource2 { get; set; }

    protected override IDisposable LinkExternalInput(ITargetBlock<IProjectVersionedValue<string>> targetBlock)
    {
        // The datasource version is derived from the upstream version,
        // we do not need a version of our own. However, we must produce a value for every supplied value.
        // Even if our value does not change.
        this.JoinUpstreamDataSources(this.DataSource1, this.DataSource2); // Must join upstream to avoid deadlocks!

        // When joining multiple sources, your transform block takes in a IProjectVersionedValue<Tuple<>>
        var transformBlock = new TransformBlock<IProjectVersionedValue<Tuple<string, string>>, IProjectVersionedValue<string>>(
            i => i.Derive(v => v.Item1 + v.Item2 + "New Value!"),
            new ExecutionDataFlowBlockOptions() { NameFormat = "MyChainedDataSource2 Input: {1}" ); // do some processing with this value and produce a new value
        var firstLink = ProjectDataSources.SyncLinkTo(
            this.DataSource1.SourceBlock.SyncLinkOptions(),
            this.DataSource2.SourceBlock.SyncLinkOptions(),
            transformBlock);
        
        transformBlock.LinkTo(targetBlock, new DataflowLinkOptions { PropagateCompletion = true });
        return firstLink; // disposing first link will cause everything linked to it to dispose
    }
}
```
### `SyncLinkTo` Overloads

1. Overloads for 2 - 6 inputs, all typed as `Typle<T1, ...T6>`.
    ``` Csharp
    /// <summary>
    /// Links several source blocks carrying project versioned valued to a target block
    /// such that only consistently versioned tuples are posted to the target block.
    /// </summary>
    /// <typeparam name="T1">The type of the value that comes from block <paramref name="source1"/>.</typeparam>
    /// ...
    /// <typeparam name="T6">The type of the value that comes from block <paramref name="source6"/>.</typeparam>
    /// <param name="source1">A source block.</param>
    /// ...
    /// <param name="source6">A source block.</param>
    /// <param name="target">The block that receives consistently versioned tuples from the source blocks.</param>
    /// <param name="linkOptions">
    /// The link options to apply between the source blocks and the target block.
    /// If <c>null</c> the default behavior is to propagate completion.
    /// </param>
    /// <param name="cancellationToken">A token whose cancellation terminates the flow of data.</param>
    /// <returns>A link that may be disposed to terminate the flow of data.</returns>
    public static IDisposable SyncLinkTo<T1, ...T6>(
            SourceBlockAndLink<IProjectVersionedValue<T1>> source1,
            ...
            SourceBlockAndLink<IProjectVersionedValue<T6>> source6,
            ITargetBlock<IProjectVersionedValue<Tuple<T1, ...T6>>> target,
            DataflowLinkOptions linkOptions = null,
            CancellationToken cancellationToken = default(CancellationToken))
    ```

2. Another overload for immutable list of inputs and an immutable list input to the target. You will have to
   manually cast the inputs in the `target` block.
    ``` Csharp
    /// <summary>
    /// Jointly links several source blocks to a target block, taking care to ensure
    /// that any message posted to the target block carries a set of messages from the
    /// source blocks that are all based on the same version of backing data.
    /// </summary>
    /// <param name="sourceBlocks">The blocks to be joined.</param>
    /// <param name="target">The block to receive the joined messages.</param>
    /// <param name="linkOptions">The options to apply to the link to the target block.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the link (without throwing InvalidOperationExceptions due to a project unload).</param>
    /// <returns>An object that may be disposed to terminate the link.</returns>
    public static IDisposable SyncLinkTo(
            ImmutableList<SourceBlockAndLink<IProjectValueVersions>> sourceBlocks,
            ITargetBlock<Tuple<ImmutableList<IProjectValueVersions>, IImmutableDictionary<NamedIdentity, IComparable>>> target,
            DataflowLinkOptions linkOptions,
            CancellationToken cancellationToken = default(CancellationToken))
    ```