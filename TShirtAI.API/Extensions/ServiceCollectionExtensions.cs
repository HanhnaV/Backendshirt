using Microsoft.AspNetCore.Mvc;
using TShirtAI.API.Middlewares;

namespace TShirtAI.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            // Configure API Behavior Options
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true; // We'll handle validation manually
            });

            // Add custom filters
            services.AddScoped<ValidateModelAttribute>();

            return services;
        }
    }
}