namespace Artech.Cache
{
    /// <summary>
    /// 缓存枚举，用于设置缓存的更新方法
    /// </summary>
    public enum CacheMethod
    {
        /// <summary>
        /// 自动
        /// </summary>
        Auto,
        /// <summary>
        /// 根据时间
        /// </summary>
        TimeLapsed,
        /// <summary>
        /// 永不过期
        /// </summary>
        Never
    }
}