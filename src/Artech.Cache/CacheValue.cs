using System;

namespace Artech.Cache
{
    public class CacheValue : ICacheValue
    {
        private readonly CachePolicy _cp;
        private readonly object _lockObj;
        private DateTime _startCache;
        private object _value;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="value">缓存中的值</param>
        /// <param name="cachePolicy">缓存策略</param>
        public CacheValue(object value, CachePolicy cachePolicy)
        {
            _lockObj = new object();
            _startCache = DateTime.Now;
            _cp = cachePolicy;
            _value = value;
        }


        #region >IDispose Impl

        public void Dispose()
        {
            Delegate[] invocationList;
            OnDispose(new EventArgs());
            if (UpdateCache != null)
            {
                invocationList = UpdateCache.GetInvocationList();
                foreach (Delegate delegate2 in invocationList)
                {
                    UpdateCache = (EventHandler)Delegate.Remove(UpdateCache, delegate2); //(EventHandler)delegate2
                }
            }
            if (DisposeCache == null) return;
            invocationList = DisposeCache.GetInvocationList();
            foreach (Delegate delegate2 in invocationList)
            {
                DisposeCache = (EventHandler)Delegate.Remove(DisposeCache, delegate2);  //(EventHandler)delegate2
            }
        }

        #endregion

        #region >ICacheValue Impl

        public event EventHandler DisposeCache;
        public void OnDispose(EventArgs e)
        {
            EventHandler handler = DisposeCache;
            if (handler != null) handler(this, e);
        }

        public event EventHandler UpdateCache;
        public void OnUpdate(EventArgs e)
        {
            EventHandler handler = UpdateCache;
            if (handler != null) handler(this, e);
        }
        /// <summary>
        /// 更新缓存，更新的同时刷新StartCache
        /// </summary>
        public void Update()
        {
            if ((_cp.CacheMethod == CacheMethod.Never)) return;
            _startCache = DateTime.Now;
            OnUpdate(new EventArgs());
        }

        public CachePolicy CachePolicy
        {
            get { return _cp; }
        }

        /// <summary>
        /// 缓存是否过期，为0时表示永不过期
        /// </summary>
        public bool IsExpire
        {
            get
            {
                if (_cp.CacheMethod != CacheMethod.TimeLapsed || _cp.ExpiredTime <= 0)
                {
                    return false;
                }
                TimeSpan span = (DateTime.Now - _startCache);
                return (span.TotalMinutes > _cp.ExpiredTime);
            }
        }
        /// <summary>
        /// 缓存开始时间
        /// </summary>
        public DateTime StartCache
        {
            get
            {
                return _startCache;
            }
        }
        /// <summary>
        /// 缓存中的值
        /// </summary>
        public object Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (IsExpire) return;
                lock (_lockObj)
                {
                    _value = value;
                }
            }
        }

        #endregion



    }
}