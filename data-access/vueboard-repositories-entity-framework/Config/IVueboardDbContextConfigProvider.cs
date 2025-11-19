namespace Vueboard.DataAccess.EntityFramework.Config
{
  public interface IVueboardDbContextConfigProvider
  {
    IVueboardDbContextConfig Provide();
  }
}