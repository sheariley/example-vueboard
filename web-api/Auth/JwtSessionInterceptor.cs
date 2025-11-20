using System.Data.Common;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Vueboard.Api.Auth
{
  public class JwtSessionInterceptor : DbConnectionInterceptor
  {
    private readonly IHttpContextAccessor _httpContextAccessor;

    public JwtSessionInterceptor(IHttpContextAccessor accessor)
    {
      _httpContextAccessor = accessor;
    }

    public override async Task ConnectionOpenedAsync(
      DbConnection connection,
      ConnectionEndEventData eventData,
      CancellationToken cancellationToken = default
    )
    {
      var context = _httpContextAccessor.HttpContext;

      if (context?.User?.Identity?.IsAuthenticated != true)
        return;

      var claims = context.User.Claims.ToDictionary(c => c.Type, c => c.Value);
      var claimsJson = JsonSerializer.Serialize(claims);

      using var cmd = connection.CreateCommand();
      cmd.CommandText = "SELECT set_config('request.jwt.claims', @claims::text, true);";
      var param = cmd.CreateParameter();
      param.ParameterName = "claims";
      param.Value = claimsJson;
      cmd.Parameters.Add(param);

      await cmd.ExecuteNonQueryAsync(cancellationToken);
    }
  }
}
