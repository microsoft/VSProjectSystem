# `IProjectPropertiesProvider`

TODO

### Notes

This should not be used to add user input validation. User input validation
should be done inside your MSBuild targets at build time. Remember that users
may set properties to `$(Macros)`, use conditions, etc., that make it difficult
or impossible for you to truly validate user input at design-time. [Custom value
editors](property_value_editors.md) can help you guide your customers
to create valid input. 
