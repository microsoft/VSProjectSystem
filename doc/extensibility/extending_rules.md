# Extending XAML Rules

In CPS, you can extend a XAML rule of the same name by setting `OverrideMode="Extend"`.
Alternatively, you can use `OverrideMode="Replace"` if you desire to completely override any
existing rule of the same name.

This is useful if you are building ontop of an existing project system and want to add
or change properties to the existing rules.

Please refer to the [CpsExtension sample](/samples/CpsExtension/) that shows adding a custom property to to a .cs file in an SDK-Style C# project.

Example:

```xml
<Rule
  Name="RuleToExend"
  DisplayName="File Properties"
  PageTemplate="generic"
  Description="File Properties"
  OverrideMode="Extend"
  xmlns="http://schemas.microsoft.com/build/2009/properties">
  <!-- Add new properties, data source, categories, etc -->

</Rule>
```

## Extending Behavior

- Can only extend/replace rules of the __same name__.
- __(17.0 and earlier)__ Can only be performed on rules imported via MSBuild, and not by extension points.
- __(17.1 and later)__ Rules imported via MSBuild and extension points can be extended (but see Source Order, below).
- Can be performed multiple times. Including multiple replaces.
- Performed in the order they are returned from MSBuild evaluation.
- When extending a rule, a 3rd rule is created that is a combination of the original rule
and extending rule.
- When replacing a rule, the old rule is discarded and the new rule used.
- All values are taken from the extending rule if they exist, and defaulting
to the original rule if they do not.
- `Properties`, `Categories`, and `Metadata` of a rule are merged together between the base
and extending rule. In the case of collisions, the value of the extending rule is taken.

## Source Order

The source of a rule affects which rules it can override/extend, and which rules can override/extend it. Here are the sources in decreasing priority order (i.e. rules from sources earlier in the list can override/extend those later in the list):

1. MSBuild `PropertyPageSchema` items
    * Note that in SDK-style projects, the order of these items can be difficult to control due to how the SDK props and targets are included implicitly.
    * Using the default SDK-Style projects behavior, this mechanism works as expected if the `.targets` file defining the custom `PropertyPageSchema` items is imported via nuget package, but it doesn't work if it is imported from the project file directly.
2. Rule _files_ added via `IAdditionalRuleDefinitionsService`
3. Rule _objects_ added via `IAdditionalRuleDefinitionsService`
4. Embedded `XAML` rules
5. `IRuleObjectProvider` implementations

Also, each source has its own means of ordering its rules for the purposes of overriding/extending (e.g., MSBuild items follow the item declaration order in the project; MEF-exported sources use `[Order(...)]` attributes, etc.).