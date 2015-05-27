Interfaces defined on IVsProject object
=======================================

CPS implementation of the IVsProject object is the main presentation of
the project in Visual Studio IDE, and also the main entry point for the
other components to interact with the project. It implements a bunch of
interfaces. Some of them are mandatory to be implemented on a project,
some of them are optional but nice to have, some of them are for internal
purposes. I am going to list and categorize most of the interfaces except
the ones for internal usages. Hopefully this will demonstrate what could
be expected from the IVsProject object. Noting all the interfaces below are
designed for other Visual Studio components to interact with the project.
CPS extensions should use CPS APIs. 


The Visual Studio COM interfaces that people would expect from IVsProject
object in general:

- [IVsGetCfgProvider](https://msdn.microsoft.com/en-us/library/microsoft.visualstudio.shell.interop.ivsgetcfgprovider.aspx)
- [IVsProjectBuildSystem](https://msdn.microsoft.com/en-us/library/microsoft.visualstudio.shell.interop.ivsprojectbuildsystem.aspx)
- [IVsBuildPropertyStorage](https://msdn.microsoft.com/en-us/library/microsoft.visualstudio.shell.interop.ivsbuildpropertystorage.aspx)
- [IVsSccProject2](https://msdn.microsoft.com/en-us/library/Microsoft.VisualStudio.Shell.Interop.IVsSccProject2.aspx)
- [IVsProject4](https://msdn.microsoft.com/en-us/library/microsoft.visualstudio.shell.interop.ivsproject4.aspx)
- [IVsUIHierarchy](https://msdn.microsoft.com/en-us/library/microsoft.visualstudio.shell.interop.ivsuihierarchy.aspx)
- [IVsPersistHierarchyItem2](https://msdn.microsoft.com/en-us/library/microsoft.visualstudio.shell.interop.ivspersisthierarchyitem2.aspx)
- [IOleCommandTarget](https://msdn.microsoft.com/en-us/library/microsoft.visualstudio.ole.interop.iolecommandtarget.aspx)
- [IVsHierarchyDeleteHandler](https://msdn.microsoft.com/en-us/library/Microsoft.VisualStudio.Shell.Interop.IVsHierarchyDeleteHandler.aspx)
- [IVsProjectSpecialFiles](https://msdn.microsoft.com/en-us/library/microsoft.visualstudio.shell.interop.ivsprojectspecialfiles.aspx)
- [IVsFileBackup](https://msdn.microsoft.com/en-us/library/microsoft.visualstudio.textmanager.interop.ivsfilebackup.aspx)
- [IPersistFileFormat](https://msdn.microsoft.com/en-us/library/microsoft.visualstudio.shell.interop.ipersistfileformat.aspx)
- [IVsProjectSpecificEditorMap](https://msdn.microsoft.com/en-us/library/Microsoft.VisualStudio.Shell.Interop.IVsProjectSpecificEditorMap.aspx)
- [IVsDependencyProvider](https://msdn.microsoft.com/en-us/library/Microsoft.VisualStudio.Shell.Interop.IVsDependencyProvider.aspx)
- [IVsDesignTimeAssemblyResolution](https://msdn.microsoft.com/en-us/library/Microsoft.VisualStudio.Shell.Interop.IVsDesignTimeAssemblyResolution.aspx)

The extended Visual Studio COM interfaces:

- [IVsBuildPropertyStorage3](https://msdn.microsoft.com/en-us/library/microsoft.visualstudio.shell.interop.ivsbuildpropertystorage3.aspx)
- [IVsProject5](https://msdn.microsoft.com/en-us/library/microsoft.visualstudio.shell.interop.ivsproject5.aspx)
- [IVsHierarchyDeleteHandler3](https://msdn.microsoft.com/en-us/library/microsoft.visualstudio.shell.interop.ivshierarchydeletehandler3.aspx)
- [IVsFileBackup2](https://msdn.microsoft.com/en-us/library/microsoft.visualstudio.shell.interop.IVsFileBackup2.aspx)

The drag/drop Visual Studio COM interfaces:

- [IVsHierarchyDropDataSource](https://msdn.microsoft.com/en-us/library/microsoft.visualstudio.shell.interop.IVsHierarchyDropDataSource.aspx)
- [IVsHierarchyDropDataSource2](https://msdn.microsoft.com/en-us/library/microsoft.visualstudio.shell.interop.IVsHierarchyDropDataSource2.aspx)
- [IVsHierarchyDropDataTarget](https://msdn.microsoft.com/en-us/library/microsoft.visualstudio.shell.interop.IVsHierarchyDropDataTarget.aspx)
- [IVsHierarchyDirectionalDropDataTarget](https://msdn.microsoft.com/en-us/library/microsoft.visualstudio.shell.interop.IVsHierarchyDirectionalDropDataTarget.aspx)
- [IVsUIHierWinClipboardHelperEvents](https://msdn.microsoft.com/en-us/library/microsoft.visualstudio.shell.interop.IVsUIHierWinClipboardHelperEvents.aspx)

This CPS interface is designed as the entry point for other Visual Studio
components to get into CPS world. 

- IVsBrowseObjectContext

The additional Visual Studio COM interfaces for variant purposes:

- [IVsProjectFlavorReferences](https://msdn.microsoft.com/en-us/library/microsoft.visualstudio.shell.interop.IVsProjectFlavorReferences.aspx)
- [IVsProjectFaultResolver](https://msdn.microsoft.com/en-us/library/microsoft.visualstudio.shell.interop.IVsProjectFaultResolver.aspx)
- [IVsManifestReferenceResolver](https://msdn.microsoft.com/en-us/library/microsoft.visualstudio.shell.interop.IVsManifestReferenceResolver.aspx)
- [IVsSupportItemHandoff](https://msdn.microsoft.com/en-us/library/microsoft.visualstudio.shell.interop.IVsSupportItemHandoff.aspx)
- [IVsParentProject](https://msdn.microsoft.com/en-us/library/microsoft.visualstudio.shell.interop.IVsParentProject.aspx)
