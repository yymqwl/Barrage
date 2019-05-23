using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;

namespace GameMain
{
    public abstract class AGameTimer
    {
        protected DateTime m_Cur_DateTime;//上次刷新
        protected DateTime m_Last_DateTime;//这次刷新
        protected TimeSpan m_Delta_TimeSpan;//间隔


        public  float UnScaledDeltaTime
        {
            get => (float)m_Delta_TimeSpan.TotalSeconds;
        }
        public  float DeltaTime
        {
            get => ( (float)m_Delta_TimeSpan.TotalSeconds * TimeScale);
        }

        protected float m_TimeScale;
        public float TimeScale { get =>m_TimeScale; set { m_TimeScale = value; } }

        public virtual bool Start()
        {
            m_Cur_DateTime = DateTime.UtcNow;
            m_Last_DateTime = DateTime.UtcNow;
            m_Delta_TimeSpan = new TimeSpan(0);
            m_TimeScale = 1;

            return true;
        }
        public void Update()
        {
            m_Cur_DateTime  = DateTime.UtcNow;
            m_Delta_TimeSpan = m_Cur_DateTime - m_Last_DateTime ;
            m_Last_DateTime = m_Cur_DateTime;
        }

        public  bool Shutdown()
        {
            return true;
        }
    }
}
