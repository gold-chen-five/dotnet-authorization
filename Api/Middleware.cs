
namespace Api
{
    public partial class Server
    {
        public void SetupMiddleware()
        {
            _app.UseWhen(
                context => context.Request.Path.StartsWithSegments("/api"),
                app => app.Use(AuthorizationMiddleware)
            );
        }

        private async Task AuthorizationMiddleware(HttpContext ctx, RequestDelegate next)
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
                var payload = _tokenMaker.VerifyToken(token);
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
