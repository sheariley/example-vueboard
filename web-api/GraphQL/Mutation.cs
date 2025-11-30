using System.Security.Authentication;
using Vueboard.Api.Auth;
using Vueboard.Api.GraphQL.Models;
using Vueboard.DataAccess.Models;
using Vueboard.DataAccess.Repositories;
using Vueboard.Server.Environment.Auth;

namespace Vueboard.Api.GraphQL
{
  public class Mutation
  {
    public Project? CreateProject(
      [Service] IProjectRepository projectRepo,
      [Service] IUserIdAccessor userIdAccessor,
      CreateProjectInput input
    )
    {
      var userId = userIdAccessor.GetUserId();

      if (!userId.HasValue) throw new AuthenticationException();

      var projectUid = String.IsNullOrWhiteSpace(input.Uid) ? Guid.NewGuid() : Guid.Parse(input.Uid);
      var project = InputMapping.MapToProject(input, userId.Value!, projectUid);
      var newProject = projectRepo.Create(project);
      if (newProject != null) projectRepo.CommitChanges();
      return newProject;
    }

    public Project? UpdateProject(
      [Service] IProjectRepository projectRepo,
      [Service] IUserIdAccessor userIdAccessor,
      UpdateProjectInput input
    )
    {
      var userId = userIdAccessor.GetUserId();

      if (!userId.HasValue) throw new AuthenticationException();

      var project = InputMapping.MapToProject(input, userId.Value!);
      var success = projectRepo.Update(project);
      if (success) projectRepo.CommitChanges();
      return success ? project : null;
    }

    public bool DeleteProject(
      [Service] IProjectRepository projectRepo,
      string uid
    )
    {
      var guid = Guid.Parse(uid);
      var success = projectRepo.DeleteByUid(guid);
      if (success) projectRepo.CommitChanges();
      return success;
    }
  }
}
