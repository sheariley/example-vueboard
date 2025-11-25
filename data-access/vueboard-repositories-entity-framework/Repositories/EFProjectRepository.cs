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

      var allWorkItemTags = _context.WorkItemTags.ToList();

      if (existingProject == null)
        return false;

      // Update scalar properties only (not navigation properties)
      existingProject.UpdateScalarProperties(project);

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
        var columnsToRemove = existingProject.ProjectColumns.Where(ec => !ec.IsDeleted && !project.ProjectColumns.Any(pc => pc.Uid == ec.Uid)).ToList();
        foreach (var col in columnsToRemove)
        {
          _projectColumnRepo.Delete(col);
        }

        // Add new columns from incoming project
        var columnsToAdd = project.ProjectColumns.Where(pc => !existingProject.ProjectColumns.Any(ec => ec.Uid == pc.Uid)).ToList();
        foreach (var col in columnsToAdd)
        {
          // Create a new column object without copying WorkItems so
          // we don't attach them yet (happens later).
          var newCol = new ProjectColumn();
          newCol.UpdateScalarProperties(col);
          existingProject.ProjectColumns.Add(newCol);
        }

        foreach (var column in project.ProjectColumns)
        {
          var existingColumn = existingProject.ProjectColumns.FirstOrDefault(c => c.Uid == column.Uid);
          if (existingColumn != null)
          {
            // Update scalar properties for column
            existingColumn.UpdateScalarProperties(column);
            
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
              var workItemsToRemove = existingColumn.WorkItems.Where(ew => !ew.IsDeleted && !column.WorkItems.Any(w => w.Uid == ew.Uid)).ToList();
              foreach (var wi in workItemsToRemove)
              {
                existingColumn.WorkItems.Remove(wi);
                // If the WorkItem is no longer attached to any columns, soft-delete it in the DB.
                if (!project.ProjectColumns.Any(col => col.WorkItems?.Any(w => w.Uid == wi.Uid) ?? false))
                  _workItemRepo.Delete(wi);
              }

              // Add new work items from incoming column
              var workItemsToAdd = column.WorkItems.Where(w => !existingColumn.WorkItems.Any(ew => ew.Uid == w.Uid)).ToList();
              foreach (var wi in workItemsToAdd)
              {
                if (wi.Id <= 0)
                {
                  existingColumn.WorkItems.Add(wi);
                }
                else
                {
                  // pull existingWorkItem from local entities, because it may not exist
                  // in the incoming ProjectColumn, as the user may have moved it to a diff column.
                  // This avoids an attempt to attach duplicate entities to the context.
                  var existingWorkItem = _context.Entries<WorkItem>()
                    .FirstOrDefault(entry => entry.Entity.Uid == wi.Uid)?
                    .Entity;
                  if (existingWorkItem != null)
                  {
                    existingWorkItem.UpdateScalarProperties(wi);
                    _context.SetEntityState(existingWorkItem, EntityState.Modified);
                    // make sure we don't remove tags just because we removed the work item from a column
                    // as a result of moving it to another column
                    var tagRefs = _context.Entries<WorkItemTagRef>()
                      .Where(r => r.Entity.WorkItemId == existingWorkItem.Id);
                    foreach (var tagRef in tagRefs)
                    {
                      _context.SetEntityState(tagRef.Entity, EntityState.Unchanged);
                    }
                  }
                  existingColumn.WorkItems.Add(existingWorkItem ?? wi);
                }
              }

              foreach (var workItem in column.WorkItems)
              {
                var existingWorkItem = existingColumn.WorkItems.FirstOrDefault(w => w.Uid == workItem.Uid);
                if (existingWorkItem != null)
                {
                  // Update scalar properties for work item
                  existingWorkItem.UpdateScalarProperties(workItem);

                  // Synchronize tags (many-to-many)
                  var incomingTags = workItem.WorkItemTags ?? new List<WorkItemTag>();
                  var tagsToAdd = incomingTags.Where(t => !existingWorkItem.WorkItemTags.Any(et => et.Uid.Equals(t.Uid)))
                    .Select(t => t.Id <= 0 ? t : allWorkItemTags.First(et => et.Uid.Equals(t.Uid)))
                    .ToList();
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

    protected override void AfterDelete(Project entity)
    {
      foreach (var col in entity.ProjectColumns ?? [])
      {
        _projectColumnRepo.Delete(col);
      }
    }
  }
}
