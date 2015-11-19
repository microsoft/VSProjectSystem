Responsive design
=================

CPS has what we call a "responsive design." This isn't in reference to UI
responsiveness, but rather a description of how much of CPS is architected.

### A traditional project system

In a traditional project system, when code changes the project file, that
same code is responsible for everything else the IDE expects, such as:

- Updating a view model (the model that drives Solution Explorer)
- Update running document table as necessary
- Raise `IVsTrackProjectDocuments` events
- Invoke single file generators

This means that every place where code changes the project file, that code
must have complete knowledge of their host environment and fulfill all the
requirements of that hosting environment. In traditional project systems,
this isn't too bad because there is a fixed and small number of mutating
operations in the project system and maintaining them, or adding one more
once or twice a cycle, is manageable. 

All mutations and events are raised synchronously on the main thread.
Mutations that necessarily take a long time (like those requiring a
design-time build to complete) must block the UI thread for potentially
long periods for the operation to complete.

The object model is mutable and private, so clients that discover project
system data always clone it into their own object models, resulting in
project data replication and increased VM usage.

### Responsive Design

In CPS, when code changes the project file, that's all it does. It's job
is over. The rest of the steps that must accompany that change based on
the host (Visual Studio) are implemented elsewhere by other CPS extensions
(many of them built-in). After a project change is completed and the project
write lock released, CPS takes an immutable snapshot of the updated project.
It then compares the new snapshot to the old snapshot to produce a diff.
In this way, it can discern all the effects of the changes (including the
side effects that may not have been intended by the original mutation)
and take appropriate actions.

Project data can be discovered by legacy clients of project systems via the
legacy `IVs*` interfaces, which all respond based on this immutable snapshot
of project data. This allows project mutations to occur on background
threads without compromising backward compatibility with STA clients that
expect the project to not change while they're on the UI thread.

#### Why did we create this?

We were actually somewhat driven into this design during the VS 2012 cycle
while writing "CPS-VS" for JavaScript projects. The issue was we really
wanted to allow concurrency for project reads. That required introducing
locks. Locks are pretty incompatible with an STA thread in managed code
since blocking an STA on a contested lock invites reentrancy and has
been shown to cause crashes, corruption, and hangs. So by saying we want
concurrent reads, all reads and writes have to be off the STA thread. But
being off the STA for changes makes raising events on the STA for those
changes, and protecting clients from seeing changes midway, required a
design that included immutable snapshots for the STA and being able to
raise events after the project change had previously completed. 

We have found that this design has some very unique pros and cons, the
magnitude of some of them could not be appreciated until we'd implemented
it.

#### Advantages to Responsive Design:

1. The same code that performs project file changes is rehostable. It can 
   work equally well in VS as in Napa, or other hosts, regardless of their 
   requirements for specific eventing or threading models.
  - Project system extensions can be written that have no knowledge of or 
    complexity from the rules that have to be followed based on the host.
2. Additional events can be defined and raised appropriately with changes 
   only to a concentrated area of code rather than updating all locations 
   that happen to mutate the project in a way that should raise the event.
3. Arbitrary changes to the project file can be consumed, and the right 
   events get raised to update the IDE, which enables scenarios such as:
  - The user editing the project file in the text editor while it is still 
    open, and the project system responds to changes the user makes.
  - SCC operations such as Undo & Get Latest Version don't have to reload 
    the project when the project file is changed.
4. Provides opportunity for concurrency and asynchronous execution, leaving 
   the IDE responsive during background or long-running work.
5. Bulk changes can be made to the project file and only raise minimal 
   events for the net changes that were applied to the project file.
6. Unpredictable side effects of project file changes are automatically 
   accounted for, whereas traditional project systems will be unaware of 
   them until a project reload, or (worse) in the middle of the project's 
   lifetime resulting in misbehavior.
7. Project system clients can subscribe to project data and get exactly 
   the events and change descriptions that they want, in snapshot form so 
   they can have confidence they don't have to clone all the data into 
   their own proprietary copy of the project object model.

#### Disadvantages to Responsive Design:

1. Project snapshot diffs don't always capture important semantics of the 
   original changes. For example, a rename is significantly different from a 
   delete and an add in how the host expects to see events raised. Preserving 
   these semantics sometimes requires extra code on the mutation side to tuck 
   hints away for later discovery by the diffing system. We minimize this 
   burden by adding these hints to shared code so that folks can still mutate 
   the project simply, but the code is nevertheless in CPS, making the 
   codebase non-traditional and a bit more complex.
2. The project tree data structure itself has a particularly tough job when 
   accommodating project changes. The code in this area has very deep 
   conditional branching and it's a lot of dev and testing work to have 
   confidence that it covers the required scenarios. It's unlikely that, in 
   its current form anyway, it could ever really handle any arbitrary project 
   change, although it seems we can maintain a codebase that handles the 
   subset of project changes that the IDE in practice actually performs.
3. Integration with the VS running document table has been a particularly 
   buggy area historically.
4. Asynchronous, distributed responses to project changes are harder to 
   predict and follow while developing or debugging code than the 
   traditional imperative style where one method is responsible for 
   everything and doesn't return until it's done.
