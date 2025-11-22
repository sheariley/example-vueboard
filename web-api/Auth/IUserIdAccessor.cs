namespace Vueboard.Api.Auth
{
  public interface IUserIdAccessor
  {
    Guid GetUserId();
  }
}