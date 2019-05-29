//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System;

namespace GameFramework
{
    /// <summary>
    /// 变量。
    /// </summary>
    /// <typeparam name="T">变量类型。</typeparam>
    public abstract class Variable<T> : Variable
    {
        private T m_Value;

        private event Action<T> m_Evt_Set;
        public event Action<T> Evt_Set
        {
            add
            {
                if (value != null)
                {
                    m_Evt_Set += value;
                }
            }
            remove
            {
                if (value != null)
                {
                    m_Evt_Set -= value;
                }
            }
        }
        private event Action<T> m_Evt_ValueChange;
        public event Action<T> Evt_ValueChange
        {
            add
            {
                if (value != null)
                {
                    m_Evt_ValueChange += value;
                }
            }
            remove
            {
                if (value != null)
                {
                    m_Evt_ValueChange -= value;
                }
            }
        }
        /// <summary>
        /// 初始化变量的新实例。
        /// </summary>
        protected Variable()
        {
            m_Value = default(T);
        }

        /// <summary>
        /// 初始化变量的新实例。
        /// </summary>
        /// <param name="value">初始值。</param>
        protected Variable(T value)
        {
            m_Value = value;
        }

        /// <summary>
        /// 获取变量类型。
        /// </summary>
        public override Type Type
        {
            get
            {
                return typeof(T);
            }
        }

        /// <summary>
        /// 获取或设置变量值。
        /// </summary>
        public T Value
        {
            get
            {
                return m_Value;
            }
            set
            {
                m_Value = value;
            }
        }

        /// <summary>
        /// 获取变量值。
        /// </summary>
        /// <returns>变量值。</returns>
        public virtual T GetValue()
        {
            return m_Value;
        }

        /// <summary>
        /// 设置变量值。
        /// </summary>
        /// <param name="value">变量值。</param>
        public virtual void SetValue(T value)
        {
            m_Value = (T)value;
            m_Evt_Set.InvokeGracefully(m_Value);
        }

        /// <summary>
        /// 手动调用方法
        /// </summary>
        public virtual void ValueChangeAct()
        {
            m_Evt_ValueChange.InvokeGracefully(m_Value);
        }
        /// <summary>
        /// 重置变量值。
        /// </summary>
        public override void Reset()
        {
            m_Value = default(T);
        }

        /// <summary>
        /// 获取变量字符串。
        /// </summary>
        /// <returns>变量字符串。</returns>
        public override string ToString()
        {
            return (m_Value != null) ? m_Value.ToString() : "<Null>";
        }
    }
}
