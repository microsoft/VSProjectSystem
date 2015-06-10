Analyze hangs
====================

## Async hang debugging

An asynchronous hang is where an async method never completes its execution,
resulting in some thread (usually the UI thread) that must wait for that completion
to block forever, resulting in the user having to terminate the process.

Finding the cause for an asynchronous hang can be challenging because unlike a
classic synchronous deadlock, there often is not a callstack on any thread to
inspect to know exactly where execution was suspended and why. Instead, we look
in the heap for these async methods' state machines that the compiler generated
for clues. 

Here are some techniques that are useful. They are ordered based on their ease
of execution and their likelihood of producing useful results:

1. Check background threads for any that are blocked on an STA COM call that's waiting to marshal to the UI thread. 
2. If the hang shows a JoinableTaskFactory.WaitSynchronously frame near the top of the callstack on the UI thread:
   1. No background threads will be able to rely on a COM STA marshal to succeed. If this is what's happening, the fix is to [explicitly switch to the UI thread][SwitchToMainThreadAsync] before making the COM call. Look for background threads callstacks that are trying to marshal to the UI thread using COM RPC calls. This would be a violation of [rule #1][Rule1].
   2. there is probably a DGML file on the disk of the repro machine that contains a hang report that shows you what went wrong. That way you may be able to avoid any WinDBG heap scouring manual investigation. Look for directories with this pattern: "%temp%\CPS.*" where * is a random GUID.
   3. If you don't have access to the TEMP directory, you can also get the hang report from the dump file itself:
 ```
!dumpheap -stat -type CpsJoinableTaskContext
!dumpheap -mt <MTAddress from previous command>
!do <object address from previous command>
!do <address from mostRecentHangReport field>
```
3. If at all possible, run a full GC in the debuggee before going further. This will clear out all completed awaits from the heap so that in your investigation you know that everything you see represents incomplete work.
4. Be sure you're using a matching version of SOS for the clr.dll you have. For example: 

        .loadby sos clr
        # Then test this by typing something like 
        !dumpheap -stat
        The version of SOS does not match the version of CLR you are debugging.  Please
        load the matching version of SOS for the version of CLR you are debugging.
        CLR Version: 4.0.30319.32553
        SOS Version: 4.0.30319.18207
        # In this case we see the versions mismatch. So we load the right version of SOS explicitly:
        .load path\to\sos.dll

5. Using WinDBG either on a dump file or a live debugging session, use the technique described in the next section to dump all async state machines.
When dumping async state machines, the <>1__state field should be interpreted as:
   1. `-2`: the async method has run to completion. This state machine is likely unrooted and subject to garbage collection.
   2. `-1`: the async method is currently executing (you should find it on a callstack somewhere)
   3. `>= 0`: the 0-index into which "await" has most recently yielded. The list of awaits for the method are in *strict syntax appearance order*. That is, regardless of code execution, if branching, etc., it's the index into which await in code syntax order has yielded. For example, if you position the caret at the top of the method definition and search for "await ", and count how many times you hit a match, starting from 0, when you arrive at the number that you found in the state field, you've found the await that has most recently yielded.
**When the code being debugged is compiled with certain versions of the Roslyn compiler, this index is 1-based instead of 0-based**.

## Print all async methods in WinDBG

This prints out ALL async methods that are on the heap, whether they
are completed or not. The way to ensure that you only see incompleted 
async methods is to be sure to execute a GC prior to running this script.

This formatted loop is here for readability and maintenance.  But the
actual string to copy into WinDBG is the ugly one-liner below.

    .foreach /pS 7 /ps 1000 (mt {!name2ee
    mscorlib.dll!System.Runtime.CompilerServices.AsyncMethodBuilderCore+MoveNextRunner})
    {
        .foreach (movenext { !dumpheap -short -mt ${mt} })
        {
            .foreach /pS 1 (method { dd ${movenext}+8 l1 })
            {
                .foreach /pS 1 /ps 1000 (methodName { !do -nofields ${method} })
                {
                    .echo ${method} ${methodName}
                }
            }
        }
    }

This is the one-liner you have to actually use to keep WinDBG happy:

    .foreach /pS 7 /ps 1000 (mt {!name2ee mscorlib.dll!System.Runtime.CompilerServices.AsyncMethodBuilderCore+MoveNextRunner})  {  	.foreach (movenext { !dumpheap -short -mt ${mt} })  	{  		.foreach /pS 1 (method { dd ${movenext}+8 l1 })  		{  			.foreach /pS 1 /ps 1000 (methodName { !do -nofields ${method} })  			{  				.echo ${method} ${methodName}  			}  		}  	}  }  

