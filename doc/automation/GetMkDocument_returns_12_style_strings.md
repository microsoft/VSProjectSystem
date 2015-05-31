`GetMkDocument` returns ">12" style strings
=========================================

The [`GetMkDocument` method is
documented](http://msdn.microsoft.com/en-us/library/microsoft.visualstudio.shell.interop.ivsproject.getmkdocument.aspx)
as being required to return the full path to any item that represents a
file on disk. In JavaScript projects, not all items represent files. For
example, the References folder, and items within it, do not represent
files or folders on disk. Therefore, the `GetMkDocument` returned value is
left up to the project system as an implementation detail. Clients who
call `GetMkDocument` should therefore not assume that the result is always
a filename unless they already know that the item they’re asking about
represents a file.

Callers that intend to parse the string returned from this method should
not attempt to do so without first calling `IVsHierarchy::GetProperty(itemid,
__VSHPROPID.VSHPROPID_TypeGuid)` to check that the result is either
`VSConstants.GUID_ItemType_PhysicalFile` or `VSConstants.GUID_ItemType_PhysicalFolder`.
If it is either of those two values, it should be safe to expect that the
`GetMkDocument` returned value is a file or folder path. But if the `TypeGuid`
property is neither of these values, the result of `GetMkDocument` must be
interpreted as an opaque string, useful only for passing into other methods
of that same project such as `IsDocumentInProject`.

