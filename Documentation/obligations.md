Your obligations as a project system extender
=============================================

**WARNING**: The project system extensibility APIs are not stable. 
Any dependency you take on them means you will have to recompile 
(and possibly change source code) for each major release of VS.

When building on the APIs under the Microsoft.VisualStudio.ProjectSystem namespace,
to keep your own scenarios running smoothly and those other scenarios that are
important to your customers, it is imperative that you:

- Respect [project capabilities](Project_Capabilities.md). Define them exactly
  when appropriate (no more, no less). This means your team will not punt bugs
  filed against your project type that describe inappropriately defined project
  capabilities. These bugs may not cause problems in your scenarios -- but 
  project capabilities are part of your public surface area and are very
  important to get right to avoid regressions in other teams' scenarios, and
  to ensure confidence to 3rd parties who use project capabilities as their
  means of recognizing project types.
- Focus your `[AppliesTo]` exports to exactly the set of scenarios you intend
  to impact. Remember if you understate the restrictions that you can impact
  partner's or customer's scenarios in adverse ways.
- Follow the [VS threading rules](3_Threading_Rules.md)
