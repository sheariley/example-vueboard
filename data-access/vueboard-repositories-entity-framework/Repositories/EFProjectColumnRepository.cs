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
      return GetQueryRoot().Where(pc => projectIds.Contains(pc.ProjectId)).ToList();
    }

    protected override void AfterDelete(ProjectColumn entity)
    {
      foreach (var workItem in entity.WorkItems ?? [])
      {
        _workItemRepo.Delete(workItem);
      }
    }
  }
}
