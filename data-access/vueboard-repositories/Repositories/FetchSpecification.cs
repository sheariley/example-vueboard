using System.Linq.Expressions;

public class FetchSpecification<T>
{
  public Expression<Func<T, bool>>? Criteria { get; init; }
  public Func<IQueryable<T>, IOrderedQueryable<T>>? OrderBy { get; init; }
}