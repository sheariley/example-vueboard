namespace Vueboard.DataAccess.Models
{
  public class WorkItem
  {
    public int Id { get; set; }
    public Guid Uid { get; set; } = Guid.NewGuid();
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime Updated { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; } = false;
    public int ProjectColumnId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Notes { get; set; }
    public string? FgColor { get; set; }
    public string? BgColor { get; set; }
    public int Index { get; set; }
    public List<string> Tags { get; set; } = new(); // Abstract tags as strings
  }
}