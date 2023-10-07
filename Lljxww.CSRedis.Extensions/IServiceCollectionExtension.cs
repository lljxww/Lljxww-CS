using CSRedis;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.DependencyInjection;

namespace Lljxww.CSRedis.Extensions;

public static class IServiceCollectionExtension
{
    public static IServiceCollection ConfigureCache(this IServiceCollection services, string redisConnectionString)
    {
        CSRedisClient csredis = new(redisConnectionString);
        RedisHelper.Initialization(csredis);
        _ = services.AddSingleton<IDistributedCache>(new CSRedisCache(RedisHelper.Instance));
        _ = services.AddSingleton<Caching>();

        return services;
    }
}