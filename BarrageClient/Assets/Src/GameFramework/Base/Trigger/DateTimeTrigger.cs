using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework
{
    public class DateTimeTrigger : ATrigger
    {

        Action<object, object> m_Act;
        DateTime m_DateTime;//触发时间
        public void Start(DateTime  dt1, Action<object, object> act)
        {
            m_Act = act;
            m_DateTime = dt1;
            TriggerTimes = 1;
        }
        public static DateTimeTrigger Create(DateTime dt, Action<object, object> act)
        {
            DateTimeTrigger dtt = new DateTimeTrigger();
            dtt.Start(dt,act);
            return dtt;
        }
        public override void Init()
        {
            base.Init();
        }
        public override void Update(float elapsedTime)
        {
            if (!HasTrigger())//
            {
                return;
            }
            if (m_DateTime <= DateTime.Now)
            {
                TriggerTimes--;
                m_Act.InvokeGracefully(m_DateTime,null);
            }
        }
    }
}
