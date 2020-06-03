using GameFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TableRoomSilo
{
    public class SiloTimer
    {
        protected DateTime m_Cur_DateTime;//上次刷新
        protected DateTime m_Last_DateTime;//这次刷新
        protected TimeSpan m_Delta_TimeSpan;//间隔

        public float DeltaTime
        {
            get => ((float)m_Delta_TimeSpan.TotalSeconds);
        }
        public void Reset()
        {
            m_Cur_DateTime = DateTime.UtcNow;
            m_Last_DateTime = DateTime.UtcNow;
            m_Delta_TimeSpan = new TimeSpan(0);
            
        }

        
        public bool CheckUpdate()
        {
           m_Cur_DateTime =  DateTime.UtcNow;
           m_Delta_TimeSpan = m_Cur_DateTime - m_Last_DateTime;
            if (DeltaTime < GameConstant.DeltaServerTime)
            {
                return false;
            }
            m_Last_DateTime = m_Cur_DateTime;
            return true;
        }

    }
}
