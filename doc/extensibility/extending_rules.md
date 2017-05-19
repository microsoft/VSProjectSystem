# Extending XAML Rules

In CPS, you can extend a XAML rule of the same name by setting `OverrideMode="Extend"`.
Alternatively, you can use `OverrideMode="Replace"` if you desire to completely override any
existing rule of the same name.

This is useful if you are building ontop of an existing project system and want to add
or change properties to the existing rules.

Example:

``` xml
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
- Can only be performed on rules imported via MSBuild, and not by extension points.
- Can be performed multiple times. Including multiple replaces.
- Performed in the order they are returned from MSBuild evaluation.
- When extending a rule, a 3rd rule is created that is a combination of the original rule
and extending rule.
- When replacing a rule, the old rule is discarded and the new rule used.
- All values are taken from the extending rule if they exist, and defaulting
to the original rule if they do not.
- `Properties`, `Categories`, and `Metadata` of a rule are merged together between the base
and extending rule. In the case of collisions, the value of the extending rule is taken.