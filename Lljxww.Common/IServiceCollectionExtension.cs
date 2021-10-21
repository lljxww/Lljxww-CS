using CSRedis;
using Lljxww.Common.Utilities.Cache;
using Lljxww.Common.WebApiCaller;
using Lljxww.Common.WebApiCaller.Models.Config;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

        #region Cache

        public static IServiceCollection ConfigureCache(this IServiceCollection services, string redisConnectionString)
        {
            CSRedisClient csredis = new(redisConnectionString);
            RedisHelper.Initialization(csredis);
            services.AddSingleton<IDistributedCache>(new CSRedisCache(RedisHelper.Instance));
            services.AddSingleton<Caching>();

            return services;
        }

        #endregion
    }
}
