using AdCommunity.Core.CustomMediator.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AdCommunity.Core.CustomMediator;

public delegate Task<TResponse> RequestHandlerDelegate<TResponse>();

internal class YtRequestHandlerWrapper<TRequest, TResponse>
    where TRequest : IYtRequest<TResponse>
{
    
    public YtRequestHandlerWrapper()
    {
    }

    public Task<TResponse> Handle(IYtRequest<TResponse> request, IServiceProvider serviceProvider,
CancellationToken cancellationToken)
    {
        Task<TResponse> Handler() => serviceProvider.GetRequiredService<IYtRequestHandler<TRequest, TResponse>>().Handle((TRequest)request, cancellationToken);
        return serviceProvider
            .GetServices<IYtPipelineBehavior<TRequest, TResponse>>()
            .Reverse()
            .Aggregate((RequestHandlerDelegate<TResponse>)Handler, (next, pipeline) => () => pipeline.Handle((TRequest)request, next, cancellationToken))();
    }
}