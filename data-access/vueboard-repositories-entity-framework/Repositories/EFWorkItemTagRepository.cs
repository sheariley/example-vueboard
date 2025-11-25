using Vueboard.DataAccess.Models;

namespace Vueboard.DataAccess.Repositories.EntityFramework
{
  public class EFWorkItemTagRepository(
    IVueboardDbContext context
  ) : EFGenericRepository<WorkItemTag>(context), IWorkItemTagRepository
  {
    private List<WorkItemTag>? _localCache = null;

    // override to add duplicate-checking logic
    public override WorkItemTag Create(WorkItemTag tag)
    {
      if (_localCache == null)
        HydrateLocalCache();

      if (!_localCache!.Any(x => x.TagText == tag.TagText))
      {
        GetDbSet().Add(tag);
        _localCache!.Add(tag);
      }
      return tag;
    }

    private void HydrateLocalCache()
    {
      _localCache = GetQueryRoot().ToList();
    }
  }
}