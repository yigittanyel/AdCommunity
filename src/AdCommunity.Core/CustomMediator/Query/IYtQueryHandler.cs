namespace AdCommunity.Core.Extensions.Query;

public interface IYtQueryHandler<in TRequest, TResponse> : IYtRequestHandler<TRequest, TResponse> where TRequest : IYtQuery<TResponse>
{
}
