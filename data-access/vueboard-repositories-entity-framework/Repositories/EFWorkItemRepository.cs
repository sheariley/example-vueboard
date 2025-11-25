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
      return GetQueryRoot()
        .Where(wi => wi.ProjectColumnId.HasValue && projectColumnIds.Contains(wi.ProjectColumnId.Value))
        .ToList();
    }

    public List<WorkItemTag> GetTagsForWorkItem(int workItemId)
    {
      var item = GetById(workItemId);
      return item?.WorkItemTags ?? new List<WorkItemTag>();
    }

    protected override void BeforeDelete(WorkItem entity)
    {
      entity.ProjectColumnId = null;
    }
  }
}
