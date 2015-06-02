How to detect whether a project is a CPS project
================================================

The following code snippet demonstrates how to detect whether a given
project is a "pure" CPS-based project system (e.g. Javascript, and not VC++
which is only half CPS). This technique does not require referencing any
CPS-specific assemblies, and therefore has the advantage of not risking
having breaks with subsequent versions of Visual Studio when CPS changes
its unstable API.

```csharp
    using Microsoft.VisualStudio.Shell;

    internal static bool IsCpsProject(this IVsHierarchy hierarchy)
    {
        Requires.NotNull(hierarchy, "hierarchy");
        return hierarchy.IsCapabilityMatch("CPS");
    }
```