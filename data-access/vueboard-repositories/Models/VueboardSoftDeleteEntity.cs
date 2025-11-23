namespace Vueboard.DataAccess.Models
{
  public class VueboardSoftDeleteEntity : VueboardEntity, IVueboardSoftDeleteEntity
  {
    public bool IsDeleted { get; set; }
  }
}
