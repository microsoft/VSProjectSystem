# List of Dataflow Sources in CPS

| Legend        |                                                                     |
|---------------|---------------------------------------------------------------------|
| __Interface__ | The exported interface                                              |
| __Value__     | Short description of the produced value. All values are immutable.  |
| __Scope__     | The MEF scope this belongs to. Service, Unconfigured, Configured    |
| __Provider__  | Who provides this. System = CPS only, Extension = extenders and CPS |

| Interface                                 | Value                                          | Scope                             | Provider  |
|-------------------------------------------|------------------------------------------------|-----------------------------------|-----------|
| `IActiveConfiguredProjectProvider`        | Configured project                             | Unconfigured                      | System    |
| `IActiveConfiguredProjectSnapshotService` | Current state of the project                   | Unconfigured                      | System    |
| `IAdditionalRuleDefinitionsService`       | Additional XAML rules to the project           | Unconfigured                      | System    |
| `IDependentFilesProviderDataSource`       | Dynamic dependent files mapping                | Unconfigured                      | Extension |
| `IDynamicDebugTargetsGenerator`           | List of debug targets to show to the user      | Configured                        | Extension |
| `IOutputGroupsService`                    | Project build outputs                          | Configured                        | System    |
| `IProjectBuildSnapshotService`            | Post-design-time build results                 | Configured                        | System    |
| `IProjectCapabilitiesProvider`            | Project capabilities                           | Service, Unconfigured, Configured | Extension |
| `IProjectGlobalPropertiesProvider`        | Properties for msbuild evaluation              | Unconfigured, Configured          | Extension |
| `IProjectImportTreeSnapshotService`       | Import tree of the project                     | Configured                        | System    |
| `IProjectItemSchemaService`               | Content type and item type for the project     | Configured                        | System    |
| `IPropertyPagesCatalogProvider`           | Project catalog, XAML rules by context         | Configured                        | System    |
| `IProjectSharedFoldersSnapshotService`    | Shared folders in the project                  | Configured                        | System    |
| `IProjectSnapshotService`                 | Current state of the project                   | Configured                        | System    |
| `IProjectSnapshotWithCapabilitiesService` | Current state of the project with capabilities | Unconfigured                      | System    |
| `IProjectTreeProvider`                    | The hierarchical project tree structure        | Unconfigured                      | Extenson  |
