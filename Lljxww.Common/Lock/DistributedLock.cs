using CSRedis;
using System;
using System.Threading;

namespace Lljxww.Common.Lock
{
    public class DistributedLock
    {
        private readonly int LOCK_SECONDS = 20;
        private readonly int RETRY_TIMES = 10;

        /// <summary>
        /// 使用分布式锁,尽量保证action是单例执行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="action"></param>
        /// <param name="onLockFunc"></param>
        /// <returns></returns>
        public T? Invoke<T>(string key, Func<T> action, Func<T>? onLockFunc = null)
        {
            (bool success, T? result) = Invoke(key, action);

            if (success)
            {
                return result;
            }

            if (onLockFunc == null)
            {
                for (int i = 0; i < RETRY_TIMES; i++)
                {
                    (success, result) = Invoke(key, action);

                    if (success)
                    {
                        return result;
                    }
                    else
                    {
                        Thread.Sleep(1000 * LOCK_SECONDS / RETRY_TIMES);
                        continue;
                    }
                }

                return default;
            }
            else
            {
                return onLockFunc.Invoke();
            }
        }

        private (bool, T?) Invoke<T>(string key, Func<T> action)
        {
            CSRedisClientLock? redisLock = RedisHelper.Lock(key, LOCK_SECONDS);

            try
            {
                // 未取得锁
                if (redisLock == null)
                {
                    return (false, default);
                }

                return (true, action.Invoke());
            }
            finally
            {
                redisLock?.Unlock();
            }
        }
    }
}
