using System.Linq.Expressions;

namespace Odex.AspNetCore.Clarc.Domain.Specifications;

/// <summary>
/// Base class for the specification pattern: a named, composable predicate over <typeparamref name="T"/>
/// expressed as a LINQ expression for database providers and as a compiled delegate for in-memory checks.
/// </summary>
/// <typeparam name="T">The entity or value type being filtered.</typeparam>
public abstract class Specification<T>
{
    /// <summary>
    /// Builds the predicate as an expression tree (preferred for ORMs that translate <see cref="Expression{TDelegate}"/> to SQL).
    /// </summary>
    /// <returns>An expression equivalent to a lambda <c>T -&gt; bool</c>.</returns>
    public abstract Expression<Func<T, bool>> ToExpression();

    /// <summary>
    /// Compiles <see cref="ToExpression"/> to a delegate for in-memory evaluation.
    /// </summary>
    /// <returns>A compiled predicate.</returns>
    public Func<T, bool> ToFunc() => ToExpression().Compile();

    /// <summary>
    /// Returns whether <paramref name="entity"/> satisfies this specification (evaluated in memory).
    /// </summary>
    /// <param name="entity">Candidate instance.</param>
    /// <returns><c>true</c> if the predicate matches; otherwise <c>false</c>.</returns>
    public bool IsSatisfiedBy(T entity)
    {
        var predicate = ToFunc();
        return predicate(entity);
    }

    /// <summary>
    /// Combines this specification with <paramref name="other"/> using logical AND.
    /// </summary>
    /// <param name="other">Right-hand specification.</param>
    /// <returns>A new specification whose predicate is the conjunction of both sides.</returns>
    public Specification<T> And(Specification<T> other)
        => new AndSpecification<T>(this, other);

    /// <summary>
    /// Combines this specification with <paramref name="other"/> using logical OR.
    /// </summary>
    /// <param name="other">Right-hand specification.</param>
    /// <returns>A new specification whose predicate is the disjunction of both sides.</returns>
    public Specification<T> Or(Specification<T> other)
        => new OrSpecification<T>(this, other);

    /// <summary>
    /// Negates this specification using logical NOT.
    /// </summary>
    /// <returns>A new specification that matches when this one does not.</returns>
    public Specification<T> Not()
        => new NotSpecification<T>(this);

    /// <summary>
    /// Allows passing a <see cref="Specification{T}"/> anywhere an <see cref="Expression{TDelegate}"/> of <c>Func&lt;T, bool&gt;</c> is expected.
    /// </summary>
    /// <param name="spec">Specification instance.</param>
    /// <returns><see cref="Specification{T}.ToExpression"/>.</returns>
    public static implicit operator Expression<Func<T, bool>>(Specification<T> spec)
        => spec.ToExpression();
}
