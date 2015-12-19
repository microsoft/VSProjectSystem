Extensibility points
====================

General guidance for extension authors
--------------------------------------

##### Understand the threading model

CPS extensions are expected to conform to the [CPS/VS threading
model](../overview/threading_model.md). Please become familiar with it 
before writing CPS extensions. This will help avoid responsiveness 
issues, deadlocks, and non-deterministic hangs in your scenarios.

##### We promise we won't bite

Please send a code review to the CPSCR alias when introducing or significantly
modifying CPS extensions, particularly for your first few extensions so
we can help catch common mistakes.

### `AppliesToAttribute`

Every extension based on MEF exports must include an `[AppliesTo("...")]`
attribute on the exported type or member. The string argument is
an expression that evaluates to a boolean value, as [documented
here](http://msdn.microsoft.com/en-us/library/microsoft.visualstudio.shell.interop.ivsbooleansymbolexpressionevaluator.evaluateexpression.aspx).
The terms that may be used are [project
capabilities](../overview/about_project_capabilities.md).

Take care to write the appropriate `AppliesTo` expression so that your
extension applies exactly to the set of projects you mean to impact. Remember
that although your expression may seem to work based on it lighting up for
your intended scenario, it may also be lighting up for unintended projects
that you're not testing. So test positive and negative cases, and ask the
cpsfriends DL for review of your `AppliesTo` expression.

Even if your extension uses code to effectively no-op for project types
that are not appropriate, doing so requires that the assembly containing
your extension be loaded and initialized in the process. It is best to
use the `AppliesTo` attribute effectively to prevent your assembly from even
loading when it does not apply to a given project.

### `OrderPrecedenceAttribute`

When more than one extension applies to a given project, CPS may pick the
"most preferred" extension to fulfill some requirement, or it may loop
through all extensions to allow each to participate. The preference order,
or the order of that loop, can be influenced by adding an `[OrderPrecedence]`
attribute to your export.

Lacking any such attribute on an export, the sort order value is effectively
`0`. When setting a nonzero value to your export, consider future requirements
of yourself or others to sort themselves above or below your export. Choose
a value that is larger than the values of those extensions you are trying to
supersede, but lower than the values of other (possibly future) extensions
that need to supersede your own extension. As examples, a common value to
use to override default behavior is `1000`. When superseding another export
with a value of `1000` you may consider using `2000`. Later if an extension
needs to be placed two other known extensions that use respectively `1000`
and `2000`, you could use `1500` as your value. And so on.

No extension should use `int.MaxValue` or similar very high values in
an attempt to force their own extension to be non-overridable. Future
requirements are not yet known, and you or others may need to override
your extension, and you should leave ample room in the value space to
supersede your extension.

Typically you may omit this attribute from your exports until you discover
a need to supersede behavior defined elsewhere. 

### Troubleshooting

#### When a CPS extension isn't invoked

Following are some things to check for when you have a CPS extension that
doesn't get invoked when you expect it to:

- Verify that the assembly containing your extension is included in the 
  VS MEF catalog.
  - Is your assembly referenced from an `extension.vsixmanifest` file 
    that is found in the VS install directory in a location that VS 
    searches for extensions?
- Does your extension have an `[Export(typeof(IExtension))]` attribute 
  or another `Export`ish attribute as documented for the extensibility point?
- Verify that your export defines an `[AppliesTo]` attribute on the 
  same type or member that the `[Export]` attribute is found on, and 
  with an expression that is appropriate for the project type(s) you 
  are targeting. In some cases this may mean you have an `[AppliesTo]` 
  attribute that appears both on the type (that is exported) and a 
  method (that is also exported).
- Verify that your MEF part isn't rejected due to some authoring error 
  by scanning for it in this file:

        %localappdata%\microsoft\VisualStudio\14.0\ComponentModelCache\Microsoft.VisualStudio.Default.err
