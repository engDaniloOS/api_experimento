using WebApi.DataProviders;
using WebApi.DataProviders.Commons;
using WebApi.Domain;
using WebApi.Domain.Providers;
using WebApi.Domain.UseCases;

namespace WebApi.Configurations
{
    public static class DependencyInjectionServiceConfig
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddScoped<ICarService, CarService>();

            services.AddScoped<ICarDataProvider, CarDataProvider>();

            services.AddSingleton<HttpCacheProvider>();
        }
    }
}
