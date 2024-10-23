using Serilog;
using Serilog.Events;

namespace WebApi.Configurations
{
    public static class LogServiceConfig
    {
        private const string OUTPUT_TEMPLATE = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";
        public static void Configure()
        {
            Log.Logger = new LoggerConfiguration()
                                .Enrich.WithProperty("ApplicationName", "App test")
                                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                                .MinimumLevel.Override("System", LogEventLevel.Information)
                                .MinimumLevel.Information()
                                .WriteTo.Console(outputTemplate: OUTPUT_TEMPLATE)
                                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Hour, outputTemplate: OUTPUT_TEMPLATE)
                                .CreateLogger();
        }
    }
}
