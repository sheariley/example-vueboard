using HotChocolate;
using HotChocolate.Types;
using Vueboard.DataAccess.Models;
using Vueboard.DataAccess.Repositories;

namespace Vueboard.Api.GraphQL
{
  public class Query
  {
    private readonly IProjectRepository _projectRepo;
    public Query(IProjectRepository projRepo)
    {
      _projectRepo = projRepo;
    }

    [UsePaging]
    public IEnumerable<Project> Projects() => _projectRepo.GetAll().Where(p => !p.IsDeleted);

    public Project? Project(string uid) => _projectRepo.GetByUid(uid);
  }
}
