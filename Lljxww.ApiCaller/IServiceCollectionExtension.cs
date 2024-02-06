using Lljxww.ApiCaller.Config;
using Lljxww.ApiCaller.RequestContextLoader;
using Lljxww.ApiCaller.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lljxww.ApiCaller;

public static class ServiceCollectionExtension
{
    #region ConfigFile

    public static IServiceCollection ConfigureCallerWithConfigFile(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.Configure<ApiCallerConfig>(configuration);
        _ = services.AddHttpClient();
        _ = services.AddSingleton<Caller>();

        _ = services.AddKeyedSingleton<IRequestContextLoader, ConfigRequestContextLoader>("config");
        _ = services.AddKeyedSingleton<IRequestContextLoader, SwaggerJsonFileRequestContextLoader>("swagger-json-file");
        _ = services.AddKeyedSingleton<IRequestContextLoader, SwaggerWebRequestContextLoader>("swagger-web");
        _ = services.AddKeyedSingleton<IRequestContextLoader, StandaloneRequestContextLoader>("standalone");

        _ = services.AddSingleton<RequestContextUtil>();

        return services;
    }

    /// <summary>
    /// 使用指定的配置文件初始化Caller配置
    /// </summary>
    /// <param name="services"></param>
    /// <param name="jsonFileName"></param>
    /// <returns></returns>
    public static IServiceCollection ConfigureCallerWithConfigFile(this IServiceCollection services, string jsonFileName)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile(jsonFileName)
            .Build();

        return ConfigureCallerWithConfigFile(services, configuration);
    }

    /// <summary>
    /// 使用默认的配置文件初始化Caller配置
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection ConfigureCallerWithConfigFile(this IServiceCollection services)
    {
        return ConfigureCallerWithConfigFile(services, "caller.json");
    }

    #endregion
}