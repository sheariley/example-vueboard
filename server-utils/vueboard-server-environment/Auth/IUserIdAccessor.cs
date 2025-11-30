namespace Vueboard.Server.Environment.Auth
{
  public interface IUserIdAccessor
  {
    Guid? GetUserId();
  }
}