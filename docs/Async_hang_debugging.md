Async hang debugging
====================

This prints out ALL async methods that are on the heap, whether they
are completed or not. 
The way to ensure that you only see incompleted async methods is to be
sure to execute a GC prior to running this script.

This formatted loop is here for readability and maintenance.  But the
actual string to copy into windbg is the ugly one-liner below.

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


