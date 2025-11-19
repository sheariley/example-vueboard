using Vueboard.DataAccess.Models;

namespace Vueboard.DataAccess.Repositories.InMemory
{
  public class InMemoryProjectColumnRepository : IProjectColumnRepository
  {
    private readonly List<ProjectColumn> _columns = new();
    private int _nextColumnId = 1;

    public IEnumerable<ProjectColumn> GetAllForProject(int projectId)
    {
      return _columns.Where(c => c.ProjectId == projectId && !c.IsDeleted);
    }

    public IEnumerable<ProjectColumn> GetAllForProjects(IEnumerable<int> projectIds)
    {
      return _columns.Where(c => projectIds.Contains(c.ProjectId) && !c.IsDeleted);
    }

    public ProjectColumn? GetById(int id)
    {
      return _columns.FirstOrDefault(c => c.Id == id && !c.IsDeleted);
    }

    public ProjectColumn? GetByUid(Guid uid)
    {
      return _columns.FirstOrDefault(c => c.Uid == uid && !c.IsDeleted);
    }

    public ProjectColumn Add(int projectId, ProjectColumn column)
    {
      column.Id = _nextColumnId++;
      column.ProjectId = projectId;
      column.Created = DateTime.UtcNow;
      column.Updated = DateTime.UtcNow;
      column.IsDeleted = false;
      _columns.Add(column);
      return column;
    }

    public bool Update(ProjectColumn column)
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

    public bool Delete(int id)
    {
      var column = _columns.FirstOrDefault(c => c.Id == id);
      if (column == null) return false;
      column.IsDeleted = true;
      return true;
    }
  }
}