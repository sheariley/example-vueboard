namespace Vueboard.Api.GraphQL.Models
{
  public class UpdateProjectInput
  {
    public int Id { get; set; }
    public Guid Uid { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? DefaultCardFgColor { get; set; }
    public string? DefaultCardBgColor { get; set; }
    public List<UpdateProjectColumnInput> ProjectColumns { get; set; } = new();
  }
}
