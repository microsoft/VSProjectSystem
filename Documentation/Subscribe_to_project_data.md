Subscribe to project data
=========================

To read some subset of data (i.e. properties and/or items) from the project
and be notified when that data changes:


private IDisposable SubscribeToProjectData(

    UnconfiguredProject unconfiguredProject, 
    
    Func<IProjectVersionedValue<IProjectSubscriptionUpdate>,
    System.Threading.Tasks.Task> receiver,
    
    params string[] ruleNames)
    
{

    var subscriptionService = unconfiguredProject.Services.ActiveConfiguredProjectSubscription;
    
    var receivingBlock = new ActionBlock<IProjectVersionedValue<IProjectSubscriptionUpdate>>(receiver);
    
    return subscriptionService.JointRuleSource.SourceBlock.LinkTo(receivingBlock,
    ruleNames: ruleNames);
    
}


private async Task ProjectUpdateAsync(IProjectVersionedValue<IProjectSubscriptionUpdate>
update) {

    // This runs on a background thread. [Switch to the Main
    thread](onenote:..\VS%20Threading.one#VS%20Scenarios&section-id={46FEAAD0-0131-45EE-8C52-C9893F1FD331}&page-id={2C8E6F9B-46BF-448D-B0EE-142C1DCF3C10}&object-id={2CD5FFFB-CB6F-4AE0-8774-BF357A019765}&D&base-path=http://devdiv/sites/vspe/prjbld/OneNote/TeamInfo)
    (if necessary):
    
    await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
    

    // Process the update.
    
    // Either in terms of what's currently there…
    
    var typeScriptCompileItems = update.Value.CurrentState["TypeScriptCompile"].Items;
    
    // …or in terms of what has been added or removed recently:
    
    var changes = update.Value.ProjectChanges["TypeScriptCompile"].Difference;
    
    
    // Note that the first time this callback is invoked, all current
    items are presented as if they have just been added.
    
    // This allows you to always code in terms of the diff, and it
    automatically just works the first time.
    
    // If you don't like this behavior, you can turn it off by passing
    "initialDataAsNew: false" into the LinkTo method.
    
}


UnconfiguredProject unconfiguredProject; // obtained as [described
here](Finding_CPS_in_a_VS_project.md)

IDisposable disposeToUnsubscribe = SubscribeToProjectData(unconfiguredProject,
ProjectUpdateAsync, "TypeScriptCompile");

// Note that ProjectUpdateAsync is called asynchronously,

// so it probably hasn't been invoked by the time SubscribeToProjectData
has returned.


To subscribe to the entire set of source items (which is an open set, as
in CPS this is extensible by the customer):

TODO


TODO: talk about the various source blocks (including ProjectBuildRuleBlock)

