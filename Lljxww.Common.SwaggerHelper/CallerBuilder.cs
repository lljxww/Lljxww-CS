using System.Reflection;
using System.Reflection.Emit;

namespace Lljxww.Common.SwaggerHelper;

internal class CallerBuilder
{
    public void Build(SwaggerModel? model)
    {
        if (model == null)
        {
            return;
        }

        AssemblyBuilder? assemblyBuilder =
            AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(ParseAssemblyName(model.Info.Title)),
                AssemblyBuilderAccess.Run);
        //TODO Wait for "Save"
    }

    private object? BuildMethod()
    {
        return default;
    }

    private IEnumerable<MethodContext> NextMethod(SwaggerModel model)
    {
        foreach (KeyValuePair<string, Dictionary<string, SwaggerModel.MethodInfo>> path in model.Paths)
        {
            foreach ((string? key, SwaggerModel.MethodInfo? value) in path.Value)
            {
                MethodContext methodContext = new()
                {
                    HttpMethod = key.Trim().ToLower() switch
                    {
                        "get" => HttpMethod.Get,
                        "post" => HttpMethod.Post,
                        "put" => HttpMethod.Put,
                        "delete" => HttpMethod.Delete,
                        _ => throw new NotImplementedException()
                    },
                    Summary = value.Summary,
                    Name = ParseMethodName(value.OperationId),
                    Parameters = default
                };

                yield return methodContext;
            }
        }
    }

    /// <summary>
    ///     根据描述中的OperateId创建方法名
    /// </summary>
    /// <param name="operateId"></param>
    /// <returns></returns>
    private string ParseMethodName(string operateId)
    {
        return operateId.Replace("_", "");
    }

    /// <summary>
    ///     生成程序集名称
    /// </summary>
    /// <param name="title"></param>
    /// <returns></returns>
    private string ParseAssemblyName(string title)
    {
        string[]? slices = title.Split(' ');
        string name = string.Empty;

        foreach (string? slice in slices)
        {
            char[] res = new char[slice.Length];
            int i = 0;
            res[0] = slice[i++];

            foreach (char ch in slice[1..^1])
            {
                if (ch >= 'A' && ch <= 'Z')
                {
                    res[i] = (char)(ch + 32);
                }
            }

            name += res.ToString();
        }

        return name;
    }
}