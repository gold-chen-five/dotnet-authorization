namespace Test.Token
{
    public interface IMaker
    {
        // CreateToken creates a new token for a specific username and duration
        Task<(string, Payload)> CreateTokenAsync(string username, TimeSpan duration);

        // VerifyToken checks if the token is valid or not
        Task<Payload> VerifyTokenAsync(string token);
    }
}