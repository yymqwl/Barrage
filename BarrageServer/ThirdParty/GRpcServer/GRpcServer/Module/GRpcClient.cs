using System;
using System.Collections.Generic;
using System.Text;
using Grpc.Core;
using GameFramework;

namespace GRpcServer
{
    [GameFrameworkModuleAttribute]
    public class GRpcClient : GameFrameworkModule
    {

        public override int Priority => 100;
        Channel m_Channel;
        public override bool Init()
        {
            var cfgmg = GameModuleManager.Instance.GetModule<ConfigManager>();
            var item = cfgmg.GetStartConfigItem(ServerType.All);


            m_Channel = new Channel(item.Rpg_Ip, ChannelCredentials.Insecure);
            


            return base.Init();
        }

        public override void Update()
        {

        }
        public override bool ShutDown()
        {
            m_Channel.ShutdownAsync().Wait();
            return base.ShutDown();
        }
    }
}
