using System.Reflection;

namespace Lljxww.Common.SwaggerHelper
{
    internal class AssemblyBuilder
    {
        public void Build(SwaggerModel? model)
        {
            if (model == null)
            {
                return;
            }

            AssemblyName? assembly = new(ParseAssemblyName(model.Info.Title));
        }

        private object? BuildMethod()
        {
            return default;
        }

        private IEnumerable<MethodContext> NextMethod(SwaggerModel model)
        {
            foreach (KeyValuePair<string, Dictionary<string, SwaggerModel.MethodInfo>> path in model.Paths)
            {
                foreach (KeyValuePair<string, SwaggerModel.MethodInfo> method in path.Value)
                {
                    MethodContext methodContext = new()
                    {
                        HttpMethod = method.Key.Trim().ToLower() switch
                        {
                            "get" => HttpMethod.Get,
                            "post" => HttpMethod.Post,
                            "put" => HttpMethod.Put,
                            "delete" => HttpMethod.Delete,
                            _ => throw new NotImplementedException()
                        },
                        Summary = method.Value.Summary,
                        Name = ParseMethodName(method.Value.OperationId),
                        Parameters = default
                    };

                    yield return methodContext;
                }
            }
        }

        /// <summary>
        /// 根据描述中的OperateId创建方法名
        /// </summary>
        /// <param name="operateId"></param>
        /// <returns></returns>
        private string ParseMethodName(string operateId)
        {
            return operateId.Replace("_", "");
        }

        /// <summary>
        /// 生成程序集名称
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
}
