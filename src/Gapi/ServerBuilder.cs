using Microsoft.Extensions.Options;
using Token;
using Util;
namespace Gapi;
public static class ServerBuilder
{
    public static Server NewServer(string[] args)
    {   
        var builder = WebApplication.CreateBuilder(args);
        
        // load env
        Config.LoadEnv(builder);

        // Add services to the container.
        builder.Services.AddGrpc();
        builder.Services.AddGrpcReflection();
        
        // setup app
        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapGrpcReflectionService();
        }


        var config = app.Services.GetRequiredService<IOptions<Config.Configuration>>().Value;

        // create token maker
        string symmetricKey = config.TOKEN_SYMMETRIC_KEY ?? throw new Exception("SymmetricKey not found");
        PasetoMaker tokenMaker = new(symmetricKey);

        Server server = new(app, tokenMaker, config);
        
        server.SetupService();
        return server;
    }
}
