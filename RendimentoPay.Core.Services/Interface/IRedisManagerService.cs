using StackExchange.Redis;

namespace RendimentoPay.Core.Services.Interface
{
    public interface IRedisManagerService
    {
        IDatabase GetDatabase();
        void SetKeyValue(string key, string value);
        string GetKeyValue(string key);
    }
}
