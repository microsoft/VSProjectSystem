# COM Aggregation

In Visual Studio 2017 a MEF-based extension for COM aggregation has been added to CPS.
This extension point should be used only when all other CPS extension points are
insufficient and this is __absolutely__ needed. Also note that this extension does
not behave 100% like project flavoring and has other limitations. This is explained
below.

## Extension Point

* Contract: `[Export(ExportContractNames.VsTypes.ProjectNodeComExtension)]`
* Metadata: `[ComServiceIid(typeof(COMInterface))]`
  * Where `COMInterface` is the interface you want to export
  * You can use many
  * Can also use `[ComServiceIid(typeof(MyType), includeInherited: true)]`
    * This will recursivley add all interfaces found on `MyType`

## Limitations

1) __Cannot__ override existing CPS implementations of COM interfaces.
   It is only for adding __new__ COM interfaces to the CPS `IVsProject`.
2) Does not work with dynamic capabilities. Any export of this extension
   must have the capabilities in its `[AppliesTo]` be static. CPS will
   reload the project, or even throw, if it the set of these exports ever
   changes in the lifespan of a loaded project. This means that any
   capabilities that turn on one of these must come in at the unconfigured
   level and not the configured level.

## Example

``` CSharp
[Export(ExportContractNames.VsTypes.ProjectNodeComExtension)]
[AppliesTo(MyCapability)] // MUST be a static capability from the unconfigured scope
[Order(1)] // Higher values win, but put yours low as possible
[ComServiceIid(typeof(IVsComInterface))]
internal class MyComImplementation : IVsComInterface
{
  // Implementation
}
```
