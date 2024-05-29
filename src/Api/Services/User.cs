using Token;
using Api.Val;
namespace Api.Services;
public static class User
{
    private record LoginResponse(string Token, Payload Payload);

    public static async Task LoginUser(this Server server, HttpResponse response, HttpContext ctx)
    {
        try
        {
            var body = await UserValidation.LoginValidation(ctx);

            // create token
            TimeSpan duration = TimeSpan.FromMinutes(server.Config.TOKEN_DURATION_MINUTES);
            var (token, payload) = server.TokenMaker.CreateToken(body!.Username, duration);

            // response
            var rsp = new LoginResponse(token, payload);
            Handler.HandleResponseJson(ctx, StatusCodes.Status200OK, rsp);
            return;
        }
        catch (Exception err)
        {
            Handler.HandleErrorResponse(ctx, StatusCodes.Status400BadRequest, err.Message);
            return;
        }

    }

    public static async Task TestUser(this Server server, HttpResponse response, HttpContext ctx)
    {
        var payload = ctx.Items["Payload"];
        if (payload is null)
        {
            Handler.HandleErrorResponse(ctx, StatusCodes.Status404NotFound, "Payload is not found.");
            return;
        }
        Handler.HandleResponseJson(ctx, StatusCodes.Status200OK, payload);
        return;
    }
}
