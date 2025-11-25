using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Vueboard.DataAccess.Models;

namespace Vueboard.DataAccess.Repositories.EntityFramework
{
  public class VueboardDbContext(DbContextOptions<VueboardDbContext> options) : DbContext(options), IVueboardDbContext
  {
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectColumn> ProjectColumns { get; set; }
    public DbSet<WorkItem> WorkItems { get; set; }
    public DbSet<WorkItemTag> WorkItemTags { get; set; }
    public DbSet<SoftDelete> SoftDeletes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      // Project -> ProjectColumns
      modelBuilder.Entity<Project>()
        .ToTable(p => p.Metadata.SetSchema("user_data"))
        .HasMany(p => p.ProjectColumns)
        .WithOne(p => p.Project)
        .HasForeignKey(pc => pc.ProjectId);

      // ProjectColumn -> WorkItems
      modelBuilder.Entity<ProjectColumn>()
        .ToTable(p => p.Metadata.SetSchema("user_data"))
        .HasMany(pc => pc.WorkItems)
        .WithOne(w => w.ProjectColumn)
        .HasForeignKey(wi => wi.ProjectColumnId);

      // WorkItem <-> WorkItemTag (Many-to-Many using WorkItemTagRef table)
      modelBuilder.Entity<WorkItemTagRef>(entity =>
      {
        entity.ToTable(t => {
          t.Metadata.SetTableName("work_item_tag_refs");
          t.Metadata.SetSchema("user_data");
        });
        entity.HasKey(e => new { e.WorkItemId, e.WorkItemTagId }); // composite PK

        entity.HasOne<WorkItem>()
            .WithMany()
            .HasForeignKey(e => e.WorkItemId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne<WorkItemTag>()
            .WithMany()
            .HasForeignKey(e => e.WorkItemTagId)
            .OnDelete(DeleteBehavior.Cascade);
      });

      modelBuilder.Entity<WorkItem>()
        .ToTable(p => p.Metadata.SetSchema("user_data"))
        .HasMany(wi => wi.WorkItemTags)
        .WithMany(w => w.WorkItems)
        .UsingEntity<WorkItemTagRef>(
          j => j.HasOne<WorkItemTag>().WithMany().HasForeignKey(r => r.WorkItemTagId),
          j => j.HasOne<WorkItem>().WithMany().HasForeignKey(r => r.WorkItemId)
        );

      modelBuilder.Entity<WorkItemTag>()
        .ToTable(t => t.Metadata.SetSchema("user_data"));

      modelBuilder.Entity<SoftDelete>()
        .ToTable(t => t.Metadata.SetSchema("user_data"));
    }

    public void SetEntityState<TEntity>(TEntity entity, EntityState state)
      where TEntity : class
    {
      Entry(entity).State = state;
    }

    public IEnumerable<EntityEntry> Entries()
    {
      return ChangeTracker.Entries();
    }

    public IEnumerable<EntityEntry<TEntity>> Entries<TEntity>()
      where TEntity : class
    {
      return ChangeTracker.Entries<TEntity>();
    }
  }
}
