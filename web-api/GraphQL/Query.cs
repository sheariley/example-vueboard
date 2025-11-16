using HotChocolate;
using HotChocolate.Types;

namespace Vueboard.Api.GraphQL
{
    public class Query
    {
        private readonly IProjectRepository _repo;
        public Query(IProjectRepository repo) => _repo = repo;

        [UsePaging]
        public IEnumerable<Project> Projects() => _repo.GetAll();

        public Project? Project(string uid) => _repo.GetByUid(uid);
    }
}
