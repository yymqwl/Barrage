using GameFramework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace TestWebSocket
{
    public abstract class AGameMainEntry : IGameMainEntry
    {



        protected bool m_IsLoop;
        public bool IsLoop { get => m_IsLoop; set { m_IsLoop = value; } }

        protected abstract void Init();
        protected abstract void ShutDown();
        public virtual void Entry(string[] args)
        {

            m_IsLoop = true;
            ///所有消息拉到主线程
            SynchronizationContext.SetSynchronizationContext(OneThreadSynchronizationContext.Instance);
            try
            {
                ClientTimer.Instance.Start();
                Init();
                while (m_IsLoop)
                {
                    try
                    {
                        Thread.Sleep(GameConstant.TThreadInternal);
                        ClientTimer.Instance.Update();
                        GameModuleManager.Instance.Update();
                        OneThreadSynchronizationContext.Instance.Update();
                    }
                    catch (Exception e)
                    {
                        Log.Error(e);
                    }
                }
                ShutDown();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}