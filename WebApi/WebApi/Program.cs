using WebApi.Configurations;
using Prometheus;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            SwaggerServiceConfig.Configure(builder.Services);
            HttpClientServiceConfig.Configure(builder.Services);
            MemoryCacheServiceConfig.Configure(builder.Services);
            DependencyInjectionServiceConfig.Configure(builder.Services);
            MetricsServiceConfig.Configure(builder.Services);

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
            app.UseMiddleware<RequestLatencyMiddleware>();

            app.Run();
        }
    }
}