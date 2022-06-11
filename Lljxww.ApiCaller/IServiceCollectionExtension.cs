using Lljxww.ApiCaller.Models.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lljxww.ApiCaller;

public static class IServiceCollectionExtension
{
    /// <summary>
    /// 使用指定的配置节初始化Caller配置
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection ConfigureCaller(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ApiCallerConfig>(configuration);
        services.AddHttpClient();
        services.AddSingleton<Caller>();

        return services;
    }

    /// <summary>
    /// 使用指定的配置文件初始化Caller配置
    /// </summary>
    /// <param name="services"></param>
    /// <param name="jsonFileName"></param>
    /// <returns></returns>
    public static IServiceCollection ConfigureCaller(this IServiceCollection services, string jsonFileName)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile(jsonFileName)
            .Build();

        return ConfigureCaller(services, configuration);
    }

    /// <summary>
    /// 使用默认的配置文件初始化Caller配置
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection ConfigureCaller(this IServiceCollection services)
    {
        return ConfigureCaller(services, "caller.json");
    }
}