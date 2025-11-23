using Microsoft.EntityFrameworkCore;
using Vueboard.DataAccess.Models;

namespace Vueboard.DataAccess.Repositories.EntityFramework
{
  public abstract class EFGenericRepository<TEntity> : GenericRepository<TEntity>, IGenericRepository<TEntity>
    where TEntity : class, IVueboardEntity
  {
    protected IVueboardDbContext _context;
    public EFGenericRepository(IVueboardDbContext context)
    {
      _context = context;
    }

    protected virtual DbSet<TEntity> GetDbSet()
    {
      return _context.Set<TEntity>();
    }

    protected override IQueryable<TEntity> GetQueryRoot()
    {
      return GetDbSet().AsQueryable();
    }

    public override void CommitChanges()
    {
      _context.SaveChanges();
    }

    public override TEntity Create(TEntity entity)
    {
      var entry = GetDbSet().Add(entity);
      return entry.Entity;
    }

    public override bool Update(TEntity entity)
    {
      GetDbSet().Update(entity);
      return true;
    }

    public override bool Delete(TEntity? entity)
    {
      if (entity == null) return false;
      GetDbSet().Remove(entity);
      return true;
    }

  }
}