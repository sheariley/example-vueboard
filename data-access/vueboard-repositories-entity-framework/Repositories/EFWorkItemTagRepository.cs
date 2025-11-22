using Vueboard.DataAccess.Models;

namespace Vueboard.DataAccess.Repositories.EntityFramework
{
  public class EFWorkItemTagRepository : IWorkItemTagRepository
  {
    private readonly IVueboardDbContext _context;

    public EFWorkItemTagRepository(IVueboardDbContext context)
    {
      _context = context;
    }

    public IEnumerable<WorkItemTag> GetAll()
    {
      return _context.WorkItemTags;
    }

    public WorkItemTag Create(WorkItemTag tag)
    {
      if (!_context.WorkItemTags.Any(x => x.TagText == tag.TagText))
      {
        _context.WorkItemTags.Add(tag);
        _context.SaveChanges();
      }
      return tag;
    }

    public bool Update(WorkItemTag tag)
    {
      _context.WorkItemTags.Update(tag);
      _context.SaveChanges();
      return true;
    }

    public bool Delete(int id)
    {
      var tag = _context.WorkItemTags.FirstOrDefault(t => t.Id == id);
      if (tag == null) return false;
      _context.WorkItemTags.Remove(tag);
      _context.SaveChanges();
      return true;
    }
  }
}