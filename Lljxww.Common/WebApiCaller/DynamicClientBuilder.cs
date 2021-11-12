using Lljxww.Common.WebApiCaller.Models.Config;
using Microsoft.Extensions.Options;
using System.Reflection;
using System.Reflection.Emit;

namespace Lljxww.Common.WebApiCaller
{
    public class DynamicClientBuilder
    {
        private readonly ApiCallerConfig _config;

        public DynamicClientBuilder(IOptions<ApiCallerConfig> apiCallerConfigOption)
        {
            _config = apiCallerConfigOption.Value;
        }

        public void Init()
        {
            AssemblyName? assemblyName = new("Lljxww.Common.WebApiCaller.DynamicClient");
            AssemblyBuilder builder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndCollect);
        }
    }
}
