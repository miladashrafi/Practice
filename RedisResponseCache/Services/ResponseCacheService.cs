using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using RedisResponseCache.Interfaces;

namespace RedisResponseCache.Services
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDistributedCache _distributedCache;

        public ResponseCacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan duration)
        {
            if (response == null)
                return;

            var serializedResponce = JsonConvert.SerializeObject(response);
            await _distributedCache.SetStringAsync(cacheKey, serializedResponce, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = duration
            });
        }

        public async Task<string> GetCachedResponseAsync(string cacheKey)
        {
            var cachedResponse = await _distributedCache.GetStringAsync(cacheKey);
            return string.IsNullOrWhiteSpace(cachedResponse) ? null : cachedResponse;
        }

    }
}
