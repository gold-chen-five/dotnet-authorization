using Token;
using Token.PasetoMaker;

namespace Api
{
    public partial class Server
    {
        private readonly WebApplication _app;
        private readonly IMaker _tokenMaker;

        public Server(string[] args)
        {
            string symmetricKey = "12345678901234567890123456789012";
            _tokenMaker = new PasetoMaker(symmetricKey);
            _app = SetupApp(args);

            SetupRouter();
            SetupSwagger();
            SetupMiddleware();

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

        private void SetupSwagger()
        {
            // Configure the HTTP request pipeline.
            if (_app.Environment.IsDevelopment())
            {
                _app.UseSwagger();
                _app.UseSwaggerUI();
            }
        }

        private void SetupRouter()
        {
            // No Auth router
            _app.MapPost("/login-user", LoginUser);

            // Api has auth 
            RouteGroupBuilder apiRouter = _app.MapGroup("/api");
            apiRouter.MapGet("/test", TestUser);
        }

        public void Start()
        {
            _app.Run();
        }
    }
}