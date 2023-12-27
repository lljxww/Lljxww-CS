using System.Collections.Specialized;
using System.Configuration;

namespace Lljxww.ApiCaller.Utils;

internal class ConfigHelper
{
    private static string GetCallerEnvironment()
    {
        object config = ConfigurationManager.GetSection("caller");

        string env = config == null || config is not NameValueCollection collection
            ? "Production"
            : collection["envrionment"] ?? "Production";

        ENV = env;

        return env;
    }

    private static string ENV = string.Empty;

    internal static string CallerEnv => string.IsNullOrWhiteSpace(ENV)
        ? GetCallerEnvironment()
        : ENV;
}
