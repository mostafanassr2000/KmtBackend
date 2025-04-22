using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Threading.Tasks;

namespace KmtBackend.Infrastructure.Localization
{
    // Middleware to handle request localization
    public class LocalizationMiddleware
    {
        // Next middleware in pipeline
        private readonly RequestDelegate _next;
        
        // Constructor with request delegate
        public LocalizationMiddleware(RequestDelegate next)
        {
            // Store next middleware
            _next = next;
        }
        
        // Process the request
        public async Task InvokeAsync(HttpContext context)
        {
            // Extract language from Accept-Language header
            var language = context.Request.Headers["Accept-Language"].ToString();
            
            // Default to English if not specified
            if (string.IsNullOrWhiteSpace(language))
            {
                // Proceed with default culture
                await _next(context);
                return;
            }
            
            // Handle Arabic language
            if (language.Contains("ar"))
            {
                // Set Arabic culture
                var arabicCulture = new CultureInfo("ar-SA");
                CultureInfo.CurrentCulture = arabicCulture;
                CultureInfo.CurrentUICulture = arabicCulture;
            }
            else
            {
                // Set English culture (default)
                var englishCulture = new CultureInfo("en-US");
                CultureInfo.CurrentCulture = englishCulture;
                CultureInfo.CurrentUICulture = englishCulture;
            }
            
            // Continue with the pipeline
            await _next(context);
        }
    }
}
