using GameFramework;
using GameMain;
using System;
using System.Collections.Generic;
using System.Text;

namespace BarrageSilo
{
    [GameFrameworkModuleAttribute]
    public class SiloNetWork: GameFrameworkModule
    {
        ServerNetWork m_ServerNetWork;
        public ServerNetWork ServerNetWork { get { return m_ServerNetWork; } }
        public override bool Init()
        {
            Log.Debug("SiloNetWork Init");
            var gameconfig = GameModuleManager.Instance.GetModule<ConfigManager>().GameConfig;
            m_ServerNetWork = new ServerNetWork(NetworkProtocol.TCP, gameconfig.NetWorkIp);
            m_ServerNetWork.MessagePacker = new Protobuf3Packer();


            var opCodeTypeBv = new OpCodeTypeBv();
            m_ServerNetWork.IOpCodeType = opCodeTypeBv;
            m_ServerNetWork.AddIBehaviour(opCodeTypeBv);


            var messageDispatherBv = new RpcMessageDispatherBv();
            m_ServerNetWork.AddIBehaviour(messageDispatherBv);
            m_ServerNetWork.IMessageDispatcher = messageDispatherBv;



            m_ServerNetWork.Init();

            
            //opCodeTypeBv.Load(GetType().Assembly);
            opCodeTypeBv.Load(typeof(IHall.IHello ).Assembly);
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
