using Token;
using Util;
namespace Gapi;
using Gapi.Services;
public static class ServerBuilder
{
    public static WebApplication NewServer(string[] args)
    {   
        var builder = WebApplication.CreateBuilder(args);
        
        // load env
        var config = Config.LoadEnv(builder);

        // create token maker
        string symmetricKey = config.TOKEN_SYMMETRIC_KEY ?? throw new Exception("SymmetricKey not found");
        PasetoMaker tokenMaker = new(symmetricKey);

        // DI 
        builder.Services.AddSingleton<IMaker>(tokenMaker);
        builder.Services.AddSingleton(config);
        
        // Add gRPC.
        builder.Services.AddGrpc();
        builder.Services.AddGrpcReflection();
        
        // setup app
        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapGrpcReflectionService();
        }
        
        app.SetupService();
       
        return app;
    }

    public static void SetupService(this WebApplication app)
    {
        app.MapGrpcService<AuthService>();
        app.MapGet("/", () => "grpc start");
    }

    public static void Start(this WebApplication app)
    {
        app.Run();
    }
}
