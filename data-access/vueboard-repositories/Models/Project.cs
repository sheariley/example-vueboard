namespace Vueboard.DataAccess.Models
{
  public class Project : VueboardSoftDeleteEntity
  {
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime Updated { get; set; } = DateTime.UtcNow;
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? DefaultCardFgColor { get; set; }
    public string? DefaultCardBgColor { get; set; }
    public List<ProjectColumn> ProjectColumns { get; set; } = new();
  }
}
