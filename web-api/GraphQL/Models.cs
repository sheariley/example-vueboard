namespace Vueboard.Api.GraphQL.Models
{
  public class CreateProjectInput
  {
    public string? Uid { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? DefaultCardFgColor { get; set; }
    public string? DefaultCardBgColor { get; set; }
  }
}