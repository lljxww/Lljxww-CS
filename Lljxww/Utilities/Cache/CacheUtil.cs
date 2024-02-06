using Microsoft.Extensions.Caching.Memory;
using System;

namespace Lljxww.Utilities.Cache;

public class CacheUtil
{
    public CacheUtil()
    {
        Init();
    }

    public Func<string, object?> GetFromCache { private set; get; }

    public Action<string, object> SetToCache { private set; get; }

    public int CacheSeconds { private set; get; } = 300;

    public CacheUtil WithCacheGet(Func<string, object> getFromCache)
    {
        GetFromCache = getFromCache;
        return this;
    }

    public CacheUtil WithCacheSet(Action<string, object> setToCache)
    {
        SetToCache = setToCache;
        return this;
    }

    public CacheUtil WithDefaultCacheSeconds(int cacheSeconds)
    {
        if (cacheSeconds > 0)
        {
            CacheSeconds = cacheSeconds;
        }

        return this;
    }

    public T? GetOrStore<T>(string key, Func<T> howToGetTheValue)
    {
        object? value = GetFromCache(key);

        if (value != null)
        {
            return (T)value;
        }

        if (howToGetTheValue == null)
        {
            return default;
        }

        T result = howToGetTheValue();

        if (result == null)
        {
            return result;
        }

        SetToCache?.Invoke(key, result);

        return result;
    }

    private void Init()
    {
        IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());

        SetToCache = (key, value) =>
        {
            if (string.IsNullOrWhiteSpace(key) || value == null)
            {
                return;
            }

            _ = cache.Set(key, value, new DateTimeOffset(DateTime.Now.AddSeconds(CacheSeconds)));
        };

        GetFromCache = key => string.IsNullOrWhiteSpace(key) ? default : cache.Get(key);
    }
}