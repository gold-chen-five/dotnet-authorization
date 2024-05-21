
namespace Api
{
    public static class Middleware 
    {
        public static void SetupMiddleware(this Server.ServerType server)
        {
            server.App.MapWhen(context => context.Request.Path.StartsWithSegments("/api"), api => api.Use(AuthorizationMiddleware));
        }

        private static async Task AuthorizationMiddleware(HttpContext context, RequestDelegate next)
        {
            if (!context.Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Authorization header missing");
                return;
            }

            if (!authHeader.ToString().StartsWith("Bearer "))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid Authorization header");
                return;
            }

            var token = authHeader.ToString().Split(" ")[1].Trim();
            context.Items["BearerToken"] = token;

            await next(context);
        }
    }
}
