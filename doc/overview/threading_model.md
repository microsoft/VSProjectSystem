Threading model
===============

CPS requires strict adherence to the rules and guidelines at [3 Threading
Rules](3_threading_rules.md)

Important: 

**Do not call Task.Wait() or Task.Result in your code because these
violate [threading rule #2](3_threading_rules.md) and will often lead
to deadlocks.**
   

Avoid use of `ThreadHelper.JoinableTaskFactory` as that does not have the intelligence
to mitigate deadlocks between the UI thread and the CPS project lock. Instead,
use a CPS-specific `JoinableTaskFactory` instance, such as the one found on
`IThreadHandling.AsyncPump`

Alternately if you want to create your own `JoinableTaskFactory` instance (for
example to associate with your own `JoinableTaskCollection`) you can call 
`IThreadHandling.JoinableTaskContext.CreateFactory(joinableTaskCollection)`.

