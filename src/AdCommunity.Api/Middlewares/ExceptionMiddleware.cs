using AdCommunity.Domain.Entities.Base;
using Serilog;
using System.Net;

namespace AdCommunity.Api.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred: {ErrorMessage}", ex.Message);

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            await httpContext.Response.WriteAsync(new ErrorInfo()
            {
                StatusCode = httpContext.Response.StatusCode,
                Message = "Internal Server Error from the custom middleware. Check logs for details."
            }.ToString());
        }
    }
}