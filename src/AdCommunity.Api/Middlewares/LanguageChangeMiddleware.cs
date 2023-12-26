using Microsoft.AspNetCore.Localization;

namespace AdCommunity.Api.Middlewares;
public class LanguageChangeMiddleware
{
    private readonly RequestDelegate _next;

    public LanguageChangeMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var culture = context.Request.Query["culture"].ToString();

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
        }

        await _next(context);
    }
}
