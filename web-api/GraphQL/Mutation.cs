using HotChocolate;
using HotChocolate.Types;

namespace Vueboard.Api.GraphQL
{
    public class Mutation
    {
        private readonly IProjectRepository _projectRepo;
        public Mutation(IProjectRepository projectRepo)
        {
            _projectRepo = projectRepo;
        }

        public Project CreateProject(CreateProjectInput input)
        {
            var project = new Project
            {
                Title = input.Title,
                Description = input.Description,
                DefaultCardFgColor = input.DefaultCardFgColor,
                DefaultCardBgColor = input.DefaultCardBgColor,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                IsDeleted = false,
                Uid = Guid.NewGuid(),
                UserId = Guid.NewGuid(), // Replace with actual user context
                Columns = new List<ProjectColumn>()
            };
            return _projectRepo.Add(project);
        }

        public Project? UpdateProject(Project updatedProject)
        {
            updatedProject.Updated = DateTime.UtcNow;
            var success = _projectRepo.Update(updatedProject);
            return success ? updatedProject : null;
        }

        public bool DeleteProject(string uid)
        {
            return _projectRepo.Delete(uid);
        }
    }
}
