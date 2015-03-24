Special nodes under projects in Solution Explorer
=================================================

To add nodes directly under the project node in Solution Explorer of a
CPS-based project, you must export an IProjectTreeProvider with a specific
contract name, as shown here:


[AppliesTo("[Project capability expression here](Extensibility_points.md)")]

[[Export](http://index/System.ComponentModel.Composition/A.html#e2e3ab45244b3c5a)([ExportContractNames](http://index/Microsoft.VisualStudio.ProjectSystem.Utilities.v12.0/A.html#924888bb0497d319).[ProjectTreeProviders](http://index/Microsoft.VisualStudio.ProjectSystem.Utilities.v12.0/A.html#a287b13bfa7423f9).[PhysicalViewRootGraft](http://index/Microsoft.VisualStudio.ProjectSystem.Utilities.v12.0/A.html#96d723950e82431b),
typeof([IProjectTreeProvider](http://index/Microsoft.VisualStudio.ProjectSystem.V12Only/A.html#69b8897bf6b80a6d)))]

internal class YourSubtreeProvider :
[ProjectTreeProviderBase](http://index/Microsoft.VisualStudio.ProjectSystem.Utilities.v12.0/A.html#3b3152ccda3f6fa8)

{

    // implementation goes here

}


For reference, you can check out [the References folder for CPS-style C#/VB
projects](http://index/#Microsoft.VisualStudio.ProjectSystem.Implementation/Designers/ReferencesProjectSubtreeProvider.cs,7beeae066dc18866,references).

