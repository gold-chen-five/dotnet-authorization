using System.Text.Json;
using Token;

namespace Api
{
    public static class User
    {
        public record LoginBody(string User, string Password);

        public record LoginResponse(string Token, Payload Payload);

        public static async Task<IResult> LoginUser(Server.ServerType server, HttpRequest request)
        {
            var body = await request.ReadFromJsonAsync<LoginBody>();
            
            // create token
            TimeSpan duration = TimeSpan.FromMinutes(15);
            var (token, payload) = server.TokenMaker.CreateToken(body.User, duration);

            //response
            var rsp = new LoginResponse(token, payload);
            //var rsp = new { message = "123456" };
            var jsonRsp = JsonSerializer.Serialize(rsp);

            return Results.Content(jsonRsp, "application/json");
        }
    }
}