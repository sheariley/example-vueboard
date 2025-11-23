using System.Linq.Expressions;
using Vueboard.DataAccess.Models;

namespace Vueboard.DataAccess.Repositories
{
  public interface IGenericRepository<TEntity>
    where TEntity : class, IVueboardEntity
  {
    IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> predicate);
    IEnumerable<TEntity> Get(FetchSpecification<TEntity> spec);
    IEnumerable<TOut> Get<TOut>(Expression<Func<TEntity, TOut>> selector, FetchSpecification<TEntity>? specification = null);
    IEnumerable<TEntity> GetAll();
    TEntity? GetById(int id);
    TEntity? GetByUid(Guid uid);
    TEntity Create(TEntity entity);
    bool Update(TEntity entity);
    bool Delete(int id);
    bool DeleteByUid(Guid uid);

    void CommitChanges();
  }
}