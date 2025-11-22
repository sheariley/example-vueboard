using Microsoft.EntityFrameworkCore;

namespace Vueboard.DataAccess.Repositories.EntityFramework.Config
{
  public interface IVueboardDbContextConfig
  {
    void Apply(DbContextOptionsBuilder optionsBuilder);
  }
}