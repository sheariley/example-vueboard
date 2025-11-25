using Microsoft.EntityFrameworkCore;
using Vueboard.DataAccess.Models;

namespace Vueboard.DataAccess.Repositories.EntityFramework
{
  public class EFProjectColumnRepository : EFGenericSoftDeleteRepository<ProjectColumn>, IProjectColumnRepository
  {
    private readonly IWorkItemRepository _workItemRepo;
    public EFProjectColumnRepository(IVueboardDbContext context, IWorkItemRepository workItemRepo)
      : base(context)
    {
      _workItemRepo = workItemRepo;
    }

    public IEnumerable<ProjectColumn> GetAllForProject(int projectId)
    {
      return GetAllForProjects([projectId]);
    }

    public IEnumerable<ProjectColumn> GetAllForProjects(IEnumerable<int> projectIds)
    {
      return GetQueryRoot()
        .Where(pc => pc.ProjectId.HasValue && projectIds.Contains(pc.ProjectId.Value))
        .ToList();
    }

    protected override void BeforeDelete(ProjectColumn entity)
    {
      entity.ProjectId = null;
      foreach (var workItem in entity.WorkItems ?? [])
      {
        _workItemRepo.Delete(workItem);
      }
    }
  }
}
