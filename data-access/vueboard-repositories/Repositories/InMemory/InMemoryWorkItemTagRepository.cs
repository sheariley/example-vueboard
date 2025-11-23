using Vueboard.DataAccess.Models;

namespace Vueboard.DataAccess.Repositories
{
  public class InMemoryWorkItemTagRepository : GenericRepository<WorkItemTag>, IWorkItemTagRepository
  {
    private int _nextTagId = 1;
    private readonly List<WorkItemTag> _tags = new();

    protected override IQueryable<WorkItemTag> GetQueryRoot()
    {
      return _tags.AsQueryable();
    }

    public override WorkItemTag Create(WorkItemTag tag)
    {
      if (!_tags.Any(x => x.TagText == tag.TagText))
      {
        tag.Id = _nextTagId++;
        _tags.Add(tag);
      }

      return tag;
    }

    public override bool Update(WorkItemTag tag)
    {
      var existing = _tags.FirstOrDefault(t => t.Id == tag.Id);
      if (existing == null) return false;
      existing.TagText = tag.TagText;
      return true;
    }

    public override bool Delete(WorkItemTag? entity)
    {
      if (entity == null) return false;
      _tags.Remove(entity);
      return true;
    }

    public override void CommitChanges()
    {
      // DO NOTHING
    }
  }
}