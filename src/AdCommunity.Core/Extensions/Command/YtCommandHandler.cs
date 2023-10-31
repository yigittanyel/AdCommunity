namespace AdCommunity.Core.Extensions.Command;

public abstract class YtCommandHandler<TRequest, TResponse> : IYtCommandHandler<TRequest, TResponse> where TRequest : IYtCommand<TResponse>
{
    public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}