using Odex.AspNetCore.Clarc.Domain.Exceptions;
using Odex.AspNetCore.Clarc.Domain.Policies;
using Xunit;

namespace Odex.AspNetCore.Clarc.Domain.Tests.Policies;

public class BasePolicyTests
{
    [Fact]
    public void RequireNullOrEmpty_accepts_null()
    {
        var policy = new BasePolicy<string>();
        policy.RequireNullOrEmpty(null);
    }

    [Fact]
    public void RequireNullOrEmpty_accepts_empty_string()
    {
        var policy = new BasePolicy<string>();
        policy.RequireNullOrEmpty(string.Empty);
    }

    [Fact]
    public void RequireNullOrEmpty_throws_when_string_has_content()
    {
        var policy = new BasePolicy<string>();
        var ex = Assert.Throws<PolicyViolationException>(() => policy.RequireNullOrEmpty("a"));
        Assert.Contains("must be null or empty", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void RequireNotNull_throws_with_stable_message_when_null()
    {
        var policy = new BasePolicy<string>();
        var ex = Assert.Throws<PolicyViolationException>(() => policy.RequireNotNull(null));
        Assert.Contains("cannot be null", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void RequireNotNullNorEmpty_accepts_non_empty_string()
    {
        var policy = new BasePolicy<string>();
        policy.RequireNotNullNorEmpty("ok");
    }

    [Fact]
    public void RequireNotNullNorEmpty_throws_for_empty_string()
    {
        var policy = new BasePolicy<string>();
        Assert.Throws<PolicyViolationException>(() => policy.RequireNotNullNorEmpty(string.Empty));
    }

    [Fact]
    public void RequireNotNullNorEmpty_TU_accepts_reference_when_non_empty()
    {
        var policy = new BasePolicy<int>(); // T irrelevant for generic overload
        policy.RequireNotNullNorEmpty("x");
    }

    [Fact]
    public void RequireNotNullNorEmpty_TU_throws_when_null()
    {
        var policy = new BasePolicy<int>();
        Assert.Throws<PolicyViolationException>(() => policy.RequireNotNullNorEmpty((string?)null));
    }
}
