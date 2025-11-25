using System.Reflection;
using Vueboard.DataAccess.Models;

namespace Vueboard.DataAccess.Repositories;

public static class RepositoryHelpers
{
  
  public static bool isScalarEntityProperty(PropertyInfo prop)
  {
    return (
      !typeof(System.Collections.IEnumerable).IsAssignableFrom(prop.PropertyType)
      || prop.PropertyType == typeof(string)
    ) && (
      !typeof(IVueboardEntity).IsAssignableFrom(prop.PropertyType)
      && prop.PropertyType != typeof(WorkItemTagRef)
    );
  }

  public static TEntity UpdateScalarProperties<TEntity>(this TEntity target, TEntity source)
    where TEntity : class, IVueboardEntity
  {
    var scalarProps = typeof(TEntity).GetProperties()
      .Where(isScalarEntityProperty);
    foreach (var prop in scalarProps)
    {
      var value = prop.GetValue(source);
      prop.SetValue(target, value);
    }
    return target;
  }

}