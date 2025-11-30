using System.Data.Common;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Vueboard.Server.Environment;
using Vueboard.Server.Environment.Auth;

namespace Vueboard.Api.Auth
{
  public class JwtSessionInterceptor : DbConnectionInterceptor
  {
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserIdAccessor _userIdAccessor;

    public JwtSessionInterceptor(
      IHttpContextAccessor accessor,
      IUserIdAccessor userIdAccessor
    )
    {
      _httpContextAccessor = accessor;
      _userIdAccessor = userIdAccessor;
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
    }
  }
}
