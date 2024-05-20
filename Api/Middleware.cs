using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Test.Api
{
    public static class Middleware
    {
        public static void SetupMiddleware(this WebApplication app)
        {
            app.Use(AuthorizationMiddleware);
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

            // Do something with the token (e.g., log it, validate it, etc.)
            context.Items["BearerToken"] = token;

            await next(context);
        }

        
    }
}
