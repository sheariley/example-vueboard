using Vueboard.DataAccess.Models;

namespace Vueboard.DataAccess.Repositories.EntityFramework.QueryRoots
{
  public class WorkItemQueryRoot : IWorkItemQueryRoot
  {
    private readonly IVueboardDbContext _ctx;

    public WorkItemQueryRoot(IVueboardDbContext ctx)
    {
      _ctx = ctx;
    }

    public IQueryable<WorkItem> Query => _ctx.WorkItems
      .AsQueryable();
  }  
}