namespace Vueboard.DataAccess.Models;

public class SoftDelete : VueboardEntity
{
  public Guid UserId { get; set; }
  public DateTime Deleted { get; set; }
  public string EntityType { get; set; } = string.Empty;
  public int? ParentId { get; set; }
}