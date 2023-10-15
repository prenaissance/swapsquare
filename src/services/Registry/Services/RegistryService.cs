using Grpc.Core;
using GrpcRegistry;

namespace SwapSquare.Registry.Services;

public class RegistryService(ILogger<RegistryService> logger) : RegistryGrpc.RegistryGrpcBase
{
    public override Task<ItemTemplateDto> RegisterItemTemplate(
        RegisterItemTemplateCommand request,
        ServerCallContext context)
    {
        logger.LogInformation("RegisterItemTemplate");
        throw new NotImplementedException();
    }

    public override Task<DeleteResult> DeleteItemTemplate(
        DeleteItemTemplateCommand request,
        ServerCallContext context)
    {
        logger.LogInformation("DeleteItemTemplate");
        throw new NotImplementedException();
    }
}
