using System.Linq.Expressions;
using Vueboard.DataAccess.Models;
using Vueboard.DataAccess.Repositories;

namespace Vueboard.DataAccess.EntityFramework.Repositories
{
    public class EFProjectRepository : IProjectRepository
    {
        private readonly IVueboardDbContext _context;
        public EFProjectRepository(IVueboardDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Project> Get(Expression<Func<Project, bool>> predicate) => Get(x => x, new FetchSpecification<Project> { Criteria = predicate });
        
        public IEnumerable<Project> Get(FetchSpecification<Project> spec) => Get(x => x, spec);

        public IEnumerable<TOut> Get<TOut>(Expression<Func<Project, TOut>> selector, FetchSpecification<Project>? specification = null)
        {
            var query = _context.Projects.AsQueryable();
            if (specification != null)
                query = specification.Apply(query);
            return query.Select(selector).ToList();
        }

        public Project? GetByUid(Guid uid)
        {           
          return _context.Projects.FirstOrDefault(p => p.Uid.Equals(uid));
        }

        public Project Add(Project project)
        {
            var entry = _context.Projects.Add(project);
            // SaveChanges should be called by the consumer
            return entry.Entity;
        }

        public bool Update(Project project)
        {
            _context.Projects.Update(project);
            // SaveChanges should be called by the consumer
            return true;
        }

        public bool Delete(Guid uid)
        {
            var project = GetByUid(uid);
            if (project == null) return false;
            _context.Projects.Remove(project);
            // SaveChanges should be called by the consumer
            return true;
        }
    }
}
