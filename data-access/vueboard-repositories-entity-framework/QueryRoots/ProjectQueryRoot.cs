using Vueboard.DataAccess.Models;

namespace Vueboard.DataAccess.Repositories.EntityFramework.QueryRoots
{
  public class ProjectQueryRoot : IProjectQueryRoot
  {
    private readonly IVueboardDbContext _ctx;

    public ProjectQueryRoot(IVueboardDbContext ctx)
    {
      _ctx = ctx;
    }

    public IQueryable<Project> Query => _ctx.Projects
      .AsQueryable();
  }  
}