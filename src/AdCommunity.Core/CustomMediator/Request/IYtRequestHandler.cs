namespace AdCommunity.Core.CustomMediator.Request;

public interface IYtRequestHandler<in TRequest, TResponse> where TRequest : IYtRequest<TResponse>
{
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}