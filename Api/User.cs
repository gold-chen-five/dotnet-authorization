using Token;

namespace Api
{
    public static class User
    {
        public record LoginBody(string Username, string Password);

        public record LoginResponse(string Token, Payload Payload);

        public static async Task LoginUser(this Server server, HttpContext ctx)
        {
            var body = await ctx.Request.ReadFromJsonAsync<LoginBody>();
            if (body is null)
            {
                Handler.HandleErrorResponse(ctx, StatusCodes.Status400BadRequest, "Invalid login request");
                return;
            }

            // create token
            TimeSpan duration = TimeSpan.FromMinutes(15);
            var (token, payload) = server.TokenMaker.CreateToken(body.Username, duration);

            // response
            var rsp = new LoginResponse(token, payload);
            Handler.HandleResponseJson(ctx, StatusCodes.Status200OK, rsp);
            return;
        }

        public static async Task TestUser(HttpContext ctx)
        {
            var rsp = new { message = "test" };
            Handler.HandleResponseJson(ctx, StatusCodes.Status200OK, rsp);
            return;
        }
    }
}