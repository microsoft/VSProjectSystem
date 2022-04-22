# Adding Xaml Rules

In CPS, the role of XAML rules is to describe to CPS _what_ and _how_ properties, items, and metadata
from msbuild matter. There are 4 ways to add XAML rules to the project for CPS to pick up. These are:

1. Including the .xaml file via MSBuild `PropertyPageSchema` items
2. Embedding the xaml in your assembly and exposing it via a MEF export
3. Adding either the xaml file or Rule object programatically via a CPS API
4. Implement `IRuleObjectProvider` and expose it via a MEF export

## When to choose what way to add the rule

### MSBuild `PropertyPageSchema` items

This is the recommended approach, and the most flexible as you can take full advantage of MSBuild to
detetermine when and how the rule is included.

This is the only option that does not require adding components to the MEF composition or access to
the CPS API, and as such is the _only_ option in scenarios where those are not available (e.g., distributing
a rule as part of a NuGet package).

One disadvantage is that the `PropertyPageSchema` items need to go into a .props or .targets file that
is imported into the user's project. If you do not already have such a .props/.targets file, one of
the other approaches may be simpler.

Another disadvantage is the difficulty around localization. If your rule contains text that needs to
be translated into the user's locale you will need to provide multiple copies of the .xaml file (one
per locale) and add MSBuild logic to pick the appropriate one based on the culture/locale settings.

Distribution/deployment is also a consideration, especially when localization is needed. You need to
ensure that all your XAML files are properly included in your final distributable, whether that be
a NuGet package, VS extension, etc.

### Embedded XAML files

This is a good option if you have access to the MEF composition, don't need the flexibility provided
by `PropertyPageSchema` items, and prefer to define rules in XAML.

Localization is still a disadvantage, as you will still need to produce locale-specific XAML and embed
them properly into the expected satellite assemblies.

Embedded XAML files cannot override or extend rules defined in XAML files. See [extending rules](extending_rules.md)
for more information.

### CPS API

You can use the `IAdditionalRuleDefinitionsService` to dynamically add and remove rule files and objects.
This is generally the most complicated approach, and is only recommended when you need a high degree
of control over when a rule is available in the project or if you need to dynamically generate the contents
(i.e., properties, categories, or metadata) of a rule.

### `IRuleObjectProvider`

This approach is only available starting in VS 2022 Update 1 (Dev17.1).

This is a good option if you have access to the MEF composition, don't need the flexibility provided
by `PropertyPageSchema` items, and prefer to define rule objects in code rather than XAML.

One significant advantage of this approach is localization, as the code generating the rule can load
localized text from a resource file or similar. This avoids the need to create and distribute localized
versions of a .xaml file.

Rule objects defined in this way can only override or extend rules from other `IRuleObjectProvider`s.
See [extending rules](extending_rules.md) for more information 

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

## Via `IRuleObjectProvider`

Define and export an `IRuleObjectProvider` as follows:

``` CSharp

[ExportRuleObjectProvider(name: "MyRuleProviderName", context: "Project")]
[Order(0)]
[AppliesTo(MyUnconfiguredProject.UniqueCapability)]
internal class MyRuleProvider : IRuleObjectProvider
{
    public IReadOnlyCollection<Rule> GetRules()
    {
        var rule = new Rule();
        rule.BeginInit();

        rule.Name = "rule_name";
        rule.PageTemplate = "generic";

        rule.Properties.Add(new StringProperty
        {
            Name = "AStringProperty"
        });

        // Add additional categories, properties, metadata, etc.

        rule.EndInit();

        // Add additional rules.

        return ImmutableList<Rule>.Empty
            .Add(rule);
    }
}
```

Notes:

1. The name given in the `ExportRuleObjectProvider` must be unique to that specific implementation of
`IRuleObjectProvider`. Duplicating the name across multiple implementations may cause them to be ignored
entirely.
2. Rule objects from providers with higher `Order` numbers can override or extend those from providers
with lower numbers.

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