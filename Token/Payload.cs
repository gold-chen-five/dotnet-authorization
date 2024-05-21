
namespace Token
{
    public readonly struct Payload(Guid ID, string Username, DateTime IssuedAt, DateTime ExpiresAt)
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
    }
}
