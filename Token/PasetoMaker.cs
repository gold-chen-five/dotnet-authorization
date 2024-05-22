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

            // Deserialize the PasetoPayload into Payload
            var pasetoPayload = result.Paseto.Payload;
            
            var payloadJson = JsonSerializer.Serialize(pasetoPayload);
            Console.WriteLine(payloadJson);
            var payload = JsonSerializer.Deserialize<Payload>(payloadJson);
            //var payload = JsonSerializer.Deserialize<Payload>(payloadJson);

            return payload;
        }
    }
}
