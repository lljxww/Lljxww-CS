using Lljxww.ApiCaller.Models.Utils;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Lljxww.ApiCaller.Models.Config;

public class Authorization
{
    public string Name { get; set; }

    public Dictionary<string, string> AuthorizationInfos { get; set; } = [];

    [JsonIgnore]
    public string AuthorizationInfo
    {
        get => AuthorizationInfos.ContainsKey(ConfigHelper.CallerEnv) ? AuthorizationInfos[ConfigHelper.CallerEnv] : string.Empty;
        set
        {
            if (value == null || string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            if (AuthorizationInfos.ContainsKey(ConfigHelper.CallerEnv))
            {
                AuthorizationInfos[ConfigHelper.CallerEnv] = value;
            }
            else
            {
                AuthorizationInfos.Add(ConfigHelper.CallerEnv, value);
            }
        }
    }
}