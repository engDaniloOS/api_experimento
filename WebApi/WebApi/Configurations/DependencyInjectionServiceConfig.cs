using WebApi.Domain;
using WebApi.Domain.UseCases;

namespace WebApi.Configurations
{
    public static class DependencyInjectionServiceConfig
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddSingleton<ICarService, CarService>();
        }
    }
}
