using Vueboard.DataAccess.Models;

namespace Vueboard.DataAccess.Repositories.EntityFramework
{
  public class EFWorkItemTagRepository : EFGenericRepository<WorkItemTag>, IWorkItemTagRepository
  {
    private List<WorkItemTag>? _localCache = null;
    public EFWorkItemTagRepository(IVueboardDbContext context)
      : base(context)
    {
    }

    // override to add duplicate-checking logic
    public override WorkItemTag Create(WorkItemTag tag)
    {
      if (_localCache == null)
        hydrateLocalCache();

      if (!_localCache!.Any(x => x.TagText == tag.TagText))
      {
        GetDbSet().Add(tag);
        _localCache!.Add(tag);
      }
      return tag;
    }

    private void hydrateLocalCache()
    {
      _localCache = GetQueryRoot().ToList();
    }
  }
}