using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using System.Reflection;

namespace VsSummitApi.Helpers;
public static class DependencyInjectionExtensions
{
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

        logger.Information($"Log filename: {fileName} - Level: {Enum.GetValues(typeof(LogEventLevel)).Cast<LogEventLevel>().Where(logger.IsEnabled).Min()}");

        builder.Host.UseSerilog(logger);
    }
}