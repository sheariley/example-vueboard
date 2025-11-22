namespace Vueboard.Api.GraphQL.Models
{
  public class UpdateProjectColumnInput
  {
    public int Id { get; set; }
    public Guid Uid { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsDefault { get; set; }
    public string? FgColor { get; set; }
    public string? BgColor { get; set; }
    public int Index { get; set; }
    public int ProjectId { get; set; }
    public List<UpdateWorkItemInput> WorkItems { get; set; } = new();
  }
}