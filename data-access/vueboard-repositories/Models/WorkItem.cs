namespace Vueboard.DataAccess.Models
{
  public class WorkItem : VueboardSoftDeleteEntity
  {
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime Updated { get; set; } = DateTime.UtcNow;
    public int ProjectColumnId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Notes { get; set; }
    public string? FgColor { get; set; }
    public string? BgColor { get; set; }
    public int Index { get; set; }
    public ProjectColumn? ProjectColumn { get; set; }
    public List<WorkItemTag> WorkItemTags { get; set; } = new();
  }
}