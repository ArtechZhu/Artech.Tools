namespace Artech.Cache
{
    /// <summary>
    /// 缓存策略
    /// </summary>
    public class CachePolicy
    {
        /// <summary>
        /// 缓存枚举：更新方式
        /// </summary>
        public CacheMethod CacheMethod { get; set; }

        /// <summary>
        /// 过期时间（分钟），当使用CacheMethod.TimeLapsed的时候，该值必须设定为>0的值，否则无效
        /// </summary>
        public int ExpiredTime { get; set; }

        /// <summary>
        /// 更新周期
        /// </summary>
        public long UpdateFrequency { get; set; }

        public CachePolicy()
        {
            this.CacheMethod = CacheMethod.Auto;
            this.ExpiredTime = 20;
            this.UpdateFrequency = 0L;
        }

    }
}