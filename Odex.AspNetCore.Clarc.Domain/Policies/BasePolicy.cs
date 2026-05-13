using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Odex.AspNetCore.Clarc.Domain.Exceptions;

namespace Odex.AspNetCore.Clarc.Domain.Policies;

/// <summary>
/// Default implementation of <see cref="IBasePolicy{T}"/> with common guard rules. "Empty" handling covers strings,
/// arrays, collections, <see cref="Guid.Empty"/>, default value types, and null-valued nullable value types.
/// </summary>
/// <typeparam name="T">The primary policy type used with <see cref="RequireNotNullNorEmpty(T?)"/>.</typeparam>
public class BasePolicy<T> : IBasePolicy<T>
{
    #region Private Helper Methods

    private void CheckEmpty(object obj, string policyName, out string message)
    {
        message = string.Empty;

        if (!IsEmpty(obj)) return;
        message = GetEmptyErrorMessage(obj);
        throw new PolicyViolationException(policyName, message);
    }

    private bool IsEmpty(object obj)
    {
        switch (obj)
        {
            // String check
            case string str:
                return string.IsNullOrEmpty(str);
            case Array array:
                return array.Length == 0;
            // ICollection<T> and ICollection
            case ICollection collection:
                return collection.Count == 0;
            // Collections (IEnumerable)
            case IEnumerable enumerable:
                return !enumerable.Cast<object>().Any();
            // Guid check
            case Guid guid:
                return guid == Guid.Empty;
        }

        // Nullable<T> check for value types
        var type = obj.GetType();
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            var value = type.GetProperty("HasValue")?.GetValue(obj);
            if (value != null && !(bool)value)
                return true; // Nullable has no value
        }

        // For other types, consider default value as empty
        if (type.IsValueType)
        {
            var defaultValue = Activator.CreateInstance(type);
            return obj.Equals(defaultValue);
        }

        // For reference types that aren't specially handled, 
        // we don't consider them "empty" just because they're not null
        return false;
    }

    private string GetEmptyErrorMessage(object obj)
    {
        switch (obj)
        {
            case string _:
                return "String cannot be null or empty";
            case IEnumerable _:
                return "Collection cannot be null or empty";
            case Guid _:
                return "Guid cannot be empty";
            default:
                return $"Object of type {obj.GetType().Name} cannot be null or empty";
        }
    }

    #endregion

    /// <inheritdoc />
    /// <exception cref="PolicyViolationException">Thrown when <paramref name="obj"/> is null.</exception>
    [return: NotNull]
    public void RequireNotNull([NotNull] T? obj)
    {
        if (obj is null) throw new PolicyViolationException("RequireNotNull", "The value cannot be null.");
    }

    /// <inheritdoc />
    /// <exception cref="PolicyViolationException">Thrown when <paramref name="obj"/> is null.</exception>
    [return: NotNull]
    public void RequireNotNull<TU>([NotNull] TU? obj) where TU : class
    {
        if (obj is null) throw new PolicyViolationException("RequireNotNull", "The value cannot be null.");
    }

    /// <inheritdoc />
    /// <exception cref="PolicyViolationException">Thrown when <paramref name="obj"/> is not null.</exception>
    public void RequireNull(T? obj)
    {
        if (obj != null) throw new PolicyViolationException("RequireNull", $"{obj} must be null");
    }

    /// <inheritdoc />
    /// <exception cref="PolicyViolationException">Thrown when the value is null or considered empty.</exception>
    [return: NotNull]
    public void RequireNotNullNorEmpty([NotNull] T? obj)
    {
        RequireNotNull(obj);

        CheckEmpty(obj, "RequireNotNullNorEmpty", out var message);
    }

    /// <inheritdoc />
    /// <exception cref="PolicyViolationException">Thrown when the value is null or considered empty.</exception>
    [return: NotNull]
    public void RequireNotNullNorEmpty<TU>([NotNull] TU? obj) where TU : class
    {
        RequireNotNull(obj);
        CheckEmpty(obj!, "RequireNotNullNorEmpty", out _);
    }

    /// <inheritdoc />
    /// <exception cref="PolicyViolationException">Thrown when the value is neither null nor empty.</exception>
    public void RequireNullOrEmpty(T? obj)
    {
        if (obj is null || IsEmpty(obj)) return;
        throw new PolicyViolationException("RequireNullOrEmpty", $"Object must be null or empty, but was: {obj}");
    }

    /// <inheritdoc />
    /// <exception cref="PolicyViolationException">Thrown when <paramref name="obj"/> is <c>false</c>.</exception>
    public void RequireTrue(bool obj)
    {
        if (!obj) throw new PolicyViolationException("RequireTrue", $"{obj} must be true");
    }

    /// <inheritdoc />
    /// <exception cref="PolicyViolationException">Thrown when <paramref name="obj"/> is <c>true</c>.</exception>
    public void RequireFalse(bool obj)
    {
        if (obj) throw new PolicyViolationException("RequireFalse", $"{obj} must be false");
    }
}
