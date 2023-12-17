
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace AdCommunity.Core.Helpers
{
    public static class LocalizationHelper
    {
        public static string Translate(IStringLocalizer localizer, string key, params object[] parameters)
        {
            var lang = GetLanguageCode();

            var translation = localizer[lang, key];

            return translation ?? string.Format(key, parameters);
        }

        private static string GetLanguageCode()
        {
            var httpContext = GetHttpContextAccessor()?.HttpContext;
            var lang = httpContext?.Items["lang"]?.ToString();

            return string.IsNullOrEmpty(lang) ? "en" : lang;
        }

        private static IHttpContextAccessor GetHttpContextAccessor()
        {
            try
            {
                return new HttpContextAccessor();
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }
    }
}
