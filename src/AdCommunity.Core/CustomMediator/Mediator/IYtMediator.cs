using AdCommunity.Core.CustomMediator.Request;

namespace AdCommunity.Core.Extensions.Mediator;

public interface IYtMediator
{
    Task<TResponse> Send<TResponse>(IYtRequest<TResponse> request,CancellationToken cancellationToken=default);
}