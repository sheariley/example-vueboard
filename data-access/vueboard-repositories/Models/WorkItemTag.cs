namespace Vueboard.DataAccess.Models
{
  public class WorkItemTag
  {
    public int Id { get; set; }
    public Guid Uid { get; set; }
    public string TagText { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public List<WorkItem> WorkItems { get; set; } = new();
  }
}