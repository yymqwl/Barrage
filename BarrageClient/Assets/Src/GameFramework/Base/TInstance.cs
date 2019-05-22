namespace GameFramework
{
    /// <summary>
    /// 单例模板
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TInstance<T> where T : new()
    {
        private static readonly T m_Instance = new T();

        /// <summary>
        /// 单例
        /// </summary>
        public static T Instance { get { return m_Instance; } }
        
    }


}
