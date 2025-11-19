using System.Linq.Expressions;

public class FetchSpecification<T>
{
  public Expression<Func<T, bool>>? Criteria { get; init; }
  public List<Expression<Func<T, object>>> Includes { get; } = new();
  public Func<IQueryable<T>, IOrderedQueryable<T>>? OrderBy { get; init; }
}