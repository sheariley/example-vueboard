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

    protected override Task<IReadOnlyDictionary<int, Project>> LoadBatchAsync(IReadOnlyList<int> keys, CancellationToken cancellationToken)
    {
      var projects = _queryRoot.Query.Where(p => keys.Contains(p.Id)).ToDictionary(p => p.Id);
      return Task.FromResult((IReadOnlyDictionary<int, Project>)projects);
    }
  }

  // Group DataLoader for ProjectColumns by ProjectId
  public class ProjectColumnsByProjectIdDataLoader : GroupedDataLoader<int, IEnumerable<ProjectColumn>>
  {
    private readonly IProjectColumnQueryRoot _queryRoot;
    public ProjectColumnsByProjectIdDataLoader(IBatchScheduler batchScheduler, IProjectColumnQueryRoot queryRoot)
        : base(batchScheduler, new DataLoaderOptions())
    {
      _queryRoot = queryRoot;
    }

    protected override Task<ILookup<int, IEnumerable<ProjectColumn>>> LoadGroupedBatchAsync(IReadOnlyList<int> keys, CancellationToken cancellationToken)
    {
      var columns = _queryRoot.Query
        .Where(x => keys.Contains(x.ProjectId))
        .GroupBy(x => x.ProjectId)
        .ToLookup(g => g.Key, g => g.AsEnumerable());
      return Task.FromResult(columns);
    }
  }

  // Group DataLoader for WorkItems by ProjectColumnId
  public class WorkItemsByProjectColumnIdDataLoader : GroupedDataLoader<int, IEnumerable<WorkItem>>
  {
    private readonly IWorkItemQueryRoot _queryRoot;
    public WorkItemsByProjectColumnIdDataLoader(IBatchScheduler batchScheduler, IWorkItemQueryRoot queryRoot)
        : base(batchScheduler, new DataLoaderOptions())
    {
      _queryRoot = queryRoot;
    }

    protected override Task<ILookup<int, IEnumerable<WorkItem>>> LoadGroupedBatchAsync(IReadOnlyList<int> keys, CancellationToken cancellationToken)
    {
      var workItems = _queryRoot.Query
        .Where(x => keys.Contains(x.ProjectColumnId))
        .GroupBy(x => x.ProjectColumnId)
        .ToLookup(g => g.Key, g => g.AsEnumerable());
      return Task.FromResult(workItems);
    }
  }
}
