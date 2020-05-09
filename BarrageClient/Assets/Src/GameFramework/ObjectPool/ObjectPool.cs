using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework
{
    public class ObjectPool<T>
    {

        /// <summary>
        /// 内存池的容量
        /// </summary>
        private readonly uint m_InitialCapacity;
        private uint ExtendNub { get; set; }


        /// <summary>
        /// 内存池
        /// </summary>
        protected LinkedList<T> m_FreePool = new LinkedList<T>();//

        protected LinkedList<T> m_UsedPool = new LinkedList<T>();//

        ObjectFactory<T> m_ObjFactory;
        public ObjectFactory<T> ObjFactory
        {
            get
            {
                return m_ObjFactory;
            }
            set { m_ObjFactory = value; }
        }
        public ObjectPool(ObjectFactory<T> factory, uint iInitialCapacity = 4, uint extendnub = 4)
        {
            m_ObjFactory = factory;
            m_InitialCapacity = iInitialCapacity;
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
            var obj = m_ObjFactory.CreateObj();
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
        public T Spawn()
        {
            if (m_FreePool.First == null)
            {
                Extend();
            }
            var obj = m_FreePool.First;
            m_FreePool.RemoveFirst();
            m_UsedPool.AddLast(obj);
            return obj.Value;
        }
        public void UnSpawn(T content)
        {
            var obj = m_UsedPool.Find(content);
            if (obj == null)
            {
                throw new GameFrameworkException("UnSpawn Null");
            }
            m_UsedPool.Remove(obj);
            m_FreePool.AddLast(obj);
        }


        /// <summary>
        /// 释放内存池内全部的数据
        /// </summary>
        public void Free()
        {
            foreach (var obj in m_FreePool)
            {
                m_ObjFactory.ReleaseObj(obj);
                //obj.Release();
            }
            foreach (var obj in m_UsedPool)
            {
                m_ObjFactory.ReleaseObj(obj);
            }
        }

    }
}
