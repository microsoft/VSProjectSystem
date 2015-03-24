CPS Core vs. CPS VS
===================

A project system may have the potential to be used outside of the VS IDE.
  A developer might want to test his project out of the IDE environment,
or even host the project outside VS.  It is desirable that a carefully
designed project system can be used both inside and outside the Visual
Studio IDE.


On the other hand, the Visual Studio IDE is a rich environment; therefore,
the primary reason to create a project system  is to make it work with
Visual Studio, and integrate it with the rest of the IDE features, e.g.
the solution explorer as well as various type of designers and editors, so
that the project system provides a seamless environment for a developer to
create an application.  That all being said, it means the project system
has to implement contracts which the Visual Studio environment defined
and use other contracts to talk to the rest of the Visual Studio, which
requires the implementation of the project system depend on some parts of
the Visual Studio.


The CPS system, with the purpose of it being used out of the IDE, especially
for the testability, was designed to contain two pieces, namely, a core
piece (the CPS Core), which is expected to be independent of the VS IDE
environment; and another piece (CPS VS) which is deeply integrated with
the Visual Studio IDE, and therefore is not expected to work in a different
environment.


Most extension developers do not need to pay much attention to the difference
between these two pieces.  Advanced developers who want to write unit
tests and control the dependency to VS can benefit from paying attention
to the difference between the two pieces.


Both pieces of the CPS contain one or more contract assemblies, or one or
more implementation assemblies, or possibly some utilities.


### CPS Core

CPS core contains such assemblies as:

- Microsoft.VisualStudio.ProjectSystem.dll
    This is the assembly that contains core contracts of the CPS system,
    including both contracts of components provided by CPS system, and
    those expected to be provided by extensions. The implementation of
    a certain contract in an extension may depend on the Visual Studio
    environment.
    
- Microsoft.VisualStudio.ProjectSystem.Utilities.v14.0.dll
    This contains reusable base classes or utility classes, both of which
    make creating a CPS extensions easier or help other features talk to
    CPS projects.  It is expected to be used by CPS extensions.
    
- Microsoft.VisualStudio.ProjectSystem.Core.Impl.dll
    This contains MEF components in the CPS core system, and it is not
    expected to be directly referenced by an extension assembly.
    
    
### CPS VS

CPS VS contains such assemblies as:

- Microsoft.VisualStudio.ProjectSystem.VS
    This contains contracts of the CPS VS system, including both contracts
    of components provided by CPS VS layer and those expected to be provided
    by extensions and consumed by this layer.  Contracts defined here may
    depend on other VS contracts.
    
- Microsoft.VisualStudio.ProjectSystem.VS.Implementation
    This is the implementation of the CPS VS layer, which contains both
    the implementation of MEF components in the CPS VS layer and objects
    required by VS.  It is exposed to VS as a Visual Studio package, so
    it can be loaded and bootstrap the entire CPS system inside VS.
    
    It is not expected to be referenced by an extension directly.
    
