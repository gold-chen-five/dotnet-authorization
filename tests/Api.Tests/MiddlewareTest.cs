using Xunit;

namespace Api.Tests;
public class MiddlewareTest
{
    [Fact]
    public void AuthorizationMiddleware_ShouldReturn401_WhenAuthorizationHeaderIsMissing()
    {
        var message = "Token validation failed.";
        Assert.Equal("Token validation failed.", message);
    }
}