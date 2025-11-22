using Vueboard.DataAccess.Models;

namespace Vueboard.Api.GraphQL
{
  public class ProjectType : ObjectType<Project>
  {
    protected override void Configure(IObjectTypeDescriptor<Project> descriptor)
    {
      descriptor.BindFieldsExplicitly();
      descriptor.Field(f => f.Id).Type<NonNullType<IntType>>();
      descriptor.Field(f => f.Uid).Type<NonNullType<UuidType>>();
      descriptor.Field(f => f.Created).Type<NonNullType<DateTimeType>>();
      descriptor.Field(f => f.Updated).Type<NonNullType<DateTimeType>>();
      descriptor.Field(f => f.IsDeleted).Type<NonNullType<BooleanType>>();
      descriptor.Field(f => f.Title).Type<NonNullType<StringType>>();
      descriptor.Field(f => f.Description).Type<StringType>();
      descriptor.Field(f => f.DefaultCardFgColor).Type<StringType>();
      descriptor.Field(f => f.DefaultCardBgColor).Type<StringType>();
      descriptor.Field(f => f.ProjectColumns)
        .ResolveWith<ProjectResolvers>(r => r.GetColumnsAsync(default!, default!, default))
        .Type<ListType<ProjectColumnType>>();
    }
  }

  public class ProjectColumnType : ObjectType<ProjectColumn>
  {
    protected override void Configure(IObjectTypeDescriptor<ProjectColumn> descriptor)
    {
      descriptor.BindFieldsExplicitly();
      descriptor.Field(f => f.Id).Type<NonNullType<IntType>>();
      descriptor.Field(f => f.Uid).Type<NonNullType<UuidType>>();
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

  public class WorkItemTagType : ObjectType<WorkItemTag>
  {
    protected override void Configure(IObjectTypeDescriptor<WorkItemTag> descriptor)
    {
      descriptor.BindFieldsExplicitly();
      descriptor.Field(f => f.Id).Type<NonNullType<IntType>>();
      descriptor.Field(f => f.Uid).Type<NonNullType<UuidType>>();
      descriptor.Field(f => f.TagText).Type<NonNullType<StringType>>();
      descriptor.Field(f => f.WorkItems)
        .Type<ListType<WorkItemType>>()
        .Ignore();
    }
  }

  public class WorkItemType : ObjectType<WorkItem>
  {
    protected override void Configure(IObjectTypeDescriptor<WorkItem> descriptor)
    {
      descriptor.BindFieldsExplicitly();
      descriptor.Field(f => f.Id).Type<NonNullType<IntType>>();
      descriptor.Field(f => f.Uid).Type<NonNullType<UuidType>>();
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
      descriptor.Field(f => f.WorkItemTags)
        .ResolveWith<WorkItemResolvers>(r => r.GetTagsAsync(default!, default!, default))
        .Type<ListType<WorkItemTagType>>();
    }
  }
}
