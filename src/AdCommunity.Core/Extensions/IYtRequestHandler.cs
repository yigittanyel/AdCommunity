namespace AdCommunity.Core.Extensions;

public interface IYtRequestHandler<in TRequest, TResponse> where TRequest : IYtRequest<TResponse>
{
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}