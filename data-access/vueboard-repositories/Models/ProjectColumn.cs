namespace Vueboard.DataAccess.Models
{
  public class ProjectColumn
  {
    public int Id { get; set; }
    public Guid Uid { get; set; } = Guid.NewGuid();
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime Updated { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; } = false;
    public int ProjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsDefault { get; set; }
    public string? FgColor { get; set; }
    public string? BgColor { get; set; }
    public int Index { get; set; }
    public Project? Project { get; set; }
    public List<WorkItem> WorkItems { get; set; } = new();
  }
}