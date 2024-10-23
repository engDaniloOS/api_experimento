using Serilog.Context;
using System.Diagnostics;

namespace WebApi.Configurations
{
    public class RequestMiddleware
    {
        private readonly ILogger<RequestMiddleware> _logger;
        private readonly RequestDelegate _requestDelegate;

        public RequestMiddleware(RequestDelegate requestDelegate,
                                        ILogger<RequestMiddleware> logger)
        {
            _requestDelegate = requestDelegate;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            var correlationId = context.Request.Headers["CorrelationId"].FirstOrDefault();

            if (string.IsNullOrWhiteSpace(correlationId))
                correlationId = Guid.NewGuid().ToString();

            using (LogContext.PushProperty("correlation_id", correlationId))
                await _requestDelegate(context);

            stopwatch.Stop();

            _logger.LogInformation($"Latency: {stopwatch.Elapsed.TotalMilliseconds} to {context.Request.Path}");
        }
    }
}
