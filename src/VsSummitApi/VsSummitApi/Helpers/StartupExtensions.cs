using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using System.Reflection;
using System.Threading.RateLimiting;
using VsSummitApi.Data;

namespace VsSummitApi.Helpers;
public static class StartupExtensions
{
    public static void AddStartupConfig(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddHealthChecks();
        services.ConfigureDatabase(configuration);
        services.ConfigureOutputCache();
        services.AddLimiterRules();
        services.ScanDependencyInjection(Assembly.GetExecutingAssembly(), "Service");
    }
    public static void ScanDependencyInjection(this IServiceCollection services, Assembly projectAssembly,
        string classesEndWith)
    {
        var types = projectAssembly.GetTypes().Where(x => x.GetInterfaces().Any(i => i.Name.EndsWith(classesEndWith)));

        foreach (var type in types)
        {
            var interfaces = type.GetInterfaces();
            foreach (var inter in interfaces)
                services.AddScoped(inter, type);
        }
    }

    public static void ConfigureLogging(this WebApplicationBuilder builder)
    {
        var shortDate = DateTime.Now.ToString("yyyy-MM-dd_HH");
        var path = builder.Configuration.GetSection("LoggerBasePath").Value;
        var fileName = $@"{path}\{shortDate}.log";
        var logTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";

        var configuration = builder.Configuration;
        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("ApplicationName", $"Api VsSummit - {builder.Environment.EnvironmentName} ")
            .WriteTo.Console()
            .WriteTo.File(fileName, outputTemplate: logTemplate)
            .CreateLogger();

        logger.Information($"Log filename: {fileName} - Level:" +
            $" {Enum.GetValues(typeof(LogEventLevel)).Cast<LogEventLevel>().Where(logger.IsEnabled).Min()}");

        builder.Host.UseSerilog(logger);
    }

    public static void AddLimiterRules(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
            options.AddFixedWindowLimiter(policyName: "fixed", options =>
                    {
                        options.PermitLimit = 4;
                        options.Window = TimeSpan.FromSeconds(20);
                        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                        options.QueueLimit = 0;
                    })
                    .AddFixedWindowLimiter(policyName: "period3", options =>
                    {
                        options.PermitLimit = 4;
                        options.Window = TimeSpan.FromSeconds(20);
                        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                        options.QueueLimit = 3;
                    })
                    .OnRejected = async (context, token) =>
                    {
                        context.HttpContext.Response.StatusCode = 429;
                        if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                        {
                            await context.HttpContext.Response.WriteAsync(
                                $"Too many requests. Please try again after {retryAfter.TotalMinutes} minute(s). " +
                                $"Read more about our rate limits at https://example.org/docs/ratelimiting.", cancellationToken: token);
                        }
                        else
                        {
                            await context.HttpContext.Response.WriteAsync(
                                "Too many requests. Please try again later. " +
                                "Read more about our rate limits at https://example.org/docs/ratelimiting.", cancellationToken: token);
                        }
                    }
              );
    }

    public static void ConfigureDatabase(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddDbContext<VsSummitApiContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("VsSummitApiContext") ?? throw new InvalidOperationException("Connection string 'VsSummitApiContext' not found.")));
    }

    public static void ConfigureOutputCache(this IServiceCollection services)
    {
        services.AddOutputCache(options =>
        {
            options.DefaultExpirationTimeSpan = TimeSpan.FromSeconds(10);
        });
    }

}