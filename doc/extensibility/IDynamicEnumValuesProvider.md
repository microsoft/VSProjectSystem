IDynamicEnumValuesProvider
==========================

**[Item template:](project_item_templates.md)** Dynamic Enum Values Provider

Provides a dynamic list of enum values using an `IDynamicEnumValuesGenerator`. You can also provide an optional list of name value pairs when using the editor from a xaml rule (see Usage below).

##IDynamicEnumValuesProvider
```csharp
    [ExportDynamicEnumValuesProvider("DynamicEnumValues1Provider")]
    [AppliesTo("...")]
    public class DynamicEnumValues1Provider : IDynamicEnumValuesProvider
    {
        /// <summary>
        /// Returns an <see cref="IDynamicEnumValuesGenerator"/> instance prepared to generate dynamic enum values
        /// given an (optional) set of options.
        /// </summary>
        /// <param name="options">
        /// A set of options set in XAML that helps to customize the behavior of the
        /// <see cref="IDynamicEnumValuesGenerator "/> instance in some way.
        /// </param>
        /// <returns>
        /// Either a new <see cref="IDynamicEnumValuesGenerator"/> instance
        /// or an existing one, if the existing one can serve responses based on the given <paramref name="options"/>.
        /// </returns>
        public async Task<IDynamicEnumValuesGenerator> GetProviderAsync(IList<NameValuePair> options)
        {
            // TODO: Provide your own implementation
            await Task.Yield();

            return new DynamicEnumValues1Generator();
        }
    }
```
##IDynamicEnumValuesGenerator
**Note:**
Issue #53 
`AllowCustomValues` and `TryCreateEnumValueAsync` are currently ignored by the Project System runtime in the RTM version of Visual Studio 2015, but may be supported in future releases. The suggested implementations below attempt to match the current behavior (any value gets accepted) to minimize disruption when this feature is enabled.
```csharp
    /// <summary>
    /// Generates dynamic enum values.
    /// </summary>
	public class DynamicEnumValues1Generator : IDynamicEnumValuesGenerator
    {
        /// <summary>
        /// Gets whether the dropdown property UI should allow users to type in custom strings
        /// which will be validated by <see cref="TryCreateEnumValueAsync"/>.
        /// </summary>
        public bool AllowCustomValues
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// The list of values for this property that should be displayed to the user as common options.
        /// It may not be a comprehensive list of all admissible values however.
        /// </summary>
        /// <seealso cref="AllowCustomValues"/>
        /// <seealso cref="TryCreateEnumValueAsync"/>
        public async Task<ICollection<IEnumValue>> GetListedValuesAsync()
        {
            await Task.Yield();

            List<IEnumValue> values = new List<IEnumValue>();
            values.Add(new PageEnumValue(new EnumValue() { Name = "abc", DisplayName = "abc", IsDefault = true }));
            values.Add(new PageEnumValue(new EnumValue() { Name = "def", DisplayName = "def" }));
            values.Add(new PageEnumValue(new EnumValue() { Name = "ghi", DisplayName = "ghi" }));

            return values;
        }

        /// <summary>
        /// Tries to find or create an <see cref="IEnumValue"/> based on some user supplied string.
        /// </summary>
        /// <param name="userSuppliedValue">The string entered by the user in the property page UI.</param>
        /// <returns>
        /// An instance of <see cref="IEnumValue"/> if the <paramref name="userSuppliedValue"/> was successfully used
        /// to generate or retrieve an appropriate matching <see cref="IEnumValue"/>.
        /// A task whose result is <c>null</c> otherwise.
        /// </returns>
        /// <remarks>
        /// If <see cref="AllowCustomValues"/> is false, this method is expected to return a task with a <c>null</c> result
        /// unless the <paramref name="userSuppliedValue"/> matches a value in <see cref="GetListedValuesAsync"/>.
        /// A new instance of an <see cref="IEnumValue"/> for a value
        /// that was previously included in <see cref="GetListedValuesAsync"/> may be returned.
        /// </remarks>
        public async Task<IEnumValue> TryCreateEnumValueAsync(string userSuppliedValue)
        {
            await Task.Yield();

            return new PageEnumValue(new EnumValue() { Name = userSuppliedValue, DisplayName = userSuppliedValue });
        }
    }
```

## Usage

```xml
<DynamicEnumProperty Name="MyProperty" DisplayName="My Property" Visible="True" Description="Sample property" EnumProvider="DynamicEnumValues1Provider" />
```
You can also specify a set of options to parameterize your provider using the `ProviderSettings` property. That will be passed to the provider via the `options` parameter of the `GetProviderAsync` method.
```xml
    <DynamicEnumProperty Name="MyProperty" DisplayName="My Property" Visible="True" Description="Sample property" EnumProvider="DynamicEnumValues1Provider" >
        <DynamicEnumProperty.ProviderSettings>
            <NameValuePair Name="Param1" Value="SomeValue" />
            <NameValuePair Name="Param2" Value="Some Other Value" />
        </DynamicEnumProperty.ProviderSettings>
    </DynamicEnumProperty>
```

