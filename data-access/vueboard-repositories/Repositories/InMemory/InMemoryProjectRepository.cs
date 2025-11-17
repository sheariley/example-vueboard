using Vueboard.DataAccess.Models;
using Vueboard.DataAccess.Repositories;

namespace Vueboard.DataAccess.Repositories.InMemory
{
  public class InMemoryProjectRepository : IProjectRepository
  {
    private readonly List<Project> _projects = new();
    private readonly IProjectColumnRepository _columnRepo;
    private readonly IWorkItemRepository _workItemRepo;

    public InMemoryProjectRepository(IProjectColumnRepository columnRepo, IWorkItemRepository workItemRepo)
    {
      _columnRepo = columnRepo;
      _workItemRepo = workItemRepo;
    }

    public IEnumerable<Project> GetAll()
    {
      foreach (var project in _projects.Where(p => !p.IsDeleted))
      {
        project.Columns = _columnRepo.GetAllForProject(project.Id).ToList();
        foreach (var column in project.Columns)
        {
          column.WorkItems = _workItemRepo.GetWorkItemsForColumn(column.Id);
        }
      }
      return _projects.Where(p => !p.IsDeleted);
    }

    public Project? GetByUid(string uid)
    {
      var project = _projects.FirstOrDefault(p => p.Uid.Equals(Guid.Parse(uid)) && !p.IsDeleted);
      if (project != null)
      {
        project.Columns = _columnRepo.GetAllForProject(project.Id).ToList();
        foreach (var column in project.Columns)
        {
          column.WorkItems = _workItemRepo.GetWorkItemsForColumn(column.Id);
        }
      }
      return project;
    }

    public Project Add(Project project)
    {
      _projects.Add(project);
      if (project.Columns != null)
      {
        foreach (var column in project.Columns)
        {
          _columnRepo.Add(project.Id, column);
          if (column.WorkItems != null)
          {
            foreach (var workItem in column.WorkItems)
            {
              _workItemRepo.CreateWorkItem(workItem, project.UserId);
            }
          }
        }
      }
      return project;
    }

    public bool Update(Project project)
    {
      var existing = _projects.FirstOrDefault(p => p.Uid == project.Uid);
      if (existing == null) return false;
      existing.Title = project.Title;
      existing.Description = project.Description;
      existing.DefaultCardFgColor = project.DefaultCardFgColor;
      existing.DefaultCardBgColor = project.DefaultCardBgColor;
      existing.Updated = DateTime.UtcNow;
      if (project.Columns != null)
      {
        foreach (var column in project.Columns)
        {
          if (column.Id == 0)
          {
            _columnRepo.Add(existing.Id, column);
          }
          else
          {
            _columnRepo.Update(column);
          }
          if (column.WorkItems != null)
          {
            foreach (var workItem in column.WorkItems)
            {
              if (workItem.Id == 0)
              {
                _workItemRepo.CreateWorkItem(workItem, existing.UserId);
              }
              else
              {
                _workItemRepo.UpdateWorkItem(workItem, existing.UserId);
              }
            }
          }
        }
      }
      return true;
    }

    public bool Delete(string uid)
    {
      var project = _projects.FirstOrDefault(p => p.Uid.Equals(Guid.Parse(uid)));
      if (project == null) return false;
      project.IsDeleted = true;
      if (project.Columns != null)
      {
        foreach (var column in project.Columns)
        {
          _columnRepo.Delete(column.Id);
          if (column.WorkItems != null)
          {
            foreach (var workItem in column.WorkItems)
            {
              _workItemRepo.DeleteWorkItem(workItem.Id);
            }
          }
        }
      }
      return true;
    }
  }
}
