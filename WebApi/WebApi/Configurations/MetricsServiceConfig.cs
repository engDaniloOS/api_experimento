namespace WebApi.Configurations
{
    public static class MetricsServiceConfig
    {
        public static void Configure(IServiceCollection services)
            => services.AddHealthChecks();

    }
}
