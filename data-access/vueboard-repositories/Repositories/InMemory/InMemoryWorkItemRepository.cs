using Vueboard.DataAccess.Models;

namespace Vueboard.DataAccess.Repositories.InMemory
{
  public class InMemoryWorkItemRepository : IWorkItemRepository
  {
    private readonly IWorkItemTagRepository _tagRepo;
    private readonly List<WorkItem> _workItems = new();
    private int _nextWorkItemId = 1;

    public InMemoryWorkItemRepository(IWorkItemTagRepository tagRepo)
    {
      _tagRepo = tagRepo;
    }

    public IEnumerable<WorkItem> GetAllForProjectColumn(int projectColumnId)
    {
      return _workItems.Where(w => w.ProjectColumnId == projectColumnId && !w.IsDeleted);
    }

    public IEnumerable<WorkItem> GetAllForProjectColumns(IEnumerable<int> projectColumnIds)
    {
      return _workItems.Where(w => projectColumnIds.Contains(w.ProjectColumnId) && !w.IsDeleted);
    }

    public WorkItem? GetById(int id)
    {
      return _workItems.FirstOrDefault(w => w.Id == id && !w.IsDeleted);
    }

    public WorkItem? GetByUid(Guid uid)
    {
      return _workItems.FirstOrDefault(w => w.Uid == uid && !w.IsDeleted);
    }

    public WorkItem CreateWorkItem(WorkItem item, Guid userId)
    {
      item.Id = _nextWorkItemId++;
      item.Uid = Guid.NewGuid();
      item.Created = DateTime.UtcNow;
      item.Updated = DateTime.UtcNow;
      if (item.WorkItemTags?.Count > 0)
      {
        item.WorkItemTags.ForEach(x => _tagRepo.Create(x));
      }
      _workItems.Add(item);
      return item;
    }

    public bool UpdateWorkItem(WorkItem item, Guid userId)
    {
      var existing = _workItems.FirstOrDefault(w => w.Id == item.Id);
      if (existing == null) return false;
      item.Updated = DateTime.UtcNow;
      existing.Title = item.Title;
      existing.Description = item.Description;
      existing.Notes = item.Notes;
      existing.FgColor = item.FgColor;
      existing.BgColor = item.BgColor;
      existing.Index = item.Index;
      existing.ProjectColumnId = item.ProjectColumnId;
      existing.IsDeleted = item.IsDeleted;
      existing.WorkItemTags = item.WorkItemTags;
      if (existing.WorkItemTags?.Count > 0)
      {
        existing.WorkItemTags.ForEach(x => _tagRepo.Create(x));
      }
      return true;
    }

    public bool DeleteWorkItem(int workItemId)
    {
      var item = _workItems.FirstOrDefault(w => w.Id == workItemId);
      if (item == null) return false;
      item.IsDeleted = true;
      return true;
    }

    public WorkItem? GetWorkItem(int workItemId)
    {
      return _workItems.FirstOrDefault(w => w.Id == workItemId && !w.IsDeleted);
    }

    public List<WorkItem> GetWorkItemsForColumn(int projectColumnId)
    {
      return _workItems.Where(w => w.ProjectColumnId == projectColumnId && !w.IsDeleted).ToList();
    }

    public List<WorkItemTag> GetTagsForWorkItem(int workItemId)
    {
      return _workItems.First(x => x.Id == workItemId).WorkItemTags;
    }
  }
}