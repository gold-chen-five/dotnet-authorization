using Api.Services;
using Token;
using Util;

namespace Api;
public class Server(WebApplication app, IMaker tokenMaker, Config.Configuration config)
{
    public readonly WebApplication App = app;
    public readonly IMaker TokenMaker = tokenMaker;
    public readonly Config.Configuration Config = config;
}

public static class ExtensionServer
{
    public static void SetupSwagger(this Server server)
    {
        var app = server.App;
        // Configure the HTTP request pipeline.
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
