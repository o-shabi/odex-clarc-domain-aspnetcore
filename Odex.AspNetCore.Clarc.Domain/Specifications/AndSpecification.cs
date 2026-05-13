using System.Linq.Expressions;

namespace Odex.AspNetCore.Clarc.Domain.Specifications;

/// <summary>
/// Logical AND of two specifications over the same <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">Entity type.</typeparam>
/// <param name="left">Left-hand predicate.</param>
/// <param name="right">Right-hand predicate.</param>
public class AndSpecification<T>(Specification<T> left, Specification<T> right) : Specification<T>
{
    /// <inheritdoc />
    public override Expression<Func<T, bool>> ToExpression()
    {
        var leftExpr = left.ToExpression();
        var rightExpr = right.ToExpression();

        var parameter = Expression.Parameter(typeof(T));
        var leftVisitor = new ReplaceParameterVisitor(leftExpr.Parameters[0], parameter);
        var rightVisitor = new ReplaceParameterVisitor(rightExpr.Parameters[0], parameter);

        var leftBody = leftVisitor.Visit(leftExpr.Body);
        var rightBody = rightVisitor.Visit(rightExpr.Body);

        var body = Expression.AndAlso(leftBody, rightBody);
        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }
}
