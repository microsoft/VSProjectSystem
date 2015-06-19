Property value editors
======================

**[Item template:](project_item_templates.md)** Property Page Value Editor extension

In some cases, you may want to expand the set of editors to provide a richer experience. For example, you may want to provide a file picker editor to select a file, or a color editor to pick a color.

This can be done by implementing a custom value editor. For that, you will need to:
### 1. Export the `IPropertyPageUIValueEditor` interface to provide a custom value editor

Use the `ExportMetadata` attribute to associate a `Name` with the custom editor. This value will be used for consuming the editor in the xaml rule.

```csharp
    [Export(typeof(IPropertyPageUIValueEditor))]
    [ExportMetadata("Name", "MyValueEditor")]
    [AppliesTo(MyUnconfiguredProject.UniqueCapability)]
    internal class MyValueEditor : IPropertyPageUIValueEditor
    {
        /// <summary>
        /// Invokes the editor.
        /// </summary>
        /// <param name="serviceProvider">The set of potential services the component can query for, mainly for access back to the host itself.</param>
        /// <param name="ruleProperty">the property being edited</param>
        /// <param name="currentValue">the current value of the property (may be different than property.Value - for example if host UI caches the new values until Apply button)</param>
        /// <returns>The new value.  May be <paramref name="currentValue"/> if no change is intended.</returns>
        public async Task<object> EditValueAsync(IServiceProvider serviceProvider, IProperty ruleProperty, object currentValue)
        {
            // TODO: Provide your own editor implementation
            await Task.Yield();
            string currentString = currentValue as string;

            // For exemplification purposes, using a simple editor that reverts the original string
            char[] characters = currentString.ToCharArray();
            Array.Reverse(characters);
            string newString = new string(characters);

            return newString;
        }

    }
```

### 2. Specify which properties should use the custom editor in the Xaml rule

```xml
    <StringProperty Name="MyProperty" DisplayName="My Property" Visible="True" Description="Sample property">
        <StringProperty.ValueEditors>
            <ValueEditor EditorType="MyValueEditor" DisplayName="&lt;MyValueEditor...&gt;" />
        </StringProperty.ValueEditors>
    </StringProperty>
```

You should now be able to invoke your custom editor by using the arrow next to the text input field for your property and choosing `<MyValueEditor...>` from the list.
