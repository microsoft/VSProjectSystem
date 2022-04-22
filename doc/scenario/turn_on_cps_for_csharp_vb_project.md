Turn on CPS for a C#/VB project
=======================================

Note that SDK-style projects automatically open in CPS. C#, VB and F# projects use the [Managed Language Project System](https://github.com/dotnet/project-system).

You can tell a project is loaded by CPS because the "Refresh" button in Solution Explorer is removed.

To force a legacy project to be loaded by CPS do one of the following:

- Rename a .csproj or .vbproj to .msbuildproj and add to a new solution, or
- In your .sln file, replace the project type GUID {FAE04EC0-301F-11d3-BF4B-00C04F79EFBC} with {13B669BE-BB05-4DDF-9536-439F39A36129}
