IProjectPropertiesProvider
==========================


### Notes

This should not be used to add user input validation. User input validation
should be done inside your MSBuild targets at build time. Remember that users
may set properties to $(Macros), use conditions, etc., that make it difficult
or impossible for you to truly validate user input at design-time. [Custom value
editors](onenote:Documentation.one#Property%20value%20editors&section-id={225839EC-68C9-40B3-AD41-FD0EC362CEDE}&page-id={ED60D113-DC4C-49FD-B75D-019E13934728}&base-path=https://microsoft.sharepoint.com/teams/vsprojectsystem/Shared
Documents/VS Project System Documentation) can help you guide your customers
to create valid input. 

