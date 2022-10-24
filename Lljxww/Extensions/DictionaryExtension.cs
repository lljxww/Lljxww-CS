using System;
using System.Collections.Generic;
using System.Linq;

namespace Lljxww.Extensions;

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

        predicate ??= (key1, key2)
            => string.Equals(key1?.ToString(), key2?.ToString(), StringComparison.OrdinalIgnoreCase);

        Dictionary<T, U> newDic = new();

        foreach (KeyValuePair<T, U> item in dic)
        {
            if (!newDic.Keys.Any(k => predicate.Invoke(k, item.Key)))
            {
                newDic.Add(item.Key, item.Value);
            }
            else
            {
                T key = newDic.Keys.Single(k => predicate.Invoke(k, item.Key));
                newDic[key] = merge.Invoke(newDic[key], item.Value);
            }
        }

        return newDic;
    }
}