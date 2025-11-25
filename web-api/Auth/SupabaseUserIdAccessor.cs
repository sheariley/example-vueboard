using System.Security.Claims;
using Vueboard.Server.Environment.Auth;

namespace Vueboard.Api.Auth
{
  public class SupabaseUserIdAccessor : IUserIdAccessor
  {
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SupabaseUserIdAccessor(IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor;
    }

    public Guid GetUserId()
    {
      // Extract Supabase User ID from JWT claims
      var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier) ??
                        _httpContextAccessor.HttpContext?.User?.FindFirst("sub");
      if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
      {
        throw new Exception("Invalid user ID.");
      }
      
      return userId;
    }
  }
}