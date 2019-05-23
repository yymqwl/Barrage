using GameFramework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace GameMain
{
    public class GameMainEntry : IGameMainEntry
    {
        public IGameModuleManager GameModuleManager => m_GameModuleManager;

        protected bool m_IsLoop;
        public bool IsLoop { get => m_IsLoop; set { m_IsLoop = value; } }

        protected IGameModuleManager m_GameModuleManager;

        protected virtual void Init()
        {

        }
        public virtual void Main(string[] args)
        {

            m_IsLoop = true;
            ///所有消息拉到主线程
            SynchronizationContext.SetSynchronizationContext(OneThreadSynchronizationContext.Instance);
            try
            {

                Init();
                //m_Cur_Dt = DateTime.Now;
                //m_Last_Dt = DateTime.Now;
                while (m_IsLoop)
                {
                    try
                    {
                        Thread.Sleep(GameConstant.TThreadInternal);
                        /*m_Cur_Dt = DateTime.Now;
                        m_Cur_Ts = m_Cur_Dt - m_Last_Dt;
                        */
                        //m_GameModuleManager.Update(m_Cur_Ts.TotalSeconds, m_Cur_Ts.TotalSeconds);
                        OneThreadSynchronizationContext.Instance.Update();
                    }
                    catch (Exception e)
                    {
                        Log.Error(e);
                    }
                }


            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}
