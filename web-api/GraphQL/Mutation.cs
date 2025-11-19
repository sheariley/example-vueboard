using Vueboard.Api.GraphQL.Models;
using Vueboard.DataAccess.Models;
using Vueboard.DataAccess.Repositories;

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
      var projectUid = String.IsNullOrWhiteSpace(input.Uid) ? Guid.NewGuid() : Guid.Parse(input.Uid);

      // TODO: Replace with Supabase User ID obtained via Supabase using JWT
      var userId = Guid.Parse("e971375a-2a56-4661-84bb-ec88b6e88b05");

      var project = new Project
      {
        Title = input.Title,
        Description = input.Description,
        DefaultCardFgColor = input.DefaultCardFgColor,
        DefaultCardBgColor = input.DefaultCardBgColor,
        Created = DateTime.UtcNow,
        Updated = DateTime.UtcNow,
        IsDeleted = false,
        Uid = projectUid,
        UserId = userId, // Replace with actual user context
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
      var guid = Guid.Parse(uid);
      return _projectRepo.Delete(guid);
    }
  }
}
