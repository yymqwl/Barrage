using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TableRoom
{
    public class GServerTimer : Grain, IServerTimer
    {
        protected DateTime m_Cur_DateTime;//
        protected DateTime m_Last_DateTime;//
        protected TimeSpan m_Delta_TimeSpan;//

        public async Task<bool> CheckUpdate()
        {

            m_Cur_DateTime = await GetDateTimeNow();
            m_Delta_TimeSpan = m_Cur_DateTime - m_Last_DateTime;
            var dt = await DeltaTime();

            if (dt < GameConstant.DeltaServerTime)
            {
                return false;
            }
            m_Last_DateTime = m_Cur_DateTime;
            return true;
        }

        public Task<float> DeltaTime()
        {
            return Task.FromResult((float) m_Delta_TimeSpan.TotalSeconds);
        }

        public Task<DateTime> GetDateTimeNow()
        {
            return Task.FromResult(DateTime.Now);
        }
        public override Task OnActivateAsync()
        {
            this.Reset();
            return base.OnActivateAsync();
        }
        protected void Reset()
        {
            m_Cur_DateTime = DateTime.UtcNow;
            m_Last_DateTime = DateTime.UtcNow;
            m_Delta_TimeSpan = new TimeSpan(0);
        }


    }
}
