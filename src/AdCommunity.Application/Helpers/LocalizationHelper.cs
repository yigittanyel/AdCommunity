using Microsoft.AspNetCore.Http;
using System.Resources;

namespace AdCommunity.Application.Helpers
{
    public static class LocalizationHelper
    {
        private static ResourceManager resourceManager;

        static LocalizationHelper()
        {
            resourceManager = new ResourceManager("AdCommunity.Application.Resources.ApplicationResource", typeof(LocalizationHelper).Assembly);
        }

        public static string Translate(string key, string languageCode = "en")
        {
            var translation = resourceManager.GetString(key, new System.Globalization.CultureInfo(languageCode));

            return translation ?? key;
        }

        public static string TranslateWithEntity(string key, string entityName, string languageCode = "en")
        {
            var baseTranslation = Translate(key, languageCode);
            return string.Format(baseTranslation, entityName);
        }

        public static string GetLanguageCode(HttpContext httpContext)
        {
            // Dil bilgisini HttpContext içinde sakladığımız yerden al
            var languageCode = httpContext.Items["LanguageCode"]?.ToString();

            return string.IsNullOrEmpty(languageCode) ? "en" : languageCode;
        }
    }
}
