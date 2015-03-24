Interfaces NOT defined on IVsProject object
===========================================

Comparing to the current VB/C# projects, you may find some interfaces are
missing on CPS's implementation of IVsProject object. 


These interfaces are not implemented currently.

- [IVsSingleFileGeneratorFactory](https://msdn.microsoft.com/en-us/library/Microsoft.VisualStudio.Shell.Interop.IVsSingleFileGeneratorFactory.aspx) 
- [IVsProjectResources](https://msdn.microsoft.com/en-us/library/Microsoft.VisualStudio.Shell.Interop.IVsProjectResources.aspx)
- [IVsDeferredSaveProject](https://msdn.microsoft.com/en-us/library/Microsoft.VisualStudio.Shell.Interop.IVsDeferredSaveProject.aspx)
- [IVsAsynchOpenFromSccProjectEvents](https://msdn.microsoft.com/en-us/library/Microsoft.VisualStudio.Shell.Interop.IVsAsynchOpenFromSccProjectEvents.aspx)
- [IVsProjectBuildMessageReporter](https://msdn.microsoft.com/en-us/library/Microsoft.VisualStudio.Shell.Interop.IVsProjectBuildMessageReporter.aspx)
    
It is by design that these 2 interfaces are not implemented, because CPS
is designed to be extended via MEF instead of COM aggregation. There is
no support for the old flavors.  

- [IVsAggregatableProject](https://msdn.microsoft.com/en-us/library/Microsoft.VisualStudio.Shell.Interop.IVsAggregatableProject.aspx)
- [IVsProjectFlavorCfgProvider](https://msdn.microsoft.com/en-us/library/Microsoft.VisualStudio.Shell.Interop.IVsProjectFlavorCfgProvider.aspx)
