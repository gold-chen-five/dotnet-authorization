using Gapi.Services;
using Token;
using Util;

namespace Gapi;
public class Server(WebApplication app, IMaker tokenMaker, Config.Configuration config)
{
    public readonly WebApplication App = app;
    public readonly IMaker TokenMaker = tokenMaker;
    public readonly Config.Configuration Config = config;
}

public static class ExtensionServer
{
    public static void SetupService(this Server server)
    {
        Console.WriteLine("start dev");
        server.App.MapGrpcService<GreeterService>();
        server.App.MapGet("/", () => "grpc start");
    }

    public static void Start(this Server server)
    {
        server.App.Run();
    }
}
