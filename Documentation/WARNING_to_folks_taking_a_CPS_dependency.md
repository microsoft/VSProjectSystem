WARNING to folks taking a CPS dependency
========================================

WARNING: The CPS APIs are not stable. Any dependency you take on them means you will have to recompile (and possibly change source code) for each major release of VS.
----------------------------------------------------------------------------------------------------------------------------------------------------------------------


TODO: Expand on below points


[Itemid changes](onenote:Documentation.one#ItemIDs&section-id={768BD288-CDB5-4DCE-83D2-FC3994703CEA}&page-id={B30BC875-A380-4FE1-9010-F9A4D3F30727}&base-path=http://devdiv/sites/vspe/prjbld/OneNote/TeamInfo/CPS)

Extensibility story (no flavors, several but limited extensibility points)

Supporting arbitrary item types not listed 

Project load performance has few optimizations - work in progress

No default app designer pages built into CPS


### Your Obligations

By building on CPS, you solemnly swear to:

- Respect [project capabilities](Project_Capabilities.md). Define them exactly when appropriate (no more, no less). This means your team will not punt bugs filed against your project type that describe inappropriately defined project capabilities. These bugs may not cause problems in your scenarios -- but project capabilities are part of your public surface area and are very important to get right to avoid regressions in other teams' scenarios, and to ensure confidence to 3rd parties who use project capabilities as their means of recognizing project types.
- Focus your [AppliesTo] exports to exactly the set of scenarios you intend to impact. Remember if you understate the restrictions that you can impact partner's or customer's scenarios in adverse ways.
- Honor the VS [threading rules](onenote:..\VS%20Threading.one#Threading%20Rules&section-id={F67B08B3-2B6A-4472-A492-EEE6749BF8A3}&page-id={D0EEFAB9-99C0-4B8F-AA5F-4287DD69A38F}&end&base-path=http://devdiv/sites/vspe/prjbld/OneNote/TeamInfo)
