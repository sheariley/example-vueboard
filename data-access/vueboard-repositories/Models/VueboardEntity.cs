namespace Vueboard.DataAccess.Models
{
  public class VueboardEntity : IVueboardEntity
  {
    public int Id { get; set; }
    public Guid Uid { get; set; } = Guid.NewGuid();
  }
}