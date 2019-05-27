using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework
{
    public class UpdateTime : CObject
    {
        private event Action m_Evt_Act;
        public event Action Evt_Act
        {
            add
            {
                if (value != null)
                {
                    m_Evt_Act += value;
                }
            }
            remove
            {
                if (value != null)
                {
                    m_Evt_Act -= value;
                }
            }
        }
        private float m_TimeSpan;
        private float m_Internal;//

        public UpdateTime(float Internal)
        {
            m_Internal = Internal;
            m_TimeSpan = 0;
        }
        public virtual void Update(float elapsedTime)
        {
            m_TimeSpan += elapsedTime;
            if(m_TimeSpan >=m_Internal)
            {
                m_TimeSpan -= m_Internal;
                m_Evt_Act();//触发
            }
     
        }

        public override void Dispose()
        {
            m_Evt_Act = null;
            base.Dispose();
        }

    }
}
