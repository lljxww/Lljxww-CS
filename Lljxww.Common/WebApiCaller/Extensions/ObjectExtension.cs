using System.Collections.Generic;
using System.Linq;

namespace Lljxww.Common.WebApiCaller.Extensions
{
    public static class ObjectExtension
    {
        public static Dictionary<string, string>? AsDictionary(this object source)
        {
            if (source is Dictionary<string, string>)
            {
                return source as Dictionary<string, string>;
            }
            else
            {
                return source.GetType().GetProperties().ToDictionary
                (
                    propInfo => propInfo.Name,
                    propInfo => propInfo.GetValue(source, null)?.ToString()
                );
            }
        }
    }
}
