using Token.PasetoMaker;
using DotNetEnv;

namespace Api
{
    public static class ServerBuilder
    {
        public static Server NewServer(string[] args)
        {
            var app = SetupApp(args);

            string symmetricKey = Environment.GetEnvironmentVariable("TOKEN_SYMMETRIC_KEY") ?? throw new Exception("SymmetricKey not found");
            Console.WriteLine(symmetricKey);
            PasetoMaker tokenMaker = new(symmetricKey);
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
    }
}