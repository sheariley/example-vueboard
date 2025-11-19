using Microsoft.EntityFrameworkCore;

namespace Vueboard.DataAccess.EntityFramework.Config
{
  public class VueboardDbContextConfig : IVueboardDbContextConfig
  {
    public required string Host { get; init; }
    public required int Port { get; init; }
    public required string DbName { get; init; }
    public required string DbUser { get; init; }
    public required string DbPassword {get; init; }

    public void Apply(DbContextOptionsBuilder optionsBuilder)
    {
      // var connectionString = $"postgres://{DbUser}:{DbPassword}@{Host}:{Port}/{DbName}";
      var connectionString = $"Host={Host};Port={Port};Database={DbName};Username={DbUser};Password={DbPassword}";
      optionsBuilder.UseNpgsql(connectionString);
    }
  }
}