using HotChocolate;
using HotChocolate.Types;

namespace Vueboard.Api.GraphQL
{
  public class Query
  {
    private readonly IProjectRepository _projectRepo;
    private readonly IProjectColumnRepository _projectColumnRepo;
    private readonly IWorkItemRepository _workItemRepo;
    public Query(IProjectRepository projRepo, IProjectColumnRepository colRepo, IWorkItemRepository workItemRepo)
    {
      _projectRepo = projRepo;
      _projectColumnRepo = colRepo;
      _workItemRepo = workItemRepo;
    }

    [UsePaging]
    public IEnumerable<Project> Projects() => _projectRepo.GetAll();

    public Project? Project(string uid) => _projectRepo.GetByUid(uid);
  }
}
