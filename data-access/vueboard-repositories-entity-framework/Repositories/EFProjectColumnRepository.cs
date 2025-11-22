using Vueboard.DataAccess.Models;

namespace Vueboard.DataAccess.Repositories.EntityFramework
{
    public class EFProjectColumnRepository : IProjectColumnRepository
    {
        private readonly IVueboardDbContext _context;
        public EFProjectColumnRepository(IVueboardDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ProjectColumn> GetAllForProject(int projectId)
        {
            return _context.ProjectColumns.Where(pc => pc.ProjectId == projectId).ToList();
        }

        public IEnumerable<ProjectColumn> GetAllForProjects(IEnumerable<int> projectIds)
        {
            return _context.ProjectColumns.Where(pc => projectIds.Contains(pc.ProjectId)).ToList();
        }

        public ProjectColumn? GetById(int id)
        {
            return _context.ProjectColumns.FirstOrDefault(pc => pc.Id == id);
        }

        public ProjectColumn? GetByUid(Guid uid)
        {
            return _context.ProjectColumns.FirstOrDefault(pc => pc.Uid == uid);
        }

        public ProjectColumn Add(int projectId, ProjectColumn column)
        {
            column.ProjectId = projectId;
            var entry = _context.ProjectColumns.Add(column);
            // SaveChanges should be called by the consumer
            return entry.Entity;
        }

        public bool Update(ProjectColumn column)
        {
            _context.ProjectColumns.Update(column);
            // SaveChanges should be called by the consumer
            return true;
        }

        public bool Delete(int id)
        {
            var column = GetById(id);
            if (column == null) return false;
            _context.ProjectColumns.Remove(column);
            // SaveChanges should be called by the consumer
            return true;
        }
    }
}
