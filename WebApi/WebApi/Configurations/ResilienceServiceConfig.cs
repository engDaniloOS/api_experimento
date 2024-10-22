using Polly;
using Polly.Extensions.Http;

namespace WebApi.Configurations
{
    public static class ResilienceServiceConfig
    {
        public static IAsyncPolicy<HttpResponseMessage> BuildRetryPolicy()
            => HttpPolicyExtensions.HandleTransientHttpError()
                                   .RetryAsync(3);

        public static IAsyncPolicy<HttpResponseMessage> BuildCircuitBreakPolicy()
            => HttpPolicyExtensions.HandleTransientHttpError()
                                   .CircuitBreakerAsync(3, TimeSpan.FromSeconds(30),
                                        onBreak: (outcome, breakDelay) => Console.WriteLine($"Circuit breaker opened for {breakDelay.TotalSeconds} seconds due to: {outcome.Exception?.Message}"),
                                        onReset: () => Console.WriteLine("Circuit breaker reset."),
                                        onHalfOpen: () => Console.WriteLine("Circuit breaker half-open, testing..."));
    }
}
