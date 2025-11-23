using Vueboard.DataAccess.Models;

namespace Vueboard.DataAccess.Repositories
{
  public abstract class GenericSoftDeleteRepository<TEntity> : GenericRepository<TEntity>, IGenericSoftDeleteRepository<TEntity>
    where TEntity : class, IVueboardSoftDeleteEntity
  {

    protected abstract IQueryable<TEntity> GetRawQueryRoot();

    protected override IQueryable<TEntity> GetQueryRoot()
    {
      return GetRawQueryRoot().Where(x => !x.IsDeleted);
    }

    protected virtual IEnumerable<IVueboardSoftDeleteEntity> GetNestedSoftDeleteEntities(TEntity entity)
    {
      return Enumerable.Empty<IVueboardSoftDeleteEntity>();
    }

    public override bool Delete(TEntity? entity)
    {
      if (entity == null) return false;
      entity.IsDeleted = true;
      var nestedEntities = GetNestedSoftDeleteEntities(entity);
      foreach (var nestedEntity in nestedEntities)
      {
        nestedEntity.IsDeleted = true;
      }
      return true;
    }
  }
}