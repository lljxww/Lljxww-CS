using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Lljxww.Dapper.Extensions;

public static partial class SqlGenerator
{
    /// <summary>
    /// 使用指定的模型生成SQL查询语句
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static string BuildQuery<T>() where T : class
    {
        Dictionary<string, string> propNames = GetCustomPropName<T>();
        StringBuilder fieldsSegment = new();

        foreach (KeyValuePair<string, string> propName in propNames)
        {
            string appendText = string.Equals(propName.Key, propName.Value)
                ? $"`{propName.Value}`,"
                : $"`{propName.Value}` as `{propName.Key}`,";
            fieldsSegment.Append(appendText);
        }

        return string.Format(QUERY_STR, fieldsSegment.ToString().TrimEnd(','), GetCustomTableName<T>());
    }

    /// <summary>
    /// 使用指定的模型生成SQL更新语句
    /// </summary>
    /// <param name="updateFieldNames">要更新的字段属性名，默认为所有字段</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static string BuildUpdate<T>(IList<string>? updateFieldNames = null) where T : class
    {
        Dictionary<string, string> propNames = GetCustomPropName<T>();
        StringBuilder fieldsSegment = new();

        Dictionary<string, string> updateProps = propNames;
        if (updateFieldNames != null)
        {
            updateProps = updateProps.Where(p => updateFieldNames.Contains(p.Key))
                .ToDictionary(p => p.Key, p => p.Value);
        }
        else
        {
            updateProps.Remove(GetCustomKey<T>().Value);
        }

        foreach (KeyValuePair<string, string> propName in updateProps)
        {
            fieldsSegment.Append($"`{propName.Value}` = @{propName.Key},");
        }

        return string.Format(UPDATE_STR, GetCustomTableName<T>(), fieldsSegment.ToString().TrimEnd(','));
    }

    /// <summary>
    /// 使用指定的模型生成SQL插入语句
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static string BuildInsert<T>(bool withPrimary = false) where T : class
    {
        Dictionary<string, string> propNames = GetCustomPropName<T>();
        StringBuilder fieldsSegment = new();
        StringBuilder valuesSegment = new();

        KeyValuePair<string, string> primary = default;
        if (withPrimary)
        {
            primary = GetCustomKey<T>();
        }

        foreach (KeyValuePair<string, string> propName in propNames)
        {
            if (!withPrimary)
            {
                if (string.Equals(propName.Key, primary.Key))
                {
                    continue;
                }
            }

            fieldsSegment.Append($"`{propName.Value}`,");
            valuesSegment.Append($"@{propName.Key},");
        }

        return string.Format(INSERT_STR, GetCustomTableName<T>(),
            fieldsSegment.ToString().TrimEnd(','), valuesSegment.ToString().TrimEnd(','));
    }
}

public static partial class SqlGenerator
{
    private static readonly Dictionary<int, string> tableInfos = new();
    private static readonly Dictionary<int, Dictionary<string, string>> propInfos = new();
    private static readonly Dictionary<int, KeyValuePair<string, string>> keyInfos = new();

    private static readonly string QUERY_STR = "SELECT {0} FROM {1}";
    private static readonly string UPDATE_STR = "UPDATE {0} SET {1}";
    private static readonly string INSERT_STR = "INSERT INTO {0} ({1}) values ({2})";
}

public static partial class SqlGenerator
{
    /// <summary>
    /// 查询指定类型的每个字段对应的数据库列名
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    private static Dictionary<string, string> GetCustomPropName<T>() where T : class
    {
        Type type = typeof(T);

        int hash = type.GetHashCode();
        Dictionary<string, string>? cachedInfo = GetCachedInfo(hash);
        if (cachedInfo != null)
        {
            return cachedInfo;
        }

        PropertyInfo[] props = type.GetProperties();

        Dictionary<string, string> propNames = new();

        foreach (PropertyInfo prop in props)
        {
            ColumnAttribute? columnAttr = prop.GetCustomAttribute<ColumnAttribute>();
            if (columnAttr != null)
            {
                propNames.Add(prop.Name, columnAttr.Name ?? prop.Name);
            }
            else
            {
                propNames.Add(prop.Name, prop.Name);
            }
        }

        CacheInfo(hash, propNames);

        return propNames;
    }

    /// <summary>
    /// 查询指定类型对应的数据库表名
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    private static string GetCustomTableName<T>() where T : class
    {
        Type type = typeof(T);
        int hash = type.GetHashCode();

        if (tableInfos.ContainsKey(hash))
        {
            return tableInfos[hash];
        }

        TableAttribute? tableAttr = type.GetCustomAttribute<TableAttribute>();
        string tableName = tableAttr?.Name ?? type.Name;

        if (!tableInfos.ContainsKey(hash))
        {
            tableInfos.Add(hash, tableName);
        }

        return tableName;
    }

    /// <summary>
    /// 查询指定类型设置的主键名
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    private static KeyValuePair<string, string> GetCustomKey<T>() where T : class
    {
        Type type = typeof(T);
        int hash = type.GetHashCode();

        if (keyInfos.ContainsKey(hash))
        {
            return keyInfos[hash];
        }

        PropertyInfo[] props = type.GetProperties();
        foreach (PropertyInfo prop in props)
        {
            if (prop.GetCustomAttribute<KeyAttribute>() == null)
            {
                continue;
            }

            ColumnAttribute? columnAttr = prop.GetCustomAttribute<ColumnAttribute>();
            KeyValuePair<string, string> pair = new(prop.Name, columnAttr?.Name ?? prop.Name);
            keyInfos.Add(hash, pair);
            return pair;
        }

        throw new InvalidOperationException($"未设置该模型的主键：{nameof(T)}");
    }
}

public static partial class SqlGenerator
{
    private static void CacheInfo(int hash, Dictionary<string, string> propNames)
    {
        if (propInfos.ContainsKey(hash))
        {
            return;
        }

        propInfos.Add(hash, propNames);
    }

    private static Dictionary<string, string>? GetCachedInfo(int hash)
    {
        return propInfos.ContainsKey(hash) ? propInfos[hash] : null;
    }
}