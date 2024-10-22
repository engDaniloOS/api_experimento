namespace WebApi.Configurations
{
    public static class HttpClientServiceConfig
    {
        private const string RETRY_POLICY = "RetryPolicy";
        private const string CIRCUIT_BREAK_POLICY = "CircuitBreakerPolicy";

        public static readonly string HTTP_CLIENT_DEFAULT = "default";

        public static void Configure(IServiceCollection services)
        {
            var policyRegistry = services.AddHttpClient().AddPolicyRegistry();

            policyRegistry.Add(RETRY_POLICY, ResilienceServiceConfig.BuildRetryPolicy());
            policyRegistry.Add(CIRCUIT_BREAK_POLICY, ResilienceServiceConfig.BuildCircuitBreakPolicy());

            services.AddHttpClient(HTTP_CLIENT_DEFAULT)
                    .AddPolicyHandlerFromRegistry(RETRY_POLICY)
                    .AddPolicyHandlerFromRegistry(CIRCUIT_BREAK_POLICY);
        }
    }
}
