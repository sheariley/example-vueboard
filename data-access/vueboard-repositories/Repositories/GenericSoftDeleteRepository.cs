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

    protected virtual void BeforeDelete(TEntity entity)
    {
      // DO NOTHING
    }

    public override bool Delete(TEntity? entity)
    {
      if (entity == null) return false;
      BeforeDelete(entity);
      entity.IsDeleted = true;
      return true;
    }
  }
}