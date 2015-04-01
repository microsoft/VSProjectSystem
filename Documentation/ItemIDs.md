ItemIDs
=======

Background on ItemIDs
---------------------

In many other project systems, ItemIDs are actually raw memory pointers--opaque
to the caller but meaningful within a project system, sort of like a VSCOOKIE.
If someone held onto an itemid beyond its invalidation, and then passed
that itemid into the project system, the project system might dereference
it and end up crashing the product because of an access violation. It has
therefore always been critical for anyone with an itemid to not use it
after the item it represents is destroyed.

### When can ItemIDs be destroyed?

ItemIDs can be destroyed only on the UI thread. 

When destroying them, a project system is obliged to raise an
IVsHierarchyEvent::OnItemDeleted event notifying listeners of that item's
demise. In fact, OnItemDeleted is called just before destroying the ItemID,
which gives event handlers a chance to ask questions about the ItemID one
last time before it is impossible to do so.

This is true not just for native project systems that only operate on
the UI thread. It's true by the undocumented, de facto rule that that is
where they have always been destroyed. And since the only way for clients
to be notified of an itemid invalidation is on the UI thread, a client on
the UI thread that has not been notified of an invalidation must be able
to assume it is therefore safe to pass that itemid back to the project
system. We can't have project systems issuing itemid's, then invalidating
them on background threads while the poor client on the UI thread is about
to hand it back to the project system now, can we? ;-p

### How can a client be sure an ItemID has not been destroyed?

The simplest way is to never store them, and always obtain them on the UI
thread. For example, this code is always safe (when executed on the main
thread):

    int rootFirstChildItemId;

    ErrorHandler.ThrowOnFailure(hierarchy.GetProperty(
        VSConstants.VSITEMID_Root,
        VSHPROPID_FirstChild,
        out rootFirstChildItemId));

    int nextSiblingItemId;

    ErrorHandler.ThrowOnFailure(hierarchy.GetProperty(
        rootFirstChildItemId,
        VSHPROPID_NextSibling,
        out nextSiblingItemId));

    // ...


The above code is safe because:

- It passes ItemIDs to the project system that it just barely obtained.
  Nothing could have occurred in between the steps of acquisition and
  reuse to invalidate the itemid.
- The code does not mutate the project in any way. 
- The code is executing on the UI thread.
    
#### FAQ

- Q: COM STA rules will marshal all my calls to the UI thread for me. 
     Why must I be on the UI thread if dealing with itemid's?
 **A**: If you're on a background thread, the call into IVsHierarchy will 
     only get marshaled to the UI thread if that particular project system
     implementation is a native COM STA object. Calls to a project system
     implemented in managed code will generally not marshal to the UI thread.
     But even if marshaling did occur, it's still not safe. Remember that
     between the call where you acquire the ItemID and reuse it, you're no
     longer on the UI thread and thus other code could be on the UI thread
     mutating the project. And if that's the case, then the ItemID that you
     just obtained may have been destroyed and you wouldn't know about it.
- Q: What if I'm mutating the project? Could the ItemID be invalidated then?
 **A**: Yes. Obviously if you ask the project to delete a file represented
     by that ItemID, that ItemID will likely be destroyed immediately. But
     even seemingly benign mutations could invalidate an ItemID. For example,
     setting a project property could cause ItemIDs to be destroyed (if the
     MSBuild project file beneath the project defines an item conditionally
     based on that property, for example). 

### How can a client be notified when an ItemID is destroyed?

If you must store an ItemID longer than you directly control the main
thread, you must take steps to ensure that you destroy that ItemID when
the project system does. 

Call IVsHierarchy::AdviseHierarchyEvents passing in your implementation
of IVsHierarchyEvents. When an itemid is destroyed, either of two things
will happen:

- `IVsHierarchyEvents::OnItemDeleted` will be raised with the itemid to be destroyed.
- `IVsHierarchyEvents::OnInvalidateItems` will be raised on some ancestor of the itemid, which at this point may be destroyed or may not be. You have to assume that it's destroyed and rediscover the current itemid by getting the project system to give it to you again.

#### FAQ

- Q: Why doesn't the project system just offer a simple and safe way to check
     whether an ItemID is still valid so I don't have to deal with this event
     handler stuff?
 **A**: Even if there were a safe way to simply ask the project system if an
     ItemID were valid before using it, that would be imprecise since a
     destroyed ItemID can be recycled to represent a different item in which
     case it would be valid, but not have the same significance that your code
     may apply to it.
     So really the "is this itemid valid?" query is not the one your code would
     need to be asking. Rather, it would be "Is this itemid valid, and does it
     represent foo.cs?" 
- Q: Then how can store state regarding an item without dealing with event
     handlers?
 **A**: Instead of storing itemids, store the item moniker. Then whenever you
     need the itemid you obtain it using the moniker:

        IVsHierarchy::GetCanonicalName(itemid, out moniker)
        IVsHierarchY::ParseCanonicalName(moniker, out itemid)

What is unique about CPS and ItemIDs? 
--------------------------------------

CPS isn't a project system written in native code, so we don't use pointers
for our itemids. We just use an incrementing integer. Since ItemIDs are
to be treated as opaque values, this shouldn't make any difference to you.
But one thing it does mean is that if you pass in an invalid itemid CPS
won't crash the product, but CPS will throw an exception back at you. 

It so happens that the C#, VB, and C++ project systems don't change their
itemids very often. In fact they almost never do. A file moved within a
project tends to keep its itemid. This is an implementation detail of a
project system and nothing that anyone outside should count on. But we've
found that some folks do. They don't listen to events, and they assume an
itemid is valid forever. 

CPS has a greater tendency to change itemids for project items and open
documents than earlier project systems. For example during a file move
or rename there is a very good chance the itemid will change. This means
that clients that were previously tested on the legacy c# project system
may need to be re-tested when run against CPS and fixed to be more careful
when handling itemids.
