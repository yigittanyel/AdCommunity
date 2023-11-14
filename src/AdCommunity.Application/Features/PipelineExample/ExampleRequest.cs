using AdCommunity.Core.CustomMediator.Interfaces;

namespace AdCommunity.Application.Features.PipelineExample;

public class ExampleReq : IYtRequest<string>
{
    public string Message { get; set; }
}
