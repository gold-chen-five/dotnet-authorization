using Token;

namespace Api.Services
{
    public static class User
    {
        private record LoginBody(string Username, string Password);

        private record LoginResponse(string Token, Payload Payload);

        public static async Task LoginUser(this Server server, HttpResponse response, HttpContext ctx)
        {
            var body = await ctx.Request.ReadFromJsonAsync<LoginBody>();
            if (body is null)
            {
                Handler.HandleErrorResponse(ctx, StatusCodes.Status400BadRequest, "Invalid login request");
                return;
            }

            // create token
            TimeSpan duration = TimeSpan.FromMinutes(1);
            var (token, payload) = server.TokenMaker.CreateToken(body.Username, duration);

            // response
            var rsp = new LoginResponse(token, payload);
            Handler.HandleResponseJson(ctx, StatusCodes.Status200OK, rsp);
            return;
        }

        public static async Task TestUser(this Server server, HttpResponse response, HttpContext ctx)
        {
            var payload = ctx.Items["Payload"];
            if(payload is null)
            {
                Handler.HandleErrorResponse(ctx, StatusCodes.Status404NotFound, "Payload not found.");
                return;
            }
            Handler.HandleResponseJson(ctx, StatusCodes.Status200OK, payload);
            return;
        }
    }
}