namespace Vueboard.Api.GraphQL
{
  public interface IProjectRepository
  {
    IEnumerable<Project> GetAll();
    Project? GetByUid(string uid);
    Project Add(Project project);
    bool Update(Project project);
    bool Delete(string uid);
  }

  public interface IProjectColumnRepository
  {
    IEnumerable<ProjectColumn> GetAllForProject(int projectId);
    ProjectColumn? GetById(int id);
    ProjectColumn Add(int projectId, ProjectColumn column);
    bool Update(ProjectColumn column);
    bool Delete(int id);
  }

  public class InMemoryProjectRepository : IProjectRepository
  {
    private readonly List<Project> _projects = new();

    public IEnumerable<Project> GetAll() => _projects;

    public Project? GetByUid(string uid) => _projects.FirstOrDefault(p => p.Uid.Equals(Guid.Parse(uid)));

    public Project Add(Project project)
    {
      _projects.Add(project);
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
      return true;
    }

    public bool Delete(string uid)
    {
      var project = _projects.FirstOrDefault(p => p.Uid.Equals(Guid.Parse(uid)));
      if (project == null) return false;
      project.IsDeleted = true;
      return true;
    }
  }

  public class InMemoryProjectColumnRepository : IProjectColumnRepository
  {
    private readonly List<ProjectColumn> _columns = new();
    private int _nextColumnId = 1;

    public IEnumerable<ProjectColumn> GetAllForProject(int projectId)
    {
      return _columns.Where(c => c.ProjectId == projectId && !c.IsDeleted);
    }

    public ProjectColumn? GetById(int id)
    {
      return _columns.FirstOrDefault(c => c.Id == id && !c.IsDeleted);
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

  public interface IWorkItemRepository
  {
    WorkItem CreateWorkItem(WorkItem item, Guid userId);
    bool UpdateWorkItem(WorkItem item, Guid userId);
    bool DeleteWorkItem(int workItemId);
    WorkItem? GetWorkItem(int workItemId);
    List<WorkItem> GetWorkItemsForColumn(int projectColumnId);
    List<string> GetTagsForWorkItem(int workItemId);
  }

  public class InMemoryWorkItemRepository : IWorkItemRepository
  {
    private readonly List<WorkItem> _workItems = new();
    private readonly List<(int workItemId, int tagId)> _workItemTagRefs = new();
    private readonly List<WorkItemTag> _tags = new();
    private int _nextTagId = 1;
    private int _nextWorkItemId = 1;

    // Create a new work item
    public WorkItem CreateWorkItem(WorkItem item, Guid userId)
    {
      item.Id = _nextWorkItemId++;
      item.Uid = Guid.NewGuid();
      item.Created = DateTime.UtcNow;
      item.Updated = DateTime.UtcNow;
      SaveWorkItem(item, userId);
      _workItems.Add(item);
      return item;
    }

    // Update an existing work item
    public bool UpdateWorkItem(WorkItem item, Guid userId)
    {
      var existing = _workItems.FirstOrDefault(w => w.Id == item.Id);
      if (existing == null) return false;
      item.Updated = DateTime.UtcNow;
      SaveWorkItem(item, userId);
      // Update fields
      existing.Title = item.Title;
      existing.Description = item.Description;
      existing.Notes = item.Notes;
      existing.FgColor = item.FgColor;
      existing.BgColor = item.BgColor;
      existing.Index = item.Index;
      existing.ProjectColumnId = item.ProjectColumnId;
      existing.IsDeleted = item.IsDeleted;
      existing.Tags = item.Tags;
      return true;
    }

    // Delete (soft delete) a work item
    public bool DeleteWorkItem(int workItemId)
    {
      var item = _workItems.FirstOrDefault(w => w.Id == workItemId);
      if (item == null) return false;
      item.IsDeleted = true;
      return true;
    }

    // Get a work item by id
    public WorkItem? GetWorkItem(int workItemId)
    {
      return _workItems.FirstOrDefault(w => w.Id == workItemId && !w.IsDeleted);
    }

    // Get all work items for a column
    public List<WorkItem> GetWorkItemsForColumn(int projectColumnId)
    {
      return _workItems.Where(w => w.ProjectColumnId == projectColumnId && !w.IsDeleted).ToList();
    }

    // Save or update a work item, associating tags by string
    public void SaveWorkItem(WorkItem item, Guid userId)
    {
      _workItemTagRefs.RemoveAll(r => r.workItemId == item.Id);
      foreach (var tag in item.Tags.Distinct())
      {
        var existingTag = _tags.FirstOrDefault(t => t.TagText == tag && t.UserId == userId);
        if (existingTag == null)
        {
          existingTag = new WorkItemTag { Id = _nextTagId++, Uid = Guid.NewGuid(), TagText = tag, UserId = userId };
          _tags.Add(existingTag);
        }
        _workItemTagRefs.Add((item.Id, existingTag.Id));
      }
    }

    // Retrieve tags for a work item as strings
    public List<string> GetTagsForWorkItem(int workItemId)
    {
      var tagIds = _workItemTagRefs.Where(r => r.workItemId == workItemId).Select(r => r.tagId);
      return _tags.Where(t => tagIds.Contains(t.Id)).Select(t => t.TagText).ToList();
    }
  }

  public class WorkItemTag
  {
    public int Id { get; set; }
    public Guid Uid { get; set; }
    public string TagText { get; set; } = string.Empty;
    public Guid UserId { get; set; }
  }
}
