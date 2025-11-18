using HotChocolate;
using HotChocolate.Types;
using Vueboard.DataAccess.Models;

namespace Vueboard.Api.GraphQL
{
  public class ProjectType : ObjectType<Project>
  {
    protected override void Configure(IObjectTypeDescriptor<Project> descriptor)
    {
      descriptor.Field(f => f.Id).Type<NonNullType<IntType>>();
      descriptor.Field(f => f.Uid).Type<NonNullType<StringType>>();
      descriptor.Field(f => f.Created).Type<NonNullType<DateTimeType>>();
      descriptor.Field(f => f.Updated).Type<NonNullType<DateTimeType>>();
      descriptor.Field(f => f.IsDeleted).Type<NonNullType<BooleanType>>();
      descriptor.Field(f => f.UserId).Type<NonNullType<StringType>>();
      descriptor.Field(f => f.Title).Type<NonNullType<StringType>>();
      descriptor.Field(f => f.Description).Type<StringType>();
      descriptor.Field(f => f.DefaultCardFgColor).Type<StringType>();
      descriptor.Field(f => f.DefaultCardBgColor).Type<StringType>();
      descriptor.Field(f => f.Columns)
        .ResolveWith<ProjectResolvers>(r => r.GetColumnsAsync(default!, default!, default))
        .Type<ListType<ProjectColumnType>>();
    }
  }

  public class ProjectColumnType : ObjectType<ProjectColumn>
  {
    protected override void Configure(IObjectTypeDescriptor<ProjectColumn> descriptor)
    {
      descriptor.Field(f => f.Id).Type<NonNullType<IntType>>();
      descriptor.Field(f => f.Uid).Type<NonNullType<StringType>>();
      descriptor.Field(f => f.Created).Type<NonNullType<DateTimeType>>();
      descriptor.Field(f => f.Updated).Type<NonNullType<DateTimeType>>();
      descriptor.Field(f => f.IsDeleted).Type<NonNullType<BooleanType>>();
      descriptor.Field(f => f.ProjectId).Type<NonNullType<IntType>>();
      descriptor.Field(f => f.Name).Type<NonNullType<StringType>>();
      descriptor.Field(f => f.IsDefault).Type<NonNullType<BooleanType>>();
      descriptor.Field(f => f.FgColor).Type<StringType>();
      descriptor.Field(f => f.BgColor).Type<StringType>();
      descriptor.Field(f => f.Index).Type<NonNullType<IntType>>();
      descriptor.Field(f => f.WorkItems)
        .ResolveWith<ProjectColumnResolvers>(r => r.GetWorkItemsAsync(default!, default!, default))
        .Type<ListType<WorkItemType>>();
    }
  }

  public class WorkItemType : ObjectType<WorkItem>
  {
    protected override void Configure(IObjectTypeDescriptor<WorkItem> descriptor)
    {
      descriptor.Field(f => f.Id).Type<NonNullType<IntType>>();
      descriptor.Field(f => f.Uid).Type<NonNullType<StringType>>();
      descriptor.Field(f => f.Created).Type<NonNullType<DateTimeType>>();
      descriptor.Field(f => f.Updated).Type<NonNullType<DateTimeType>>();
      descriptor.Field(f => f.IsDeleted).Type<NonNullType<BooleanType>>();
      descriptor.Field(f => f.ProjectColumnId).Type<NonNullType<IntType>>();
      descriptor.Field(f => f.Title).Type<NonNullType<StringType>>();
      descriptor.Field(f => f.Description).Type<StringType>();
      descriptor.Field(f => f.Notes).Type<StringType>();
      descriptor.Field(f => f.FgColor).Type<StringType>();
      descriptor.Field(f => f.BgColor).Type<StringType>();
      descriptor.Field(f => f.Index).Type<NonNullType<IntType>>();
      descriptor.Field(f => f.Tags).Type<ListType<NonNullType<StringType>>>(); // Expose tags as string array
    }
  }

  public class ProjectResolvers
  {
    public async Task<IEnumerable<ProjectColumn>> GetColumnsAsync(
      [Parent] Project project,
      ProjectColumnsByProjectIdDataLoader dataLoader,
      CancellationToken cancellationToken)
    {
      var columns = await dataLoader.LoadAsync(project.Id, cancellationToken);
      return columns ?? Enumerable.Empty<ProjectColumn>();
    }
  }

  public class ProjectColumnResolvers
  {
    public async Task<IEnumerable<WorkItem>> GetWorkItemsAsync(
      [Parent] ProjectColumn column,
      WorkItemsByProjectColumnIdDataLoader dataLoader,
      CancellationToken cancellationToken)
    {
      var workItems = await dataLoader.LoadAsync(column.Id, cancellationToken);
      return workItems ?? Enumerable.Empty<WorkItem>();
    }
  }
}
