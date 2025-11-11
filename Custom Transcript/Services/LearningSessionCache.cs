using CornerstoneApiServices.Models.Learning;
using CustomTranscript.App.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace CustomTranscript.App.Services
{
    public class LearningSessionCache
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<LearningSessionCache> _logger;
        private readonly TimeProvider _timeProvider;

        public LearningSessionCache(IMemoryCache memoryCache, ILogger<LearningSessionCache> logger, TimeProvider timeProvider)
        {
            _memoryCache = memoryCache;
            _logger = logger;
            _timeProvider = timeProvider;
        }

        public async Task<List<LrnSession>> GetOrAddAsync(string externalId, Func<Task<List<LrnSession>>> getSessionsCall, TimeSpan ttl)
        {
            if (_memoryCache.TryGetValue<CacheItem>(externalId, out var cacheItem))
            {
                var age = _timeProvider.GetUtcNow() - cacheItem.CachedAt;
                if (age < ttl)
                {
                    _logger.LogDebug("Cache hit for {ExternalId}, age {Age} min", externalId, age.TotalMinutes);
                    return cacheItem.Sessions;
                }

                _logger.LogDebug("Cache expired for {ExternalId}, refreshing", externalId);
            }
            else
            {
                _logger.LogDebug("Cache miss for {ExternalId}", externalId);
            }

            var sessions = await getSessionsCall();

            var newItem = new CacheItem
            {
                Sessions = sessions,
                CachedAt = _timeProvider.GetUtcNow(),
            };

            _memoryCache.Set(externalId, newItem, ttl);

            _logger.LogDebug("Cache updated for {ExternalId}, {Count} sessions", externalId, sessions.Count);
            return sessions;
        }
    }
}
