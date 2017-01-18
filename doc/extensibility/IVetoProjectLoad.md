`IVetoProjectLoad`
============================

**[Item template:](project_item_templates.md)** Project Load Veto extension

To veto a project load there is two extension points `IVetoProjectLoad` and `IVetoProjectPreLoad`.
The difference is that `IVetoProjectPreLoad` is called with the configuration about to be loaded.

Example implementation:

```CSharp
    [Export(typeof(IVetoProjectLoad))]
    [AppliesTo(MyCapability)]
    internal class RandomVetoProjectLoad : IVetoProjectLoad
    {
        public Task<bool> AllowProjectLoadAsync(
            bool isNewProject,
            CancellationToken cancellationToken)
        {
            // let fate decide
            return Task.FromResult((new Random()).Next(2) == 0);
        }
    }
```


