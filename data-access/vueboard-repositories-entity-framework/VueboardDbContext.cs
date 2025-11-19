using System;
using Microsoft.EntityFrameworkCore;
using Vueboard.DataAccess.EntityFramework.Config;
using Vueboard.DataAccess.Models;

namespace Vueboard.DataAccess.EntityFramework
{
  public class VueboardDbContext : DbContext, IVueboardDbContext
  {
    private readonly IVueboardDbContextConfig _config;

    public VueboardDbContext(DbContextOptions<VueboardDbContext> options, IVueboardDbContextConfig config)
        : base(options)
    {
      _config = config;
    }

    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectColumn> ProjectColumns { get; set; }
    public DbSet<WorkItem> WorkItems { get; set; }
    public DbSet<WorkItemTag> WorkItemTags { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      if (!optionsBuilder.IsConfigured)
      {
        _config.Apply(optionsBuilder);
      }
    }

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
