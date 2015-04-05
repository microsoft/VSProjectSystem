Property pages
==============

You can define your own custom property pages that can be displayed by
Visual Studio for Projects, Project items, Debuggers, Compiler options,
etc.

This is done using a data driven model, by defining a set of "xaml rules"
that get referenced from the .targets file using the PropertyPageSchema
tag.

When you create a new Project Type, you get out of the box a comprehensive
set of such xaml rules that you can use as a model.

For more details, please refer to the following blog posts:

1. [Platform Extensibility part 1](http://blogs.msdn.com/b/vsproject/archive/2009/06/10/platform-extensibility-part-1.aspx)
2. [Platform Extensibility part 2](http://blogs.msdn.com/b/vsproject/archive/2009/06/18/platform-extensibility-part-2.aspx)

These properties get compiled into .cs at build time 

Note: In Visual Studio, when you create a new xaml file, by default, it
gets included into the project as "Page". You will need to change it to
be 

 custom property pages

TODO

MSBuild "Rules"

What are rules?

- We provide out of the box (wizard) a large set of rules,
  what do they represent?
- Where can users find documentation about each of them
