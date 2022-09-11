namespace RedisResponseCache.Interfaces
{
    public interface IResponseCacheService
    {
        Task CacheResponseAsync(string cacheKey, object response, TimeSpan duration);
        Task<string> GetCachedResponseAsync(string cacheKey);
    }
}
