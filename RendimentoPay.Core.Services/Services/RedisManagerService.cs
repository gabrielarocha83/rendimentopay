using StackExchange.Redis;
using Microsoft.Extensions.Configuration;
using RendimentoPay.Core.Services.Interface;

namespace RendimentoPay.Core.Services.Services
{
    public class RedisManagerService : IRedisManagerService
    {
        private readonly IConnectionMultiplexer redisConnection;
        private readonly ConnectionMultiplexer connect;

        public RedisManagerService(IConfiguration configuration, IConnectionMultiplexer redisConnection)
        {
            this.redisConnection = redisConnection;
            connect = ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnection"));
        }

        public IDatabase GetDatabase()
        {
            IDatabase db = connect.GetDatabase();
            return db;
        }

        public void SetKeyValue(string key, string value)
        {
            var db = GetDatabase();
            db.StringSet(key, value);
        }

        public string GetKeyValue(string key)
        {
            var db = GetDatabase();
            return db.StringGet(key);
        }
    }
}