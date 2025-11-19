using Vueboard.DataAccess.Models;

namespace Vueboard.DataAccess.Repositories
{
  public interface IWorkItemRepository
  {
    IEnumerable<WorkItem> GetAllForProjectColumn(int projectColumnId);
    IEnumerable<WorkItem> GetAllForProjectColumns(IEnumerable<int> projectColumnIds);
    WorkItem? GetById(int id);
    WorkItem? GetByUid(Guid uid);
    WorkItem CreateWorkItem(WorkItem item, Guid userId);
    bool UpdateWorkItem(WorkItem item, Guid userId);
    bool DeleteWorkItem(int workItemId);
    WorkItem? GetWorkItem(int workItemId);
    List<WorkItem> GetWorkItemsForColumn(int projectColumnId);
    List<string> GetTagsForWorkItem(int workItemId);
  }
}
