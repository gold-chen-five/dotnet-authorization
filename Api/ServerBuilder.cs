using Token.PasetoMaker;
using Util;

namespace Api
{
    public static class ServerBuilder
    {
        public static Server NewServer(string[] args)
        {
            var app = SetupApp(args);

            // create token maker
            string symmetricKey = app.Configuration["SymmetricKey"] ?? throw new Exception("SymmetricKey not found");
            PasetoMaker tokenMaker = new(symmetricKey);

            // create server
            Server server = new(app, tokenMaker);
            server.SetupRouter();
            server.SetupSwagger();
            server.SetupMiddleware();
            return server;
        }

        private static WebApplication SetupApp(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Config.LoadEnv(builder);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseHttpsRedirection();

            return app;
        }

        
    }
}