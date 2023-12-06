namespace AdCommunity.Core.CustomMediator.Interfaces;

public interface IYtRequest<TResponse>
{
    bool IsCommand { get; }
}

