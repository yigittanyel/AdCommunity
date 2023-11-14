using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Core.CustomMediator;
using Microsoft.Extensions.Logging;
using System.Reflection.Metadata;

namespace AdCommunity.Application.Features.PipelineExample;

public class LoggingBehavior<TReq, TRes> : IYtPipelineBehavior<TReq, TRes>
    where TReq : IYtRequest<TRes>
{
    private readonly ILogger<LoggingBehavior<TReq, TRes>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TReq, TRes>> logger)
    {
        _logger = logger;
    }

    //buradaki delegate bir isteği işlemek üzere sonraki işleme geçmeyi temsil eder.
    //Handle metodunun başında loglama yapılır.
    //next() çağrısı ile bir sonraki aşama(davranış veya işleyici) çalıştırılır.
    //next() tarafından dönen sonuç alınır.
    //Handle metodunun sonunda loglama yapılır.
    public async Task<TRes> Handle(TReq req, RequestHandlerDelegate<TRes> next, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Request processing: {req.GetType().Name}");

        var response = await next();

        _logger.LogInformation($"Request processing completed.: {req.GetType().Name}");

        return response;
    }
}
