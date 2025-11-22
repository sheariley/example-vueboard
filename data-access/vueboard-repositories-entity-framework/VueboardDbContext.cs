using Microsoft.EntityFrameworkCore;
using Vueboard.DataAccess.Models;

namespace Vueboard.DataAccess.Repositories.EntityFramework
{
  public class VueboardDbContext : DbContext, IVueboardDbContext
  {
    public VueboardDbContext(DbContextOptions<VueboardDbContext> options)
        : base(options)
    {
    }

    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectColumn> ProjectColumns { get; set; }
    public DbSet<WorkItem> WorkItems { get; set; }
    public DbSet<WorkItemTag> WorkItemTags { get; set; }

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
      modelBuilder.Entity<WorkItem>()
        .ToTable(p => p.Metadata.SetSchema("user_data"))
        .HasMany(wi => wi.WorkItemTags)
        .WithMany(wt => wt.WorkItems)
        .UsingEntity<WorkItemTagRef>();

      modelBuilder.Entity<WorkItemTag>()
        .ToTable(t => t.Metadata.SetSchema("user_data"));

      modelBuilder.Entity<WorkItemTagRef>()
        .ToTable(r => {
          r.Metadata.SetTableName("work_item_tag_refs");
          r.Metadata.SetSchema("user_data");
        });
    }
  }
}
