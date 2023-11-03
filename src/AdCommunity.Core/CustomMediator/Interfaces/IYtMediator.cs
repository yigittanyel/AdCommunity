namespace AdCommunity.Core.CustomMediator.Interfaces;

public interface IYtMediator
{
    Task<TResponse> Send<TResponse>(IYtRequest<TResponse> request, CancellationToken cancellationToken = default);
}

