Command handlers
================

Visual Studio command routing is a complex system. Some commands get routed
to a project, and when that occurs you may want to handle that command on
behalf of a project. This document describes how to do that.

You may export either of two interfaces:

- `ICommandGroupHandler` 
- `IAsyncCommandGroupHandler`

Either interface works for any command routed to the project. The one
you choose should be based on whether or not your command handler is more
appropriately implemented as an async method. You can change which interface
you export on an existing command handler without backward breaking changes
and handle the same commands as before.

When you export the interface you should use the `ExportCommandGroup`
attribute which allows you to specify the command group GUID your extension
handles commands for. The GUID comes from the vsct file that defines the
Visual Studio command(s) you will handle. While your extension can handle
any number of commands from one command group, a given exported class
can only handle commands from one command group. If you need to handle
commands from multiple command groups, you must define a unique command
handler extension for each command group.

When your command handler is invoked, you will be provided the context on
which the command was invoked. For example, the set of items in Solution
Explorer that were selected when the command was invoked will be passed
to your extension. 

When your handler is invoked, you can inspect the inputs and decide whether
you want to handle the command or return a `NotHandled` result. CPS will daisy
chain in each command handler in [priority order](Extensibility_points.md)
till one of them indicate that they have fully handled the command.

To add a command handler to a JavaScript project for example, you might
code up something like this:

```csharp
    [ExportCommandGroup("some-guid-here")]
    [AppliesTo("your appliesTo expression")]
    internal class MyOwnCommands : ICommandGroupHandler
    {
        public CommandStatusResult GetCommandStatus(...) { }
        public bool TryHandleCommand(...) { }
    }
```
