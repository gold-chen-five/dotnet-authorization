using Microsoft.Extensions.Options;
using Token.PasetoMaker;
using Util;

namespace Api
{
    public static class ServerBuilder
    {
        public static Server NewServer(string[] args)
        {
            var app = SetupApp(args);

            var config = app.Services.GetRequiredService<IOptions<Config.Configuration>>().Value;

            // create token maker
            string symmetricKey = config.TOKEN_SYMMETRIC_KEY ?? throw new Exception("SymmetricKey not found");
            PasetoMaker tokenMaker = new(symmetricKey);

            Server server = new(app, tokenMaker, config);
            server.SetupRouter();
            server.SetupSwagger();
            server.SetupMiddleware();
            return server;
        }

        private static WebApplication SetupApp(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // load env
            Config.LoadEnv(builder);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseHttpsRedirection();

            return app;
        }
    }
}