using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Vueboard.DataAccess.Models;

namespace Vueboard.DataAccess.Repositories.EntityFramework
{
  public interface IVueboardDbContext : IDisposable
  {
    DbSet<Project> Projects { get; }
    DbSet<ProjectColumn> ProjectColumns { get; }
    DbSet<WorkItem> WorkItems { get; }
    DbSet<WorkItemTag> WorkItemTags { get; }

    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    void SetEntityState<TEntity>(TEntity entity, EntityState state)
      where TEntity : class;

    EntityEntry<TEntity> Attach<TEntity>(TEntity entity)
      where TEntity : class;

    DbSet<TEntity> Set<TEntity>() where TEntity : class;

    IEnumerable<EntityEntry<TEntity>> Entries<TEntity>()
      where TEntity : class;

    IEnumerable<EntityEntry> Entries();
  }
}
