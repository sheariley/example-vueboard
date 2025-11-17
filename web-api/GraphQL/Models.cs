namespace Vueboard.Api.GraphQL.Models
{
  public class CreateProjectInput
  {
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? DefaultCardFgColor { get; set; }
    public string? DefaultCardBgColor { get; set; }
  }

  public class CreateColumnInput
  {
    public string Name { get; set; } = string.Empty;
    public bool IsDefault { get; set; } = false;
    public int Index { get; set; } = 0;
  }
}