using GameFramework;
using GameMain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace BarrageSilo
{
    public class SiloEntry: AGameMainEntry
    {
        public static SiloEntry Instance { get; } = new SiloEntry();
        protected override void Init()
        {
            GameModuleManager.Instance.CreateModule<ConsoleModule>().IGameMainEntry = this;
            GameModuleManager.Instance.CreateModules(typeof(SiloEntry).Assembly,(Type tp)=>
            {
                object[] objects = tp.GetCustomAttributes(typeof(GameFrameworkModuleAttribute), false);
                if (objects.Length > 0 && tp.Namespace.Contains("BarrageSilo") )
                {
                    return true;
                }
                
                return false;

            });
            

            GameModuleManager.Instance.Init();
        }
        protected override void ShutDown()
        {
            GameModuleManager.Instance.ShutDown();
        }
        public override void Entry(string[] args)
        {
            
            m_IsLoop = true;
            try
            {

                ClientTimer.Instance.Start();
                Init();

                SynchronizationContext.SetSynchronizationContext(OneThreadSynchronizationContext.Instance);

                while (m_IsLoop)
                {
                    try
                    {
                        Thread.Sleep(GameConstant.TThreadInternal);
                        ClientTimer.Instance.Update();
                        GameModuleManager.Instance.Update();
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
