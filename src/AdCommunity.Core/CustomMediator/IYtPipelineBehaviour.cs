using AdCommunity.Core.CustomMediator.Request;

namespace AdCommunity.Core.Extensions;

public interface IYtPipelineBehavior<in TRequest, TResponse> where TRequest : IYtRequest<TResponse>
{
    Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken);
}

