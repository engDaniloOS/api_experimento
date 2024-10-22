using System.Diagnostics;

namespace WebApi.Configurations
{
    public class RequestLatencyMiddleware
    {
        private readonly RequestDelegate _requestDelegate;

        public RequestLatencyMiddleware(RequestDelegate requestDelegate)
            => _requestDelegate = requestDelegate;


        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            await _requestDelegate(context);

            stopwatch.Stop();

            Console.WriteLine($"Latency: {stopwatch.Elapsed.TotalMilliseconds} to {context.Request.Path}");
        }
    }
}
