Query output groups
===================

Having [obtained a ConfiguredProject](Finding_CPS_in_a_VS_project.md),
obtain the reference to the IOutputGroupsService:


IOutputGroupsService outputGroupsService = configuredProject.Services.OutputGroups;

var outputGroup = await outputGroupsService.GetOutputGroupsAsync("Built");


Remember that if you need to synchronously block on the result
instead of "await"ing it, that you follow [the threading
rules](onenote:..\VS%20Threading.one#VS%20Scenarios&section-id={46FEAAD0-0131-45EE-8C52-C9893F1FD331}&page-id={2C8E6F9B-46BF-448D-B0EE-142C1DCF3C10}&end&base-path=http://devdiv/sites/vspe/prjbld/OneNote/TeamInfo).

