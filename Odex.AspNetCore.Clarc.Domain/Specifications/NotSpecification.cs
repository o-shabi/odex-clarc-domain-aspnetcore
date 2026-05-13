using System.Linq.Expressions;

namespace Odex.AspNetCore.Clarc.Domain.Specifications;

/// <summary>
/// Logical NOT of a specification over <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">Entity type.</typeparam>
/// <param name="spec">Inner specification to negate.</param>
public class NotSpecification<T>(Specification<T> spec) : Specification<T>
{
    /// <inheritdoc />
    public override Expression<Func<T, bool>> ToExpression()
    {
        var expr = spec.ToExpression();
        var body = Expression.Not(expr.Body);
        return Expression.Lambda<Func<T, bool>>(body, expr.Parameters[0]);
    }
}
