namespace Vueboard.DataAccess.Models
{
  public interface IVueboardSoftDeleteEntity : IVueboardEntity
  {
    bool IsDeleted { get; set; }
  }
}
