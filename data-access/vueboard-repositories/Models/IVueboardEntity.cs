namespace Vueboard.DataAccess.Models
{
  public interface IVueboardEntity
  {
    int Id { get; set; }
    Guid Uid { get; set; }
  }
}
