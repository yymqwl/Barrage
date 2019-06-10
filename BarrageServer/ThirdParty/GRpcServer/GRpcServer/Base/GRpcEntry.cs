using GameFramework;
using GameMain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace GRpcServer
{

    public enum ServerType
    {
        None=0,
        MsgParse=1,//解析,网关
        Room=1<<1,//角色跟房间

        All= MsgParse|Room
    }

    public class GRpcEntry : AGameMainEntry
    {

        public ServerType ServerType
        {
            get
            {
                return m_ServerType;
            }
        }
        protected ServerType m_ServerType;

        public static GRpcEntry Instance { get; } = new GRpcEntry();
        protected override void Init()
        {
            m_ServerType = ServerType.All;



            AssemblyManager.Instance.Add(GetType().Assembly);
            GameModuleManager.Instance.CreateModule<ConsoleModule>().IGameMainEntry = this;

            GameModuleManager.Instance.CreateModule<ConfigManager>();
            GameModuleManager.Instance.CreateModule<GRpcNetWork>();
            GameModuleManager.Instance.CreateModule<GRpcClient>();
            //GameModuleManager.Instance.CreateModule<SiloNetWork>();

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
