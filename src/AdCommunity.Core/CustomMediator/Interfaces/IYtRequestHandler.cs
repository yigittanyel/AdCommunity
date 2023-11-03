namespace AdCommunity.Core.CustomMediator.Interfaces;

public interface IYtRequestHandler<TRequest, TResponse> where TRequest : IYtRequest<TResponse>
{
    Task<TResponse> Handle(TRequest request);
}

