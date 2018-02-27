using System;
using System.Collections.Generic;
using System.Threading;

namespace Artech.Cache
{
    /// <summary>
    /// 
    /// </summary>
    public class GlobalCache : IDisposable
    {
        private readonly Dictionary<string, ICacheValue> _cache;
        private static GlobalCache _globalCache;
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
        private GlobalCache()
        {
            _cache = new Dictionary<string, ICacheValue>();
        }

        /// <summary>
        /// 
        /// </summary>
        public static GlobalCache Instance
        {
            get
            {
                if (_globalCache != null) return _globalCache;
                var temp = new GlobalCache();
                Interlocked.CompareExchange(ref _globalCache, temp, null);
                return _globalCache;
            }
        }
        /// <summary>
        /// 所有缓存的键
        /// </summary>
        public List<string> Keys
        {
            get
            {
                Dictionary<string, ICacheValue>.KeyCollection collection = _cache.Keys;
                return (collection.Count == 0) ? new List<string>() : new List<string>(collection);
            }
        }

        /// <summary>
        /// 添加至缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="icv"></param>
        public Boolean AddCache(string key, ICacheValue icv)
        {
            
            if (!string.IsNullOrEmpty(key) && _lock.TryEnterWriteLock(50))
            {
                try
                {
                    if (_cache.ContainsKey(key))
                    {
                        _cache[key] = icv;
                    }
                    else
                    {
                        _cache.Add(key, icv);
                    }
                    return true;
                }
                finally
                {
                    _lock.ExitWriteLock();
                }
            }
            return false;
        }
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public ICacheValue GetCache(string key)
        {
            ICacheValue value2;
            _cache.TryGetValue(key, out value2);
            return value2;

        }
        /// <summary>
        /// 获取缓存中的值
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">键</param>
        /// <returns></returns>
        public T GetValue<T>(string key)
        {
            ICacheValue cache = GetCache(key);
            if (cache == null)
            {
                return default(T);
            }
            return (T)cache.Value;
        }
        /// <summary>
        /// 根据key移除缓存中的值
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            ICacheValue cache = GetCache(key);
            if (cache == null) return;
            if (!_lock.TryEnterWriteLock(50)) return;
            try
            {
                cache.Dispose();
                _cache.Remove(key);
            }
            finally
            {
                _lock.ExitWriteLock();
            }

        }

        /// <summary>
        /// 释放缓存（移除所有缓存）
        /// </summary>
        public void Dispose()
        {
            Dictionary<string, ICacheValue>.KeyCollection keys = _cache.Keys;
            if ((keys.Count == 0)) return;
            List<string> list = Keys;
            while (list.Count > 0)
            {
                Remove(list[0]);
                list.RemoveAt(0);
            }
        }
    }
}