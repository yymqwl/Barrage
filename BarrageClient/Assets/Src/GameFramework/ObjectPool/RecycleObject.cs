using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameFramework
{
    

    public abstract class RecycleObject<T> : IRecycle
    {
        private bool m_InUse;
        private bool m_Locked;
        private DateTime m_LastUseTime;
        private readonly T m_Owner;


        public bool InUse
        {
            get
            {
                return m_InUse;
            }
            set
            {
                m_InUse = value;
            }
        }
        /// <summary>
        /// 获取或设置对象是否被加锁。
        /// </summary>
        public bool Locked
        {
            get
            {
                return m_Locked;
            }
            set
            {
                m_Locked = value;
            }
        }

        /// <summary>
        /// 获取对象上次使用时间。
        /// </summary>
        public DateTime LastUseTime
        {
            get
            {
                return m_LastUseTime;
            }
            internal set
            {
                m_LastUseTime = value;
            }
        }

        public T Owner
        {
            get
            {
                return m_Owner;
            }
        }

        public RecycleObject(T owner)
        {
            m_Owner = owner;
            m_Locked = false;
            m_LastUseTime = DateTime.Now;
            m_InUse = false;

        }
        /// <summary>
        /// 
        /// 初始化调用
        /// </summary>
        /// <returns></returns>
        public virtual bool Init()
        {
            return true;
        }
        public virtual void Release()
        {

        }


        /// <summary>
        /// 获取对象时的事件。
        /// </summary>
        public  virtual void OnSpawn()
        {
            m_InUse = true;
        }
        public virtual void OnUnspawn()
        {
            m_InUse = false;
        }
    }


   


}
