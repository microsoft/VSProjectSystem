# Property pages

**[Item template:](project_item_templates.md)** XAML Rule

You can define your own custom property pages that can be displayed by
Visual Studio for Projects, Project items, Debuggers, Compiler options,
etc.

This is done using a data driven model, by defining a set of "XAML rules"
that get referenced from the .targets file using the `PropertyPageSchema`
tag.

When you create a new Project Type, you get out of the box a comprehensive
set of such XAML rules that you can use as a model.

For more details, please refer to the following blog posts:

1. [Platform Extensibility part 1](https://learn.microsoft.com/archive/blogs/vsproject/platform-extensibility-part-1)
2. [Platform Extensibility part 2](https://learn.microsoft.com/archive/blogs/vsproject/platform-extensibility-part-2)

These properties get compiled into `.cs` file at build time 

Note: In Visual Studio, when you create a new XAML file, by default, it
gets included into the project as "Page". You will need to change it to
be custom property pages.

## TODO

MSBuild "rules"

What are rules?

- We provide out of the box (wizard) a large set of rules,
  what do they represent?
- Where can users find documentation about each of them?
