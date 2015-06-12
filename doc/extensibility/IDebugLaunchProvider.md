IDebugLaunchProvider
===============

**[Item template:](project_item_templates.md)** Custom Debugger extension

## Tutorial
To add a debugger to a project you should export an `IDebugLaunchProvider`.
The simplest way to do this for Visual Studio 2015 is by using the [item template](project_item_templates.md)
for that purpose:

1. Project -> Add New Item
2. C# -> Extensibility -> Project System
3. Pick the "Custom Debugger Extension" template
4. Follow the instructions on screen.

## Support for older versions of Visual Studio

This particular extensibility point also exists as far back as Visual C++ 2010.

There are different extensions for each version of VS. Downloading the 
extension and creating a project from its project template will pop up 
a README file that explains how to use it.

- [Visual C++ 2015 Debugger Launch Extension][1]
- [Visual C++ 2013 Debugger Launch Extension][2]
- [Visual C++ 2012 Debugger Launch Extension][3]
- [Visual C++ 2010 Debugger Launch Extension][4]

Consider renaming debugger class (or rule file) so they don't collide
(namespace is the only distinguisher)
    
 [1]: https://visualstudiogallery.msdn.microsoft.com/7fe7f19f-ceb9-47e3-b440-c62df2b85281
 [2]: http://visualstudiogallery.msdn.microsoft.com/e831676e-9510-4651-b724-cf4299b220b5
 [3]: http://visualstudiogallery.msdn.microsoft.com/8d2faf2c-3937-489a-9e0a-c43ff26ca427
 [4]: http://visualstudiogallery.msdn.microsoft.com/f1e9c8b5-134e-4bb1-bd0e-37a220dae99e
