using System.Linq.Expressions;

namespace Odex.AspNetCore.Clarc.Domain.Specifications;

/// <summary>
/// Rewrites a lambda body so two expression trees can share a single <see cref="ParameterExpression"/> when combining predicates.
/// </summary>
/// <param name="oldParameter">Parameter to replace (typically from a nested lambda).</param>
/// <param name="newParameter">Unified parameter used in the merged expression.</param>
public class ReplaceParameterVisitor(ParameterExpression oldParameter, ParameterExpression newParameter)
    : ExpressionVisitor
{
    /// <inheritdoc />
    protected override Expression VisitParameter(ParameterExpression node)
        => node == oldParameter ? newParameter : base.VisitParameter(node);
}
