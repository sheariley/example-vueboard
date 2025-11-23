namespace Vueboard.DataAccess.Models
{
  public class WorkItemTag : VueboardEntity
  {
    public string TagText { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public List<WorkItem> WorkItems { get; set; } = new();
  }
}