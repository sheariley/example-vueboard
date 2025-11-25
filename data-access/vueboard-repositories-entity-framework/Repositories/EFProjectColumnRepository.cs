using Microsoft.EntityFrameworkCore;
using Vueboard.DataAccess.Models;
using Vueboard.Server.Environment.Auth;

namespace Vueboard.DataAccess.Repositories.EntityFramework
{
  public class EFProjectColumnRepository(
    IVueboardDbContext context,
    IUserIdAccessor userIdAccessor,
    IWorkItemRepository workItemRepo
    ) : EFGenericSoftDeleteRepository<ProjectColumn>(context, userIdAccessor), IProjectColumnRepository
  {
    private readonly IWorkItemRepository _workItemRepo = workItemRepo;

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

    protected override int? GetParentID(ProjectColumn entity)
    {
      return entity.ProjectId;
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
