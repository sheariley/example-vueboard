using Vueboard.DataAccess.Models;

namespace Vueboard.DataAccess.Repositories
{
  public interface IWorkItemRepository : IGenericSoftDeleteRepository<WorkItem>
  {
    IEnumerable<WorkItem> GetAllForProjectColumn(int projectColumnId);
    IEnumerable<WorkItem> GetAllForProjectColumns(IEnumerable<int> projectColumnIds);
    List<WorkItemTag> GetTagsForWorkItem(int workItemId);
  }
}
