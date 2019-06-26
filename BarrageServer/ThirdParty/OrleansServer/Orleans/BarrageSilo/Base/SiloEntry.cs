using GameFramework;
using GameMain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BarrageSilo
{
    public class SiloEntry: AGameMainEntry
    {
        public static SiloEntry Instance { get; } = new SiloEntry();
        protected override void Init()
        {
            AssemblyManager.Instance.Add(typeof( IHall.IHello).Assembly);
            AssemblyManager.Instance.Add(GetType().Assembly);
            AssemblyManager.Instance.Add(typeof(HallGrains.HelloGrain).Assembly);
            GameModuleManager.Instance.CreateModule<ConsoleModule>().IGameMainEntry = this;
            GameModuleManager.Instance.CreateModule<SiloNetWork>();
            GameModuleManager.Instance.CreateModule<SiloModule>();
            GameModuleManager.Instance.CreateModule<SiloClient>();
            GameModuleManager.Instance.CreateModule<ConfigManager>();

            /*GameModuleManager.Instance.CreateModules(typeof(SiloEntry).Assembly,(Type tp)=>
            {
                object[] objects = tp.GetCustomAttributes(typeof(GameFrameworkModuleAttribute), false);
                if (objects.Length > 0 && tp.Namespace.Contains("BarrageSilo") )
                {
                    return true;
                }
                
                return false;

            });
            */

            GameModuleManager.Instance.Init();
        }
        protected override void ShutDown()
        {
            GameModuleManager.Instance.ShutDown();
        }
        async Task DoAsync()
        {
            await Task.Run(() => { });
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
                        OneThreadSynchronizationContext.Instance.Update();
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
