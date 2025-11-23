using Vueboard.DataAccess.Models;
using Vueboard.DataAccess.Repositories.EntityFramework.QueryRoots;

namespace Vueboard.Api.GraphQL
{
  // Batch DataLoader for Projects by Id
  public class ProjectByIdDataLoader : BatchDataLoader<int, Project>
  {
    private readonly IProjectQueryRoot _queryRoot;
    public ProjectByIdDataLoader(IBatchScheduler batchScheduler, IProjectQueryRoot queryRoot)
        : base(batchScheduler, new DataLoaderOptions())
    {
      _queryRoot = queryRoot;
    }

    protected override async Task<IReadOnlyDictionary<int, Project>> LoadBatchAsync(IReadOnlyList<int> keys, CancellationToken cancellationToken)
    {
      return await Task.Run(
        () => _queryRoot.Query.Where(p => keys.Contains(p.Id) && !p.IsDeleted).ToDictionary(p => p.Id)
      , cancellationToken);
    }
  }

  // Group DataLoader for ProjectColumns by ProjectId
  public class ProjectColumnsByProjectIdDataLoader : GroupedDataLoader<int, ProjectColumn>
  {
    private readonly IProjectColumnQueryRoot _queryRoot;
    public ProjectColumnsByProjectIdDataLoader(IBatchScheduler batchScheduler, IProjectColumnQueryRoot queryRoot)
        : base(batchScheduler, new DataLoaderOptions())
    {
      _queryRoot = queryRoot;
    }

    protected override async Task<ILookup<int, ProjectColumn>> LoadGroupedBatchAsync(IReadOnlyList<int> keys, CancellationToken cancellationToken)
    {
      return await Task.Run(
        () => _queryRoot.Query
          .Where(x => keys.Contains(x.ProjectId) && !x.IsDeleted)
          .ToLookup(x => x.ProjectId)
      , cancellationToken);
    }
  }

  // Group DataLoader for WorkItems by ProjectColumnId
  public class WorkItemsByProjectColumnIdDataLoader : GroupedDataLoader<int, WorkItem>
  {
    private readonly IWorkItemQueryRoot _queryRoot;
    public WorkItemsByProjectColumnIdDataLoader(IBatchScheduler batchScheduler, IWorkItemQueryRoot queryRoot)
        : base(batchScheduler, new DataLoaderOptions())
    {
      _queryRoot = queryRoot;
    }

    protected override async Task<ILookup<int, WorkItem>> LoadGroupedBatchAsync(IReadOnlyList<int> keys, CancellationToken cancellationToken)
    {
      return await Task.Run(
        () => _queryRoot.Query
          .Where(x => keys.Contains(x.ProjectColumnId) && !x.IsDeleted)
          .ToLookup(x => x.ProjectColumnId)
        , cancellationToken);
    }
  }

  // Group DataLoader for WorkItemTags by WorkItemId
  public class WorkItemTagsByWorkItemIdDataLoader : GroupedDataLoader<int, WorkItemTag>
  {
    private readonly IWorkItemQueryRoot _queryRoot;

    public WorkItemTagsByWorkItemIdDataLoader(
      IBatchScheduler batchScheduler,
      DataLoaderOptions options,
      IWorkItemQueryRoot queryRoot)
      : base(batchScheduler, options)
    {
      _queryRoot = queryRoot;
    }

    protected override async Task<ILookup<int, WorkItemTag>> LoadGroupedBatchAsync(
      IReadOnlyList<int> keys,
      CancellationToken cancellationToken)
    {
      return await Task.Run(
        () => _queryRoot.Query
          .Where(w => keys.Contains(w.Id) && !w.IsDeleted)
          .SelectMany(w => w.WorkItemTags
            .Select(t => new
            {
              WorkItemId = w.Id,
              Tag = t
            })
          )
          .ToLookup(x => x.WorkItemId, x => x.Tag)
        , cancellationToken);
    }
  }

  // Group DataLoader for WorkItems by WorkItemTagId
  public class WorkItemsByWorkItemTagIdDataLoader : GroupedDataLoader<int, WorkItem>
  {
    private readonly IWorkItemTagQueryRoot _queryRoot;

    public WorkItemsByWorkItemTagIdDataLoader(
      IBatchScheduler batchScheduler,
      DataLoaderOptions options,
      IWorkItemTagQueryRoot queryRoot)
      : base(batchScheduler, options)
    {
      _queryRoot = queryRoot;
    }

    protected override async Task<ILookup<int, WorkItem>> LoadGroupedBatchAsync(
      IReadOnlyList<int> keys,
      CancellationToken cancellationToken)
    {
      return await Task.Run(
        () => _queryRoot.Query
          .Where(t => keys.Contains(t.Id))
          .SelectMany(t => t.WorkItems
            .Where(w => !w.IsDeleted)
            .Select(w => new
            {
              WorkItemTagId = t.Id,
              WorkItem = w
            })
          )
          .ToLookup(x => x.WorkItemTagId, x => x.WorkItem)
      , cancellationToken);
    }
    
  }
}
