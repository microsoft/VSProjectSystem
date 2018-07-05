namespace CpsExtension
{
    using System;
    using System.Collections.Generic;
    using System.Composition;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.ProjectSystem;
    using Microsoft.VisualStudio.ProjectSystem.Debug;
    using Microsoft.VisualStudio.ProjectSystem.VS.Debug;

    [ExportDebugger(Rules.CustomDebugger.SchemaName)]
    [AppliesTo(ProjectCapability.CpsExtension)]
    internal class CustomDebugger : DebugLaunchProviderBase
    {
        [ImportingConstructor]
        public CustomDebugger(ConfiguredProject configuredProject)
            : base(configuredProject)
        {
        }

        public override Task<bool> CanLaunchAsync(DebugLaunchOptions launchOptions)
        {
            // Check if you can launch here

            return Task.FromResult(false);
        }

        public override Task<IReadOnlyList<IDebugLaunchSettings>> QueryDebugTargetsAsync(DebugLaunchOptions launchOptions)
        {
            // Configure debug launch here

            throw new NotImplementedException();
        }
    }
}
