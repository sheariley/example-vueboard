using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Vueboard.DataAccess.Models;

namespace Vueboard.DataAccess.Repositories.EntityFramework
{
    public interface IVueboardDbContext
    {
        DbSet<Project> Projects { get; set; }
        DbSet<ProjectColumn> ProjectColumns { get; set; }
        DbSet<WorkItem> WorkItems { get; set; }
        DbSet<WorkItemTag> WorkItemTags { get; set; }

        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        void SetEntityState<TEntity>(TEntity entity, EntityState state)
          where TEntity : class;

        EntityEntry<TEntity> Attach<TEntity>(TEntity entity)
          where TEntity : class;
    }
}
