﻿namespace GameFramework.Event
{
    public partial class EventPool<T>
    {
        /// <summary>
        /// 事件结点。
        /// </summary>
        private sealed class Event
        {
            private readonly object m_Sender;
            private readonly T m_EventArgs;

            public Event(object sender, T e)
            {
                m_Sender = sender;
                m_EventArgs = e;
            }

            public object Sender
            {
                get
                {
                    return m_Sender;
                }
            }

            public T EventArgs
            {
                get
                {
                    return m_EventArgs;
                }
            }
        }
    }
}
