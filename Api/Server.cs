
namespace Api
{
    public static class Server
    {
        public struct ServerType(WebApplication app, Token.IMaker tokenMaker)
        {
            public WebApplication App { get; set;} = app;
            public Token.IMaker TokenMaker { get; set;} = tokenMaker;
        }

        public static ServerType NewServer(string[] args)
        {
           
            string symmetricKey = "12345678901234567890123456789012";
           
            var tokenMaker = new Token.PasetoMaker(symmetricKey);
            
            var app = SetupApp(args);
            ServerType server = new(app,tokenMaker);

            server.SetupSwagger();
            server.SetupRouter();
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

        private static void SetupSwagger(this ServerType server)
        {
            // Configure the HTTP request pipeline.
            var app = server.App;
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
        }

        private static void SetupRouter(this ServerType server)
        {
            // No Auth router
            server.App.MapPost("/login-user",async context => await User.LoginUser(server, context.Request));

            // Api has auth 
            RouteGroupBuilder apiRouter = server.App.MapGroup("/api");
            apiRouter.MapGet("/test",() => {
                return Results.Ok("Test endpoint reached successfully.");
            });
        }

        public static void Start(this ServerType server)
        {
            server.App.Run();
        }

    }
}