using AdCommunity.Core.CustomMediator.Interfaces;
using System.Collections.Concurrent;

namespace AdCommunity.Core.CustomMediator;

internal class YtMediator : IYtMediator
{
    private readonly IServiceProvider _serviceProvider;
    private static readonly ConcurrentDictionary<Type, dynamic> _requestHandlers = new();

    public YtMediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task<TResponse> Send<TResponse>(IYtRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var requestType = request.GetType();

        var handler = _requestHandlers.GetOrAdd(requestType, (Activator.CreateInstance(typeof(YtRequestHandlerWrapper<,>).MakeGenericType(requestType, typeof(TResponse)))
                                             ?? throw new InvalidOperationException($"Could not create wrapper type for {requestType}")));


        return handler.Handle(request, _serviceProvider, cancellationToken);
    }
}
