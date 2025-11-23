using Vueboard.DataAccess.Models;

namespace Vueboard.DataAccess.Repositories.InMemory
{
  public class InMemoryWorkItemRepository : GenericRepository<WorkItem>, IWorkItemRepository
  {
    private readonly IWorkItemTagRepository _tagRepo;
    private readonly List<WorkItem> _workItems = new();
    private int _nextWorkItemId = 1;

    public InMemoryWorkItemRepository(IWorkItemTagRepository tagRepo)
    {
      _tagRepo = tagRepo;
    }

    protected override IQueryable<WorkItem> GetQueryRoot()
    {
      return _workItems.AsQueryable().Where(x => !x.IsDeleted);
    }
    
    public IEnumerable<WorkItem> GetAllForProjectColumn(int projectColumnId)
    {
      return GetQueryRoot().Where(w => w.ProjectColumnId == projectColumnId);
    }

    public IEnumerable<WorkItem> GetAllForProjectColumns(IEnumerable<int> projectColumnIds)
    {
      return GetQueryRoot().Where(w => projectColumnIds.Contains(w.ProjectColumnId));
    }

    public override WorkItem Create(WorkItem item)
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

    public override bool Update(WorkItem item)
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

    public override bool Delete(WorkItem? entity)
    {
      if (entity == null) return false;
      entity.IsDeleted = true;
      return true;
    }

    public List<WorkItemTag> GetTagsForWorkItem(int workItemId)
    {
      return GetQueryRoot().First(x => x.Id == workItemId).WorkItemTags;
    }

    public override void CommitChanges()
    {
      // DO NOTHING
    }
  }
}