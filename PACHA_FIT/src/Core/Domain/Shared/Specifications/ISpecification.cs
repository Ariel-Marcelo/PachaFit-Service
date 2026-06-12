using System.Linq.Expressions;

namespace PACHA_FIT.Core.Domain.Shared.Specifications;

public interface ISpecification<T>
{
    Expression<Func<T, bool>> Criteria { get; }
    List<Expression<Func<T, object>>> Includes { get; }
}
