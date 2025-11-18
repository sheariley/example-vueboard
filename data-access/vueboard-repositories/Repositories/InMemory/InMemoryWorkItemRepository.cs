using Vueboard.DataAccess.Models;
using Vueboard.DataAccess.Repositories;

namespace Vueboard.DataAccess.Repositories.InMemory
{
  public class InMemoryWorkItemRepository : IWorkItemRepository
  {
    private readonly List<WorkItem> _workItems = new();
    private readonly List<(int workItemId, int tagId)> _workItemTagRefs = new();
    private readonly List<WorkItemTag> _tags = new();
    private int _nextTagId = 1;
    private int _nextWorkItemId = 1;

    public IEnumerable<WorkItem> GetAllForProjectColumn(int projectColumnId)
    {
      return _workItems.Where(w => w.ProjectColumnId == projectColumnId && !w.IsDeleted);
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
      SaveWorkItem(item, userId);
      _workItems.Add(item);
      return item;
    }

    public bool UpdateWorkItem(WorkItem item, Guid userId)
    {
      var existing = _workItems.FirstOrDefault(w => w.Id == item.Id);
      if (existing == null) return false;
      item.Updated = DateTime.UtcNow;
      SaveWorkItem(item, userId);
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

    public List<string> GetTagsForWorkItem(int workItemId)
    {
      var tagIds = _workItemTagRefs.Where(r => r.workItemId == workItemId).Select(r => r.tagId);
      return _tags.Where(t => tagIds.Contains(t.Id)).Select(t => t.TagText).ToList();
    }
  }
}