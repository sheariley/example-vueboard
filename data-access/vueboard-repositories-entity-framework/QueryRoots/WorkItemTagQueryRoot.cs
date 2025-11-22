using Vueboard.DataAccess.Models;

namespace Vueboard.DataAccess.Repositories.EntityFramework.QueryRoots
{
  public class WorkItemTagQueryRoot : IWorkItemTagQueryRoot
  {
    private readonly IVueboardDbContext _ctx;

    public WorkItemTagQueryRoot(IVueboardDbContext ctx)
    {
      _ctx = ctx;
    }

    public IQueryable<WorkItemTag> Query => _ctx.WorkItemTags
      .AsQueryable();
  }  
}