using Microsoft.EntityFrameworkCore;

namespace Vueboard.DataAccess.EntityFramework.Config
{
  public interface IVueboardDbContextConfig
  {
    string Host {get; }
    int Port { get; }
    string DbName { get; }
    string DbUser { get; }
    string DbPassword { get; }

    void Apply(DbContextOptionsBuilder optionsBuilder);
  }
}