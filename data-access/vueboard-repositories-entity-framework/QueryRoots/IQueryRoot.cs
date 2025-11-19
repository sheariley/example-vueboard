namespace Vueboard.DataAccess.Repositories.EntityFramework.QueryRoots
{
  public interface IQueryRoot<T>
  {
    IQueryable<T> Query { get; }
  }
}
