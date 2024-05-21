using System.Text;
using System.Text.Json;
using Paseto;
using Paseto.Builder;

namespace Token
{
    public class PasetoMaker : IMaker
    {
        private readonly Paseto.Cryptography.Key.PasetoAsymmetricKeyPair _pasetoKey;

        private readonly PasetoBuilder _pasetoBuilder;

        public PasetoMaker(string symmetricKey)
        {
            var seed = Encoding.UTF8.GetBytes(symmetricKey);
            if(seed.Length != 32)
            {
                throw new Exception("seed length should be 32 length !");
            }
            _pasetoBuilder = new PasetoBuilder().UseV4(Purpose.Public);
            _pasetoKey = _pasetoBuilder.GenerateAsymmetricKeyPair(seed);
        }

        public (string, Payload) CreateToken(string username, TimeSpan duration)
        {
            // create payload
            Payload payload = PayloadUtility.NewPayload(username, duration);

            // create token
            var token = _pasetoBuilder
                .WithKey(_pasetoKey.PublicKey)
                .Subject(payload.ID.ToString())
                .IssuedAt(payload.IssuedAt)
                .Expiration(payload.ExpiresAt)
                .AddClaim("Username", payload.Username)
                .Encode();

            return (token, payload);
        }

        public Payload VerifyToken(string token)
        {
            var valParams = new PasetoTokenValidationParameters
            {
                ValidateLifetime = true,
                ValidateSubject = true,
            };

            var result = _pasetoBuilder
                .WithKey(_pasetoKey.PublicKey)
                .Decode(token, valParams);

            if (!result.IsValid)
            {
                throw new Exception("Token validation failed.");
            }

            // Deserialize the PasetoPayload into Payload
            var pasetoPayload = result.Paseto.Payload;
            var payloadJson = JsonSerializer.Serialize(pasetoPayload);
            var payload = JsonSerializer.Deserialize<Payload>(payloadJson);

            return payload;
        }
    }
}
