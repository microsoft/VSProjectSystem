# Windows Script Sample
Simple project type that supports .js and .vbs files; can run and debug them using `cscript.exe`.

## Showcased Features
- File globbing (project automatically includes *.js and *.vbs under the project cone); globs are defined in .props file, to minimize the content of the project file
- Editing the project file without unloading the project (right click on the project node - `Edit`)
- Project handles own reload without the solution doing a full reload when the project file changes on disk
- Custom properties
- Custom item type - project defines `Script` item type to include *.js and *.vbs files 
- Custom debug launch provider using custom properties to invoke `cscript.exe` and run the script file
- Run from command line using the same parameters defined above via msbuild target:
  ```
  msbuild /t:run
  ```

## How to use the sample
### Walkthrough
1. Open `WindowsScript.sln` in Visual Studio 2017 and use Ctrl + F5 to run it. This will open the Experimental instance of Visual Studio
2. From the main menu, `File` -> `New Project`, select `WindowsScript` -> `Windows Script Project` and press `OK`. This will create a new project that contains a `Start.js` file
3. In the generated project, insert a breakpoint on the 2nd line of `Start.js`
4. Press F5 - this will start the debugger; it will stop at the breakpoint
5. Switch to the `cscript` window - the default implementation displays information about the execution context (which script is running, current directory, arguments)

### Adding new files
The sample doesn't currently provide any item templates. To create new files, you can:
- copy-paste the default `Start.js`
- create new script files (.vbs or .js) in Windows Explorer; the project includes automatically all *.js and *.vbs located under the project cone

### Specifying a different script file to run
Project uses a custom property `StartItem` to specify which script file to run. The default value is `Start.js`.

1. Open the Property Pages dialog (right click on the project node -> `Properties`)
2. Set the value of `Start Item` property (located under `Common Properties` -> `General`) to specify the file you would like to run (e.g. `Foo.js`)

This value gets persisted in the project file:
```xml
<StartItem>Foo.js</StartItem>
```

### Specifying additional debugging parameters
Project supports a few additional properties to control the script execution.
They can be set from the project properties dialog (`Configuration Properties` -> `Debugging` -> `Script Debugger`) and get persisted in the .user file (next to the project).

- `Command` (persisted as `RunCommand`) - specifies which tool to invoke to run the script; by default it uses `cscript.exe` but you can set it to `wscript.exe` (offered conveniently as a predefined value when you expand the drop down)
- `Command Arguments` (persisted as `RunCommandArguments`) - specifies additional arguments to pass to the script; by default it is empty
- `Working Directory` (persisted as `RunWorkingDirectory`) - specifies the working directory for the script; by default it is the project folder

## Implementation Notes
### Custom properties
- Default values are defined in the .props file. This allows them to be used from msbuild when the value is not specified.
- `.user` file is imported in .targets for consumption by msbuild

  ```xml
  <Import Project="$(MSBuildProjectFullPath).user" Condition="Exists('$(MSBuildProjectFullPath).user')" /> 
  ```

These properties are consumed in 2 different places:
- Debug Launch Provider (F5 and Ctrl F5 from Visual Studio) - see `ScriptDebuggerLaunchProvider.cs`
- The `Run` Target - that can be used from MSBuild - see `CustomProject.targets` using
  ```
  MSBuild /t:Run
  ```
  