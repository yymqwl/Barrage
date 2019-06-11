using GameFramework;
using GameMain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlioClient
{
    [GameFrameworkModuleAttribute]
    public class SiloNetWork : GameFrameworkModule
    {
        ServerNetWork m_ServerNetWork;
        public ServerNetWork ServerNetWork { get { return m_ServerNetWork; } }
        public override bool Init()
        {
            Log.Debug("SiloNetWork Init");
            m_ServerNetWork = new ServerNetWork(NetworkProtocol.TCP, "127.0.0.1:2010");
            m_ServerNetWork.MessagePacker = new Protobuf3Packer();


            var opCodeTypeBv = new OpCodeTypeBv();
            m_ServerNetWork.AddIBehaviour(opCodeTypeBv);

            var messageDispatherBv = new MessageDispatherBv();
            m_ServerNetWork.AddIBehaviour(messageDispatherBv);

            m_ServerNetWork.Init();


            //opCodeTypeBv.Load(GetType().Assembly);
            opCodeTypeBv.Load(typeof(IHall.IHello).Assembly);
            messageDispatherBv.Load(GetType().Assembly);



            return base.Init();
        }
        public override bool ShutDown()
        {
            Log.Debug("SiloNetWork ShutDown");
            m_ServerNetWork.ShutDown();
            return base.ShutDown();
        }
        public override void Update()
        {
            m_ServerNetWork.Update();
        }
    }
}
