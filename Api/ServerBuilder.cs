using Token.PasetoMaker;

namespace Api
{
    public static class ServerBuilder
    {
        public static Server NewServer(string[] args)
        {
            var app = SetupApp(args);
            string symmetricKey = "12345678901234567890123456789012";
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