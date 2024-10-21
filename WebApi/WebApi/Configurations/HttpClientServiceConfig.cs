namespace WebApi.Configurations
{
    public static class HttpClientServiceConfig
    {
        public static void Configure(IServiceCollection services)
            => services.AddHttpClient();
    }
}
