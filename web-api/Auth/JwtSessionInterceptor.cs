using System.Data.Common;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Vueboard.Server.Environment;
using Vueboard.Server.Environment.Auth;

namespace Vueboard.Api.Auth
{
  public class JwtSessionInterceptor : DbConnectionInterceptor
  {
    private readonly ILogger _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserIdAccessor _userIdAccessor;
    private readonly IServerEnvironment _env;

    public JwtSessionInterceptor(
      ILogger<JwtSessionInterceptor> logger,
      IHttpContextAccessor accessor,
      IUserIdAccessor userIdAccessor,
      IServerEnvironment env
    )
    {
      _logger = logger;
      _httpContextAccessor = accessor;
      _userIdAccessor = userIdAccessor;
      _env = env;
    }

    public override async Task ConnectionOpenedAsync(
      DbConnection connection,
      ConnectionEndEventData eventData,
      CancellationToken cancellationToken = default
    )
    {
      var context = _httpContextAccessor.HttpContext;
      var userId = _userIdAccessor.GetUserId();

      var isAuthenticated = context?.User?.Identity?.IsAuthenticated == true && userId != null;
      var dbRole = isAuthenticated ? "authenticated" : "anon";
      
      var claims = new Dictionary<string, string> { { "role", dbRole } };
      if (isAuthenticated) claims.Add("sub", userId.ToString()!);
      var claimsJson = JsonSerializer.Serialize(claims);

      using var cmd = connection.CreateCommand();
      cmd.CommandText = """
        SELECT set_config('request.jwt.claims', @claims, false);
        SELECT set_config('role', @role, false);
      """;
      
      var claimsParam = cmd.CreateParameter();
      claimsParam.ParameterName = "claims";
      claimsParam.Value = claimsJson;
      cmd.Parameters.Add(claimsParam);

      var roleParam = cmd.CreateParameter();
      roleParam.ParameterName = "role";
      roleParam.Value = dbRole;
      cmd.Parameters.Add(roleParam);

      await cmd.ExecuteNonQueryAsync(cancellationToken);

      if (_env.EnvironmentType == ServerEnvironmentType.Development || _env.EnvironmentType == ServerEnvironmentType.Testing)
      {
        var verifyCmd = connection.CreateCommand();
        // verifyCmd.CommandText = "SELECT auth.uid();";
        verifyCmd.CommandText = "SELECT ((current_setting('request.jwt.claims'::text, true))::json ->> 'sub'::text)";
        var results = await verifyCmd.ExecuteScalarAsync() as string;
        _logger.LogInformation($"Postgres session userId set to {results}");
      }
    }
  }
}
