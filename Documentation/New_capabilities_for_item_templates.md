New capabilities for item templates
===================================

PerPlatformCompilation (aka. #ifdef capability)

    Defined by: Shared projects 
    
    Summary: Indicates that items in this project are compiled individually
    for every target platform.
    
    What about when/if XAML gains the ability to have per-platform (or
    per-form factor) XAML sections? This might be done at runtime or
    compile time perhaps. We might want to upfront define whether either
    of these future mechanisms would merit this capability or a new one.
    
    Answer: PerPlatformXamlCompilation may come later.
    
    
MultiTarget (aka. Multiplat capability)

    Defined by: PCLs and Shared Projects
    
    Summary: Indicates that items in this project may be applied to multiple
    platforms.
    
    This capability is only present for PCLs that actually target > 1
    platform (not a PCL that targets 1).
    
    
WindowsXamlPageBackgroundPropertyUnsupported

    Defined by: Windows Store (?)
    
    Summary: Indicates that the Page.Background property isn't supported
    on this platform. 
    
    Should we indicate any/all platforms instead of "this" platform so it
    can be reasoned about in combination with MultiPlatformApplicability?
    
