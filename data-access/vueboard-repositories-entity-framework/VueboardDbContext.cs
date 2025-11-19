using Microsoft.EntityFrameworkCore;
using Vueboard.DataAccess.Models;

namespace Vueboard.DataAccess.EntityFramework
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
        .HasMany(p => p.Columns)
        .WithOne()
        .HasForeignKey(pc => pc.ProjectId);

      // ProjectColumn -> WorkItems
      modelBuilder.Entity<ProjectColumn>()
        .HasMany(pc => pc.WorkItems)
        .WithOne()
        .HasForeignKey(wi => wi.ProjectColumnId);

      // WorkItem <-> WorkItemTag (Many-to-Many using WorkItemTagRef table)
      modelBuilder.Entity<WorkItem>()
        .HasMany(wi => wi.WorkItemTags)
        .WithMany(wt => wt.WorkItems)
        .UsingEntity("WorkItemTagRef");
    }
  }
}
