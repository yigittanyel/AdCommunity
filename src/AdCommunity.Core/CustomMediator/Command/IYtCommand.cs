using AdCommunity.Core.CustomMediator.Request;

namespace AdCommunity.Core.Extensions.Command;

public interface IYtCommand<out TResponse> : IYtRequest<TResponse>
{
}