using System.Linq.Expressions;
using Vueboard.DataAccess.Models;

namespace Vueboard.DataAccess.Repositories
{
  public interface IGenericSoftDeleteRepository<TEntity> : IGenericRepository<TEntity>
    where TEntity : class, IVueboardSoftDeleteEntity
  {
    bool Delete(TEntity? entity);
  }
}