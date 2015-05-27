Reference Manager
=================

## Hide some of the tabs in the Reference Manager Dialog

The tabs in the Reference Manager are driven by CPS extensions. These
extensions are in turn driven by `[AppliesTo]` attributes. Therefore you
can hide tabs from Reference Manager by suppressing the declaration of
the corresponding Project Capabilities in your project. 

In other words, if a tab shows up in Reference Manager that you don't
think should be there, it's because your project declares that it supports
that type of reference. Remove the project capability and that tab will
disappear.

1. Open CustomProject.targets
2. Remove the capabilities that correspond to the tabs you would like to remove (e.g. WinRTReferences)

        <ProjectCapability Include="
            AssemblyReferences;
            COMReferences;
            ProjectReferences;
            WinRTReferences;
            SDKReferences;
            " />

## Add custom tabs to the Reference Manager

**Blocked** - Not currently possible because of APIs are not public

Define new class 

- Implements `IVsReferenceManagerUserAsync`
- tagged with the `ExportIVsReferenceManagerUserAsync` attribute
