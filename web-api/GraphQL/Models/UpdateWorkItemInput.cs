namespace Vueboard.Api.GraphQL.Models
{
  public class UpdateWorkItemInput
  {
    public int Id { get; set; }
    public Guid Uid { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Notes { get; set; }
    public string? FgColor { get; set; }
    public string? BgColor { get; set; }
    public int Index { get; set; }
    public int ProjectColumnId { get; set; }
    public List<UpdateWorkItemTagInput> WorkItemTags { get; set; } = new();
  }
}