using Microsoft.Extensions.Caching.Memory;

namespace WebApi.DataProviders.Commons
{
    public class HttpCacheProvider
    {
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _configuration;
        private readonly ILogger<HttpCacheProvider> _logger;

        public HttpCacheProvider(IMemoryCache cache,
                                 IConfiguration configuration,
                                 ILogger<HttpCacheProvider> logger)
        {
            _cache = cache;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<T> GetCacheOrExecuteFunction<T>(string url, Func<Task<T>> fetchFunction)
        {
            var cacheExpiresIn = _configuration.GetRequiredSection("Http")["CachePeriodInMinutes"];
            var cacheSize = _configuration.GetRequiredSection("Http")["CacheSizeInMb"];

            var cachedResponse = _cache.GetOrCreateAsync(url, async fun =>
            {
                fun.AbsoluteExpiration = DateTime.Now.AddMinutes(long.Parse(cacheExpiresIn));
                fun.Size = long.Parse(cacheSize);

                _logger.LogInformation("Cache miss. Executing function...");
                return await fetchFunction();
            });

            return await cachedResponse;
        }
    }
}
