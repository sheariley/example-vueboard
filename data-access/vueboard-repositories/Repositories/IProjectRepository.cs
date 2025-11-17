using Vueboard.DataAccess.Models;

namespace Vueboard.DataAccess.Repositories
{
  public interface IProjectRepository
  {
    IEnumerable<Project> GetAll();
    Project? GetByUid(string uid);
    Project Add(Project project);
    bool Update(Project project);
    bool Delete(string uid);
  }
}