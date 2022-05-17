using System;
using System.Collections.Generic;
using System.Linq;

namespace Lljxww.Common.Extensions;

public static class DictionaryExtension
{
    public static Dictionary<T, U>? Merge<T, U>(this Dictionary<T, U>? dic,
        Func<U, U, U> merge,
        Func<T, T, bool>? predicate = null) where T : notnull
    {
        if (dic == null)
        {
            return dic;
        }

        predicate ??= (key1, key2) =>
        {
            return string.Equals(key1?.ToString(), key2?.ToString(), StringComparison.OrdinalIgnoreCase);
        };

        Dictionary<T, U> newDic = new();

        foreach (var item in dic)
        {
            if (!newDic.Keys.Any(k => predicate.Invoke(k, item.Key)))
            {
                newDic.Add(item.Key, item.Value);
            }
            else
            {
                var key = newDic.Keys.Single(k => predicate.Invoke(k, item.Key));
                newDic[key] = merge.Invoke(newDic[key], item.Value);
            }
        }

        return newDic;
    }
}

