using AlexAPI.Data.DAL.WorkUnits;
using AlexAPI.Services;
using AlexAPI.Services.Interfaces;

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
            services.AddTransient<ICSVService, CSVService>();
            services.AddTransient<IOpenAIService, OpenAIService>();
            services.AddTransient<IFTPService, FTPService>();

            //Work Unit
            services.AddTransient<YachtWorkUnit>();
            services.AddTransient<ItineraryWorkUnit>();
            return services;
        }
    }
}
