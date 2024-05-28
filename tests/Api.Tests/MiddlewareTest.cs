using Xunit;

namespace Api.Tests;
public static class MiddlewareTest
{
    [Fact]
    public async Task AuthorizationMiddleware_ShouldReturn401_WhenAuthorizationHeaderIsMissing()
    {
        
    }
}