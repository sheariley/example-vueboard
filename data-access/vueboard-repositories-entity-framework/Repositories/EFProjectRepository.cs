using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Vueboard.DataAccess.Models;

namespace Vueboard.DataAccess.Repositories.EntityFramework
{
  public class EFProjectRepository : IProjectRepository
  {
    private readonly IVueboardDbContext _context;
    public EFProjectRepository(IVueboardDbContext context)
    {
      _context = context;
    }

    public IEnumerable<Project> Get(Expression<Func<Project, bool>> predicate) => Get(x => x, new FetchSpecification<Project> { Criteria = predicate });

    public IEnumerable<Project> Get(FetchSpecification<Project> spec) => Get(x => x, spec);

    public IEnumerable<TOut> Get<TOut>(Expression<Func<Project, TOut>> selector, FetchSpecification<Project>? specification = null)
    {
      var query = _context.Projects.AsQueryable();
      if (specification != null)
        query = specification.Apply(query);
      return query.Select(selector).ToList();
    }

    public Project? GetByUid(Guid uid)
    {
      return _context.Projects.FirstOrDefault(p => p.Uid.Equals(uid));
    }

    public Project Add(Project project)
    {
      // Ensure all sub-entities have appropriate UserId fields set
      // NOTE: This will have to change if we implement team-based project ownership.
      project.ProjectColumns?.ForEach(col =>
      {
        col.WorkItems?.ForEach(w =>
        {
          w.WorkItemTags?.ForEach(t => t.UserId = project.UserId);
        });
      });

      var entry = _context.Projects.Add(project);
      _context.SaveChanges();
      return entry.Entity;
    }

    public bool Update(Project project)
    {
      var existingProject = _context.Set<Project>()
        .Include(p => p.ProjectColumns)
          .ThenInclude(pc => pc.WorkItems)
            .ThenInclude(wi => wi.WorkItemTags)
        .FirstOrDefault(p => p.Uid == project.Uid);

      if (existingProject == null)
        return false;

      // Update scalar properties only (not navigation properties)
      var scalarProperties = typeof(Project).GetProperties()
        .Where(isScalarEntityProperty);
      foreach (var prop in scalarProperties)
      {
        var value = prop.GetValue(project);
        prop.SetValue(existingProject, value);
      }

      // Synchronize columns
      if (project.ProjectColumns == null)
      {
        // Remove all columns if incoming collection is null
        existingProject.ProjectColumns.Clear();
      }
      else
      {
        // Remove columns not present in incoming project
        var columnsToRemove = existingProject.ProjectColumns.Where(ec => !project.ProjectColumns.Any(pc => pc.Uid == ec.Uid)).ToList();
        foreach (var col in columnsToRemove)
        {
          existingProject.ProjectColumns.Remove(col);
        }

        // Add new columns from incoming project
        var columnsToAdd = project.ProjectColumns.Where(pc => !existingProject.ProjectColumns.Any(ec => ec.Uid == pc.Uid)).ToList();
        foreach (var col in columnsToAdd)
        {
          existingProject.ProjectColumns.Add(col);
        }

        foreach (var column in project.ProjectColumns)
        {
          var existingColumn = existingProject.ProjectColumns.FirstOrDefault(c => c.Uid == column.Uid);
          if (existingColumn != null)
          {
            // Update scalar properties for column
            var colScalarProps = typeof(ProjectColumn).GetProperties()
              .Where(isScalarEntityProperty);
            foreach (var prop in colScalarProps)
            {
              var value = prop.GetValue(column);
              prop.SetValue(existingColumn, value);
            }

            // Synchronize work items
            if (column.WorkItems == null)
            {
              // Remove all work items if incoming collection is null
              existingColumn.WorkItems.Clear();
            }
            else
            {
              // Remove work items not present in incoming column
              var workItemsToRemove = existingColumn.WorkItems.Where(ew => !column.WorkItems.Any(w => w.Uid == ew.Uid)).ToList();
              foreach (var wi in workItemsToRemove)
              {
                existingColumn.WorkItems.Remove(wi);
              }

              // Add new work items from incoming column
              var workItemsToAdd = column.WorkItems.Where(w => !existingColumn.WorkItems.Any(ew => ew.Uid == w.Uid)).ToList();
              foreach (var wi in workItemsToAdd)
              {
                existingColumn.WorkItems.Add(wi);
              }

              foreach (var workItem in column.WorkItems)
              {
                var existingWorkItem = existingColumn.WorkItems.FirstOrDefault(w => w.Uid == workItem.Uid);
                if (existingWorkItem != null)
                {
                  // Update scalar properties for work item
                  var wiScalarProps = typeof(WorkItem).GetProperties()
                    .Where(isScalarEntityProperty);
                  foreach (var prop in wiScalarProps)
                  {
                    var value = prop.GetValue(workItem);
                    prop.SetValue(existingWorkItem, value);
                  }

                  // Synchronize tags (many-to-many)
                  var incomingTags = workItem.WorkItemTags ?? new List<WorkItemTag>();
                  var tagsToAdd = incomingTags.Where(t => !existingWorkItem.WorkItemTags.Any(et => et.Uid.Equals(t.Uid))).ToList();
                  var tagsToRemove = existingWorkItem.WorkItemTags.Where(et => !incomingTags.Any(t => t.Uid.Equals(et.Uid))).ToList();

                  foreach (var tag in tagsToAdd)
                  {
                    if (tag.Id <= 0)
                      tag.UserId = project.UserId;
                    existingWorkItem.WorkItemTags.Add(tag);
                  }

                  foreach (var tag in tagsToRemove)
                  {
                    existingWorkItem.WorkItemTags.Remove(tag);
                  }
                }
              }
            }
          }
        }
      }

      _context.SaveChanges();
      return true;
    }

    public bool Delete(Guid uid)
    {
      var project = GetByUid(uid);
      if (project == null) return false;
      _context.Projects.Remove(project);
      _context.SaveChanges();
      return true;
    }

    private bool isScalarEntityProperty(PropertyInfo prop)
    {
      return (
        !typeof(System.Collections.IEnumerable).IsAssignableFrom(prop.PropertyType)
        || prop.PropertyType == typeof(string)
      ) && (
        prop.PropertyType != typeof(Project)
        && prop.PropertyType != typeof(ProjectColumn)
        && prop.PropertyType != typeof(WorkItem)
        && prop.PropertyType != typeof(WorkItemTag)
        && prop.PropertyType != typeof(WorkItemTagRef)
      );
    }

    // Equality comparer for WorkItemTag based on Uid
    private class WorkItemTagUidEqualityComparer : IEqualityComparer<WorkItemTag>
    {
      public bool Equals(WorkItemTag? x, WorkItemTag? y)
      {
        if (ReferenceEquals(x, y)) return true;
        if (x is null || y is null) return false;
        return x.Uid.Equals(y.Uid);
      }
  
      public int GetHashCode(WorkItemTag obj)
      {
        return obj.Uid.GetHashCode();
      }
    }
  }
}
