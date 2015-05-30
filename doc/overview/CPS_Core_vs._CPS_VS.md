CPS Core vs. CPS VS
===================
A well-designed project system can be used inside and outside the Visual Studio IDE to enable increased productivity and flexibility. For example, a developer may want to test their project system outside of the IDE or even host the project system in an entirely different environment. 

On the other hand, the most likely reason one would create a CPS-based project system is to make it work with VS. Integrating it with other VS features (e.g., Solution Explorer, editor, designers) is what leads to the seamless experience developers desire. This rich integration requires parts of your project system to implement contracts defined by, and specific to, Visual Studio.

To enable the flexibility described above, CPS is made up of two parts. The 'CPS Core' is designed to be independent of VS and 'CPS VS' is what enables the deep integration with VS.

Most extension developers do not need to pay much attention to the difference between these two pieces. However, developers who want to write unit tests and control their project system's dependency on VS will benefit from understanding the role each part plays in the system.

Both pieces of CPS contain one or more contract assemblies, one or more implementation assemblies, and possibly some utility assemblies.

### CPS Core

CPS Core contains such assemblies as:

- Microsoft.VisualStudio.ProjectSystem.dll
  - This assembly contains core CPS contracts including contracts of components provided by CPS and those expected to be provided by extensions. The implementation of a certain contract in an extension may depend on the Visual Studio environment.

- Microsoft.VisualStudio.ProjectSystem.Utilities.v14.0.dll
  - This assembly contains reusable base classes or utility classes that make creating CPS extensions easier or help other features talk to CPS projects.  It is expected to be used by CPS extensions.

- Microsoft.VisualStudio.ProjectSystem.Core.Impl.dll
  - This assembly contains MEF components in the CPS Core system. It is not expected to be directly referenced by an extension assembly.
    
### CPS VS

CPS VS contains such assemblies as:

- Microsoft.VisualStudio.ProjectSystem.VS
  - This assembly contains CPS VS contracts including contracts of components provided by the CPS VS layer and those expected to be provided by extensions and consumed by this layer. Contracts defined here may depend on other VS contracts.
    
- Microsoft.VisualStudio.ProjectSystem.VS.Implementation
  - This assembly is the CPS VS layer's implementation. It contains the implementation of CPS VS MEF components and objects required by VS. It is exposed to VS as a Visual Studio Package so it can be loaded and bootstrap the entire CPS system inside VS.
    
    It is not expected to be referenced by an extension directly.
