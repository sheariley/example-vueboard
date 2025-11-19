using System.Linq.Expressions;
using Vueboard.DataAccess.Models;

namespace Vueboard.DataAccess.Repositories
{
  public interface IProjectRepository
  {
    IEnumerable<TOut> Get<TOut>(Expression<Func<Project, TOut>> selector, FetchSpecification<Project>? specification = null);
    IEnumerable<Project> Get(FetchSpecification<Project> spec);
    IEnumerable<Project> Get(Expression<Func<Project, bool>> criteria);
    Project? GetByUid(Guid uid);
    Project Add(Project project);
    bool Update(Project project);
    bool Delete(Guid uid);
  }
}