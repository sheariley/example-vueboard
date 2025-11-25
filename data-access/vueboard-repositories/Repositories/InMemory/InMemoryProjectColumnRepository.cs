using Vueboard.DataAccess.Models;

namespace Vueboard.DataAccess.Repositories.InMemory
{
  public class InMemoryProjectColumnRepository : GenericRepository<ProjectColumn>, IProjectColumnRepository
  {
    private readonly List<ProjectColumn> _columns = new();
    private int _nextColumnId = 1;

    protected override IQueryable<ProjectColumn> GetQueryRoot()
    {
      return _columns.AsQueryable().Where(x => !x.IsDeleted);
    }

    public IEnumerable<ProjectColumn> GetAllForProject(int projectId)
    {
      return GetAllForProjects([projectId]);
    }

    public IEnumerable<ProjectColumn> GetAllForProjects(IEnumerable<int> projectIds)
    {
      return GetQueryRoot().Where(c => c.ProjectId.HasValue && projectIds.Contains(c.ProjectId.Value));
    }

    public override ProjectColumn Create(ProjectColumn column)
    {
      column.Id = _nextColumnId++;
      column.Created = DateTime.UtcNow;
      column.Updated = DateTime.UtcNow;
      column.IsDeleted = false;
      _columns.Add(column);
      return column;
    }

    public override bool Update(ProjectColumn column)
    {
      var existing = _columns.FirstOrDefault(c => c.Id == column.Id);
      if (existing == null) return false;
      existing.Name = column.Name;
      existing.IsDefault = column.IsDefault;
      existing.Index = column.Index;
      existing.FgColor = column.FgColor;
      existing.BgColor = column.BgColor;
      existing.Updated = DateTime.UtcNow;
      return true;
    }
    
    public override bool Delete(ProjectColumn? entity)
    {
      if (entity == null) return false;
      entity.IsDeleted = true;
      return true;
    }

    public override void CommitChanges()
    {
      // DO NOTHING
    }
  }
}