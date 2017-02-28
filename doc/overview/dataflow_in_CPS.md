# Dataflow in CPS

One of the main goals of CPS is to move the bulk of the project system work to background threads,
while still maintaining data consistency. To accomplish this, CPS leverages the [TPL.Dataflow](https://msdn.microsoft.com/en-us/library/hh228603%28v=vs.110%29.aspx?f=255&MSPPError=-2147217396)
library to produce a versioned, immutable, producer-consumer pattern to flow changes through the
project system. Dataflow is not always easy, and if used wrong it can quickly lead to corrupt
project states or deadlocks.

## Types of Dataflow in CPS

Dataflow in CPS comes primarily in two types, an original source or a chained source.

1. Original Source
   * Depends on an original source of data that is not part of dataflow.
   * Always has its own version.
   * IE: a file on disk
2. Chained Source
   * Chains into existing dataflow.
   * Can be one or multiple dataflow blocks that feed into this one.
   * Very __rarely__ has its own version. Typically if it does, it can
     be pulled out into an original source.
   * Carries all the versions of the dataflow it chains to.
     * More about versioning later

## [Dataflow Examples](../extensibility/dataflow_example.md)

## Data Consistency Problem

Dataflow is simple when you have a single line of dependencies, but in CPS it is much more complex.
It is common for a chained datasource to require input from multiple upstream sources. It is also
common for those upstream sources to also have multiple inputs. This pattern introduces a data
consistency problem. Take a look at the dataflow diagram below (arrows represent dataflow):

```
A --> C --> D
      ^     ^
      |     |
B ----|-----|
```

In the above layout, `A` and `B` are original sources. `C` listens to both `A` and `B`, but since
they are _original_ sources `C` can produce a new value when either change. `D` is where it gets
complex. `D` can only produce values when it has `B` and `C` of the same source version. `D` only
produces a value when the version of `C` it has was produced from the same version of `B` that
`D` currently has. To solve this consistency issue CPS versions all dataflow and then synchronizes
around these published versions.

## Datflow Versioning

To solve the problem described above, all dataflow in CPS produces types of `IProjectVersionedValue<T>`.
This type combines `T Value` and `IImmutableDictionary<NamedIdentity, IComparable> DataSourceVersions`.

> In Visual Studio 2015, it is `Guid` instead of `NamedIdentity`

Then, chained dataflow will cary the versions of its upstream data sources. When a chained source has
multiple upstream sources its published version becomes the merged value of the its upstream sources.
This functionality is facilitated via `ProjectDataSources.SyncLinkTo`. When using that method to link
to multiple upstream sources, a middle dataflow block is created that only publishes to your block when
all recieved values are in a consistent state. See [this example](../extensibility/dataflow_example.md#chained-data-source-multiple-sources)
for how to use `SyncLinkTo`.

### Rules to Follow with Versioning

__When you are a...__
* __Original source__ you have your own `DataSourceKey` and `DataSourceVersion`. The key
  identifies who you are, and the version must incremenet whenever you produce a new value.
  The only value in your `DataSourceVersions` published is your own.
* __Chained source__ you must merge and carry the versions of all dataflow you are chained
  to in your own `DataSourceVersions`. You very rarely have your own version because your
  version is just the combined versions that you chained to. If you do need your own version,
  consider pulling the part that publishes the original data into its own source.

### Allowing Inconsistent Versions

In special cases that require it, it is possible to allow for inconsistent versions in your dataflow.
This is for when you depend on multiple upstream sources where one is drastically slower at producing
values than others, but you want to be able to produce intermediate values while the slow one is still
processing. Unfortunately, there is no CPS base class equivalent to `ProjectValueDataSourceBase` or
`ChainedProjectValueDataSourceBase` for this scenario. You will have to manually link to your upstream
sources and synchronizing between multiple sources publishing at once. For calculating the data versions
to publish, use `ProjectDataSources.MergeDataSourceVersions`.