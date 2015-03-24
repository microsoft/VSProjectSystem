Obtaining the MSBuild.Project from CPS
======================================

- Acquire the [IProjectLockService](Obtaining_the_IProjectLockService.md)
- [Obtain a ConfiguredProject](Finding_CPS_in_a_VS_project.md) for which you want to get the MSBuild Project object.
- Acquire a read, upgradeable read or write lock, as appropriate, and use the MSBuild Project object exclusively within the lock:
using (var access = await projectLockService.WriteLockAsync()) {

    MSBuild.Project project = await access.GetProjectAsync(configuredProject);
    
    // party on it, respecting the type of lock you've acquired. 
    
    // If you're going to change the project in any way, 
    
    // check it out from SCC first:
    
    await access.CheckoutAsync(configuredProject.UnconfiguredProject.FullPath);
    
}


Note that it's important that you use await. Do not use .Result or
.Wait() on these async methods or your code will malfunction and/or hang.
If you must do this within a synchronous method, see [threading rule
#2](onenote:..\VS%20Threading.one#Threading%20Rules&section-id={46FEAAD0-0131-45EE-8C52-C9893F1FD331}&page-id={D0EEFAB9-99C0-4B8F-AA5F-4287DD69A38F}&object-id={A500C3BF-DDDE-4AC2-BEFD-8CF6BD4F2B31}&29&base-path=http://devdiv/sites/vspe/prjbld/OneNote/TeamInfo).

    
Please observe CPS [project locking rules](The_Project_Lock.md) by not
retaining any references to MSBuild objects beyond the scope of the lock and
only using these objects while not on the UI thread.  Violating this exposes
your code and other project-related code to the risk of multithread-related
IDE crashes, even if you're just reading the project.

