using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artech.Cache
{
    public interface ICacheValue : IDisposable
    {
        event EventHandler DisposeCache;
        event EventHandler UpdateCache;

        void Update();

        /// <summary>
        /// 缓存策略
        /// </summary>
        CachePolicy CachePolicy { get; }
        /// <summary>
        /// 是否过期
        /// </summary>
        bool IsExpire { get; }
        /// <summary>
        /// 缓存开始时间
        /// </summary>
        DateTime StartCache { get; }
        /// <summary>
        /// 缓存中的值
        /// </summary>
        object Value { get; set; }
    }
}
