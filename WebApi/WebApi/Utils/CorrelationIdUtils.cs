namespace WebApi.Utils
{
    public static class CorrelationIdUtils
    {
        public static string Get(HttpContext httpContext)
        {
            var correlationId = httpContext.Request.Headers["CorrelationId"].FirstOrDefault();

            if (string.IsNullOrWhiteSpace(correlationId))
            {
                correlationId = Guid.NewGuid().ToString();
                httpContext.Request.Headers["CorrelationId"] = correlationId;
            }

            return correlationId;
        }
    }
}
