using WebApi.Entrypoints.Handlers;

namespace WebApi.Configurations
{
    public static class ExceptionHandlerConfig
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddExceptionHandler<ApiExceptionHandler>();
            services.AddProblemDetails();
        }
    }
}
