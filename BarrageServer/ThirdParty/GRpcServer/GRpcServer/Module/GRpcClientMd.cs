﻿using System;
using System.Collections.Generic;
using System.Text;
using Grpc.Core;
using GameFramework;
using GameMain.Msg;

namespace GRpcServer
{
    [GameFrameworkModuleAttribute]
    public class GRpcClientMd : GameFrameworkModule
    {

        public override int Priority => 100;
        Channel m_Channel;
        Hello.HelloClient m_HelloClient;
        public Hello.HelloClient HelloClient
        {
            get
            {
                return m_HelloClient;
            }
        }
        public override bool Init()
        {
            var cfgmg = GameModuleManager.Instance.GetModule<ConfigManager>();
            var item = cfgmg.GetStartConfigItem(ServerType.All);


            m_Channel = new Channel(item.Rpg_Ip, ChannelCredentials.Insecure);
            m_HelloClient = new Hello.HelloClient(m_Channel);



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
