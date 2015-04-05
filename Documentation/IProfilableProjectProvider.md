IProfilableProjectProvider
==========================

The `IProfilableProjectProvider` interface can be exported to provide
functionality for the `IVsProfilableProjectCfg` interface in Visual Studio.

    [Export(typeof(IVsProfilableProjectCfg))]
    [AppliesTo("MyProjectType")]
    internal class MyProfilingProvider : IVsProfilableProjectCfg
    {
        // implementation of IVsProfilableProjectCfg members:
    }

