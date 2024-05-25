
namespace Token
{
    public class Payload(Guid ID, string Username, DateTime IssuedAt, DateTime ExpiresAt)
    {
        public Guid ID { get; } = ID;
        public string Username { get; } = Username;
        public DateTime IssuedAt { get; } = IssuedAt;
        public DateTime ExpiresAt { get; } = ExpiresAt;
    }

    public static class PayloadUtility
    {
        public static Payload NewPayload(string username, TimeSpan duration)
        {
            var id = Guid.NewGuid();
            var issuedAt = DateTime.UtcNow;
            var expiresAt = issuedAt.Add(duration);

            return new Payload(id, username, issuedAt, expiresAt);
        }

        // Switch obj to Payload type.
        public static Payload ParsePayload(object idObj, object usernameObj, object iatObj, object expObj)
        {
            if (
                Guid.TryParse(idObj?.ToString(), out var id) &&
                usernameObj is string username &&
                DateTime.TryParse(iatObj?.ToString(), out var iat) &&
                DateTime.TryParse(expObj?.ToString(), out var exp)
            )
            {
                Payload payload = new(id, username, iat, exp);
                return payload;
            }
            else
            {
                throw new Exception("Payload contains invalid types.");
            }
        }
    }
}
