using WebApi.DataProviders;
using WebApi.Domain;
using WebApi.Domain.Providers;
using WebApi.Domain.UseCases;

namespace WebApi.Configurations
{
    public static class DependencyInjectionServiceConfig
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddSingleton<ICarService, CarService>();

            services.AddSingleton<ICarDataProvider, CarDataProvider>();
        }
    }
}
