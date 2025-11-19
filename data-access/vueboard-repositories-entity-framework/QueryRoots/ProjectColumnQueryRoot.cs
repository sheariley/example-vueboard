using Vueboard.DataAccess.EntityFramework;
using Vueboard.DataAccess.Models;

namespace Vueboard.DataAccess.Repositories.EntityFramework.QueryRoots
{
  public class ProjectColumnQueryRoot : IProjectColumnQueryRoot
  {
    private readonly IVueboardDbContext _ctx;

    public ProjectColumnQueryRoot(IVueboardDbContext ctx)
    {
      _ctx = ctx;
    }

    public IQueryable<ProjectColumn> Query => _ctx.ProjectColumns
      .AsQueryable();
  }  
}