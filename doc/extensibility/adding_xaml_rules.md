# Adding Xaml Rules

In CPS, the role of XAML rules is to describe to CPS _what_ and _how_ properties, items, and metadata
from msbuild matter. There are 3 ways to add XAML rules to the project for CPS to pick up. These are:

1. Including the .xaml file via MSBuild `PropertyPageSchema` items
2. Embedding the xaml in your assembly and exposing it via a MEF export
3. Adding either the xaml file or Rule object programatically via a CPS API

## When to choose what way to add the rule
We recommend adding rules via MSBuild items whenever possible. This method is the most flexible as you can take
full advantage of MSBuild to determine when and how the rule is included. CPS also supports
[extending rules](extending_rules.md) imported via MSBuild. This also does not require your assembly to be
part of the MEF composition.

The two extension ways of adding rules to CPS are both equally acceptable. They are useful for when your rule
is not intended to be extended and essentially "private" to you. An example is a rule backing an
`IDebugLaunchProvider`. Between the two the MEF Export is simpler and leaves the logic of dynamically adding and
removing the rule to CPS.

## Via MSBuild items
This is the recommended way of adding rules to CPS. A xaml rule can be simply included via msbuild evaluation.

``` xml
  <ItemGroup>
    <PropertyPageSchema Include="my_rule.xaml">
      <!-- The Context determines what it applies to. See below for more details -->
      <Context>File;BrowseObject;</Context>
    </PropertyPageSchema>
  </ItemGroup>
```

## Via MEF Export
This method is recommended when your rule is "private" to your implementation, like backing an
`IDebugLaunchProvider`. With the MEF export method, CPS will handle adding/removing the rule
the rule for you.

1. Reference the ProjectSystem SDK Nuget: https://www.nuget.org/packages/Microsoft.VisualStudio.ProjectSystem.SDK.Tools/

2. Include the rule as `XamlPropertyRule` in your project. This will embed the rule in your assembly (named `XamlRuleToCode:{rule_name}.xaml`)
and optionaly generate a partial class for easy access to the rule.
``` xml
  <ItemGroup>
    <XamlPropertyRule Include="my_rule.xaml">
      <Namespace>MyNameSpace</Namespace> <!-- optional -->
      <DataAccess>IRule</DataAccess>  <!-- None or IRule. IRule adds APIs for accessing the properties -->
      <RuleInjectionClassName>ProjectProperties</RuleInjectionClassName> <!-- Name of the generated class. -->
      <RuleInjection>ProjectLevel</RuleInjection> <!-- None or ProjectLevel. None means no class is generated. -->
    </XamlPropertyRule>
  </ItemGroup>
```

3. Add a MEF export. CPS uses __only__ the metadata from this export, we will never evaluate the export:

``` CSharp
    [ExportPropertyXamlRuleDefinition("YourAssemblyName", "XamlRuleToCode:my_rule.xaml", "{Context}")]
    [AppliesTo(MyUnconfiguredProject.UniqueCapability)]
    private object MyRule { get { throw new NotImplementedException(); } }
```

## Via Automation

1. Import `IAdditionalRuleDefinitionsService`. This is in the `UnconfiguredProject` scope.
2. Add the rule via `AddRuleDefinition(string path, string context)` or `AddRuleDefinition(Rule rule, string context)`

``` CSharp

    [Import]
    IAdditionalRuleDefinitionsService AdditionaRuleDefinitionsService { get; }

    /// <summary>
    /// You are responsible for making sure this is called. Can be via an auto or dynamic load component.
    /// </summary>
    void RegisterMyRules()
    {
        this.AdditionaRuleDefinitionsService.AddRuleDefinition(@"path\to\my_rule.xaml", "{Context}");
        this.AdditionaRuleDefinitionsService.AddRuleDefinition(this.GetMyXamlRule(), "{Context}");
    }

    Microsoft.Build.Framework.XamlType.Rule GetMyXamlRule()
    {
        // return a fully constructed Microsoft.Build.Framework.XamlType.Rule instance
    }
```

3. You can also remove the rules added via `RemoveRuleDefinition`.

## What is Context?

Rule `Context` is what determines which catalog the rule shows up in CPS. There are a few options:

``` Csharp
    /// <summary>
    /// Well known property page (rule) contexts as they may appear in .targets files.
    /// </summary>
    public static class PropertyPageContexts
    {
        /// <summary>
        /// Rules that apply at a per-item level, or at the project level to apply defaults to project items.
        /// </summary>
        public const string File = "File";

        /// <summary>
        /// Rules that apply only at the project level.
        /// </summary>
        public const string Project = "Project";

        /// <summary>
        /// Rules that apply only to property sheets.
        /// </summary>
        public const string PropertySheet = "PropertySheet";

        /// <summary>
        /// Rules that are invisible except for purposes of programmatic subscribing to project data.
        /// </summary>
        public const string ProjectSubscriptionService = "ProjectSubscriptionService";

        /// <summary>
        /// A special rule catalog for purposes of programmatic subscribing to project data.
        /// </summary>
        public const string Invisible = "Invisible";

        /// <summary>
        /// Rules that describe properties that appear in the Properties tool window
        /// while an item is selected in Solution Explorer.
        /// </summary>
        public const string BrowseObject = "BrowseObject";

        /// <summary>
        /// Rules that describe configured project properties.
        /// This context currently only supports the Xaml rule to define configuration related project level properties.
        /// </summary>
        public const string ConfiguredBrowseObject = "ConfiguredBrowseObject";
    }
```