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

        protected IGameModuleManager m_GameModuleManager;

        protected virtual void Init()
        {
        }
        public virtual void Main(string[] args)
        {
            ///所有消息拉到主线程
            SynchronizationContext.SetSynchronizationContext(OneThreadSynchronizationContext.Instance);
            try
            {

                Init();

                while (true)
                {
                    try
                    {
                        Thread.Sleep(1);
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
