using Vueboard.DataAccess.Models;

namespace Vueboard.DataAccess.Repositories
{
  public class InMemoryWorkItemTagRepository : IWorkItemTagRepository
  {
    private readonly List<WorkItemTag> _tags = new();

    public IEnumerable<WorkItemTag> GetAll()
    {
      return _tags;
    }

    public WorkItemTag Create(WorkItemTag tag)
    {
      if (!_tags.Any(x => x.TagText == tag.TagText))
      {
        tag.Id = _nextTagId++;
        _tags.Add(tag);
      }

      return tag;
    }

    public bool Update(WorkItemTag tag)
    {
      var existing = _tags.FirstOrDefault(t => t.Id == tag.Id);
      if (existing == null) return false;
      existing.TagText = tag.TagText;
      return true;
    }

    public bool Delete(int id)
    {
      var tag = _tags.FirstOrDefault(t => t.Id == id);
      if (tag == null) return false;
      _tags.Remove(tag);
      return true;
    }

    private int _nextTagId = 1;
  }
}