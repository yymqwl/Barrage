using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework
{
    public class TimeTrigger :ATrigger
    {

        Action<object, object> m_Act;
        float m_StartTime;//开始时间
        float m_TrgTime;//触发时间
        bool m_IsStart;//是否已开始
        public void Start(float TrgTime, Action<object, object>  act)
        {
            m_StartTime = 0 ;
            m_TrgTime = TrgTime;
            m_IsStart = true;
            m_Act = act;
            TriggerTimes = 1;
        }
        public override void Init()
        {
            base.Init();
            m_StartTime = 0;
            m_TrgTime = 0;
            m_IsStart = false;
        }
        public static TimeTrigger Create(float TrgTime, Action<object, object> act)
        {
            TimeTrigger tt = new TimeTrigger();
            tt.Start(TrgTime,act);
            return tt;
        }



        public override void Update(float elapsedTime)
        {
            if (!m_IsStart )
            {
                return;
            }
            if (!HasTrigger())//没有触发次数
            {
                return;
            }
            m_StartTime +=elapsedTime;
            if (m_StartTime >=m_TrgTime)
            {
                m_Act.InvokeGracefully(m_StartTime,null);
                TriggerTimes--;
            }
        }


    }
}
