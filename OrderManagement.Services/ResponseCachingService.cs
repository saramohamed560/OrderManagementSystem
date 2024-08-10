using OrderManagement.Core.Services.Contracts;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OrderManagement.Services
{
    public class ResponseCachingService : IResponseCachingService
    {
        private readonly IDatabase _database;

        public ResponseCachingService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public async Task CaschResponseAsync(string key, object response, TimeSpan timeToLive)
        {
            if(response is null) return ;
            var serializedOption=new JsonSerializerOptions { PropertyNamingPolicy= JsonNamingPolicy.CamelCase };
            var serializedResponse= JsonSerializer.Serialize(response,serializedOption);
            await _database.StringSetAsync(key, serializedResponse, timeToLive);

        }

        public async Task<string?> GetCachedResponseAsync(string key)
        {
            var response = await _database.StringGetAsync(key);
            if (response.IsNullOrEmpty) return null;
            return response;
        }
    }
}
