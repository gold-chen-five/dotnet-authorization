using Token;
using Token.PasetoMaker;

namespace Api
{
    public struct Server(WebApplication app, IMaker tokenMaker)
    {
        public WebApplication App { get; set; } = app;
        public IMaker TokenMaker { get; set; } = tokenMaker;
    }

    public static class App
    {
        public static Server NewServer(string[] args)
        {
            string symmetricKey = "12345678901234567890123456789012";

            var tokenMaker = new PasetoMaker(symmetricKey);

            var app = SetupApp(args);
            Server server = new(app, tokenMaker);
            server.SetupRouter();
            server.SetupSwagger();
            server.SetupMiddleware();

            return server;
        }

        private static WebApplication SetupApp(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseHttpsRedirection();

            return app;
        }

        public static void SetupSwagger(this Server server)
        {
            // Configure the HTTP request pipeline.
            var app = server.App;
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
        }

        public static void SetupRouter(this Server server)
        {
            // No Auth router
            server.App.MapPost("/login-user", (HttpResponse response, HttpContext ctx) => server.LoginUser(response, ctx));

            // Api has auth 
            RouteGroupBuilder apiRouter = server.App.MapGroup("/api");
            apiRouter.MapGet("/test", (HttpResponse response, HttpContext ctx) => server.TestUser(response, ctx));
        }

        public static void Start(this Server server)
        {
            server.App.Run();
        }
    }

}