using System.Diagnostics.CodeAnalysis;
using Odex.AspNetCore.Clarc.Domain.Exceptions;

namespace Odex.AspNetCore.Clarc.Domain.Policies;

/// <summary>
/// Contract for lightweight guard-style checks used inside domain methods. Implementations throw
/// <see cref="PolicyViolationException"/> when a rule fails.
/// </summary>
/// <typeparam name="T">The primary type used with <see cref="RequireNotNullNorEmpty(T?)"/> and related overloads keyed on <c>T</c>.</typeparam>
public interface IBasePolicy<in T>
{
    /// <summary>
    /// Ensures <paramref name="obj"/> is not a null reference.
    /// </summary>
    /// <param name="obj">The value to validate.</param>
    /// <exception cref="PolicyViolationException">Thrown when <paramref name="obj"/> is null.</exception>
    [return: NotNull]
    void RequireNotNull([NotNull] T? obj);

    /// <summary>
    /// Ensures <paramref name="obj"/> is not a null reference (reference type <typeparamref name="TU"/>).
    /// </summary>
    /// <typeparam name="TU">Reference type to validate.</typeparam>
    /// <param name="obj">The value to validate.</param>
    /// <exception cref="PolicyViolationException">Thrown when <paramref name="obj"/> is null.</exception>
    [return: NotNull]
    void RequireNotNull<TU>([NotNull] TU? obj) where TU : class;

    /// <summary>
    /// Ensures <paramref name="obj"/> is null.
    /// </summary>
    /// <param name="obj">The value to validate.</param>
    /// <exception cref="PolicyViolationException">Thrown when <paramref name="obj"/> is not null.</exception>
    void RequireNull(T? obj);

    /// <summary>
    /// Ensures <paramref name="obj"/> is not null and not "empty" (see <see cref="BasePolicy{T}"/> for empty rules).
    /// </summary>
    /// <param name="obj">The value to validate.</param>
    /// <exception cref="PolicyViolationException">Thrown when the value is null or considered empty.</exception>
    [return: NotNull]
    void RequireNotNullNorEmpty([NotNull] T? obj);

    /// <summary>
    /// Ensures <paramref name="obj"/> is not null and not empty for reference type <typeparamref name="TU"/>.
    /// </summary>
    /// <typeparam name="TU">Reference type to validate.</typeparam>
    /// <param name="obj">The value to validate.</param>
    /// <exception cref="PolicyViolationException">Thrown when the value is null or considered empty.</exception>
    [return: NotNull]
    void RequireNotNullNorEmpty<TU>([NotNull] TU? obj) where TU : class;

    /// <summary>
    /// Ensures <paramref name="obj"/> is either null or empty (for example empty string or empty collection).
    /// </summary>
    /// <param name="obj">The value to validate.</param>
    /// <exception cref="PolicyViolationException">Thrown when the value is non-null and not empty.</exception>
    void RequireNullOrEmpty(T? obj);

    /// <summary>
    /// Ensures <paramref name="obj"/> is <c>true</c>.
    /// </summary>
    /// <param name="obj">The condition to assert.</param>
    /// <exception cref="PolicyViolationException">Thrown when <paramref name="obj"/> is <c>false</c>.</exception>
    void RequireTrue(bool obj);

    /// <summary>
    /// Ensures <paramref name="obj"/> is <c>false</c>.
    /// </summary>
    /// <param name="obj">The condition to assert.</param>
    /// <exception cref="PolicyViolationException">Thrown when <paramref name="obj"/> is <c>true</c>.</exception>
    void RequireFalse(bool obj);
}
