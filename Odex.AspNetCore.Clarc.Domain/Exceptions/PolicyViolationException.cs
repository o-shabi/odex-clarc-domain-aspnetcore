using Odex.AspNetCore.Clarc.Domain.Constants;

namespace Odex.AspNetCore.Clarc.Domain.Exceptions;

/// <summary>
/// Thrown when a guard policy or invariant expressed through <see cref="Policies.BasePolicy{T}"/> is violated.
/// </summary>
/// <param name="rule">Short name of the rule (for example <c>RequireNotNull</c>).</param>
/// <param name="details">Additional context explaining the violation.</param>
public class PolicyViolationException(string rule, string details)
    : DomainException($"The policy '{rule}' has been violated: {details}", ExceptionType.PolicyViolation);
