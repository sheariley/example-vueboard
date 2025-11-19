using System.Linq.Expressions;
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

    public IEnumerable<Project> Get(FetchSpecification<Project> spec) => Get(x => x, spec);

    public IEnumerable<Project> Get(Expression<Func<Project, bool>> predicate) => Get(new FetchSpecification<Project> { Criteria = predicate });

    public IEnumerable<TOut> Get<TOut>(Expression<Func<Project, TOut>> selector, FetchSpecification<Project>? specification = null)
    {
      var query = _projects.Where(p => !p.IsDeleted).AsQueryable();
      
      if (specification != null)
      {
        // Apply Criteria
        if (specification.Criteria != null)
        {
          query = query.Where(specification.Criteria);
        }

        // Apply Includes (simulate by loading navigation properties if needed)
        if (specification.Includes != null)
        {
          foreach (var include in specification.Includes)
          {
            // TODO: Figure this out for InMemory implementations
            // In-memory, navigation properties are already loaded if present.
            // No action needed unless lazy loading is implemented.
          }
        }

        // Apply OrderBy
        if (specification.OrderBy != null)
        {
          query = specification.OrderBy(query);
        }
      }

      return query.Select(selector).ToList();
    }

    

    public Project? GetByUid(string uid)
    {
      var project = _projects.FirstOrDefault(p => p.Uid.Equals(Guid.Parse(uid)) && !p.IsDeleted);
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
      var existingProj = _projects.FirstOrDefault(p => p.Uid == project.Uid);
      if (existingProj == null) return false;
      existingProj.Title = project.Title;
      existingProj.Description = project.Description;
      existingProj.DefaultCardFgColor = project.DefaultCardFgColor;
      existingProj.DefaultCardBgColor = project.DefaultCardBgColor;
      existingProj.Updated = DateTime.UtcNow;
      if (project.Columns != null)
      {
        foreach (var column in project.Columns)
        {
          if (column.Id == 0)
          {
            _columnRepo.Add(existingProj.Id, column);
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
                _workItemRepo.CreateWorkItem(workItem, existingProj.UserId);
              }
              else
              {
                _workItemRepo.UpdateWorkItem(workItem, existingProj.UserId);
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
