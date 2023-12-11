namespace AdCommunity.Api.Middlewares;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System;
using System.Threading.Tasks;

public class LanguageChangeMiddleware
{
    private readonly RequestDelegate _next;

    public LanguageChangeMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var culture = context.Request.Query["culture"].ToString(); //culture yok. lang olarak gönder

        if (!string.IsNullOrEmpty(culture))
        {
            var cookieOptions = new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddYears(1),
                IsEssential = true,
                HttpOnly = false
            };

            context.Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                cookieOptions
            );

            context.Items["LanguageCode"] = culture;
        }

        await _next(context);
    }
}
