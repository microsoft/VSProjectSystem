`IFileSystemErrorMessageProvider`
=================================

This interface may be exported to the `UnconfiguredProject` scope to supply
alternative messages to display to the user in the event of certain file
I/O errors.

For example, if you have an out-of-proc design-time component that may lock
files and cause common user operations to fail, you may want to customize
the error message to help the user understand what they need to do to
unlock the file to unblock the operation.

