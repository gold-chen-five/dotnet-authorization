using Grpc.Core;
using Protos;
using Token;
using Util;
using Google.Protobuf.WellKnownTypes;

namespace Gapi.Services;

public partial class AuthService : Auth.AuthBase
{
    public override Task<LoginUserResponse> LoginUser(LoginUserRequest request, ServerCallContext context)
    {
        // Validate user credentials (this is a simplified example)
        if (request.Username == "testuser" && request.Password == "password")
        {
            // Generate token
            TimeSpan duration = TimeSpan.FromMinutes(_config.TOKEN_DURATION_MINUTES);
            var (token, payload) = _tokenMaker.CreateToken(request.Username, duration);
            var protoPayload = new Protos.Payload
            {
                Username = payload.Username,
                Id = payload.ID.ToString(),
                IssuedAt = Timestamp.FromDateTime(payload.IssuedAt),
                ExpiresAt = Timestamp.FromDateTime(payload.ExpiresAt)
            };

            return Task.FromResult(new LoginUserResponse
            {
                Token = token,
                Payload = protoPayload
            });
        }
        else
        {
            // Invalid credentials
            throw new RpcException(new Status(StatusCode.Unauthenticated, "Invalid username or password"));
        }
    }
}