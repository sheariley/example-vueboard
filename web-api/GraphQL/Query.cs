using Vueboard.DataAccess.Models;
using Vueboard.DataAccess.Repositories.EntityFramework.QueryRoots;

namespace Vueboard.Api.GraphQL
{
  public class Query
  {
    [UsePaging]
    public IQueryable<Project> ProjectsPaged(
      [Service] IProjectQueryRoot projectQueryRoot
    ) => projectQueryRoot.Query
      .Where(p => !p.IsDeleted);

    public IQueryable<Project> Projects(
      [Service] IProjectQueryRoot projectQueryRoot
    ) => projectQueryRoot.Query
      .Where(p => !p.IsDeleted);

    public Project? Project(
      [Service] IProjectQueryRoot projectQueryRoot,
      Guid uid
    ) => projectQueryRoot.Query
      .Where(p => !p.IsDeleted && p.Uid.Equals(uid))
      .FirstOrDefault();

    [UsePaging]
    public IQueryable<WorkItemTag> WorkItemTagsPaged(
      [Service] IWorkItemTagQueryRoot workItemTagQueryRoot
    ) => workItemTagQueryRoot.Query;

    public IQueryable<WorkItemTag> WorkItemTags(
      [Service] IWorkItemTagQueryRoot workItemTagQueryRoot
    ) => workItemTagQueryRoot.Query;
  }
}
