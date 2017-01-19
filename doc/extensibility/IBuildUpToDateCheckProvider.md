How to add a fast up-to-date check?
===================================

**[Item template:](project_item_templates.md)** Build Up-To-Date Check extension

Applicable exports of this class are called as part of CPS' implementation of 
`IVsBuildableProjectCfg.StartUpToDateCheck` ([MSDN](https://msdn.microsoft.com/en-us/library/microsoft.visualstudio.shell.interop.ivsbuildableprojectcfg.startuptodatecheck.aspx)).

CPS filters the up-to-date check providers into 2 categories: before draining [critical tasks](https://github.com/Microsoft/VSProjectSystem/blob/b70104c4781749369c995a48f83e64607a0c6594/doc/scenario/defer_critical_project_operations.md),
and after. With each set being called before or after the draining of critical tasks. If you do not
depend on critical tasks it is recommended to set this to `true` so CPS can avoid unnecessary
draining of critical tasks.

Example usage:

```CSharp
[Export(typeof(IBuildUpToDateCheckProvider))]
[ExportMetadata("BeforeDrainCriticalTasks", true)] // Optional, default value is false
[AppliesTo("MyCapability")]
private class RandomBuildUpToDateCheckProvider : IBuildUpToDateCheckProvider
{
    /// <summary>
    /// Check if project outputs are up-to-date (i.e there is no need to build)
    /// </summary>
    /// <param name="buildAction">The build action to perform.</param>
    /// <param name="logger">A logger that may be used to write out status or information messages regarding the up-to-date check.</param>
    /// <param name="cancellationToken">A token that is cancelled if the caller loses interest in the result.</param>
    /// <returns>A task whose result is true if project is up-to-date</returns>
    public Task<bool> IsUpToDateAsync(BuildAction buildAction, TextWriter logger, CancellationToken cancellationToken = default(CancellationToken))
    {
        // let fate decide
        return Task.FromResult((new Random()).Next(2) == 0);
    }

    /// <summary>
    /// Gets a value indicating whether the up-to-date check is available at the moment.
    /// </summary>
    /// <param name="cancellationToken">A token that is cancelled if the caller loses interest in the result.</param>
    /// <returns>A task whose result is <c>true</c> if the up-to-date check is enabled.</returns>
    public Task<bool> IsUpToDateCheckEnabledAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        // let fate decide
        return Task.FromResult((new Random()).Next(2) == 0);
    }
}
```
