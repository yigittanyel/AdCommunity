using AdCommunity.Core.CustomMediator.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AdCommunity.Core.CustomMediator;

public class YtMediator : IYtMediator
{
    public Task<TResponse> Send<TResponse>(IYtRequest<TResponse> request)
    {
        var reqType = request.GetType();

        var reqTypeInterface = reqType.GetInterfaces()
            .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IYtRequest<>))
            .FirstOrDefault();

        var responseType = reqTypeInterface.GetGenericArguments()[0];


        var genericType = typeof(IYtRequestHandler<,>).MakeGenericType(reqType, responseType);

        var handler = YtServiceProvider.ServiceProvicer.GetService(genericType);

        return (Task<TResponse>)genericType.GetMethod("Handle").Invoke(handler, new object[] { request });
    }
}

