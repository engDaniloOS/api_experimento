using WebApi.Configurations;
using Prometheus;
using Serilog;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            SwaggerServiceConfig.Configure(builder.Services);
            MemoryCacheServiceConfig.Configure(builder.Services);
            HttpClientServiceConfig.Configure(builder.Services);
            DependencyInjectionServiceConfig.Configure(builder.Services);
            MetricsServiceConfig.Configure(builder.Services);
            LogServiceConfig.Configure();

            builder.Host.UseSerilog();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //from prometheus config
            app.UseHttpMetrics();

            app.UseAuthorization();

            app.MapControllers();
            
            //from prometheus config
            app.MapMetrics();

            //from health check
            app.MapHealthChecks("/health");

            //latency
            app.UseMiddleware<RequestMiddleware>();

            app.Run();
        }
    }
}