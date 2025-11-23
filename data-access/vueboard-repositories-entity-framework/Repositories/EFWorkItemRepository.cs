using Microsoft.EntityFrameworkCore;
using Vueboard.DataAccess.Models;

namespace Vueboard.DataAccess.Repositories.EntityFramework
{
  public class EFWorkItemRepository : EFGenericSoftDeleteRepository<WorkItem>, IWorkItemRepository
  {
    public EFWorkItemRepository(IVueboardDbContext context)
      : base(context)
    {
    }

    public IEnumerable<WorkItem> GetAllForProjectColumn(int projectColumnId)
    {
      return GetAllForProjectColumns([projectColumnId]);
    }

    public IEnumerable<WorkItem> GetAllForProjectColumns(IEnumerable<int> projectColumnIds)
    {
      return GetQueryRoot().Where(wi => projectColumnIds.Contains(wi.ProjectColumnId)).ToList();
    }

    public List<WorkItemTag> GetTagsForWorkItem(int workItemId)
    {
      var item = GetById(workItemId);
      return item?.WorkItemTags ?? new List<WorkItemTag>();
    }
  }
}
