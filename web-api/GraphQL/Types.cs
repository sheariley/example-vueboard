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
      descriptor.Field(f => f.Columns).Type<ListType<ProjectColumnType>>();
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
      descriptor.Field(f => f.WorkItems).Type<ListType<WorkItemType>>();
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
}
