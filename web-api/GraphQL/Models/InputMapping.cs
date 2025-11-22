using Vueboard.DataAccess.Models;

namespace Vueboard.Api.GraphQL.Models
{
  public static class InputMapping
  {
    public static Project MapToProject(UpdateProjectInput input, Guid userId)
    {
      return new Project
      {
        Id = input.Id,
        Uid = input.Uid,
        Title = input.Title,
        Description = input.Description,
        DefaultCardFgColor = input.DefaultCardFgColor,
        DefaultCardBgColor = input.DefaultCardBgColor,
        Updated = DateTime.UtcNow,
        UserId = userId,
        ProjectColumns = input.ProjectColumns?.ConvertAll(col => new ProjectColumn
        {
          Id = col.Id,
          Uid = col.Uid,
          Name = col.Name,
          IsDefault = col.IsDefault,
          FgColor = col.FgColor,
          BgColor = col.BgColor,
          Index = col.Index,
          Updated = DateTime.UtcNow,
          ProjectId = col.ProjectId,
          WorkItems = col.WorkItems?.ConvertAll(item => new WorkItem
          {
            Id = item.Id,
            Uid = item.Uid,
            Title = item.Title,
            Description = item.Description,
            Notes = item.Notes,
            FgColor = item.FgColor,
            BgColor = item.BgColor,
            Index = item.Index,
            Updated = DateTime.UtcNow,
            ProjectColumnId = item.ProjectColumnId,
            WorkItemTags = item.WorkItemTags?.ConvertAll(tag => new WorkItemTag
            {
              Id = tag.Id,
              Uid = tag.Uid,
              UserId = userId,
              TagText = tag.TagText
              // WorkItems intentionally omitted
            }) ?? new List<WorkItemTag>()
          }) ?? new List<WorkItem>()
        }) ?? new List<ProjectColumn>()
      };
    }

    public static Project MapToProject(CreateProjectInput input, Guid userId, Guid projectUid)
    {
      return new Project
      {
        Title = input.Title,
        Description = input.Description,
        DefaultCardFgColor = input.DefaultCardFgColor,
        DefaultCardBgColor = input.DefaultCardBgColor,
        Created = DateTime.UtcNow,
        Updated = DateTime.UtcNow,
        IsDeleted = false,
        Uid = projectUid,
        UserId = userId,
        ProjectColumns = new List<ProjectColumn>()
      };
    }
  }
}
