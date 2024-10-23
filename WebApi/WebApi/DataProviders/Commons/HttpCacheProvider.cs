using Microsoft.Extensions.Caching.Memory;

namespace WebApi.DataProviders.Commons
{
    public class HttpCacheProvider
    {
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _configuration;

        public HttpCacheProvider(IMemoryCache cache, IConfiguration configuration)
        {
            _cache = cache;
            _configuration = configuration;
        }

        public async Task<T> GetCacheOrExecuteFunction<T>(string url, Func<Task<T>> fetchFunction)
        {
            var cacheExpiresIn = _configuration.GetRequiredSection("Http")["CachePeriodInMinutes"];
            var cacheSize = _configuration.GetRequiredSection("Http")["CacheSizeInMb"];

            var cachedResponse = _cache.GetOrCreateAsync(url, async fun =>
            {
                fun.AbsoluteExpiration = DateTime.Now.AddMinutes(long.Parse(cacheExpiresIn));
                fun.Size = long.Parse(cacheSize);

                return await fetchFunction();
            });

            return await cachedResponse;
        }
    }
}
