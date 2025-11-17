using Vueboard.DataAccess.Models;

namespace Vueboard.DataAccess.Repositories
{
  public interface IProjectColumnRepository
  {
    IEnumerable<ProjectColumn> GetAllForProject(int projectId);
    ProjectColumn? GetById(int id);
    ProjectColumn Add(int projectId, ProjectColumn column);
    bool Update(ProjectColumn column);
    bool Delete(int id);
  }
}