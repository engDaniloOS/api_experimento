﻿using Polly;
using Polly.Extensions.Http;

namespace WebApi.Configurations
{
    public static class ResilienceServiceConfig
    {
        private const int RETRIES = 3;
        private const int ERRORS_TO_OPEN_CIRCUIT = 5;
        private const int CIRCUIT_BREAK_OPEN_TIME = 10;

        public static IAsyncPolicy<HttpResponseMessage> BuildRetryPolicy()
            => HttpPolicyExtensions.HandleTransientHttpError()
                                   .RetryAsync(RETRIES);

        public static IAsyncPolicy<HttpResponseMessage> BuildCircuitBreakPolicy()
            => HttpPolicyExtensions.HandleTransientHttpError()
                                   .CircuitBreakerAsync(ERRORS_TO_OPEN_CIRCUIT, 
                                                        TimeSpan.FromSeconds(CIRCUIT_BREAK_OPEN_TIME),
                                                        onBreak: (outcome, breakDelay) => Console.WriteLine($"Circuit breaker opened for {breakDelay.TotalSeconds} seconds due to: {outcome.Exception?.Message}"),
                                                        onReset: () => Console.WriteLine("Circuit breaker reset."),
                                                        onHalfOpen: () => Console.WriteLine("Circuit breaker half-open, testing..."));
    }
}
