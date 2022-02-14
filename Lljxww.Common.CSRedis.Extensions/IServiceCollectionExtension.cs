using CSRedis;
using Lljxww.Common.CSRedis.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.DependencyInjection;

namespace Lljxww.Common
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection ConfigureCache(this IServiceCollection services, string redisConnectionString)
        {
            CSRedisClient csredis = new(redisConnectionString);
            RedisHelper.Initialization(csredis);
            services.AddSingleton<IDistributedCache>(new CSRedisCache(RedisHelper.Instance));
            services.AddSingleton<Caching>();

            return services;
        }
    }
}
