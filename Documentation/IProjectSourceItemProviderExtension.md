[IProjectSourceItemProviderExtension](http://index/Microsoft.VisualStudio.ProjectSystem.V14Only/R/90124dbaa6314d68.html)
========================================================================================================================

This interface may be exported at the ConfiguredProject scope to intercept
project item manipulations. For example, if you want items to be added,
renamed, or removed from a custom imported project file instead of the
root project file (the one the user opened in VS), you should export this
interface to do so.

Note that Shared Projects already have this behavior by exporting this
interface under an AppliesTo expression that evaluates to true when the
"SourceItemsFromImports" project capability is present.


For example:


    [[Export](http://index/System.ComponentModel.Composition/A.html#a2a14721c3852bea)(typeof([IProjectSourceItemProviderExtension](http://index/Microsoft.VisualStudio.ProjectSystem.V14Only/A.html#90124dbaa6314d68)))]

    [[AppliesTo](http://index/Microsoft.VisualStudio.ProjectSystem.Utilities.v14.0/A.html#84c3c630297dcc4b)("YourUniqueCapability")]

    public class
[YourProjectSourceItemProviderExtension](http://index/#Microsoft.VisualStudio.ProjectSystem.Implementation/Items/ImportedProjectSourceItemProviderExtension.cs,fd7236653bc03381)
: [IProjectSourceItemProviderExtension](http://index/Microsoft.VisualStudio.ProjectSystem.V14Only/A.html#90124dbaa6314d68)

    {

        // implement interface here

    }



