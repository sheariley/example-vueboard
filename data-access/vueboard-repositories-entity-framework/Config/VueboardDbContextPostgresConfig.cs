using Microsoft.EntityFrameworkCore;
using EFCore.NamingConventions;

namespace Vueboard.DataAccess.Repositories.EntityFramework.Config
{
  public class VueboardDbContextPostgresConfig : IVueboardDbContextConfig
  {
    public required string Host { get; init; }
    public required int Port { get; init; }
    public required string DbName { get; init; }
    public required string DbUser { get; init; }
    public required string DbPassword {get; init; }
    public string? PoolerTenantId { get; init; } = null;

    public void Apply(DbContextOptionsBuilder optionsBuilder)
    {
      var actualUserName = PoolerTenantId?.Length > 0 ? $"{DbUser}.{PoolerTenantId}" : DbUser;
      var connectionString = $"Host={Host};Port={Port};Database={DbName};Username={actualUserName};Password={DbPassword};SearchPath=const_data,user_data,public,extensions";
      optionsBuilder
        .UseNpgsql(connectionString)
        .UseSnakeCaseNamingConvention();
    }
  }
}