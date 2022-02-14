using Lljxww.Common.ApiCaller.Models.Config;
using Lljxww.Common.WebApiCaller;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lljxww.Common.ApiCaller
{
    public static class IServiceCollectionExtension
    {
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
    }
}
