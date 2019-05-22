using System;
namespace GameFramework
{
    public class ContextInfo
    {

        public readonly string Name;

        public DoubleMap<Type, int> DMap;
        
        public ContextInfo(string name)
        {
            DMap = new DoubleMap<Type, int>(); 
        }
        public int GetComponentIndex<T>()
        {
            int index = DMap.GetValueByKey(typeof(T));
            if(index<=0)
            {
                throw new GameFrameworkException($"GetComponentIndexe Error{index}");
            }

            return DMap.GetValueByKey(typeof(T));
        }
        public int TotalComponents()
        {
            return DMap.Keys.Count;
        }
    }
}
