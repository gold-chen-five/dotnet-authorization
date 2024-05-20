namespace Test.Api
{
    public static class Server
    {
        public readonly struct Payload(Guid ID, string Username, DateTime IssuedAt, DateTime ExpiresAt)
        {
            public Guid ID { get; } = ID;
            public string Username { get; } = Username;
            public DateTime IssuedAt { get; } = IssuedAt;
            public DateTime ExpiresAt { get; } = ExpiresAt;
        }

        public static WebApplication NewServer(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            return app;
        }

        public static void SetupRouter(this WebApplication app)
        {
            RouteGroupBuilder apiRouter = app.MapGroup("/api");
            apiRouter.MapGet("/test", () =>
            {
                return TypedResults.Ok("test");
            });

        }

    }
}