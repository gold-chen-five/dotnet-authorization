using Grpc.Core;
using Protos;

namespace Gapi.Services;

public class GreeterService(ILogger<GreeterService> logger) : Auth.AuthBase
{
    private readonly ILogger<GreeterService> _logger = logger;

    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        return Task.FromResult(new HelloReply
        {
            Message = "Hello " + request.Name
        });
    }
}
