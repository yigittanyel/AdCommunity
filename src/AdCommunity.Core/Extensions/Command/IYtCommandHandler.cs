namespace AdCommunity.Core.Extensions.Command;

public interface IYtCommandHandler<in TRequest, TResponse> : IYtRequestHandler<TRequest, TResponse> where TRequest : IYtCommand<TResponse>
{
}
