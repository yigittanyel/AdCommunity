namespace AdCommunity.Core.Extensions;

public interface IYtPipelineBehavior<in TRequest, TResponse> where TRequest : IYtRequest<TResponse>
{
    Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken);
}

public delegate Task<TResponse> RequestHandlerDelegate<TResponse>();
