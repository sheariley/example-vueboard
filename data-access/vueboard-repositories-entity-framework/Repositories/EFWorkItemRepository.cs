using Microsoft.EntityFrameworkCore;
using Vueboard.DataAccess.Models;
using Vueboard.Server.Environment.Auth;

namespace Vueboard.DataAccess.Repositories.EntityFramework
{
  public class EFWorkItemRepository(
    IVueboardDbContext context,
    IUserIdAccessor userIdAccessor
    ) : EFGenericSoftDeleteRepository<WorkItem>(context, userIdAccessor), IWorkItemRepository
  {
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

    protected override int? GetParentID(WorkItem entity)
    {
      return entity.ProjectColumnId;
    }

    protected override void BeforeDelete(WorkItem entity)
    {
      entity.ProjectColumnId = null;
    }
  }
}
