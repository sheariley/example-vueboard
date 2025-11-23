using Microsoft.EntityFrameworkCore;
using Vueboard.DataAccess.Models;

namespace Vueboard.DataAccess.Repositories.EntityFramework
{
  public abstract class EFGenericSoftDeleteRepository<TEntity> : GenericSoftDeleteRepository<TEntity>, IGenericRepository<TEntity>
    where TEntity : class, IVueboardSoftDeleteEntity
  {
    protected IVueboardDbContext _context;
    public EFGenericSoftDeleteRepository(IVueboardDbContext context)
    {
      _context = context;
    }

    protected virtual DbSet<TEntity> GetDbSet()
    {
      return _context.Set<TEntity>();
    }

    protected override IQueryable<TEntity> GetRawQueryRoot()
    {
      return _context.Set<TEntity>().AsQueryable();
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

  }
}