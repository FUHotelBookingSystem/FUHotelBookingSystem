using System;
using System.Text.Json;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace PRN222.Assignment.FUHotelBookingSystem.Service.RedisService
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDatabase _cacheDb;

        public RedisCacheService(IConnectionMultiplexer redis)
        {
            _cacheDb = redis.GetDatabase();
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var jsonData = await _cacheDb.StringGetAsync(key);
            if (jsonData.IsNullOrEmpty) return default;
            return JsonSerializer.Deserialize<T>(jsonData);
        }

        public async Task RemoveAsync(string key)
        {
            await _cacheDb.KeyDeleteAsync(key);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan expiration)
        {
            var jsonData = JsonSerializer.Serialize(value);
            await _cacheDb.StringSetAsync(key, jsonData, expiration);
        }
    }
}
