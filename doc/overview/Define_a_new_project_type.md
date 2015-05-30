Define a new project type
=========================

## When is it appropriate to define a new project type?

Defining a new project type is aligned with defining a new project file
extension. This may be appropriate when any of the following are true:

1. The project file schema is drastically altered and isn't compatible with the existing schema
2. The project will compile a new language.

You usually should *not* define a new project type when you only intend
to [modify, enhance, or "flavor" an existing project type](Extend_an_existing_project_type.md).

## How to define a new project type

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

A project type template and several item templates can help you get started
with your own project type and some common customizations. 

- Install the [prerequisites](PreReqs.md)
- Launch Visual Studio 2015
- Open the New Project dialog
- Set the target framework to .NET Framework 4.6
- Select "Project Type" from the "Visual C# -> Extensibility" node
- Complete the new project wizard

You've got yourself your own project type. You can press F5 to launch the
Experimental instance of Visual Studio 2015 and see your new project appear
in the New Project dialog now.

Now you can customize it. Notice the Add New Item dialog has several
special templates for adding extensions that customize the behavior of
your project type under the Extensibility category.
