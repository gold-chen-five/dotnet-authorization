using Token;

namespace Api
{
    public partial class Server
    {
        private record LoginBody(string Username, string Password);

        private record LoginResponse(string Token, Payload Payload);


        private async Task LoginUser(HttpResponse response, HttpContext ctx)
        {
            var body = await ctx.Request.ReadFromJsonAsync<LoginBody>();
            if (body is null)
            {
                Handler.HandleErrorResponse(ctx, StatusCodes.Status400BadRequest, "Invalid login request");
                return;
            }

            // create token
            TimeSpan duration = TimeSpan.FromMinutes(1);
            var (token, payload) = _tokenMaker.CreateToken(body.Username, duration);

            // response
            var rsp = new LoginResponse(token, payload);
            Handler.HandleResponseJson(ctx, StatusCodes.Status200OK, rsp);
            return;
        }

        private async Task TestUser(HttpResponse response, HttpContext ctx)
        {
            var rsp = new { message = "test" };
            Handler.HandleResponseJson(ctx, StatusCodes.Status200OK, rsp);
            return;
        }
    }
    // public static class User
    // {
    //     public record LoginBody(string Username, string Password);

    //     public record LoginResponse(string Token, Payload Payload);

    //     public static async Task LoginUser(this Server server, HttpResponse response, HttpContext ctx)
    //     {
    //         var body = await ctx.Request.ReadFromJsonAsync<LoginBody>();
    //         if (body is null)
    //         {
    //             Handler.HandleErrorResponse(ctx, StatusCodes.Status400BadRequest, "Invalid login request");
    //             return;
    //         }

    //         // create token
    //         TimeSpan duration = TimeSpan.FromMinutes(1);
    //         var (token, payload) = server.TokenMaker.CreateToken(body.Username, duration);

    //         // response
    //         var rsp = new LoginResponse(token, payload);
    //         Handler.HandleResponseJson(ctx, StatusCodes.Status200OK, rsp);
    //         return;
    //     }

    //     public static async Task TestUser(this Server server, HttpResponse response, HttpContext ctx)
    //     {
    //         var rsp = new { message = "test" };
    //         Handler.HandleResponseJson(ctx, StatusCodes.Status200OK, rsp);
    //         return;
    //     }
    // }
}