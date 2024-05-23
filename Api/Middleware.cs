
namespace Api
{
    public static class Middleware
    {
        public static void SetupMiddleware(this Server server)
        {
            server.App.UseWhen(
                context => context.Request.Path.StartsWithSegments("/api"),
                app => app.Use((ctx, next) => server.AuthorizationMiddleware(ctx, next))
            );
        }

        private static async Task AuthorizationMiddleware(this Server server, HttpContext ctx, RequestDelegate next)
        {
            if (!ctx.Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                Handler.HandleErrorResponse(ctx, StatusCodes.Status401Unauthorized, "Authorization header missing");
                return;
            }

            if (!authHeader.ToString().StartsWith("Bearer"))
            {
                Handler.HandleErrorResponse(ctx, StatusCodes.Status401Unauthorized, "Invalid Authorization header");
                return;
            }

            var token = authHeader.ToString().Split(" ")[1].Trim();

            // verify token
            try
            {
                var payload = server.TokenMaker.VerifyToken(token);
                Console.WriteLine(payload.Username);
            }
            catch(Exception err)
            {
                Console.WriteLine(err);
                Handler.HandleErrorResponse(ctx, StatusCodes.Status401Unauthorized, "Token not correct");
                return;
            }

            await next(ctx);
        }
    }
}
