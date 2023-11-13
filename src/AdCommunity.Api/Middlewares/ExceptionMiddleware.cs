using AdCommunity.Domain.Entities.Base;
using System.Net;

namespace AdCommunity.Api.Middlewares;

public class ExceptionMiddleware:IMiddleware
{
    private readonly Serilog.ILogger _logger;

    public ExceptionMiddleware(Serilog.ILogger logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred: {ErrorMessage}", ex.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            await context.Response.WriteAsync(new ErrorInfo()
            {
                StatusCode = context.Response.StatusCode,
                Message = "Internal Server Error from the custom middleware. Check logs for details."
            }.ToString());
        }
    }
}