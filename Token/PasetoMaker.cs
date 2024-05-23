using System.Text;
using System.Text.Json;
using Paseto;
using Paseto.Builder;

namespace Token.PasetoMaker
{
    public class PasetoMaker : IMaker
    {
        private readonly Paseto.Cryptography.Key.PasetoAsymmetricKeyPair _pasetoKey;

        public PasetoMaker(string symmetricKey)
        {
            var seed = Encoding.UTF8.GetBytes(symmetricKey);
            if (seed.Length != 32)
            {
                throw new Exception("seed length should be 32 length !");
            }
            var pasetoBuilder = new PasetoBuilder().UseV4(Purpose.Public);
            _pasetoKey = pasetoBuilder.GenerateAsymmetricKeyPair(seed);
        }

        public (string, Payload) CreateToken(string username, TimeSpan duration)
        {
            // create payload
            Payload payload = PayloadUtility.NewPayload(username, duration);

            // create token
            var pasetoBuilder = new PasetoBuilder().UseV4(Purpose.Public);
            var token = pasetoBuilder
                .WithKey(_pasetoKey.SecretKey)
                .Subject(payload.ID.ToString())
                .IssuedAt(payload.IssuedAt)
                .Expiration(payload.ExpiresAt)
                .AddClaim("Username", payload.Username)
                .Encode();

            return (token, payload);
        }

        public Payload VerifyToken(string token)
        {
            if(string.IsNullOrEmpty(token))
            {
                throw new Exception("Token validation failed.");
            }

            var valParams = new PasetoTokenValidationParameters
            {
                ValidateLifetime = true,
            };

            var pasetoBuilder = new PasetoBuilder().UseV4(Purpose.Public);
            var result = pasetoBuilder
                .WithKey(_pasetoKey.PublicKey)
                .Decode(token, valParams);

            if (!result.IsValid)
            {
                throw new Exception("Token validation failed.");
            }

            // Deserialize the PasetoPayload to Payload
            var pasetoPayload = result.Paseto.Payload;
            if(IsCheckedPasetoPayload(pasetoPayload, out var p))
            {
                var payload = PayloadUtility.ParsePayload(p.Id, p.Username, p.Iat, p.Exp);
                return payload;
            }
            else
            {
                throw new Exception("Payload missing.");
            }
        }

        private struct CheckPayload
        {
           public object Id { get; set;}
           public object Username { get; set;}
           public object Iat { get; set;}
           public object Exp { get; set;}
        }

        private static bool IsCheckedPasetoPayload(PasetoPayload pasetoPayload , out CheckPayload payload)
        {
            payload = default;
            if(
                pasetoPayload.TryGetValue("sub", out var id) && 
                pasetoPayload.TryGetValue("Username", out var username) &&
                pasetoPayload.TryGetValue("iat", out var iat) && 
                pasetoPayload.TryGetValue("exp", out var exp) 
            )
            {
                payload = new CheckPayload
                {
                    Id = id,
                    Username = username,
                    Iat = iat,
                    Exp = exp
                };
                return true;
            }
            return false;
        }
    }
}
