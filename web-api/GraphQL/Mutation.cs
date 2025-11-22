using System.Security.Claims;
using Vueboard.Api.GraphQL.Models;
using Vueboard.DataAccess.Models;
using Vueboard.DataAccess.Repositories;

namespace Vueboard.Api.GraphQL
{
  public class Mutation
  {
    private readonly IProjectRepository _projectRepo;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public Mutation(IProjectRepository projectRepo, IHttpContextAccessor httpContextAccessor)
    {
      _projectRepo = projectRepo;
      _httpContextAccessor = httpContextAccessor;
    }

    public Project CreateProject(CreateProjectInput input)
    {
      var projectUid = String.IsNullOrWhiteSpace(input.Uid) ? Guid.NewGuid() : Guid.Parse(input.Uid);

      // Extract Supabase User ID from JWT claims
      var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier) ??
                        _httpContextAccessor.HttpContext?.User?.FindFirst("sub");
      if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
      {
        throw new Exception("Invalid user ID.");
      }

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
        UserId = userId, // Extracted from JWT
        ProjectColumns = new List<ProjectColumn>()
      };
      return _projectRepo.Add(project);
    }

    public Project? UpdateProject(Project project)
    {
      project.Updated = DateTime.UtcNow;
      var success = _projectRepo.Update(project);
      return success ? project : null;
    }

    public bool DeleteProject(string uid)
    {
      var guid = Guid.Parse(uid);
      return _projectRepo.Delete(guid);
    }
  }
}
