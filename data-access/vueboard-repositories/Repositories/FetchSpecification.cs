using System.Linq.Expressions;

public class FetchSpecification<T>
{
  public Expression<Func<T, bool>>? Criteria { get; init; }
  public Func<IQueryable<T>, IOrderedQueryable<T>>? OrderBy { get; init; }

  public IQueryable<T> Apply(IQueryable<T> source)
  {
    // Apply Criteria
    if (Criteria != null)
    {
      source = source.Where(Criteria);
    }

    // Apply OrderBy
    if (OrderBy != null)
    {
      source = OrderBy(source);
    }

    return source;
  }
}