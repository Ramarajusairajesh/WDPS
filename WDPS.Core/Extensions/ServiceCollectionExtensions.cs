using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using WDPS.Core.Configuration;
using WDPS.Core.Data;
using WDPS.Core.Services;

namespace WDPS.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWDPServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure settings
            services.Configure<AppSettings>(configuration.GetSection("AppSettings"));

            // Configure database
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

            // Configure logging
            var loggingSettings = configuration.GetSection("AppSettings:Logging").Get<AppSettings.LoggingSettings>();
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(loggingSettings.LogFilePath)
                .MinimumLevel.Information()
                .CreateLogger();

            services.AddSingleton(Log.Logger);

            // Register services
            services.AddScoped<SystemMetricsService>();
            services.AddHostedService<MetricsCollectionService>();

            return services;
        }
    }
} 