using Vueboard.DataAccess.Models;
using Vueboard.DataAccess.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using GreenDonut;
using HotChocolate.Execution;

namespace Vueboard.Api.GraphQL
{
    // Batch DataLoader for Projects by Id
    public class ProjectByIdDataLoader : BatchDataLoader<int, Project>
    {
        private readonly IProjectRepository _projectRepo;
        public ProjectByIdDataLoader(IBatchScheduler batchScheduler, IProjectRepository projectRepo)
            : base(batchScheduler, new DataLoaderOptions())
        {
            _projectRepo = projectRepo;
        }

        protected override Task<IReadOnlyDictionary<int, Project>> LoadBatchAsync(IReadOnlyList<int> keys, CancellationToken cancellationToken)
        {
            var projects = _projectRepo.GetAll().Where(p => keys.Contains(p.Id)).ToDictionary(p => p.Id);
            return Task.FromResult((IReadOnlyDictionary<int, Project>)projects);
        }
    }

    // Group DataLoader for ProjectColumns by ProjectId
    public class ProjectColumnsByProjectIdDataLoader : GroupedDataLoader<int, ProjectColumn>
    {
        private readonly IProjectColumnRepository _columnRepo;
        public ProjectColumnsByProjectIdDataLoader(IBatchScheduler batchScheduler, IProjectColumnRepository columnRepo)
            : base(batchScheduler, new DataLoaderOptions())
        {
            _columnRepo = columnRepo;
        }

        protected override Task<ILookup<int, ProjectColumn>> LoadGroupedBatchAsync(IReadOnlyList<int> keys, CancellationToken cancellationToken)
        {
            var columns = keys.SelectMany(id => _columnRepo.GetAllForProject(id)).ToLookup(c => c.ProjectId);
            return Task.FromResult(columns);
        }
    }
}
