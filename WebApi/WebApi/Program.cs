using WebApi.Configurations;

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

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}