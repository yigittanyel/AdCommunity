using AdCommunity.Core.CustomMediator.Interfaces;

namespace AdCommunity.Application.Features.PipelineExample;

public class ExampleReqHandler : IYtRequestHandler<ExampleReq, string>
{
    public async Task<string> Handle(ExampleReq request, CancellationToken cancellationToken)
    {
        return "Handled: " + request.Message;
    }
}
