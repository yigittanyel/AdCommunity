namespace AdCommunity.Core.Extensions.Query;

public abstract class YtQueryHandler<TRequest, TResponse> : IYtQueryHandler<TRequest, TResponse> where TRequest : IYtQuery<TResponse>
{
    public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}