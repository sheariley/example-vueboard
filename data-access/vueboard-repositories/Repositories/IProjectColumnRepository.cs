using Vueboard.DataAccess.Models;

namespace Vueboard.DataAccess.Repositories
{
  public interface IProjectColumnRepository
  {
    IEnumerable<ProjectColumn> GetAllForProject(int projectId);
    IEnumerable<ProjectColumn> GetAllForProjects(IEnumerable<int> projectIds);
    ProjectColumn? GetById(int id);
    ProjectColumn? GetByUid(Guid uid);
    ProjectColumn Add(int projectId, ProjectColumn column);
    bool Update(ProjectColumn column);
    bool Delete(int id);
  }
}