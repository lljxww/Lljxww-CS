# Lljxww

## 各项目功能介绍
- Lljxww 各种基础工具，包括各种内置类型扩展、通用类、辅助工具（时间戳、图片验证码等）
- Lljxww.ApiCaller WebApi调用工具，可通过简单配置统一管理项目中的所有WebApi调用，支持调用缓存、自定义调用日志、自定义异常处理、自定义超时处理等。支持自定义请求，理论上支持任意形式的WebApi的调用
- Lljxww.CSRedis.Extensions 对CSRedis的二次封装，简化了一些基本操作
- Lljxww.EntityFramework.Extensions 对EntityFramework的一些扩展，简化了使用方式
- Lljxww.Dapper.Extensions 对Dapper的扩展，支持了通过特性处理column映射

## ApiCaller的基本使用
1. 通过参考[caller.schema.json](https://github.com/liang1224/Lljxww/blob/main/Lljxww.ApiCaller/Configs/caller.schema.json)编写配置文件，命名为caller.json, 保存在项目根目录。
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
Assert.AreEqual("liang1224", result!["login"]); // true
Assert.AreEqual("liang1224", result!["Login"]); // true, 不区分大小写
```
也可通过ApiResult.JsonObject读取较深的属性，或直接使用ApiResult.RawStr读取原始返回结果。
# Lljxww

## 各项目功能介绍
* Lljxww 各种基础工具，包括各种内置类型扩展、通用类、辅助工具（时间戳、图片验证码等）
* Lljxww.ApiCaller WebApi调用工具，可通过简单配置统一管理项目中的所有WebApi调用，支持调用缓存、自定义调用日志、自定义异常处理、自定义超时处理等。支持自定义请求，理论上支持任意形式的WebApi的调用
* Lljxww.CSRedis.Extensions 对CSRedis的二次封装，简化了一些基本操作
* Lljxww.EntityFramework.Extensions 对EntityFramework的一些扩展，简化了使用方式
* Lljxww.Dapper.Extensions 对Dapper的扩展，支持了通过特性处理column映射

## ApiCaller的基本使用
1. 配置文件
通过参考[caller.schema.json](https://github.com/liang1224/Lljxww/blob/main/Lljxww.ApiCaller/Configs/caller.shema.json)编写配置文件，命名为caller.json, 保存在项目根目录。
示例配置：
```json
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
配置注入：
```csharp
// 若文件名为caller.json, 可直接使用
services.ConfigureCaller();
// 也可指定配置文件名
services.ConfigureCaller(your_filename.json);
// 也可传入IConfiguration
services.ConfigureCaller(config.GetSection("your_section_name"));
```
通过依赖注入创建Caller实例，按以下方式使用
```csharp
ApiResult? result = await _caller.InvokeAsync("gh.GetUserInfo", new
{
    username
});
```
其中，"gh.GetUserInfo"为配置文件中的ApiName和Method，使用小数点分隔，不区分大小写。第二个参数为匿名对象，如果需要动态拼接，也可使用Dictionary<string, string>实例作为参数。

2. ApiResult
结果ApiResult针对jsons tring实现了索引器，可通过索引器直接读取结果：
```csharp
Assert.AreEqual("liang1224", result!["login"]); // true
Assert.AreEqual("liang1224", result!["Login"]); // true, 不区分大小写
```
也可通过ApiResult.JsonObject读取较深的属性，或直接使用ApiResult.RawStr读取原始返回结果。

3. 配置缓存
有些情况下无需对WebApi进行频繁调用，仅需在一定时间内使用上次结果即可，可在配置文件中进行如下配置：
``` json
{
  "Method": "GetUserInfo",
  "Url": "/users/{username}",
  "HTTPMethod": "GET",
  "ParamType": "path",
	"NeedCache": true,
	"CacheTime": 20 //分钟
}
```
同时， 需要为Caller配置缓存操作：
```csharp
Caller.SetCacheEvent += context => {
	// your set cache action
};

Caller.GetCacheEvent += context => {
	// your get cache action
};
```
此时，针对此节点的调用将在有效期内不再重复调用。

4. 配置接口授权
事实上绝大部分接口需要额外的授权操作才可以使用，可通过配置和少量的代码即可为某方法或方法组配置授权操作。

在配置文件的Authorizations节点添加配置：
```json
{
  "Authorizations": [
    {
      "Name": "MyAuthorization",
      "AuthorizationInfo": "your_custom_info"
    }
  ]
}
```

在指定方法或方法组中添加授权方式：
```json
{
  //配置授权名，然后在代码中注册；也可在此节点的父级配置为该url下的所有请求统一配置。 子节点将覆盖父节点的配置
	"AuthorizationType": "MyAuthorization", 
  // 自定义的用于授权的信息，ClientID和ClientSecret等
	"AuthorizationInfo": "",
  "Method": "GetUserInfo",
  "Url": "/users/{username}",
  "HTTPMethod": "GET",
  "ParamType": "path"
}
```

由于暴露了原生的Request，所以理论上可编写任意方式的授权实现：
```sharp
public static class MyAuthorize
{
  public static CallerContext Func(CallerContext context)
  {
    // 通过对context.RequestMessage的操作实现任意类型的授权操作
    // 可通过context.Authorization.AuthorizationInfo读取在配置中设置的信息
  }
}
```

有时授权信息是通过其他途径获取的（动态的），那么也可在调用接口时配置，见以下示例：
```csharp
_caller.InvokeAsync("some_site.some_method", new {
    param1 = 1,
    param2 = 2
  }, new RequestOption { 
    CustomAuthorizeInfo = "your_authorize_info" 
    // 此信息将可在自定义授权时通过context.Authorization.AuthorizationInfo获取。同样地，此信息将覆盖在配置文件中指定的信息
});
```

注册授权：
```csharp
CallerContext.AddAuthFunc("MyAuthorization", MyAuthorize.Func);
```

此后， 配置了"MyAuthorization"授权操作的方法将会通过MyAuthorize.Func实现的方法进行授权操作。

5. 其他事件
```csharp
Caller.LogHandler += context => { // 自定义日志事件 }
Caller.OnExecuted += context => { // 请求完成后触发 }
Caller.OnException += context => { // 请求发生异常时触发 }
Caller.OnRequestTimeout += context => { // 请求超时时触发 }
```

6. 请求配置
有些请求可能参数较为特殊，又或者对Context-Type有特殊要求，此时可通过自定义请求参数实现：
```csharp
_caller.Invoke("some_site.some_method", new {
    param1 = 1,
    param2 = 2
  }, new RequestOption{
    IsTriggerOnExecuted = true, // 是否触发OnExecuted方法
    IsFromCache = false, // 即使为此方法配置了缓存，此次操作也不从缓存中读取结果
    CustomFinalUrlHandler = url => { 
      // 处理url，用于在发送请求前对url进行修改
      return url；	
    }，
    CustomHttpContent = myHttpContent, // 自定义的改请求的HttpContent，用于默认的实现不能满足需求时
    DontLog = true, // 指定此方法不记录日志。有时记录日志的方法也是一个WebApi，这种情况将会引发类似无限递归的情况
    CustomAuthorizeInfo = "my_autthorize_info", // 自定义的授权信息，上文已提到
    Timeout = 1000 // 单位ms，指定此请求的超时事件，会覆盖配置文件中的值（如果有）
});
```

## 结语
已发布到NuGet: [点击此处查看](https://www.nuget.org/packages?q=Lljxww)

Github: [liang1224/Lljxww](https://github.com/liang1224/Lljxww)

水平有限，欢迎批评指正。
邮箱：[liangjw1224@foxmail.com](mailto:liangjw1224@foxmail.com)

[![.NET](https://github.com/liang1224/Lljxww/actions/workflows/publish_to_nuget.yml/badge.svg?branch=main&event=push)](https://github.com/liang1224/Lljxww/actions/workflows/publish_to_nuget.yml)