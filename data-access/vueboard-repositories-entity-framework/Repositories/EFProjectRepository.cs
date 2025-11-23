using Microsoft.EntityFrameworkCore;
using Vueboard.DataAccess.Models;

namespace Vueboard.DataAccess.Repositories.EntityFramework
{
  public class EFProjectRepository : EFGenericSoftDeleteRepository<Project>, IProjectRepository
  {
    private IProjectColumnRepository _projectColumnRepo;
    private IWorkItemRepository _workItemRepo;
    
    public EFProjectRepository(
      IVueboardDbContext context,
      IProjectColumnRepository projectColumnRepo,
      IWorkItemRepository workItemRepo
    )
      : base(context)
    {
      _projectColumnRepo = projectColumnRepo;
      _workItemRepo = workItemRepo;
    }

    public override Project Create(Project project)
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

      return base.Create(project);
    }

    public override bool Update(Project project)
    {
      var existingProject = GetDbSet()
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
        foreach (var col in existingProject.ProjectColumns)
        {
          _projectColumnRepo.Delete(col);
        }
      }
      else
      {
        // Remove columns not present in incoming project
        var columnsToRemove = existingProject.ProjectColumns.Where(ec => !project.ProjectColumns.Any(pc => pc.Uid == ec.Uid)).ToList();
        foreach (var col in columnsToRemove)
        {
          _projectColumnRepo.Delete(col);
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
              foreach (var workItem in existingColumn.WorkItems)
              {
                _workItemRepo.Delete(workItem);
              }
            }
            else
            {
              // Remove work items not present in incoming column
              var workItemsToRemove = existingColumn.WorkItems.Where(ew => !column.WorkItems.Any(w => w.Uid == ew.Uid)).ToList();
              foreach (var wi in workItemsToRemove)
              {
                _workItemRepo.Delete(wi);
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
      return true;
    }

    protected override IEnumerable<IVueboardSoftDeleteEntity> GetNestedSoftDeleteEntities(Project entity)
    {
      var nestedEntities = new List<IVueboardSoftDeleteEntity>();
      foreach (var col in entity.ProjectColumns ?? [])
      {
        nestedEntities.Add(col);
        foreach (var workItem in col.WorkItems ?? [])
        {
          nestedEntities.Add(workItem);
        }
      }
      return nestedEntities;
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
