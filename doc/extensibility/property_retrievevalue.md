How to retrieve the value of a property
=======================================

Properties defined in the xaml rule file get converted a build time into a set of .cs files that expose helpers to easily access their values.

The following sample shows how to retrieve the value of the TargetFrameworkProperty.

We used an [IDeployProvider](IDeployProvider.md) because it has a convenient way to output text. To see it in action, right click in the Solution Explorer on a project of your project type and select Deploy. 

```CSharp
[Export(typeof(IDeployProvider))]
[AppliesTo(MyUnconfiguredProject.UniqueCapability)]
internal class DeployProvider1 : IDeployProvider
{
    /// <summary>
    /// Provides access to the project's properties.
    /// </summary>
    [Import]
    private ProjectProperties Properties { get; set; }

    public async Task DeployAsync(CancellationToken cancellationToken, TextWriter outputPaneWriter)
    {
        var generalProperties = await this.Properties.GetConfigurationGeneralPropertiesAsync();
        string targetFramework = await generalProperties.TargetFrameworkVersion.GetEvaluatedValueAtEndAsync();
        await outputPaneWriter.WriteAsync(targetFramework);
    }

    public bool IsDeploySupported
    {
        get { return true; }
    }

    public void Commit()
    {
    }

    public void Rollback()
    {
    }
}
```
