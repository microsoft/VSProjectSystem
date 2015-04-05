How to create a new CPS-based project type
==========================================

If you want to create a new type of project (typically associated with
defining a new file extension for your project such as .booproj), CPS
may be able to greatly reduce the cost of creating and maintaining such
a project system as has historically been the case.

Prior to CPS, options for defining a project system mostly were focused
on [MPFProj](http://mpfproj12.codeplex.com/). Project systems are quite often
100,000s of lines of code. The bugs never end, and each release requires a
great deal of work merely to keep up with the new features of the IDE that
require investment of each project system in order to keep a consistent
experience for all project types. CPS ends this by being a built-in project
system that may serve many project types. This means that you can get
started with your own project type with literally as little as one small
.pkgdef file. Beyond that, the cost you pay is limited to exactly those
aspects of the build and project systems that you wish to customize from
reasonably standard behavior. 

We have created project and item templates to help you get started
with your own project type and some common customizations. These aren't
yet published, and we're hoping to find time to finish them. Even
so, several project types have been defined based on CPS successfully
already. In the meantime, please email the vscpsfte@microsoft.com
alias with your interests, and check out some of our 
[brownbag videos](), particularly [one
where we discuss an overview of project systems and demo these project and item
templates]().

Join the [Common Project System (CPS) Friends]() alias.

The VSIX to install that helps you (w/o any consideration for Razzle)
create new CPS-based project types can be found here:

- Install the [VS SDK](http://www.microsoft.com/en-us/download/details.aspx?id=40758) if you haven't already.
- Install the [Project System Extensibility VSIX]()
- Launch Dev14 (depending on the version of the SDK you installed)
- Open the New Project Dialog
- Set the target framework to .NET Framework 4.6
- Select "Project Type" from the "Visual C# -> Extensibility" node.
- Complete the new project wizard.

You've got yourself your own project type. You can press F5 to launch the
Experimental instance of Dev14 and see your new project appear in the New
Project dialog now.

Now you can customize it. Notice the Add New Item dialog has several
special templates for adding extensions that customize the behavior of
your project type under the Extensibility category.

Note: This VSIX is not yet fully supported, and it does not represent the
full functionality or all the extensibility points available to CPS based
project types.
