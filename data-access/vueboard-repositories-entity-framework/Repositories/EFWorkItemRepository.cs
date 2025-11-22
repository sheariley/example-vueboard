using Vueboard.DataAccess.Models;

namespace Vueboard.DataAccess.Repositories.EntityFramework
{
    public class EFWorkItemRepository : IWorkItemRepository
    {
        private readonly IVueboardDbContext _context;
        public EFWorkItemRepository(IVueboardDbContext context)
        {
            _context = context;
        }

        public IEnumerable<WorkItem> GetAllForProjectColumn(int projectColumnId)
        {
            return _context.WorkItems.Where(wi => wi.ProjectColumnId == projectColumnId).ToList();
        }

        public IEnumerable<WorkItem> GetAllForProjectColumns(IEnumerable<int> projectColumnIds)
        {
            return _context.WorkItems.Where(wi => projectColumnIds.Contains(wi.ProjectColumnId)).ToList();
        }

        public WorkItem? GetById(int id)
        {
            return _context.WorkItems.FirstOrDefault(wi => wi.Id == id);
        }

        public WorkItem? GetByUid(Guid uid)
        {
            return _context.WorkItems.FirstOrDefault(wi => wi.Uid == uid);
        }

        public WorkItem CreateWorkItem(WorkItem item, Guid userId)
        {
            var entry = _context.WorkItems.Add(item);
            _context.SaveChanges();
            return entry.Entity;
        }

        public bool UpdateWorkItem(WorkItem item, Guid userId)
        {
            _context.WorkItems.Update(item);
            _context.SaveChanges();
            return true;
        }

        public bool DeleteWorkItem(int workItemId)
        {
            var item = GetById(workItemId);
            if (item == null) return false;
            _context.WorkItems.Remove(item);
            _context.SaveChanges();
            return true;
        }

        public WorkItem? GetWorkItem(int workItemId)
        {
            return GetById(workItemId);
        }

        public List<WorkItem> GetWorkItemsForColumn(int projectColumnId)
        {
            return _context.WorkItems.Where(wi => wi.ProjectColumnId == projectColumnId).ToList();
        }

        public List<WorkItemTag> GetTagsForWorkItem(int workItemId)
        {
            var item = GetById(workItemId);
            return item?.WorkItemTags ?? new List<WorkItemTag>();
        }
    }
}
