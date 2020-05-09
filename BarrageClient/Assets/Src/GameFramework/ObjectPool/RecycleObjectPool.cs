using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace GameFramework
{
    
    /// <summary>
    /// 对象池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RecycleObjectPool<T,V> where  T :  RecycleObject<V>
    { 

        /// <summary>
        /// 内存池的容量
        /// </summary>
        private readonly uint m_InitialCapacity;

        /// </summary>
        public uint MaxCapacity { get; set; }

        private uint ExtendNub { get; set; }

        /// <summary>
        /// 内存池
        /// </summary>
        protected LinkedList<T> m_FreePool = new LinkedList<T>();//

        protected LinkedList<T> m_UsedPool = new LinkedList<T>();//


        RecycleObjectFactory<T> m_RcFactory;

        public RecycleObjectFactory<T> RecycleObjectFactory
        {
            get
            {
                return m_RcFactory;
            }
            set { m_RcFactory = value; }
        }





     
        public RecycleObjectPool(RecycleObjectFactory<T>  factory , uint iInitialCapacity = 64,  uint extendnub =64, uint maxCapacity = int.MaxValue)
        {
            m_RcFactory = factory;
            m_InitialCapacity = iInitialCapacity;
            MaxCapacity = maxCapacity;
            ExtendNub = extendnub;
            m_FreePool.Clear();
            m_UsedPool.Clear();
            for (int iIndex = 0; iIndex < iInitialCapacity; ++iIndex)
            {
                m_FreePool.AddLast(CreateByFactory());
            }
        }
        private T CreateByFactory()
        {
            var obj = m_RcFactory.CreateObject();
            obj.Init();
            return obj;
        }

        /// <summary>
        /// 扩展数据
        /// </summary>
        private void Extend()
        {
            for (int iIndex = 0; iIndex < ExtendNub; ++iIndex)
            {
                m_FreePool.AddLast(CreateByFactory());
            }
        }




        /// <summary>
        /// 
        /// </summary>
        ~RecycleObjectPool()
        {

        }

        /// <summary>
        /// 输出对象池的一些状态
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {

            StringBuilder ret = new StringBuilder(512);
            ret.AppendFormat("{0}\r\n", Name);
            ret.AppendFormat("FreeCount:{0}\r\n", m_FreePool.Count);
            ret.AppendFormat("InitialCapacity:{0}\r\n", m_InitialCapacity);
            return ret.ToString();
        }

        /// <summary>
        /// 初始化内存池
        /// </summary>
        /// <param name="name">对象池的名字</param>
        /// <param name="iInitialCapacity">初始化内存池对象的数量</param>
/*        public RecycleObjectPool(string name, RecycleObjectFactory<T> factory, uint iInitialCapacity = 1024)
            : this(factory, iInitialCapacity)
        {
            m_Name = name;
        }

*/


    


        /// <summary>
        /// 内存池请求数据
        /// </summary>
        /// <returns></returns>
        public T Spawn()
        {

            if (m_FreePool.First ==null)
            {
                Extend();
            }
            var obj = m_FreePool.First;
            m_FreePool.Remove(obj);
            m_UsedPool.AddLast(obj);
            obj.Value.OnSpawn();

            return obj.Value;

        }


        /// <summary>
        /// 内存池释放数据
        /// </summary>
        /// <param name="content"></param>
        public void UnSpawn(T content)
        {
            content.OnUnspawn();
            var obj = m_UsedPool.Find(content);
            if(obj == null)
            {
                throw new  GameFrameworkException("UnSpawn Null");
            }
             
            m_UsedPool.Remove(obj);
            m_FreePool.AddLast(obj);

        }

        /// <summary>
        /// 释放内存池内全部的数据
        /// </summary>
        public void Free()
        {
            foreach(var obj in m_FreePool)
            {
                obj.Release();
            }
            foreach (var obj in m_UsedPool)
            {
                obj.Release();
            }
        }


        /// <summary>
        /// 给出内存池的详细信息
        /// </summary>
        /// <returns></returns>
        public PoolInfo GetPoolInfo()
        {
            // 不需要锁定的，因为只是给出没有修改数据
            return new PoolInfo
            {
                Name = Name,
                FreeCount = m_FreePool.Count,
                InitialCapacity = m_InitialCapacity,
            };
        }

        


        private string m_Name;

        /// <summary>
        /// 对象池的名字
        /// </summary>
        public string Name
        {
            get
            {
                if (m_Name == null)
                {
                    var type = typeof(T);
                    if (type.IsGenericType)
                    {
                        m_Name = type.Name + type.GetGenericArguments()[0].Name;
                    }
                    else
                    {
                        m_Name = type.Name;
                    }
                }
                return m_Name;
            }
        }

        
    }

}