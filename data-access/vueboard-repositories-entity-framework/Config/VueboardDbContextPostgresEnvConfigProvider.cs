using Vueboard.Server.Environment;

namespace Vueboard.DataAccess.Repositories.EntityFramework.Config
{
  public class VueboardDbContextEnvPostgresConfigProvider : IVueboardDbContextConfigProvider
  {
    private readonly IServerEnvironment _serverEnv;
    public VueboardDbContextEnvPostgresConfigProvider(IServerEnvironment serverEnvironment)
    {
      _serverEnv = serverEnvironment;
    }

    public IVueboardDbContextConfig Provide()
    {
      return new VueboardDbContextPostgresConfig()
      {
        Host = Environment.GetEnvironmentVariable("SUPABASE_HOST") ?? "localhost",
        Port = int.TryParse(Environment.GetEnvironmentVariable("SUPABASE_PORT"), out var p) ? p : 54329,
        DbName = Environment.GetEnvironmentVariable("POSTGRES_DB_NAME") ?? "postgres",
        DbUser = Environment.GetEnvironmentVariable("POSTGRES_USER") ?? "postgres",
        DbPassword = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? "postgres",
        PoolerTenantId = Environment.GetEnvironmentVariable("POOLER_TENANT_ID"),
        EnableSensitvieDataLogging = _serverEnv.EnvironmentType == ServerEnvironmentType.Development
      };
    }
  }
}