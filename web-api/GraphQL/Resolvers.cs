using Vueboard.DataAccess.Models;

namespace Vueboard.Api.GraphQL
{
  public class ProjectResolvers
  {
    public async Task<IEnumerable<ProjectColumn>?> GetColumnsAsync(
      [Parent] Project project,
      ProjectColumnsByProjectIdDataLoader dataLoader,
      CancellationToken cancellationToken
    ) => await dataLoader.LoadAsync(project.Id, cancellationToken);
  }

  public class ProjectColumnResolvers
  {
    public async Task<IEnumerable<WorkItem>?> GetWorkItemsAsync(
      [Parent] ProjectColumn column,
      WorkItemsByProjectColumnIdDataLoader dataLoader,
      CancellationToken cancellationToken
    ) => await dataLoader.LoadAsync(column.Id, cancellationToken);
  }

  public class WorkItemResolvers
  {
    public async Task<IEnumerable<WorkItemTag>> GetTagsAsync(
      [Parent] WorkItem workItem,
      WorkItemTagsByWorkItemIdDataLoader dataLoader,
      CancellationToken cancellationToken)
    {
      var result = await dataLoader.LoadAsync(workItem.Id, cancellationToken);
      return result ?? System.Array.Empty<WorkItemTag>();
    }
  }
}