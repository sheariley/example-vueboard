namespace Vueboard.DataAccess.Repositories.EntityFramework.Config
{
  public class VueboardDbContextEnvPostgresConfigProvider : IVueboardDbContextConfigProvider
  {
    public IVueboardDbContextConfig Provide()
    {
      return new VueboardDbContextPostgresConfig()
      {
        Host = Environment.GetEnvironmentVariable("SUPABASE_HOST") ?? "localhost",
        Port = int.TryParse(Environment.GetEnvironmentVariable("SUPABASE_PORT"), out var p) ? p : 54329,
        DbName = Environment.GetEnvironmentVariable("POSTGRES_DB_NAME") ?? "postgres",
        DbUser = Environment.GetEnvironmentVariable("POSTGRES_USER") ?? "postgres",
        DbPassword = Environment.GetEnvironmentVariable("POSTGRES_PASS") ?? "postgres",
        PoolerTenantId = Environment.GetEnvironmentVariable("POOLER_TENANT_ID")
      };
    }
  }
}