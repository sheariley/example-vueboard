using System.Linq.Expressions;
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
      var projectWorkItemTags = project.ProjectColumns?.SelectMany(
        c => c.WorkItems?.SelectMany(w => w.WorkItemTags) ?? Enumerable.Empty<WorkItemTag>()
      ).Distinct(new WorkItemTagUidEqualityComparer());

      // Prevent EF from updating existing WorkItemTag entities
      foreach (var tag in projectWorkItemTags ?? Enumerable.Empty<WorkItemTag>())
      {
        // If tag exists, mark as unchanged
        if (tag.Id > 0)
        {
          _context.Attach(tag);
          _context.SetEntityState(tag, EntityState.Unchanged);
        }
      }

      _context.Projects.Update(project);
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
