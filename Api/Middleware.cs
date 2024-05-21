
namespace Api
{
    public static class Middleware
    {
        public static void SetupMiddleware(this Server server)
        {
            server.App.MapWhen(
                context => context.Request.Path.StartsWithSegments("/api"),
                api => api.Use(AuthorizationMiddleware)
            );
        }

        private static async Task AuthorizationMiddleware(HttpContext ctx, RequestDelegate next)
        {
            if (!ctx.Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                Handler.HandleErrorResponse(ctx, StatusCodes.Status401Unauthorized, "Authorization header missing");
                return;
            }

            if (!authHeader.ToString().StartsWith("Bearer "))
            {
                Handler.HandleErrorResponse(ctx, StatusCodes.Status401Unauthorized, "Invalid Authorization header");
                return;
            }

            var token = authHeader.ToString().Split(" ")[1].Trim();
            ctx.Items["BearerToken"] = token;

            await next(ctx);
        }
    }
}
