
namespace Test.Api
{
    public static class Router
    {
        public static void SetupRouter(this WebApplication app)
        {
            RouteGroupBuilder apiRouter = app.MapGroup("/api");
            apiRouter.MapGet("/test", () =>
            {
                return TypedResults.Ok("test");
            });

        }
    }
}