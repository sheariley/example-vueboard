namespace Vueboard.Api.GraphQL.Models
{
  public class UpdateWorkItemTagInput
  {
    public int Id { get; set; }
    public Guid Uid { get; set; }
    public string TagText { get; set; } = string.Empty;
    // WorkItems property intentionally omitted
  }
}