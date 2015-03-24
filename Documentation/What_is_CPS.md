What is CPS?
============

Introduction
------------

CPS stands for the Common Project System. It is a unified, extensible,
scalable, performant project system that ships in the box with Visual
Studio and is also rehostable to other hosts such as Blend and Azure web
applications.


### Why CPS?

Every language in VS has at least one project system behind it, and sometimes
more. Each language also has many "flavors" that ship in the box. More
languages and more flavors also ship as extensions to VS. Each of these
comes with large source code bases and binary images that must be loaded
in memory. Inconsistent behaviors between project system implementations
make writing a component that interacts with more than one type of project
very difficult. Documentation on expected behavior is sparse and often
ambiguous.

CPS targets replacing all the project systems Microsoft ships, and allowing
3rd parties to also replace their own, with just one Common project system
implementation that can be extended on a per-project basis to provide the
unique experiences a type of project may require, but with very consistent
behaviors everywhere else.

CPS has been designed with modern requirements in mind, so rebasing a
project system on CPS automatically gives it the promise of rehostability
(Monaco, Azure, Blend), scalability and performance that customers demand
for their large solutions.


### Where can I find more documentation?

This OneNote section probably has the most up to date information. But
lots more documentation in varying states of decay exists on Sharepoint:

- [CPS Specs](http://devdiv/sites/vspe/prjbld/Lists/CPS%20Specs/AllItems.aspx) 
- [CPS Extensibility](http://devdiv/sites/vspe/prjbld/Lists/CPS%20Extensibility/AllItems.aspx)
- [CPS Design docs](http://devdiv/sites/vspe/prjbld/Lists/CPS%20Specs/AllItems.aspx)
We also have some [brownbag videos](file:///\\andarno3\public\CPS%20Brownbag%20videos)

