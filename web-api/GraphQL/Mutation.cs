using Vueboard.Api.Auth;
using Vueboard.Api.GraphQL.Models;
using Vueboard.DataAccess.Models;
using Vueboard.DataAccess.Repositories;

namespace Vueboard.Api.GraphQL
{
  public class Mutation
  {
    public Project CreateProject(
      [Service] IProjectRepository projectRepo,
      [Service] IUserIdAccessor userIdAccessor,
      CreateProjectInput input
    )
    {
      var projectUid = String.IsNullOrWhiteSpace(input.Uid) ? Guid.NewGuid() : Guid.Parse(input.Uid);
      var userId = userIdAccessor.GetUserId();
      var project = InputMapping.MapToProject(input, userId, projectUid);
      return projectRepo.Add(project);
    }

    public Project? UpdateProject(
      [Service] IProjectRepository projectRepo,
      [Service] IUserIdAccessor userIdAccessor,
      UpdateProjectInput input
    )
    {
      var userId = userIdAccessor.GetUserId();
      var project = InputMapping.MapToProject(input, userId);
      var success = projectRepo.Update(project);
      return success ? project : null;
    }

    public bool DeleteProject(
      [Service] IProjectRepository projectRepo,
      string uid
    )
    {
      var guid = Guid.Parse(uid);
      return projectRepo.Delete(guid);
    }
  }
}
