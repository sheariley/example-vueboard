using System.Linq.Expressions;
using System.Reflection;
using Vueboard.DataAccess.Models;

namespace Vueboard.DataAccess.Repositories
{
  public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity>
    where TEntity : class, IVueboardEntity
  {
    protected abstract IQueryable<TEntity> GetQueryRoot();

    public abstract TEntity Create(TEntity entity);

    public abstract bool Update(TEntity entity);

    public abstract bool Delete(TEntity? entity);

    public abstract void CommitChanges();

    public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> predicate) => Get(x => x, new FetchSpecification<TEntity> { Criteria = predicate });

    public IEnumerable<TEntity> Get(FetchSpecification<TEntity> spec) => Get(x => x, spec);

    public IEnumerable<TOut> Get<TOut>(Expression<Func<TEntity, TOut>> selector, FetchSpecification<TEntity>? specification = null)
    {
      var query = GetQueryRoot();
      if (specification != null)
        query = specification.Apply(query);
      return query.Select(selector).ToList();
    }

    public IEnumerable<TEntity> GetAll()
    {
      return GetQueryRoot().ToList();
    }

    public TEntity? GetById(int id)
    {
      return GetQueryRoot().FirstOrDefault(x=> x.Id == id);
    }

    public TEntity? GetByUid(Guid uid)
    {
      return GetQueryRoot().FirstOrDefault(x=> x.Uid.Equals(uid));
    }

    public bool Delete(int id)
    {
      var entity = GetById(id);
      return Delete(entity);
    }

    public bool DeleteByUid(Guid uid)
    {
      var entity = GetByUid(uid);
      return Delete(entity);
    }

    protected bool isScalarEntityProperty(PropertyInfo prop)
    {
      return (
        !typeof(System.Collections.IEnumerable).IsAssignableFrom(prop.PropertyType)
        || prop.PropertyType == typeof(string)
      ) && (
        !typeof(IVueboardEntity).IsAssignableFrom(prop.PropertyType)
        && prop.PropertyType != typeof(WorkItemTagRef)
      );
    }

  }
}