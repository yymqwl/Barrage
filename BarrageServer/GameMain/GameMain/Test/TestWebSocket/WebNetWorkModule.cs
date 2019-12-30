using GameFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameMain
{
    [GameFrameworkModule]
    public class WebNetWorkModule : GameFrameworkModule
    {
        ServerNetWork m_ServerNetWork;
        public ServerNetWork ServerNetWork { get { return m_ServerNetWork; } }



        public override bool Init()
        {
            Log.Debug("WebNetWorkModule Init");

            m_ServerNetWork = new ServerNetWork(NetworkProtocol.WebSocket, @"http://127.0.0.1:2000/");

            m_ServerNetWork.Init();




            return base.Init();
        }
        public override bool ShutDown()
        {
            Log.Debug("WebNetWorkModule ShutDown");
            m_ServerNetWork.ShutDown();
            return base.ShutDown();
        }
        public override void Update()
        {
            m_ServerNetWork.Update();
        }
    }
}
