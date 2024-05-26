
namespace Api.Val;
public static class UserValidation
{
    public record LoginBody(string Username, string Password);

    public static async Task<LoginBody> LoginValidation(HttpContext ctx)
    {

        var rqBody = await ctx.Request.ReadFromJsonAsync<LoginBody>() ?? throw new Exception("Request Body is required.");

        if (string.IsNullOrWhiteSpace(rqBody.Username))
        {
            throw new Exception("Username is required.");
        }

        if (string.IsNullOrWhiteSpace(rqBody.Password))
        {
            throw new Exception("Password is required.");
        }

        return rqBody;
    }
}