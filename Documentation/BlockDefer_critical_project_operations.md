Block/Defer critical project operations
=======================================

You can block critical project operations (Build, Save, Rename,
Unload) until specified tasks complete by registering your tasks via
IProjectAsynchronousTasksService

    RegisterAsycTask() with a ProjectCriticalOperation flag is provided
    to support this:
    
        void RegisterAsyncTask(JoinableTask joinableTask, ProjectCriticalOperation
        operationFlags, bool registerFaultHandler = false)
        

E.g. if you want to defer the build, you can register your tasks that you
want to wait on for build by passing in ProjectCriticalOperation.Build as
the flag.


For closing project, we provide a CancellationToken you can use to bail
out quickly instead of blocking project close for a long time.


