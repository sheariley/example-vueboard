using Vueboard.DataAccess.Models;
using Vueboard.DataAccess.Repositories.EntityFramework.QueryRoots;

namespace Vueboard.Api.GraphQL
{
  public class Query
  {
    private readonly IProjectQueryRoot _projectQueryRoot;

    public Query(IProjectQueryRoot projectQueryRoot)
    {
      _projectQueryRoot = projectQueryRoot;
    }

    [UsePaging]
    public IQueryable<Project> ProjectsPaged() => _projectQueryRoot.Query
      .Where(p => !p.IsDeleted);

    public IQueryable<Project> Projects() => _projectQueryRoot.Query
      .Where(p => !p.IsDeleted);

    public Project? Project(Guid uid) => _projectQueryRoot.Query
      .Where(p => !p.IsDeleted && p.Uid.Equals(uid))
      .FirstOrDefault();
  }
}
