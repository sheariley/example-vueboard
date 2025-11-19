using Vueboard.DataAccess.Models;

namespace Vueboard.DataAccess.Repositories
{
  public interface IWorkItemTagRepository
  {
    IEnumerable<WorkItemTag> GetAll();
    WorkItemTag Create(WorkItemTag tag);
    bool Update(WorkItemTag tag);
    bool Delete(int id);
  }
}