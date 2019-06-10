using GameFramework;
using System;
using System.Collections.Generic;
using System.Text;
using Grpc.Core;
using System.Net;
namespace GRpcServer
{
    [GameFrameworkModuleAttribute]
    public class GRpcNetWork : GameFrameworkModule
    {

        public override int Priority => 1000;
        protected Server m_Server;
        public Server Server
        {
            get
            {
                return m_Server;
            }
        }

        public override bool Init()
        {

            var cfgmg = GameModuleManager.Instance.GetModule<ConfigManager>();

            var item = cfgmg.GetStartConfigItem(ServerType.All);

            string str_ip = NetHelper.GetIp(item.Rpg_Ip);
            int port = NetHelper.GetPort(item.Rpg_Ip);
            m_Server = new Server
            {
                Ports = { new ServerPort(str_ip , port, ServerCredentials.Insecure) }
            };
            


            return base.Init();
        }
        public override void Update()
        {

        }
    }
}
