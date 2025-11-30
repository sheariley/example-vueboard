using Microsoft.EntityFrameworkCore;
using Vueboard.DataAccess.Models;
using Vueboard.Server.Environment.Auth;

namespace Vueboard.DataAccess.Repositories.EntityFramework
{
  public abstract class EFGenericSoftDeleteRepository<TEntity>(
    IVueboardDbContext context,
    IUserIdAccessor userIdAccessor
  ) : GenericSoftDeleteRepository<TEntity>, IGenericRepository<TEntity>
    where TEntity : class, IVueboardSoftDeleteEntity
  {
    protected readonly IVueboardDbContext _context = context;
    protected readonly IUserIdAccessor _userIdAccessor = userIdAccessor;

    protected virtual DbSet<TEntity> GetDbSet()
    {
      return _context.Set<TEntity>();
    }

    protected virtual int? GetParentID(TEntity entity)
    {
      return null;
    }

    protected override IQueryable<TEntity> GetRawQueryRoot()
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
      
      var result = base.Delete(entity);
      if (entity != null)
      {
        _context.SoftDeletes.Add(new SoftDelete
        {
          Uid = Guid.NewGuid(),
          Deleted = DateTime.UtcNow,
          EntityType = entity.GetType().Name,
          UserId = _userIdAccessor.GetUserId()!.Value,
          ParentId = GetParentID(entity)
        });
      }
      return result;
    }

  }
}