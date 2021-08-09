using Lljxww.Common.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lljxww.Common.Data
{
    public class Caching
    {
        private readonly IDistributedCache _redis;

        public Caching(IDistributedCache redis)
        {
            _redis = redis;
        }

        /// <summary>
        /// 对象锁
        /// </summary>
        private static readonly object objlock = new();

        /// <summary>
        /// 从缓存中读取数据, 如果数据不存在与缓存中, 则使用给定的方法查询数据, 并保存到缓存中.
        /// </summary>
        /// <typeparam name="T">缓存数据类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="howToGetValueFunc">查询数据的委托</param>
        /// <param name="expire">过期时间(分),默认为127</param>
        /// <returns>要查询的数据</returns>
        public T GetOrStore<T>(string key, Func<T> howToGetValueFunc, DateTime? expire = null)
        {
            //先查是否已经有数据
            T? value = Get<T>(key);
            if (value == null)
            {
                lock (objlock)
                {
                    value = Get<T>(key);
                    if (value == null)
                    {
                        //设置数据
                        value = howToGetValueFunc.Invoke();
                        Set(key, value, expire);
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// 从缓存中的T类型列表读取指定的一个对象. 如果在列表中不存在指定的对象, 则尝试使用给定的方法获取此对象, 并更新到缓存的列表中
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="predicate">从列表中获取对象的方法</param>
        /// <param name="howToGetValueFunc">查询数据的委托</param>
        /// <param name="expire">过期时间(分),默认为127</param>
        /// <returns>要查询的数据</returns>
        public T? GetOrStoreFromList<T>(string key, Func<T, bool> predicate, Func<T> howToGetValueFunc, DateTime? expire = null)
        {
            //先查是否已经有数据
            IList<T>? resultList = Get<IList<T>>(key);
            T? result = default;
            if (resultList == default || !resultList.Any(predicate))
            {
                lock (objlock)
                {
                    resultList = Get<IList<T>>(key);
                    if (resultList == default || !resultList.Any(predicate))
                    {
                        //设置数据
                        T item = howToGetValueFunc.Invoke();
                        if (item != null)
                        {
                            if (resultList == default)
                            {
                                resultList = new List<T>();
                            }

                            resultList.Add(item);
                            result = item;
                            Set(key, resultList, expire);
                        }
                    }
                    else
                    {
                        result = resultList.First(predicate);
                    }
                }
            }
            else
            {
                result = resultList.First(predicate);
            }

            return result;
        }

        /// <summary>
        /// 删除缓存列表中指定的一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="predicate">从列表中获取对象的方法</param>
        /// <param name="expire">过期时间(分),默认为127</param>
        public void RemoveFromList<T>(string key, Func<T, bool> predicate, DateTime? expire = null)
        {
            IList<T>? resultList = Get<IList<T>>(key);
            if (resultList == default || !resultList.Any(predicate))
            {
                lock (objlock)
                {
                    resultList = Get<IList<T>>(key);
                    if (resultList == default || !resultList.Any(predicate))
                    {
                        return;
                    }

                    T? match = resultList.SingleOrDefault(predicate);
                    if (match != null)
                    {
                        resultList.Remove(match);
                    }

                    Set(key, resultList, expire);
                }
            }
        }

        /// <summary>
        /// 存储一个对象到缓存中
        /// </summary>
        /// <typeparam name="T">缓存对象类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expire">过期时间(分), 默认为127</param>
        /// <returns>缓存结果</returns>
        public void Set<T>(string key, T value, DateTime? expire = null)
        {
            Remove(key);

            TimeSpan timespan = expire.HasValue ? expire.Value - DateTime.Now : new TimeSpan(0, 10, 0);

            _redis.Set(key, value?.ToBytes(), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = timespan
            });
        }

        /// <summary>
        /// 读取一个缓存对象
        /// </summary>
        /// <typeparam name="T">缓存对象类型</typeparam>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public T? Get<T>(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return default;
            }

            T? value = _redis.Get(key).ToObject<T>();
            return value;
        }

        /// <summary>
        /// 删除缓存中的指定对象
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>删除结果</returns>
        public void Remove(string key)
        {
            _redis.Remove(key);
        }
    }
}
