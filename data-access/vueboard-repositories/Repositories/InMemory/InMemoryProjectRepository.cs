using System.Linq.Expressions;
using Vueboard.DataAccess.Models;

namespace Vueboard.DataAccess.Repositories.InMemory
{
  public class InMemoryProjectRepository : GenericSoftDeleteRepository<Project>, IProjectRepository
  {
    private readonly List<Project> _projects = new();
    private readonly IProjectColumnRepository _columnRepo;
    private readonly IWorkItemRepository _workItemRepo;

    public InMemoryProjectRepository(IProjectColumnRepository columnRepo, IWorkItemRepository workItemRepo)
    {
      _columnRepo = columnRepo;
      _workItemRepo = workItemRepo;
    }

    protected override IQueryable<Project> GetRawQueryRoot()
    {
      return _projects.AsQueryable();
    }

    public override Project Create(Project project)
    {
      _projects.Add(project);
      if (project.ProjectColumns != null)
      {
        foreach (var column in project.ProjectColumns)
        {
          _columnRepo.Create(column);
          if (column.WorkItems != null)
          {
            foreach (var workItem in column.WorkItems)
            {
              _workItemRepo.Create(workItem);
            }
          }
        }
      }
      return project;
    }

    public override bool Update(Project project)
    {
      var existingProj = _projects.FirstOrDefault(p => p.Uid == project.Uid);
      if (existingProj == null) return false;
      existingProj.Title = project.Title;
      existingProj.Description = project.Description;
      existingProj.DefaultCardFgColor = project.DefaultCardFgColor;
      existingProj.DefaultCardBgColor = project.DefaultCardBgColor;
      existingProj.Updated = DateTime.UtcNow;
      if (project.ProjectColumns != null)
      {
        foreach (var column in project.ProjectColumns)
        {
          if (column.Id == 0)
          {
            column.ProjectId = project.Id;
            _columnRepo.Create(column);
          }
          else
          {
            _columnRepo.Update(column);
          }
          if (column.WorkItems != null)
          {
            foreach (var workItem in column.WorkItems)
            {
              workItem.ProjectColumnId = column.Id;
              if (workItem.Id == 0)
              {
                _workItemRepo.Create(workItem);
              }
              else
              {
                _workItemRepo.Update(workItem);
              }
            }
          }
        }
      }
      return true;
    }

    public override bool Delete(Project? project)
    {
      if (project == null) return false;
      project.IsDeleted = true;
      if (project.ProjectColumns != null)
      {
        foreach (var column in project.ProjectColumns)
        {
          _columnRepo.Delete(column.Id);
          if (column.WorkItems != null)
          {
            foreach (var workItem in column.WorkItems)
            {
              _workItemRepo.Delete(workItem.Id);
            }
          }
        }
      }
      return true;
    }

    public override void CommitChanges()
    {
      // DO NOTHING
    }
  }
}
