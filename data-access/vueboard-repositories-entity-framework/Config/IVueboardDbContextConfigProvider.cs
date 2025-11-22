namespace Vueboard.DataAccess.Repositories.EntityFramework.Config
{
  public interface IVueboardDbContextConfigProvider
  {
    IVueboardDbContextConfig Provide();
  }
}