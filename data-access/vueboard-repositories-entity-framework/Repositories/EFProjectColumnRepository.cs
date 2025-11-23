using Microsoft.EntityFrameworkCore;
using Vueboard.DataAccess.Models;

namespace Vueboard.DataAccess.Repositories.EntityFramework
{
  public class EFProjectColumnRepository : EFGenericSoftDeleteRepository<ProjectColumn>, IProjectColumnRepository
  {
    public EFProjectColumnRepository(IVueboardDbContext context)
      : base(context)
    {
    }

    public IEnumerable<ProjectColumn> GetAllForProject(int projectId)
    {
      return GetAllForProjects([projectId]);
    }

    public IEnumerable<ProjectColumn> GetAllForProjects(IEnumerable<int> projectIds)
    {
      return GetQueryRoot().Where(pc => projectIds.Contains(pc.ProjectId)).ToList();
    }

    protected override IEnumerable<IVueboardSoftDeleteEntity> GetNestedSoftDeleteEntities(ProjectColumn entity)
    {
      return entity.WorkItems.ToList();
    }
  }
}
