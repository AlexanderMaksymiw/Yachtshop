using AlexAPI.Data.DAL.WorkUnits;
using AlexAPI.Services;

namespace AlexAPI.Data.DAL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInjections(this IServiceCollection services)
        {
            //Database Initialization
            services.AddScoped<IDbInitializer, DbInitializer>();

            //Services Setup
            services.AddTransient<IMailService, MailService>();
            services.AddTransient<IFileTemplateService, FileTemplateService>();
            services.AddTransient<IGeminiAIService, GeminiAIService>();

            //Work Unit
            services.AddTransient<YachtWorkUnit>();
            return services;
        }
    }
}
