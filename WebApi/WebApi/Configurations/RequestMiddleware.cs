using Serilog.Context;
using System.Diagnostics;
using WebApi.Utils;

namespace WebApi.Configurations
{
    public class RequestMiddleware(RequestDelegate requestDelegate,
                                   ILogger<RequestMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext httpContext)
        {
            var stopwatch = Stopwatch.StartNew();
            var correlationId = CorrelationIdUtils.Get(httpContext);

            using (LogContext.PushProperty("correlation_id", correlationId))
            {
                await requestDelegate(httpContext);

                stopwatch.Stop();

                logger.LogInformation($"Latency: {stopwatch.Elapsed.TotalMilliseconds} to {httpContext.Request.Path}");
            }
        }
    }
}
