Automatic DependentUpon wire-up
===============================

CPS can automatically add the necessary <DependentUpon> metadata to your
project items for both Add New Item and Add Existing Item scenarios based
on file extension patterns that you prescribe in your XAML files.



Limitation:

The child file name must be formed as “<parent file name>.<dependent file
extension>”. A few examples: “.xaml.cs”, “.aspx.cs”, “.aspx.designer.cs”,
etc.

 

How-to:

Need to hint CPS the ‘dependent file extension’ being associated with
a ‘parent file extension’. That is done via authoring xaml rules. For
instance, we would like to make all the files ending with ‘.xaml.cs’ be
under the corresponding parent .xaml files.


Firstly, we need to define a content type and a mapping from ‘file
extension’ to ‘content type’. They could be added into the existing
ProjectItemsSchema.xaml file, or a new xaml file being included in project
system specific .targets file.

    <ProjectSchemaDefinitions
    
    xmlns="http://schemas.microsoft.com/build/2009/properties">
    
        <ContentType
    
            Name="PageXaml"
    
            DisplayName="XAML Page"
    
            ItemType="Page" />
    
    
      <FileExtension Name=".xaml" ContentType="PageXaml" />
    
    </ProjectSchemaDefinitions>
    

Then when CPS handles a .xaml file, it could map it to a content type
and retrieve more metadata from the content type. Next, we need to
hint CPS the ‘dependent file extension’ of ‘.xaml’ via adding metadata
“DependentFileExtensions” into the content type. Multiple dependent file
extensions are allowed and must be separated by “;” (semicolon).

        <ContentType
    
            Name="PageXaml"
    
            DisplayName="XAML Page"
    
            ItemType="Page">
    
            <NameValuePair Name="DependentFileExtensions" Value=".cs" />
    
        </ContentType>
    

Once the xaml rules are authored, CPS would fixup “DependentUpon” metadata
for the child file when the parent file or child file is being added into
project. In addition, it inherits a behavior from VB/C# that supports
adding the child files automatically when user only selects and adds the
parent file.


A real example could be found in C:\Program Files
(x86)\MSBuild\Microsoft\WindowsXaml\v12.0\8.1\en-US\CSharp.ProjectItemsSchema.xaml.