Or, when SOS and CLR versions are mismatched:

    .foreach /pS 2a /ps 1000 (mt {!name2ee mscorlib.dll!System.Runtime.CompilerServices.AsyncMethodBuilderCore+MoveNextRunner})  {  	.foreach /pS 23 (movenext { !dumpheap -short -mt ${mt} })  	{  		.foreach /pS 1 (method { dd ${movenext}+8 l1 })  		{  			.foreach /pS 24 /ps 1000 (methodName { !do -nofields ${method} })  			{  				.echo ${method} ${methodName}  			}  		}  	}  }  

or for debugging a 64-bit process: (untested, but reportedly useful)

    .foreach /pS 7 /ps 10 (mt {!name2ee mscorlib.dll!System.Runtime.CompilerServices.AsyncMethodBuilderCore+MoveNextRunner}) {.foreach (moveNextRunner {!DumpHeap -MT ${mt} -short}) {!do poi(${moveNextRunner}+10)}}

Sample output from this command: (shows the addresses of the async methods
that give more information, and the name of the async method)

    0355a078 Microsoft.VisualStudio.ProjectSystem.Utilities.Designers.ProjectDataSources+<>c__DisplayClass1a+<<SyncLinkTo>b__15>d__1f
    0362d2ac Microsoft.VisualStudio.PlatformUI.RunOnIdleTaskScheduler+<WorkLoop>d__4
    03615550 Microsoft.VisualStudio.ProjectSystem.Utilities.Designers.ProjectDataSources+<>c__DisplayClass1a+<<SyncLinkTo>b__15>d__1f
    103052f0 Microsoft.VisualStudio.ProjectSystem.Designers.ProjectBuildSnapshotService+<UpdateSnapshotCoreAsync>d__7
    10305460 Microsoft.VisualStudio.ProjectSystem.Designers.CustomizableBlockSubscriberBase`3+<UpdateSnapshotAsync>d__9[[System.Tuple`2[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectVersionedValue`1[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectSnapshot,
    103055a0 Microsoft.VisualStudio.ProjectSystem.Designers.CustomizableBlockSubscriberBase`3+<<Initialize>b__2>d__4[[System.Tuple`2[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectVersionedValue`1[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectSnapshot,
    103056dc Microsoft.VisualStudio.ProjectSystem.Utilities.DataflowExtensions+<>c__DisplayClass1f`2+<<CreateSelfFilteringTransformBlock>b__1e>d__21[[System.Tuple`2[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectVersionedValue`1[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectSnapshot,
    0358e0a0 Microsoft.VisualStudio.ProjectSystem.Utilities.Designers.ProjectDataSources+<>c__DisplayClass1a+<<SyncLinkTo>b__15>d__1f
    10318024 Microsoft.VisualStudio.ProjectSystem.Utilities.Designers.ProjectDataSources+<>c__DisplayClass1a+<<SyncLinkTo>b__15>d__1f
    03526cec Microsoft.VisualStudio.ProjectSystem.Utilities.Designers.ProjectDataSources+<>c__DisplayClass1a+<<SyncLinkTo>b__15>d__1f
    1041993c Microsoft.VisualStudio.ProjectSystem.Utilities.Designers.ProjectDataSources+<>c__DisplayClass1a+<<SyncLinkTo>b__15>d__1f
    1080b6bc Microsoft.VisualStudio.ProjectSystem.Utilities.Designers.ProjectDataSources+<>c__DisplayClass1a+<<SyncLinkTo>b__15>d__1f
    107ec1ac Microsoft.VisualStudio.ProjectSystem.Utilities.Designers.ProjectDataSources+<>c__DisplayClass1a+<<SyncLinkTo>b__15>d__1f
    108e7c38 Microsoft.VisualStudio.ProjectSystem.Utilities.Designers.ProjectDataSources+<>c__DisplayClass1a+<<SyncLinkTo>b__15>d__1f
    1081f3ac Microsoft.VisualStudio.ProjectSystem.Utilities.DataflowExtensions+<>c__DisplayClass29`2+<<LinkToJoinLatest>b__26>d__2b[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectVersionedValue`1[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectSnapshot,
    036534f4 Microsoft.VisualStudio.ProjectSystem.Utilities.DataflowExtensions+<>c__DisplayClass29`2+<<LinkToJoinLatest>b__26>d__2b[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectVersionedValue`1[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectSnapshot,
    109093d0 Microsoft.VisualStudio.ProjectSystem.Utilities.DataflowExtensions+<>c__DisplayClass29`2+<<LinkToJoinLatest>b__26>d__2b[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectVersionedValue`1[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectSnapshot,
    109202a8 Microsoft.VisualStudio.ProjectSystem.Designers.ProjectBuildSnapshotService+<UpdateSnapshotCoreAsync>d__7
    10920418 Microsoft.VisualStudio.ProjectSystem.Designers.CustomizableBlockSubscriberBase`3+<UpdateSnapshotAsync>d__9[[System.Tuple`2[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectVersionedValue`1[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectSnapshot,
    10920558 Microsoft.VisualStudio.ProjectSystem.Designers.CustomizableBlockSubscriberBase`3+<<Initialize>b__2>d__4[[System.Tuple`2[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectVersionedValue`1[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectSnapshot,
    10920694 Microsoft.VisualStudio.ProjectSystem.Utilities.DataflowExtensions+<>c__DisplayClass1f`2+<<CreateSelfFilteringTransformBlock>b__1e>d__21[[System.Tuple`2[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectVersionedValue`1[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectSnapshot,
    0358b474 Microsoft.VisualStudio.ProjectSystem.Utilities.DataflowExtensions+<>c__DisplayClass29`2+<<LinkToJoinLatest>b__26>d__2b[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectVersionedValue`1[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectSnapshot,
    107e0d60 Microsoft.VisualStudio.ProjectSystem.Utilities.Designers.ProjectDataSources+<>c__DisplayClass1a+<<SyncLinkTo>b__15>d__1f
    0366195c Microsoft.VisualStudio.ProjectSystem.Utilities.Designers.ProjectDataSources+<>c__DisplayClass1a+<<SyncLinkTo>b__15>d__1f
    18465134 Microsoft.VisualStudio.ProjectSystem.VS.Implementation.Package.UnconfiguredProjectHostBridge`3+<ApplyAsync>d__4[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectVersionedValue`1[[System.Tuple`3[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectTreeSnapshot,
    18465290 Microsoft.VisualStudio.ProjectSystem.VS.Implementation.Package.TreeService+<PublishTreeAsync>d__d
    184653dc Microsoft.VisualStudio.ProjectSystem.VS.Implementation.Package.TreeService+<PublishLatestTreeAsync>d__11
    18465530 Microsoft.VisualStudio.ProjectSystem.VS.Implementation.Designers.LanguageServiceBase+<ProjectRuleBlock_ChangedAsync>d__1b
    10912f64 Microsoft.VisualStudio.ProjectSystem.Utilities.Designers.ProjectDataSources+<>c__DisplayClass1a+<<SyncLinkTo>b__15>d__1f
    03653428 Microsoft.VisualStudio.ProjectSystem.Utilities.Designers.ProjectDataSources+<>c__DisplayClass1a+<<SyncLinkTo>b__15>d__1f
    10914284 Microsoft.VisualStudio.ProjectSystem.Utilities.Designers.ProjectDataSources+<>c__DisplayClass1a+<<SyncLinkTo>b__15>d__1f
    188249c4 Microsoft.VisualStudio.ProjectSystem.VS.Implementation.Package.UnconfiguredProjectHostBridge`3+<ApplyAsync>d__4[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectVersionedValue`1[[System.Tuple`3[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectTreeSnapshot,
    18824b44 Microsoft.VisualStudio.ProjectSystem.VS.Implementation.Package.TreeService+<PublishTreeAsync>d__d
    18824cb4 Microsoft.VisualStudio.ProjectSystem.VS.Implementation.Package.TreeService+<PublishLatestTreeAsync>d__11
    18824e2c Microsoft.VisualStudio.ProjectSystem.VS.Implementation.Package.ProjectNode+<AddItemWithSpecificAsync>d__24f
    18825064 Microsoft.VisualStudio.ProjectSystem.VS.Implementation.Package.ProjectNode+<>c__DisplayClasscd+<<AddItem>b__cc>d__cf
    188251c8 Microsoft.VisualStudio.ProjectSystem.VS.Implementation.Package.ProjectNode+<>c__DisplayClass1b7+<<HrInvoke>b__1b5>d__1ba

 [Rule1]: https://github.com/Microsoft/VSProjectSystem/blob/master/doc/overview/3_threading_rules.md#1-if-a-method-has-certain-thread-apartment-requirements-sta-or-mta-it-must-either
 [SwitchToMainThreadAsync]: https://github.com/Microsoft/VSProjectSystem/blob/async-hang-debugging/doc/overview/cookbook.md#how-to-switch-to-the-ui-thread
 
