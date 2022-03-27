# Lljxww.Common

## 各项目功能介绍
- Lljxww.Common 各种基础工具，包括各种内置类型扩展、通用类、辅助工具（时间戳、图片验证码等）
- Lljxww.Common.ApiCaller WebApi调用工具，可通过简单配置统一管理项目中的所有WebApi调用，支持调用缓存、自定义调用日志、自定义异常处理、自定义超时处理等。支持自定义请求，理论上支持任意形式的WebApi的调用
- Lljxww.Common.CSRedis.Extensions 对CSRedis的二次封装，简化了一些基本操作
- Lljxww.Common.EntityFramework.Extensions 对EntityFramework的一些扩展，简化了使用方式
- Lljxww.Common.Dapper.Extensions 对Dapper的扩展，支持了通过特性处理column映射

## ApiCaller的基本使用
1. 通过参考[caller.schema.json](https://github.com/liang1224/Lljxww.Common/blob/main/Lljxww.Common.ApiCaller/Configs/caller.shema.json)编写配置文件，命名为caller.json, 保存在项目根目录。
示例配置：
``` json
{
  "Authorizations": [
  ],
  "ServiceItems": [
    {
      "ApiName": "gh",
      "BaseUrl": "https://api.github.com",
      "ApiItems": [
        {
          "Method": "GetUserInfo",
          "Url": "/users/{username}",
          "HTTPMethod": "GET",
          "ParamType": "path"
        }
      ]
    }
  ]
}
```
2. 配置注入：
``` csharp
// 若文件名为caller.json, 可直接使用
services.ConfigureCaller();
// 也可指定配置文件名
services.ConfigureCaller(your_filename.json);
// 也可传入IConfiguration
services.ConfigureCaller(config.GetSection("your_section_name"));
```
3. 使用
``` csharp
ApiResult? result = await caller.InvokeAsync("gh.GetUserInfo", new
{
    username
});
``` 

4. 结果ApiResult针对jsons tring实现了索引器，可通过索引器直接读取结果：
``` csharp
Assert.AreEqual(“liang1224”, result!["login"]); // true
Assert.AreEqual(“liang1224”, result!["Login"]); // true, 不区分大小写
```
也可通过ApiResult.JsonObject读取较深的属性，或直接使用ApiResult.RawStr读取原始返回结果。
