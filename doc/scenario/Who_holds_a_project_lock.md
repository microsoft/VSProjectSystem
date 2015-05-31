Who holds a project lock?
=========================

The `ProjectLockService.dgml` file tells me I have several clients waiting
on a lock, and one client holding an upgradeable read lock.

![](../Images/Fig_5.png)

I know the lock is involved in the hang because the async state machine
responsible for blocking the main thread is blocked because it can't get
a read lock.

Who is holding the upgradeable read lock? WinDBG can help us find the
answer.

    0:000> !dumpheap -stat -type ProjectLockService
    Statistics:
          MT    Count    TotalSize Class Name
    62d3963c        1           12
    System.Lazy`1+Boxed[[Microsoft.VisualStudio.ProjectSystem.IProjectLockServiceInternal,
    Microsoft.VisualStudio.ProjectSystem.V14Only]]
    62bf05c8        1           12 System.Lazy`1+Boxed[[Microsoft.VisualStudio.ProjectSystem.ProjectLockService,
    Microsoft.VisualStudio.ProjectSystem.Implementation]]
    62d370d8        1           20 System.Lazy`1[[Microsoft.VisualStudio.ProjectSystem.IProjectLockServiceInternal,
    Microsoft.VisualStudio.ProjectSystem.V14Only]]
    62be0268        1           20 System.Lazy`1[[Microsoft.VisualStudio.ProjectSystem.ProjectLockService,
    Microsoft.VisualStudio.ProjectSystem.Implementation]]
    62d379cc        1           32 System.Func`1[[Microsoft.VisualStudio.ProjectSystem.IProjectLockService,
    Microsoft.VisualStudio.ProjectSystem.V14Only]]
    62d3712c        1           32 System.Func`1[[Microsoft.VisualStudio.ProjectSystem.IProjectLockServiceInternal,
    Microsoft.VisualStudio.ProjectSystem.V14Only]]
    62be845c        1           32 System.Func`1[[Microsoft.VisualStudio.ProjectSystem.ProjectLockService,
    Microsoft.VisualStudio.ProjectSystem.Implementation]]
    62a05eac        1           32 Microsoft.VisualStudio.ProjectSystem.ProjectLockService+AccessLockEnforcement
    62d395bc        5           60 System.Lazy`1+Boxed[[Microsoft.VisualStudio.ProjectSystem.IProjectLockService,
    Microsoft.VisualStudio.ProjectSystem.V14Only]]
    62d37978        5          100 System.Lazy`1[[Microsoft.VisualStudio.ProjectSystem.IProjectLockService,
    Microsoft.VisualStudio.ProjectSystem.V14Only]]
    62a060c4        5          100 Microsoft.VisualStudio.ProjectSystem.ProjectLockService+<>c__DisplayClass4
    62a04af0        1          116 Microsoft.VisualStudio.ProjectSystem.ProjectLockService
    62a0611c        5          220
    Microsoft.VisualStudio.ProjectSystem.ProjectLockService+<>c__DisplayClass4+<<Microsoft_VisualStudio_ProjectSystem_IProjectLockReleaser_OnCompleted>b__6>d__0
    Total 29 objects
    Fragmented blocks larger than 0.5 MB:
        Addr     Size      Followed by
    18aaa760    0.8MB         18b80bf0 System.Byte[]
    18b80c48    0.6MB         18c1692c System.Byte[]
    18c1aacc    0.8MB         18cea584 System.Byte[]
    18cec5a0    0.9MB         18dd7084 System.Byte[]
    18dd9090    1.2MB         18f04ea4 System.Byte[]
    18f434bc    3.6MB         192d5ff4 System.Byte[]
    0:000> !dumpheap -mt 62a04af0        
     Address       MT     Size
    0bf21e20 62a04af0      116     
    Statistics:
          MT    Count    TotalSize Class Name
    62a04af0        1          116 Microsoft.VisualStudio.ProjectSystem.ProjectLockService
    Total 1 objects
    Fragmented blocks larger than 0.5 MB:
        Addr     Size      Followed by
    18aaa760    0.8MB         18b80bf0 System.Byte[]
    18b80c48    0.6MB         18c1692c System.Byte[]
    18c1aacc    0.8MB         18cea584 System.Byte[]
    18cec5a0    0.9MB         18dd7084 System.Byte[]
    18dd9090    1.2MB         18f04ea4 System.Byte[]
    18f434bc    3.6MB         192d5ff4 System.Byte[]
    0:000> !do 0bf21e20
    Name:        Microsoft.VisualStudio.ProjectSystem.ProjectLockService
    MethodTable: 62a04af0
    EEClass:     628c47e8
    Size:        116(0x74) bytes
    File:        C:\Windows\Microsoft.Net\assembly\GAC_MSIL\Microsoft.VisualStudio.ProjectSystem.Implementation\v4.0_14.0.0.0__b03f5f7f11d50a3a\Microsoft.VisualStudio.ProjectSystem.Implementation.dll
    Fields:
          MT    Field   Offset                 Type VT     Attr    Value Name
    7188e0c8  400001e        4        System.Object  0 instance 0bf22720
    syncObject
    6d51d05c  400001f        8 ...ronizationContext  0 instance 0bf2272c
    nonConcurrentSyncContext
    6d51d0cc  4000020        c ...tudio.Threading]]  0 instance 0bf22770
    topAwaiter
    6d4d288c  4000021       10 ...tudio.Threading]]  0 instance 0bf2283c
    issuedReadLocks
    6d4d288c  4000022       14 ...tudio.Threading]]  0 instance 0bf228a8
    issuedUpgradeableReadLocks
    6d4d288c  4000023       18 ...tudio.Threading]]  0 instance 0bf228d0
    issuedWriteLocks
    6d4d2928  4000024       1c ...tudio.Threading]]  0 instance 0bf228f8
    waitingReaders
    6d4d2928  4000025       20 ...tudio.Threading]]  0 instance 0bf22928
    waitingUpgradeableReaders
    6d4d2928  4000026       24 ...tudio.Threading]]  0 instance 0bf22948
    waitingWriters
    …
    0:000> !do 0bf228a8 
    Name:        System.Collections.Generic.HashSet`1[[Microsoft.VisualStudio.Threading.AsyncReaderWriterLock+Awaiter,
    Microsoft.VisualStudio.Threading]]
    MethodTable: 6d4d288c
    EEClass:     70497e24
    Size:        40(0x28) bytes
    File:        C:\Windows\Microsoft.Net\assembly\GAC_MSIL\System.Core\v4.0_4.0.0.0__b77a5c561934e089\System.Core.dll
    Fields:
          MT    Field   Offset                 Type VT     Attr    Value Name
    7188fb80  40006f6        4       System.Int32[]  0 instance 0c55b410
    m_buckets
    7094cb3c  40006f7        8 ...non, mscorlib]][]  0 instance 0c55b3b0
    m_slots
    7188fbb8  40006f8       14         System.Int32  1 instance        1
    m_count
    7188fbb8  40006f9       18         System.Int32  1 instance        1
    m_lastIndex
    7188fbb8  40006fa       1c         System.Int32  1 instance       -1
    m_freeList
    718c7cb0  40006fb        c ...Canon, mscorlib]]  0 instance 0bf2289c
    m_comparer
    7188fbb8  40006fc       20         System.Int32  1 instance     1455
    m_version
    71893580  40006fd       10 ...SerializationInfo  0 instance 00000000
    m_siInfo
    0:000> !DumpArray /d 0c55b3b0
    Name:        System.Collections.Generic.HashSet`1+Slot[[Microsoft.VisualStudio.Threading.AsyncReaderWriterLock+Awaiter,
    Microsoft.VisualStudio.Threading]][]
    MethodTable: 6d51e134
    EEClass:     6d4e26f8
    Size:        96(0x60) bytes
    Array:       Rank 1, Number of elements 7, Type VALUETYPE
    Element Methodtable: 6d51e0f4
    [0] 0c55b3b8
    [1] 0c55b3c4
    [2] 0c55b3d0
    [3] 0c55b3dc
    [4] 0c55b3e8
    [5] 0c55b3f4
    [6] 0c55b400
    I then enumerate the array looking for valid dictionary entries. Because
    each entry is a value-type, I have to use the appropriate syntax:
    0:000> !dumpvc 6d51e0f4    0c55b3b8
    Name:        System.Collections.Generic.HashSet`1+Slot[[Microsoft.VisualStudio.Threading.AsyncReaderWriterLock+Awaiter,
    Microsoft.VisualStudio.Threading]]
    MethodTable: 6d51e0f4
    EEClass:     70499ff4
    Size:        20(0x14) bytes
    File:        C:\Windows\Microsoft.Net\assembly\GAC_MSIL\System.Core\v4.0_4.0.0.0__b77a5c561934e089\System.Core.dll
    Fields:
          MT    Field   Offset                 Type VT     Attr    Value Name
    7188fbb8  4000707        4         System.Int32  1 instance 40482544
    hashCode
    71891178  4000708        0       System.__Canon  0 instance 11b8cfb0 value
    7188fbb8  4000709        8         System.Int32  1 instance       -1 next
    0:000> !do 11b8cfb0
    Name:        Microsoft.VisualStudio.Threading.AsyncReaderWriterLock+Awaiter
    MethodTable: 6d51d124
    EEClass:     6d4e22ec
    Size:        60(0x3c) bytes
    File:        C:\Windows\Microsoft.Net\assembly\GAC_MSIL\Microsoft.VisualStudio.Threading\v4.0_14.0.0.0__b03f5f7f11d50a3a\Microsoft.VisualStudio.Threading.dll
    Fields:
          MT    Field   Offset                 Type VT     Attr    Value Name
    6d51cc58  40000ab        4 ...cReaderWriterLock  0 instance 0bf21e20 lck
    6d51dfc8  40000ac       20         System.Int32  1 instance        1 kind
    6d51d124  40000ad        8 ...riterLock+Awaiter  0 instance 00000000
    nestingLock
    7189a0c8  40000ae       28 ...CancellationToken  1 instance 11b8cfd8
    cancellationToken
    718d0a5c  40000af       2c ...TokenRegistration  1 instance 11b8cfdc
    cancellationRegistration
    6d51e000  40000b0       24         System.Int32  1 instance        0
    options
    7188de70  40000b1        c     System.Exception  0 instance 00000000 fault
    7189a934  40000b2       10        System.Action  0 instance 00000000
    continuation
    718998d0  40000b3       14 ...eading.Tasks.Task  0 instance 00000000
    releaseAsyncTask
    71898968  40000b4       18 ...ostics.StackTrace  0 instance 00000000
    requestingStackTrace
    7188e0c8  40000b5       1c        System.Object  0 instance 00000000 data
    718c826c  40000aa       b0 ...bject, mscorlib]]  0   shared   static
    cancellationResponseAction
        >> Domain:Value  013e8c60:0bf3374c <<
    71897aa8  40000b6       b4 ...endOrPostCallback  0   shared   static
    CS$<>9__CachedAnonymousMethodDelegate1
        >> Domain:Value  013e8c60:0bf3376c <<
    So I've found my awaiter that represents the lock. I need to find out who
    is holding it. 
    0:000> !gcroot 11b8cfb0
    Thread 1594:
        0113e140 6d5068dc Microsoft.VisualStudio.Threading.JoinableTaskFactory.WaitSynchronouslyCore(System.Threading.Tasks.Task)
    [f:\dd\vsproject\cps\Microsoft.VisualStudio.Threading\Microsoft.VisualStudio.Threading\JoinableTaskFactory.cs
    @ 274]
            ebp+50: 0113e140
                ->  0bf352c4 Microsoft.VisualStudio.ProjectSystem.Utilities.ProjectLockAwareJoinableTaskFactory
                ->  0377c514 Microsoft.VisualStudio.Threading.JoinableTaskContext
                ->  0377c648 System.Collections.Generic.HashSet`1[[Microsoft.VisualStudio.Threading.JoinableTask,
    Microsoft.VisualStudio.Threading]]
                ->  0c2738cc System.Collections.Generic.HashSet`1+Slot[[Microsoft.VisualStudio.Threading.JoinableTask,
    Microsoft.VisualStudio.Threading]][]
                ->  0ce64e78 Microsoft.VisualStudio.Threading.JoinableTask
                ->  0c6b1f68 Microsoft.VisualStudio.ProjectSystem.Utilities.ProjectLockAwareJoinableTaskFactory
                ->  0bf21e20 Microsoft.VisualStudio.ProjectSystem.ProjectLockService
                ->  0bf2272c Microsoft.VisualStudio.Threading.AsyncReaderWriterLock+NonConcurrentSynchronizationContext
                ->  0c5a96d8 System.Threading.Thread
                ->  03772fb4 System.Runtime.Remoting.Contexts.Context
                ->  037712e4 System.AppDomain
                ->  1198eca0 System.AssemblyLoadEventHandler
                ->  1198ec80 System.Object[]
                ->  0ccaeee0 System.AssemblyLoadEventHandler
                ->  0ccaed08 Microsoft.VisualStudio.Design.Toolbox.AutoToolboxManagerService
               ...
                ->  0ce91bf8 Microsoft.VisualStudio.ProjectSystem.Designers.ProjectDirectoryTreeProvider
                ->  11bdebfc Microsoft.VisualStudio.ProjectSystem.Designers.ProjectDirectoryTreeProvider+MyConfiguredProjectExports
                ->  1142f3b4 Microsoft.VisualStudio.ProjectSystem.PropertyPages.PropertyPagesDataModelProvider
                ->  1142f3f8
    Microsoft.VisualStudio.ProjectSystem.Utilities.OrderPrecedenceImportCollection`2[[Microsoft.VisualStudio.ProjectSystem.Designers.Properties.IDynamicEnumValuesProvider,
    Microsoft.VisualStudio.ProjectSystem.V14Only],[Microsoft.VisualStudio.ProjectSystem.PropertyPages.IDynamicEnumCategoryMetadata,
    Microsoft.VisualStudio.ProjectSystem.Implementation]]
                ->  1142f428
    System.Collections.Generic.List`1[[System.Lazy`2[[Microsoft.VisualStudio.ProjectSystem.Designers.Properties.IDynamicEnumValuesProvider,
    Microsoft.VisualStudio.ProjectSystem.V14Only],[Microsoft.VisualStudio.ProjectSystem.PropertyPages.IDynamicEnumCategoryMetadata,
    Microsoft.VisualStudio.ProjectSystem.Implementation]],
    System.ComponentModel.Composition]]
                ->  11430410 System.Object[]
                ->  1142fc90 System.Lazy`2[[Microsoft.VisualStudio.ProjectSystem.Designers.Properties.IDynamicEnumValuesProvider,
    Microsoft.VisualStudio.ProjectSystem.V14Only],[Microsoft.VisualStudio.ProjectSystem.PropertyPages.IDynamicEnumCategoryMetadata,
    Microsoft.VisualStudio.ProjectSystem.Implementation]]
                ->  1142fcb4 System.Func`1[[Microsoft.VisualStudio.ProjectSystem.Designers.Properties.IDynamicEnumValuesProvider,
    Microsoft.VisualStudio.ProjectSystem.V14Only]]
                ->  1142fca8
    Microsoft.VisualStudio.Composition.DelegateServices+<>c__DisplayClass0`1[[Microsoft.VisualStudio.ProjectSystem.Designers.Properties.IDynamicEnumValuesProvider,
    Microsoft.VisualStudio.ProjectSystem.V14Only]]
                ->  1142fc60 System.Func`1[[System.Object, mscorlib]]
                ->  1142fc48 Microsoft.VisualStudio.Composition.RuntimeExportProviderFactory+RuntimeExportProvider+<>c__DisplayClass13
                ->  11428a68
    Microsoft.VisualStudio.Composition.RuntimeExportProviderFactory+RuntimeExportProvider+RuntimePartLifecycleTracker
                ->  11428aa8
    System.Collections.Generic.HashSet`1[[Microsoft.VisualStudio.Composition.ExportProvider+PartLifecycleTracker,
    Microsoft.VisualStudio.Composition]]
                ->  1142fa10
    System.Collections.Generic.HashSet`1+Slot[[Microsoft.VisualStudio.Composition.ExportProvider+PartLifecycleTracker,
    Microsoft.VisualStudio.Composition]][]
                ->  1142bc18
    Microsoft.VisualStudio.Composition.RuntimeExportProviderFactory+RuntimeExportProvider+RuntimePartLifecycleTracker
                ->  1142bc80 Microsoft.VisualStudio.ProjectSystem.Items.ProjectItemSchemaManager
                ->  11bc6f20
    System.ComponentModel.Composition.ExportLifetimeContext`1[[System.Threading.Tasks.Dataflow.BroadcastBlock`1[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectVersionedValue`1[[Microsoft.VisualStudio.ProjectSystem.Items.IProjectItemSchema,
    Microsoft.VisualStudio.ProjectSystem.V14Only]], Microsoft.VisualStudio.ProjectSystem.V14Only]],
    System.Threading.Tasks.Dataflow]]
                ->  11bc6f00 System.Action
                ->  11bc6100
    Microsoft.VisualStudio.ProjectSystem.Utilities.DataflowExtensions+<>c__DisplayClass12`2[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectVersionedValue`1[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectSnapshot,
    Microsoft.VisualStudio.ProjectSystem.V14Only]],
    Microsoft.VisualStudio.ProjectSystem.V14Only],[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectVersionedValue`1[[Microsoft.VisualStudio.ProjectSystem.Items.IProjectItemSchema,
    Microsoft.VisualStudio.ProjectSystem.V14Only]], Microsoft.VisualStudio.ProjectSystem.V14Only]]
                ->  11bc6ee8 System.Threading.Tasks.Dataflow.Internal.Disposables+Disposable`3[[System.Object,
    mscorlib],[System.Threading.Tasks.Dataflow.Internal.TargetRegistry`1[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectVersionedValue`1[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectSnapshot,
    Microsoft.VisualStudio.ProjectSystem.V14Only]], Microsoft.VisualStudio.ProjectSystem.V14Only]],
    System.Threading.Tasks.Dataflow],[System.Threading.Tasks.Dataflow.ITargetBlock`1[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectVersionedValue`1[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectSnapshot,
    Microsoft.VisualStudio.ProjectSystem.V14Only]], Microsoft.VisualStudio.ProjectSystem.V14Only]],
    System.Threading.Tasks.Dataflow]]
                ->  11bc5058 System.Threading.Tasks.TaskCompletionSource`1[[System.Threading.Tasks.Dataflow.Internal.VoidResult,
    System.Threading.Tasks.Dataflow]]
                ->  11bc5064 System.Threading.Tasks.Task`1[[System.Threading.Tasks.Dataflow.Internal.VoidResult,
    System.Threading.Tasks.Dataflow]]
                ->  11bc51dc System.Threading.Tasks.StandardTaskContinuation
                ->  11bc50dc System.Threading.Tasks.ContinuationTaskFromTask
                ->  11bc4fe4
    System.Threading.Tasks.Dataflow.BroadcastBlock`1[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectVersionedValue`1[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectSnapshot,
    Microsoft.VisualStudio.ProjectSystem.V14Only]], Microsoft.VisualStudio.ProjectSystem.V14Only]]
                ->  11bc4ff8
    System.Threading.Tasks.Dataflow.BroadcastBlock`1+BroadcastingSourceCore`1[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectVersionedValue`1[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectSnapshot,
    Microsoft.VisualStudio.ProjectSystem.V14Only]],
    Microsoft.VisualStudio.ProjectSystem.V14Only],[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectVersionedValue`1[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectSnapshot,
    Microsoft.VisualStudio.ProjectSystem.V14Only]], Microsoft.VisualStudio.ProjectSystem.V14Only]]
                ->  11bc5090
    System.Threading.Tasks.Dataflow.Internal.TargetRegistry`1[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectVersionedValue`1[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectSnapshot,
    Microsoft.VisualStudio.ProjectSystem.V14Only]], Microsoft.VisualStudio.ProjectSystem.V14Only]]
                ->  11bc50ac
    System.Collections.Generic.Dictionary`2[[System.Threading.Tasks.Dataflow.ITargetBlock`1[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectVersionedValue`1[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectSnapshot,
    Microsoft.VisualStudio.ProjectSystem.V14Only]], Microsoft.VisualStudio.ProjectSystem.V14Only]],
    System.Threading.Tasks.Dataflow],[System.Threading.Tasks.Dataflow.Internal.TargetRegistry`1+LinkedTargetInfo[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectVersionedValue`1[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectSnapshot,
    Microsoft.VisualStudio.ProjectSystem.V14Only]], Microsoft.VisualStudio.ProjectSystem.V14Only]],
    System.Threading.Tasks.Dataflow]]
                ->  11bdadd4
    System.Collections.Generic.Dictionary`2+Entry[[System.Threading.Tasks.Dataflow.ITargetBlock`1[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectVersionedValue`1[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectSnapshot,
    Microsoft.VisualStudio.ProjectSystem.V14Only]], Microsoft.VisualStudio.ProjectSystem.V14Only]],
    System.Threading.Tasks.Dataflow],[System.Threading.Tasks.Dataflow.Internal.TargetRegistry`1+LinkedTargetInfo[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectVersionedValue`1[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectSnapshot,
    Microsoft.VisualStudio.ProjectSystem.V14Only]], Microsoft.VisualStudio.ProjectSystem.V14Only]],
    System.Threading.Tasks.Dataflow]][]
                ->  11bc5290
    System.Threading.Tasks.Dataflow.TransformBlock`2[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectVersionedValue`1[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectSnapshot,
    Microsoft.VisualStudio.ProjectSystem.V14Only]],
    Microsoft.VisualStudio.ProjectSystem.V14Only],[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectSnapshot,
    Microsoft.VisualStudio.ProjectSystem.V14Only]]
                ->  11bc56c8
    System.Threading.Tasks.Dataflow.Internal.TargetCore`1[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectVersionedValue`1[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectSnapshot,
    Microsoft.VisualStudio.ProjectSystem.V14Only]], Microsoft.VisualStudio.ProjectSystem.V14Only]]
                ->  11bc5800 System.Threading.Tasks.TaskCompletionSource`1[[System.Threading.Tasks.Dataflow.Internal.VoidResult,
    System.Threading.Tasks.Dataflow]]
                ->  11bc580c System.Threading.Tasks.Task`1[[System.Threading.Tasks.Dataflow.Internal.VoidResult,
    System.Threading.Tasks.Dataflow]]
                ->  11bc5c68 System.Threading.Tasks.StandardTaskContinuation
                ->  11bc5b68 System.Threading.Tasks.ContinuationTaskFromTask
                ->  11bc52b8
    System.Threading.Tasks.Dataflow.Internal.SourceCore`1[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectSnapshot,
    Microsoft.VisualStudio.ProjectSystem.V14Only]]
                ->  11bc565c
    System.Threading.Tasks.Dataflow.Internal.TargetRegistry`1[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectSnapshot,
    Microsoft.VisualStudio.ProjectSystem.V14Only]]
                ->  11bc5678
    System.Collections.Generic.Dictionary`2[[System.Threading.Tasks.Dataflow.ITargetBlock`1[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectSnapshot,
    Microsoft.VisualStudio.ProjectSystem.V14Only]],
    System.Threading.Tasks.Dataflow],[System.Threading.Tasks.Dataflow.Internal.TargetRegistry`1+LinkedTargetInfo[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectSnapshot,
    Microsoft.VisualStudio.ProjectSystem.V14Only]], System.Threading.Tasks.Dataflow]]
                ->  11bc5fec
    System.Collections.Generic.Dictionary`2+Entry[[System.Threading.Tasks.Dataflow.ITargetBlock`1[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectSnapshot,
    Microsoft.VisualStudio.ProjectSystem.V14Only]],
    System.Threading.Tasks.Dataflow],[System.Threading.Tasks.Dataflow.Internal.TargetRegistry`1+LinkedTargetInfo[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectSnapshot,
    Microsoft.VisualStudio.ProjectSystem.V14Only]], System.Threading.Tasks.Dataflow]][]
                ->  11bc5dac
    System.Threading.Tasks.Dataflow.BroadcastBlock`1[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectSnapshot,
    Microsoft.VisualStudio.ProjectSystem.V14Only]]
                ->  11bc5dc0
    System.Threading.Tasks.Dataflow.BroadcastBlock`1+BroadcastingSourceCore`1[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectSnapshot,
    Microsoft.VisualStudio.ProjectSystem.V14Only],[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectSnapshot,
    Microsoft.VisualStudio.ProjectSystem.V14Only]]
                ->  11bc5e58
    System.Threading.Tasks.Dataflow.Internal.TargetRegistry`1[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectSnapshot,
    Microsoft.VisualStudio.ProjectSystem.V14Only]]
                ->  11bc5e74
    System.Collections.Generic.Dictionary`2[[System.Threading.Tasks.Dataflow.ITargetBlock`1[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectSnapshot,
    Microsoft.VisualStudio.ProjectSystem.V14Only]],
    System.Threading.Tasks.Dataflow],[System.Threading.Tasks.Dataflow.Internal.TargetRegistry`1+LinkedTargetInfo[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectSnapshot,
    Microsoft.VisualStudio.ProjectSystem.V14Only]], System.Threading.Tasks.Dataflow]]
                ->  11c0e6c8
    System.Collections.Generic.Dictionary`2+Entry[[System.Threading.Tasks.Dataflow.ITargetBlock`1[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectSnapshot,
    Microsoft.VisualStudio.ProjectSystem.V14Only]],
    System.Threading.Tasks.Dataflow],[System.Threading.Tasks.Dataflow.Internal.TargetRegistry`1+LinkedTargetInfo[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectSnapshot,
    Microsoft.VisualStudio.ProjectSystem.V14Only]], System.Threading.Tasks.Dataflow]][]
                ->  11c0e194
    System.Threading.Tasks.Dataflow.ActionBlock`1[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectSnapshot,
    Microsoft.VisualStudio.ProjectSystem.V14Only]]
                ->  11c0e1d4
    System.Threading.Tasks.Dataflow.Internal.TargetCore`1[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectSnapshot,
    Microsoft.VisualStudio.ProjectSystem.V14Only]]
                ->  11c0e1b4
    System.Action`1[[System.Collections.Generic.KeyValuePair`2[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectSnapshot,
    Microsoft.VisualStudio.ProjectSystem.V14Only],[System.Int64, mscorlib]],
    mscorlib]]
                ->  11c0e1a4
    System.Threading.Tasks.Dataflow.ActionBlock`1+<>c__DisplayClass5[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectSnapshot,
    Microsoft.VisualStudio.ProjectSystem.V14Only]]
                ->  11c0e174 System.Action`1[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectSnapshot,
    Microsoft.VisualStudio.ProjectSystem.V14Only]]
                ->  11c0e0d4
    Microsoft.VisualStudio.ProjectSystem.Utilities.Designers.ProjectDataSources+<>c__DisplayClass2`1[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectSnapshot,
    Microsoft.VisualStudio.ProjectSystem.V14Only]]
                ->  11c0e704 System.Threading.Tasks.Dataflow.Internal.Disposables+Disposable`3[[System.Object,
    mscorlib],[System.Threading.Tasks.Dataflow.Internal.TargetRegistry`1[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectSnapshot,
    Microsoft.VisualStudio.ProjectSystem.V14Only]],
    System.Threading.Tasks.Dataflow],[System.Threading.Tasks.Dataflow.ITargetBlock`1[[Microsoft.VisualStudio.ProjectSystem.Designers.IProjectSnapshot,
    Microsoft.VisualStudio.ProjectSystem.V14Only]], System.Threading.Tasks.Dataflow]]
                ->  11bc5e20 System.Threading.Tasks.TaskCompletionSource`1[[System.Threading.Tasks.Dataflow.Internal.VoidResult,
    System.Threading.Tasks.Dataflow]]
                ->  11bc5e2c System.Threading.Tasks.Task`1[[System.Threading.Tasks.Dataflow.Internal.VoidResult,
    System.Threading.Tasks.Dataflow]]
                ->  11c0e8d4 System.Collections.Generic.List`1[[System.Object,
    mscorlib]]
                ->  11c0e8ec System.Object[]
                ->  11c0e81c System.Threading.Tasks.StandardTaskContinuation
                ->  11c0e71c System.Threading.Tasks.ContinuationTaskFromTask
                ->  11c0e7f4 System.Threading.Tasks.Task+ContingentProperties
                ->  11c0e7d0 System.Threading.ExecutionContext
                ->  11c0e748 System.Runtime.Remoting.Messaging.LogicalCallContext
                ->  11c0e76c System.Collections.Hashtable
                ->  11c0e7a0 System.Collections.Hashtable+bucket[]
                ->  11be94a0
    Microsoft.VisualStudio.Threading.AsyncLocal`1+IdentityNode[[Microsoft.VisualStudio.Threading.AsyncReaderWriterLock+Awaiter,
    Microsoft.VisualStudio.Threading]]
                ->  11b8cfb0 Microsoft.VisualStudio.Threading.AsyncReaderWriterLock+Awaiter
    (dependent handle)

    Thread 1344:
        10ebea40 62996a0a Microsoft.VisualStudio.ProjectSystem.ConfiguredProjectCache`1+<GetValueSlowAsync>d__1[[System.__Canon,
    mscorlib]].MoveNext() [f:\dd\vsproject\cps\components\implementations\ConfiguredProjectCache.cs
    @ 220]
            ebp+a8: 10ebea48 (interior)
                ->  11b8d024
    Microsoft.VisualStudio.ProjectSystem.ConfiguredProjectCache`1+<GetValueSlowAsync>d__1[[System.ComponentModel.ICustomTypeDescriptor,
    System]]
                ->  11b8cfb0 Microsoft.VisualStudio.Threading.AsyncReaderWriterLock+Awaiter

        10ebeb00 7184a467 System.Threading.ExecutionContext.RunInternal(System.Threading.ExecutionContext,
    System.Threading.ContextCallback, System.Object, Boolean)
    [f:\dd\NDP\clr\src\BCL\System\Threading\ExecutionContext.cs @ 825]
            ebp+44: 10ebeb18
                ->  11c0dcbc System.Threading.ExecutionContext
                ->  11c0dce0 System.Runtime.Remoting.Messaging.LogicalCallContext
                ->  11c0dd04 System.Collections.Hashtable
                ->  11c0dd38 System.Collections.Hashtable+bucket[]
                ->  11be94e8
    Microsoft.VisualStudio.Threading.AsyncLocal`1+IdentityNode[[Microsoft.VisualStudio.Threading.AsyncReaderWriterLock+Awaiter,
    Microsoft.VisualStudio.Threading]]
                ->  11be94ac Microsoft.VisualStudio.Threading.AsyncReaderWriterLock+Awaiter
    (dependent handle)
                ->  11b8cfb0 Microsoft.VisualStudio.Threading.AsyncReaderWriterLock+Awaiter

    Found 3 unique roots (run '!GCRoot -all' to see all roots).

    The gcroot #1 is the one pointed to by an ExecutionContext, so that is
    where we can look to find the async state machine that is doing work that
    is holding the upgradeable read project lock. 
    Gcroot #3 isn't interesting because it's redundant with #2 (rooted by the
    same thread).
    What's also interesting here is gcroot #2, in that it's a very short chain
    and tells me there is actually a background thread that holds some relevant
    code. That suggests this might be a simple deadlock due to a violation of
    threading rule #1. Using the "Processes and Threads" window, I translate
    thread 1344 to thread #016 and I switch to it to print the callstack. 

    0:000> ~16s
    eax=00000001 ebx=00000001 ecx=00000000 edx=00000000 esi=00000001 edi=00000001
    eip=7763aaac esp=10ebe1b8 ebp=10ebe338 iopl=0         nv up ei pl nz na
    pe nc
    cs=0023  ss=002b  ds=002b  es=002b  fs=0053  gs=002b             efl=00000206
    ntdll!NtWaitForMultipleObjects+0xc:
    7763aaac c21400          ret     14h
    0:016> k
     # ChildEBP RetAddr  
    00 10ebe1b4 75660927 ntdll!NtWaitForMultipleObjects+0xc
    [e:\9151.obj.x86fre\minkernel\ntdll\wow6432\objfre\i386\usrstubs.asm @ 825]
    01 10ebe338 748460c0 KERNELBASE!WaitForMultipleObjectsEx+0xcc
    [d:\9151\minkernel\kernelbase\synch.c @ 1471]
    02 10ebe388 74845f53 clr!WaitForMultipleObjectsEx_SO_TOLERANT+0x3c
    [f:\dd\ndp\clr\src\vm\threads.cpp @ 4892]
    03 (Inline) -------- clr!Thread::DoAppropriateAptStateWait+0x35
    [f:\dd\ndp\clr\src\vm\threads.cpp @ 4926]
    04 10ebe414 74846044 clr!Thread::DoAppropriateWaitWorker+0x237
    [f:\dd\ndp\clr\src\vm\threads.cpp @ 5066]
    05 10ebe480 74846293 clr!Thread::DoAppropriateWait+0x64
    [f:\dd\ndp\clr\src\vm\threads.cpp @ 4733]
    06 10ebe4cc 74832918 clr!CLREventBase::WaitEx+0x128 [f:\dd\ndp\clr\src\vm\synch.cpp
    @ 754]
    07 10ebe4e4 748567a7 clr!CLREventBase::Wait+0x1a [f:\dd\ndp\clr\src\vm\synch.cpp
    @ 675]
    08 (Inline) -------- clr!Thread::Wait+0x17 [f:\dd\ndp\clr\src\vm\threads.cpp
    @ 5551]
    09 10ebe4f8 748568e5 clr!Thread::Block+0x25 [f:\dd\ndp\clr\src\vm\threads.cpp
    @ 5508]
    0a 10ebe5a8 748569c6 clr!SyncBlock::Wait+0x18d [f:\dd\ndp\clr\src\vm\syncblk.cpp
    @ 3608]
    0b (Inline) -------- clr!ObjHeader::Wait+0x24 [f:\dd\ndp\clr\src\vm\syncblk.cpp
    @ 2787]
    0c (Inline) -------- clr!Object::Wait+0x24 [f:\dd\ndp\clr\src\vm\object.h
    @ 532]
    0d 10ebe644 717cbbe3 clr!ObjectNative::WaitTimeout+0xcb
    [f:\dd\ndp\clr\src\classlibnative\bcltype\objectnative.cpp @ 318]
    0e 10ebe654 7180c9e0 mscorlib_ni!System.Threading.Monitor.Wait(System.Object,
    Int32, Boolean)+0x17 [f:\dd\NDP\clr\src\BCL\System\Threading\Monitor.cs
    @ 215]
    0f 10ebe6ac 7182ac32 mscorlib_ni!System.Threading.Monitor.Wait(System.Object,
    Int32)+0xc [f:\dd\NDP\clr\src\BCL\System\Threading\Monitor.cs @ 225]
    10 10ebe6ac 718233a7 mscorlib_ni!System.Threading.ManualResetEventSlim.Wait(Int32,
    System.Threading.CancellationToken)+0x182
    [f:\dd\NDP\clr\src\BCL\System\Threading\ManualResetEventSlim.cs @ 657]
    11 10ebe6ec 71865870 mscorlib_ni!System.Threading.Tasks.Task.SpinThenBlockingWait(Int32,
    System.Threading.CancellationToken)+0x93 [f:\dd\NDP\clr\src\BCL\System\Threading\Tasks\Task.cs
    @ 3341]
    12 10ebe744 71f60792 mscorlib_ni!System.Threading.Tasks.Task.InternalWait(Int32,
    System.Threading.CancellationToken)+0x148 [f:\dd\NDP\clr\src\BCL\System\Threading\Tasks\Task.cs
    @ 3282]
    13 10ebe754 629d9c1e
    mscorlib_ni!System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(System.Threading.Tasks.Task)+0x1e
    [f:\dd\NDP\clr\src\BCL\System\Runtime\CompilerServices\TaskAwaiter.cs @
    165]
    14 10ebe768 62f57322
    Microsoft_VisualStudio_ProjectSystem_Implementation_ni!Microsoft.VisualStudio.ProjectSystem.Designers.ProjectSnapshotService.get_ProjectSnapshot()+0x66
    [f:\dd\vsproject\cps\components\implementations\Designers\ProjectSnapshotService.cs
    @ 111]
    15 10ebe7d0 62f571f0
    Microsoft_VisualStudio_ProjectSystem_VS_Implementation_ni!Microsoft.VisualStudio.ProjectSystem.VS.Implementation.Package.SimpleItemNode+<GetIsMissingItemAsync>d__1.MoveNext()+0x102
    [f:\dd\vsproject\cps\components\VsComponents\Package\Nodes\SimpleItemNode.cs
    @ 178]
    16 10ebe824 62f571a6 mscorlib_ni!System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1[[System.__Canon,
    mscorlib]].Start[[System.__Canon, mscorlib]](System.__Canon ByRef)+0xf0f605f8
    [f:\dd\NDP\clr\src\BCL\System\Runtime\CompilerServices\AsyncMethodBuilder.cs
    @ 459]
    17 10ebe874 62f3242f
    Microsoft_VisualStudio_ProjectSystem_VS_Implementation_ni!Microsoft.VisualStudio.ProjectSystem.VS.Implementation.Package.SimpleItemNode.GetIsMissingItemAsync()+0x3e
    18 10ebe8ac 62f3239b
    Microsoft_VisualStudio_ProjectSystem_VS_Implementation_ni!Microsoft.VisualStudio.ProjectSystem.VS.Implementation.Package.SimpleHierarchyNode+<CreatePropertiesAsync>d__1.MoveNext()+0x5f
    [f:\dd\vsproject\cps\components\VsComponents\Package\Nodes\SimpleHierarchyNode.cs
    @ 66]
    19 10ebe900 62f32351 mscorlib_ni!System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1[[System.__Canon,
    mscorlib]].Start[[System.__Canon, mscorlib]](System.__Canon ByRef)+0xf0f3b7a3
    [f:\dd\NDP\clr\src\BCL\System\Runtime\CompilerServices\AsyncMethodBuilder.cs
    @ 459]
    1a 10ebe940 0d959b6d
    Microsoft_VisualStudio_ProjectSystem_VS_Implementation_ni!Microsoft.VisualStudio.ProjectSystem.VS.Implementation.Package.SimpleHierarchyNode.CreatePropertiesAsync()+0x49
    1b 10ebe97c 7182b89d
    Microsoft_VisualStudio_ProjectSystem_Implementation_ni!Microsoft.VisualStudio.ProjectSystem.ConfiguredProjectCache`1+<>c__DisplayClass6+<<UpdateValue>b__8>d__0[[System.__Canon,
    mscorlib]].MoveNext()+0xdd [f:\dd\vsproject\cps\components\implementations\ConfiguredProjectCache.cs
    @ 296]
    1c 10ebe984 7184a467 mscorlib_ni!System.Runtime.CompilerServices.AsyncMethodBuilderCore+MoveNextRunner.InvokeMoveNext(System.Object)+0x19
    [f:\dd\NDP\clr\src\BCL\System\Runtime\CompilerServices\AsyncMethodBuilder.cs
    @ 1090]
    1d 10ebe9e8 7184a3b6 mscorlib_ni!System.Threading.ExecutionContext.RunInternal(System.Threading.ExecutionContext,
    System.Threading.ContextCallback, System.Object, Boolean)+0xa7
    [f:\dd\NDP\clr\src\BCL\System\Threading\ExecutionContext.cs @ 825]
    1e 10ebe9fc 7182b803 mscorlib_ni!System.Threading.ExecutionContext.Run(System.Threading.ExecutionContext,
    System.Threading.ContextCallback, System.Object, Boolean)+0x16
    [f:\dd\NDP\clr\src\BCL\System\Threading\ExecutionContext.cs @ 775]
    1f 10ebea34 0d959e4e mscorlib_ni!System.Runtime.CompilerServices.AsyncMethodBuilderCore+MoveNextRunner.Run()+0x5b
    [f:\dd\NDP\clr\src\BCL\System\Runtime\CompilerServices\AsyncMethodBuilder.cs
    @ 1070]
    20 10ebeaf0 62996a0a
    Microsoft_VisualStudio_ProjectSystem_Implementation_ni!Microsoft.VisualStudio.ProjectSystem.ConfiguredProjectCache`1+Awaiter[[System.__Canon,
    mscorlib]].Complete()+0x16 [f:\dd\vsproject\cps\components\implementations\ConfiguredProjectCache.cs
    @ 390]
    21 10ebeaf0 7182b89d
    Microsoft_VisualStudio_ProjectSystem_Implementation_ni!Microsoft.VisualStudio.ProjectSystem.ConfiguredProjectCache`1+<GetValueSlowAsync>d__1[[System.__Canon,
    mscorlib]].MoveNext()+0x612 [f:\dd\vsproject\cps\components\implementations\ConfiguredProjectCache.cs
    @ 220]
    22 10ebeaf8 7184a467 mscorlib_ni!System.Runtime.CompilerServices.AsyncMethodBuilderCore+MoveNextRunner.InvokeMoveNext(System.Object)+0x19
    [f:\dd\NDP\clr\src\BCL\System\Runtime\CompilerServices\AsyncMethodBuilder.cs
    @ 1090]
    23 10ebeb5c 7184a3b6 mscorlib_ni!System.Threading.ExecutionContext.RunInternal(System.Threading.ExecutionContext,
    System.Threading.ContextCallback, System.Object, Boolean)+0xa7
    [f:\dd\NDP\clr\src\BCL\System\Threading\ExecutionContext.cs @ 825]
    24 10ebeb70 7182b803 mscorlib_ni!System.Threading.ExecutionContext.Run(System.Threading.ExecutionContext,
    System.Threading.ContextCallback, System.Object, Boolean)+0x16
    [f:\dd\NDP\clr\src\BCL\System\Threading\ExecutionContext.cs @ 775]
    25 10ebeba8 7182b7a7 mscorlib_ni!System.Runtime.CompilerServices.AsyncMethodBuilderCore+MoveNextRunner.Run()+0x5b
    [f:\dd\NDP\clr\src\BCL\System\Runtime\CompilerServices\AsyncMethodBuilder.cs
    @ 1070]
    26 10ebebfc 6d50f6e1 mscorlib_ni!System.Threading.Tasks.SynchronizationContextAwaitTaskContinuation.<.cctor>b__6(System.Object)+0x2b
    [f:\dd\NDP\clr\src\BCL\System\Threading\Tasks\TaskContinuation.cs @ 491]
    27 10ebebfc 6d50f5cc
    Microsoft_VisualStudio_Threading_ni!Microsoft.VisualStudio.Threading.AsyncReaderWriterLock+NonConcurrentSynchronizationContext+<PostHelper>d__1.MoveNext()+0xe1
    [f:\dd\vsproject\cps\Microsoft.VisualStudio.Threading\Microsoft.VisualStudio.Threading\AsyncReaderWriterLock.cs
    @ 1874]
    28 10ebec50 6d50f588 mscorlib_ni!System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start[[System.__Canon,
    mscorlib]](System.__Canon ByRef)+0xfb518dfc
    [f:\dd\NDP\clr\src\BCL\System\Runtime\CompilerServices\AsyncMethodBuilder.cs
    @ 72]
    29 10ebec9c 6d50f533
    Microsoft_VisualStudio_Threading_ni!Microsoft.VisualStudio.Threading.AsyncReaderWriterLock+NonConcurrentSynchronizationContext.PostHelper(System.Threading.SendOrPostCallback,
    System.Object)+0x48
    2a 10ebed14 717ef226
    Microsoft_VisualStudio_Threading_ni!Microsoft.VisualStudio.Threading.AsyncReaderWriterLock+NonConcurrentSynchronizationContext.<Post>b__0(System.Object)+0x33
    [f:\dd\vsproject\cps\Microsoft.VisualStudio.Threading\Microsoft.VisualStudio.Threading\AsyncReaderWriterLock.cs
    @ 1832]
    2b 10ebed14 7184a467 mscorlib_ni!System.Threading.QueueUserWorkItemCallback.WaitCallback_Context(System.Object)+0x42
    [f:\dd\NDP\clr\src\BCL\System\Threading\ThreadPool.cs @ 1274]
    2c 10ebed14 7184a3b6 mscorlib_ni!System.Threading.ExecutionContext.RunInternal(System.Threading.ExecutionContext,
    System.Threading.ContextCallback, System.Object, Boolean)+0xa7
    [f:\dd\NDP\clr\src\BCL\System\Threading\ExecutionContext.cs @ 825]
    2d 10ebed28 71810470 mscorlib_ni!System.Threading.ExecutionContext.Run(System.Threading.ExecutionContext,
    System.Threading.ContextCallback, System.Object, Boolean)+0x16
    [f:\dd\NDP\clr\src\BCL\System\Threading\ExecutionContext.cs @ 775]
    2e 10ebed44 7180fc6b mscorlib_ni!System.Threading.QueueUserWorkItemCallback.System.Threading.IThreadPoolWorkItem.ExecuteWorkItem()+0x60
    [f:\dd\NDP\clr\src\BCL\System\Threading\ThreadPool.cs @ 1252]
    2f 10ebed94 7180fb1a mscorlib_ni!System.Threading.ThreadPoolWorkQueue.Dispatch()+0x14b
    [f:\dd\NDP\clr\src\BCL\System\Threading\ThreadPool.cs @ 820]
    30 10ebeda4 748313b6 mscorlib_ni!System.Threading._ThreadPoolWaitCallback.PerformWaitCallback()+0xa
    [f:\dd\NDP\clr\src\BCL\System\Threading\ThreadPool.cs @ 1161]
    31 10ebeda4 74846970 clr!CallDescrWorkerInternal+0x34
    [f:\dd\ndp\clr\src\vm\i386\asmhelpers.asm @ 732]
    32 10ebedf8 74847f65 clr!CallDescrWorkerWithHandler+0x6b
    [f:\dd\ndp\clr\src\vm\callhelpers.cpp @ 91]
    33 10ebee6c 7491b094 clr!MethodDescCallSite::CallTargetWorker+0x152
    [f:\dd\ndp\clr\src\vm\callhelpers.cpp @ 649]
    34 (Inline) -------- clr!MethodDescCallSite::Call_RetBool+0xb
    [f:\dd\ndp\clr\src\vm\callhelpers.h @ 423]
    35 10ebeeec 74847fb5 clr!QueueUserWorkItemManagedCallback+0x23
    [f:\dd\ndp\clr\src\vm\comthreadpool.cpp @ 513]
    36 10ebef04 74848023 clr!ManagedThreadBase_DispatchInner+0x67
    [f:\dd\ndp\clr\src\vm\threads.cpp @ 18093]
    37 10ebefa8 748480f0 clr!ManagedThreadBase_DispatchMiddle+0x82
    [f:\dd\ndp\clr\src\vm\threads.cpp @ 18143]
    38 10ebf004 7484815f clr!ManagedThreadBase_DispatchOuter+0x5b
    [f:\dd\ndp\clr\src\vm\threads.cpp @ 18397]
    39 10ebf028 7491b028 clr!ManagedThreadBase_FullTransitionWithAD+0x2f
    [f:\dd\ndp\clr\src\vm\threads.cpp @ 18461]
    3a (Inline) -------- clr!ManagedThreadBase::ThreadPool+0x10
    [f:\dd\ndp\clr\src\vm\threads.cpp @ 18502]
    3b 10ebf0d8 7491ce3f clr!ManagedPerAppDomainTPCount::DispatchWorkItem+0xc4
    [f:\dd\ndp\clr\src\vm\threadpoolrequest.cpp @ 760]
    3c 10ebf0ec 7491cbd2 clr!ThreadpoolMgr::ExecuteWorkRequest+0x42
    [f:\dd\ndp\clr\src\vm\win32threadpool.cpp @ 1863]
    3d 10ebf154 74846b2c clr!ThreadpoolMgr::WorkerThreadStart+0x36b
    [f:\dd\ndp\clr\src\vm\win32threadpool.cpp @ 2330]
    3e 10ebfaf4 757f919f clr!Thread::intermediateThreadProc+0x4d
    [f:\dd\ndp\clr\src\vm\threads.cpp @ 3478]
    3f 10ebfb00 77650bbb kernel32!BaseThreadInitThunk+0xe
    [d:\blue_gdr\base\win32\client\thread.c @ 72]
    40 10ebfb44 77650b91 ntdll!__RtlUserThreadStart+0x20 [d:\9151\minkernel\ntdll\rtlstrt.c
    @ 1024]
    41 10ebfb54 00000000 ntdll!_RtlUserThreadStart+0x1b [d:\9151\minkernel\ntdll\rtlstrt.c
    @ 939]

    Sure enough, we have a call that synchronously blocking piece of code in
    ProjectSnapshotService.ProjectSnapshot that isn't using the JoinableTaskFactory.
    This is a really bad thing and can lead to deadlocks (perhaps this very
    one!). We should fix this.

    Let's move on to find who actually acquired the project upgradeable
    read lock though. From GCRoot#2, which is conveniently short, we see
    ConfiguredProjectCache.GetValueSlowAsync is holding it. We learn by code
    inspection that this method may take either an upgradeable read or a read
    lock. Let's inspect the fields of this object to determine which one it
    would have taken to verify. Copied from above, the gcroot is:

        10ebea40 62996a0a Microsoft.VisualStudio.ProjectSystem.ConfiguredProjectCache`1+<GetValueSlowAsync>d__1[[System.__Canon,
    mscorlib]].MoveNext() [f:\dd\vsproject\cps\components\implementations\ConfiguredProjectCache.cs
    @ 220]
            ebp+a8: 10ebea48 (interior)
                ->  11b8d024
    Microsoft.VisualStudio.ProjectSystem.ConfiguredProjectCache`1+<GetValueSlowAsync>d__1[[System.ComponentModel.ICustomTypeDescriptor,
    System]]
                ->  11b8cfb0 Microsoft.VisualStudio.Threading.AsyncReaderWriterLock+Awaiter

    0:016> !do 11b8d024 
    Name:        Microsoft.VisualStudio.ProjectSystem.ConfiguredProjectCache`1+<GetValueSlowAsync>d__1[[System.ComponentModel.ICustomTypeDescriptor,
    System]]
    MethodTable: 62a1618c
    EEClass:     628c6310
    Size:        72(0x48) bytes
    File:        C:\Windows\Microsoft.Net\assembly\GAC_MSIL\Microsoft.VisualStudio.ProjectSystem.Implementation\v4.0_14.0.0.0__b03f5f7f11d50a3a\Microsoft.VisualStudio.ProjectSystem.Implementation.dll
    Fields:
          MT    Field   Offset                 Type VT     Attr    Value Name
    7188fbb8  40006db       10         System.Int32  1 instance       -1
    <>1__state
    718a6778  40006dc       18 ...Canon, mscorlib]]  1 instance 11b8d03c
    <>t__builder
    62d3a5d8  40006dd       24 ...ojectLockReleaser  1 instance 11b8d048
    <access>5__1
    62befc34  40006de        4 ...Canon, mscorlib]]  0 instance 11b8cfa0
    CS$<>8__locals1
    71896038  40006df       14       System.Boolean  1 instance        0
    isWriteLockHeld
    62a0bd7c  40006e0        8 ...Canon, mscorlib]]  0 instance 11b8cf60
    <>4__this
    6d51c598  40006e1        c ...Canon, mscorlib]]  0 instance 11c0de0c
    <valueTask>5__3
    62d3a658  40006e2       30 ...rojectLockAwaiter  1 instance 11b8d054
    <>u__$awaiter3
    634e4ec0  40006e3       3c ...Microsoft.Build]]  1 instance 11b8d060
    <>u__$awaiter4
    718a6818  40006e4       40 ...Canon, mscorlib]]  1 instance 11b8d064
    <>u__$awaiter5
    0:016> !DumpObj /d 11b8cf60
    Name:        Microsoft.VisualStudio.ProjectSystem.ConfiguredProjectCache`1[[System.ComponentModel.ICustomTypeDescriptor,
    System]]
    MethodTable: 62a1f170
    EEClass:     628c5868
    Size:        52(0x34) bytes
    File:        C:\Windows\Microsoft.Net\assembly\GAC_MSIL\Microsoft.VisualStudio.ProjectSystem.Implementation\v4.0_14.0.0.0__b03f5f7f11d50a3a\Microsoft.VisualStudio.ProjectSystem.Implementation.dll
    Fields:
          MT    Field   Offset                 Type VT     Attr    Value Name
    62a0a174  4000041        4 ...ojectCacheFactory  0 instance 11427f7c
    factory
    7188e0c8  4000042        8        System.Object  0 instance 11b8cf94
    syncObject
    00000000  4000043        c                       0 instance 11b8cf40
    cacheUpdateDelegate
    62d1b24c  4000044       20         System.Int32  1 instance        2
    options
    6d51c598  4000045       10 ...Canon, mscorlib]]  0 instance 11c0de0c cache
    6d51c598  4000046       14 ...Canon, mscorlib]]  0 instance 00000000
    cacheUnderWriteLock
    71892d34  4000047       18   System.IComparable  0 instance 1142620c
    projectVersion
    7188fbb8  4000048       24         System.Int32  1 instance       71
    evaluationCounter
    7188fbb8  4000049       28         System.Int32  1 instance       -1
    evaluationCounterUnderWriteLock
    714c01c8  400004a       1c ...tring, mscorlib]]  0 instance 00000000
    globalPropertiesAtCacheRefresh
    71896038  400004b       2c       System.Boolean  1 instance        0
    explicitlyInvalidated
    71896038  400004c       2d       System.Boolean  1 instance        0
    explicitlyInvalidatedUnderWriteLock

And we see from code inspection that for `ConfiguredProjectCacheOptions`,
a value of `2` means `ValueFactoryRequiresUpgradeableRead`. 

Q.E.D. we have identified the holder of the upgradeable read lock.
