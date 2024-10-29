using Microsoft.AspNetCore.Diagnostics;
using Serilog.Context;
using WebApi.Utils;

namespace WebApi.Entrypoints.Handlers
{
    public class ApiExceptionHandler(ILogger<ApiExceptionHandler> logger) : IExceptionHandler
    {
        private const string DEFAULT_MSG = "Error when try to process request.";

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            httpContext.Response.StatusCode = exception is HttpRequestException ? 
                                                StatusCodes.Status500InternalServerError :
                                                StatusCodes.Status400BadRequest;

            var errorMessage = $"{DEFAULT_MSG} Error: {exception.Message}";
            var correlationId = CorrelationIdUtils.Get(httpContext);

            using (LogContext.PushProperty("correlation_id", correlationId))
            {
                var logErrorMessage = $"{errorMessage}.{Environment.NewLine} Stack: {exception.StackTrace}";
                
                logger.LogError(logErrorMessage, exception);
            }

            await httpContext.Response.WriteAsJsonAsync(new { Message = errorMessage }, cancellationToken);

            return true;
        }
    }
}