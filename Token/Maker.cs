namespace Token
{
    public interface IMaker
    {
        // CreateToken creates a new token for a specific username and duration
        (string, Payload) CreateToken(string username, TimeSpan duration);

        // VerifyToken checks if the token is valid or not
        Payload VerifyToken(string token);
    }
}