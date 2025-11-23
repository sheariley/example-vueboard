using Vueboard.DataAccess.Models;

namespace Vueboard.DataAccess.Repositories
{
  public interface IProjectColumnRepository : IGenericSoftDeleteRepository<ProjectColumn>
  {
    IEnumerable<ProjectColumn> GetAllForProject(int projectId);
    IEnumerable<ProjectColumn> GetAllForProjects(IEnumerable<int> projectIds);
  }
}