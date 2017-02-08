Threading model
===============

CPS requires strict adherence to the rules and guidelines at [Threading
Rules](threading_rules.md)

Important: 

**Do not call Task.Wait() or Task.Result in your code because these
violate [threading rule #2](https://github.com/Microsoft/vs-threading/blob/master/doc/threading_rules.md#2-when-an-implementation-of-an-already-shipped-public-api-must-call) and will often lead
to deadlocks.**

**Visual Studio "15":** `IProjectThreadingService.JoinableTaskFactory`

**Visual Studio 2015:** `IThreadHandling.AsyncPump`   

Avoid use of `ThreadHelper.JoinableTaskFactory` as that does not have the intelligence
to mitigate deadlocks between the UI thread and the CPS project lock. Instead,
use a CPS-specific `JoinableTaskFactory` instance, such as the one found on
`IProjectThreadingService.JoinableTaskFactory`/`IThreadHandling.AsyncPump`

Alternately if you want to create your own `JoinableTaskFactory` instance (for
example to associate with your own `JoinableTaskCollection`) you can call `IProjectThreadingService.JoinableTaskContext.CreateFactory(joinableTaskCollection)` / `IThreadHandling.JoinableTaskContext.CreateFactory(joinableTaskCollection)`.

