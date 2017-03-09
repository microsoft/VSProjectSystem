How to examine Visual Studio 2017 registry
==========================================

Introduction
------------
In Visual Studio 2017, the registry settings are stored in a private registry files.
This enables multiple installations of Visual Studio side by side, on the same machine.
However, these entries are no longer available in the global registry file.

Here is how to open such a file in regedit:
- Close Visual Studio
- Start Regedit.exe
- Select the `HKEY_LOCAL_MACHINE` node
- From the main menu, select `File` -> `Load Hive...` and select the private registry file. That file is stored in the Local App Data
  `%localappdata%\Microsoft\VisualStudio\<config>\privateregistry.bin` where `<config>` coresponds to the configuration hive you would like to browse
- It will prompt for a name - that represents the name that will be displayed under (e.g. `IsolatedHive`)
- Now you should be able to browse the registry under the hive you created
- Before launching Visual Studio, you need to unload it: From the main menu `File` -> `Unload Hive` before attempting to run VS (otherwise regedit keeps the file locked, and Visual Studio will fail to launch)