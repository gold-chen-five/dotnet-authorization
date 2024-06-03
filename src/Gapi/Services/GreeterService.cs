using Grpc.Core;
using Protos;
using Token;
using Util;

namespace Gapi.Services;

public partial class AuthService(ILogger<AuthService> logger, IMaker tokenMaker, Config.Configuration config) : Auth.AuthBase
{
    private readonly ILogger<AuthService> _logger = logger;
    private readonly IMaker _tokenMaker = tokenMaker;
    private readonly Config.Configuration _config = config;

    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        return Task.FromResult(new HelloReply
        {
            Message = "Hello " + request.Name
        });
    }
}
