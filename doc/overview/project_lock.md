The Project Lock
================

You're only required to take your own project lock if you're accessing
MSBuild objects directly. If you're going through a CPS service which
internally access MSBuild, you can assume that service is taking the
necessary locks.

You may want to take a project lock in your code even if you don't access
MSBuild directly in order to signify a larger transaction (or batch of
changes). For instance, if you're adding several items to the project,
you could take a project write lock and hold it while adding all items.
This will cause CPS to recognize the several items being added as a single
batch and CPS will not process the changes you make until you release the
lock.

### Overview

The project lock in CPS guards the MSBuild object model for thread-safe
access. MSBuild itself is not thread-safe, so it is vitally important
that you possess a CPS project lock whenever you access or have references
to MSBuild objects.

#### Concurrency / Isolation

The project lock is similar to the `ReaderWriterLockSlim` class, in that it
allows many concurrent readers and exclusive access for writers. Someone
holding a read lock is not allowed to acquire a write lock -- the read
lock must be released first. There is a special "upgradeable reader" lock
type that allows concurrent readers to execute, and also allows upgrading
to write lock, at which point it blocks until other readers release their
locks and then grants an exclusive write lock till you release it, at
which point it reverts to an upgradeable read lock again.

Nesting project locks is allowed, but remember that ordinary read locks
are not upgradeable.

#### Threading

`IProjectThreadingService.JoinableTaskFactory`

Project locks are issued asynchronously. If a project lock cannot be
immediately assigned to you, your async method will yield and will resume
when the lock has been issued.

Project locks are always on threadpool threads. If you're on the UI thread
when you ask for a project lock, your method will resume with the issued
lock on the threadpool.

Releasing a lock does not automatically restore you to your previous thread. 
To get back to the UI thread after releasing a project lock use 
`IProjectThreadingService.JoinableTaskFactory` instead of `ThreadHelper.JoinableTaskFactory`.

It is allowed to switch back to the UI thread while holding a project
lock, but this is for purposes of calling 3rd party code when absolutely
required and should be avoided when possible. Do not reference MSBuild
objects while on the UI thread. You may release your project lock from
any thread.

### How to obtain a project lock

See [Obtaining the MSBuild.Project from CPS](../automation/obtaining_the_MSBuild.Project_from_CPS.md)
for an example.

### DO's and DON'Ts

- DO always take a project lock when accessing MSBuild objects.
- DON'T ever retain a reference to an MSBuild object beyond the scope of 
  your project lock.
- DO always call `CheckoutAsync` on the lock object passing in the full 
  path of the project file you're going to change before making your change. 
  Feel free to call it frequently. It efficiently no-ops if you've called 
  it with the same path in the same project lock already.
- DON'T hold a project lock for a long period of time. Doing so may prevent 
  other concurrently executing code from obtaining a lock.
- DON'T access MSBuild objects while on the UI thread, even if you are 
  holding a project lock. Project locks aren't considered 'active' while 
  you're on the UI thread, even if you have not yet released it.
- DON'T use `ThreadHelper.JoinableTaskFactory` in your code once you start 
  interacting directly with the CPS project lock. Import the 
  `IProjectThreadingService` service (via MEF) and use the `IProjectThreadingService.JoinableTaskFactory` 
  instance of `JoinableTaskFactory` instead. This will mitigate deadlocks 
  that can result between someone holding the UI thread and wanting a 
  project lock, and you holding a project lock and wanting the UI thread.
- DON'T try to circumvent the asynchronous nature of the project lock. 
  Calling `ReadLockAsync().Wait()` or any other synchronously blocking 
  variant will cause your code to malfunction. If you must block the calling 
  thread while doing work with a project lock, you may use 
  `IProjectThreadingService.JoinableTaskFactory.Run(Func<Task>)` for that purpose. See **Block 
  a thread while doing async work** for more information on that, but 
  remember to use `IProjectThreadingService.JoinableTaskFactory` instead of 
  `ThreadHelper.JoinableTaskFactory`.
- DO be aware that when you have a project lock and request the UI thread, 
  that your lock may be lent out to someone controlling the UI thread and 
  wanting a project lock. This is the lesser of two evils, where the 
  other evil is a deadlock.
