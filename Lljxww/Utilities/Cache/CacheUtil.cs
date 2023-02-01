using System;
using Microsoft.Extensions.Caching.Memory;

namespace Lljxww.Utilities.Cache;

public class CacheUtil
{
    public CacheUtil()
    {
        Init();
    }

    public Func<string, object> getFromCache { private set; get; }
    
    public  Action<string, object> setToCache { private set; get; }

    public int cacheSeconds { private set; get; } = 300;

    public CacheUtil WithCacheGet(Func<string, object> getFromCache)
    {
        this.getFromCache = getFromCache;
        return this;
    }

    public CacheUtil WithCacheSet(Action<string, object> setToCache)
    {
        this.setToCache = setToCache;
        return this;
    }

    public CacheUtil WithDefaultCacheSeconds(int cacheSeconds)
    {
        if (cacheSeconds > 0)
        {
            this.cacheSeconds = cacheSeconds;
        }
        
        return this;
    }

    public T GetOrStore<T>(string key, Func<T> howToGetTheValue)
    {
        object value = getFromCache(key);

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

        if (setToCache != null)
        {
            setToCache(key, result);
        }

        return result;
    }

    private void Init()
    {
        IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());

        setToCache = (key, value) =>
        {
            if (string.IsNullOrWhiteSpace(key) || value == null)
            {
                return;
            }
            
            cache.Set(key, value, new DateTimeOffset(DateTime.Now.AddSeconds(cacheSeconds)));
        };

        getFromCache = key => string.IsNullOrWhiteSpace(key) ? null : cache.Get(key);
    }
}