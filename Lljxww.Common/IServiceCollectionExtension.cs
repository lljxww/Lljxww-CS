using CSRedis;
using Lljxww.Common.Lock;
using Lljxww.Common.WebApiCaller;
using Lljxww.Common.WebApiCaller.Models.Config;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Lljxww.Common
{
    public static class IServiceCollectionExtension
    {
        #region Caller

        public static IServiceCollection ConfigureCaller(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ApiCallerConfig>(configuration);
            services.AddHttpClient();
            services.AddSingleton<Caller>();

            return services;
        }

        public static IServiceCollection ConfigureCaller(this IServiceCollection services)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddJsonFile("caller.json")
                .Build();

            return ConfigureCaller(services, configuration);
        }

        #endregion

        #region DistributedLock

        public static IServiceCollection ConfigureDistributedLock(this IServiceCollection services)
        {
            if (!services.Any(s => s.ServiceType == typeof(CSRedisClient)))
            {
                throw new InvalidOperationException("使用DistirbutedLock前, 需注册CSRedis的实例");
            }

            services.AddSingleton<DistributedLock>();

            return services;
        }

        public static IServiceCollection ConfigureDistributedLock(this IServiceCollection services, string redisConnectionString)
        {
            if (!services.Any(s => s.ServiceType == typeof(CSRedisClient)))
            {
                CSRedisClient csredis = new(redisConnectionString);
                RedisHelper.Initialization(csredis);
                services.AddSingleton<IDistributedCache>(new CSRedisCache(RedisHelper.Instance));
            }

            services.AddSingleton<DistributedLock>();

            return services;
        }

        #endregion
    }
}
