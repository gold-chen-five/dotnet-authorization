
using DotNetEnv;

namespace Util;
public static class Config
{
    public class Configuration
    {
        public string? TOKEN_SYMMETRIC_KEY { get; set; }
        public int TOKEN_DURATION_MINUTES { get; set; }
    }

    public static void LoadEnv(WebApplicationBuilder builder)
    {
        if (builder is null) throw new Exception("Builder not found");

        // Load ENV
        Env.Load("app.env");

        // Add environment variables to configuration
        builder.Configuration.AddEnvironmentVariables();

        // Bind configuration to a strongly-typed class
        builder.Services.Configure<Configuration>(builder.Configuration);

    }

}
