namespace Vueboard.DataAccess.Models
{
  public class ProjectColumn : VueboardSoftDeleteEntity
  {
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime Updated { get; set; } = DateTime.UtcNow;
    public int? ProjectId { get; set; } = null;
    public string Name { get; set; } = string.Empty;
    public bool IsDefault { get; set; }
    public string? FgColor { get; set; }
    public string? BgColor { get; set; }
    public int Index { get; set; }
    public Project? Project { get; set; }
    public List<WorkItem> WorkItems { get; set; } = new();
  }
}