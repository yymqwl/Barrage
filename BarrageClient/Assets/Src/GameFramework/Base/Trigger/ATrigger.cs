using System;
using System.Collections.Generic;


namespace GameFramework
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T0">psend</typeparam>
    /// <typeparam name="T1">param</typeparam>
    /// <returns></returns>

    
    public abstract class ATrigger :CObject
    {
        int m_TriggerTimes;

        public int TriggerTimes //大于0判断是否触发完毕
        {
            get { return m_TriggerTimes; }
            set { m_TriggerTimes = value; }
        }

        /// <summary>
        /// 是否还能触发
        /// </summary>
        /// <returns></returns>
        public virtual bool HasTrigger()
        {
            return TriggerTimes > 0;
        }

        public override void Init()
        {
            TriggerTimes = 1;
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        public virtual void Update(float elapsedTime)
        {

        }
    }
}
