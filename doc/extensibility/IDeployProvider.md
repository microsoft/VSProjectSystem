IDeployProvider
===============

**[Item template:](project_item_templates.md)** Project Deploy extension

To give a project a deploy action during a solution build, export an
`IDeployProvider` extension.
An item template exists for easier creation of this extension.
Such an extension might look like this.

```csharp
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.ProjectSystem.Build;

[Export(typeof(IDeployProvider))]
[AppliesTo(MyUnconfiguredProject.UniqueCapability)]
internal class MyDeployProvider : IDeployProvider
{
    /// <summary>
    /// Gets a value indicating whether or not deploy is currently supported.
    /// </summary>
    public bool IsDeploySupported
    {
        get { return true; }
    }

    /// <summary>
    /// Signals to start the deploy operation.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that will be set if the deploy is cancelled by the user.
    /// This cancellation token should be passed in as the CancellationToken parameter to the task that is returned.</param>
    /// <param name="outputPaneWriter">A TextWriter that will write to the deployment output pane.</param>
    /// <returns>A task that performs the deploy operation.</returns>
    public async Task DeployAsync(CancellationToken cancellationToken, TextWriter outputPaneWriter)
    {
        outputPaneWriter.WriteLine("Get out your popcorn, folks...");
        await DoMassiveWorkAsync(cancellationToken);
        outputPaneWriter.WriteLine("The party's over. Get back to work!");
    }
    
    /// <summary>
    /// Alerts a project that a deployment operation was successful. Called immediately after the project finishes deployment regardless of the result of other projects in the solution.
    /// </summary>
    public void Commit()
    {
    }

    /// <summary>
    /// Alerts a deployment project that a deployment operation has failed. Called immediately after the project fails deployment regardless of the result of other projects in the solution.
    /// </summary>
    public void Rollback()
    {
    }
}
```
