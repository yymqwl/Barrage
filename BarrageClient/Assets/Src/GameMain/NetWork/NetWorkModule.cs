using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameFramework;
namespace GameMain
{
    [GameFrameworkModule]
    public class NetWorkModule : GameFrameworkModule
    {

        ClientNetWork m_ClientNetWork;
        public ClientNetWork ClientNetWork { get { return m_ClientNetWork; } }

        public override bool Init()
        {
            Log.Debug("NetWorkModule init");
            m_ClientNetWork = new ClientNetWork(NetworkProtocol.TCP);
            m_ClientNetWork.MessagePacker = new Protobuf3Packer();


            
            
            var opCodeTypeBv = new OpCodeTypeBv();
            m_ClientNetWork.AddIBehaviour(opCodeTypeBv);
            m_ClientNetWork.IOpCodeType = opCodeTypeBv;

            var messageDispatherBv  = new MessageDispatherBv();
            m_ClientNetWork.AddIBehaviour(messageDispatherBv);
            m_ClientNetWork.IMessageDispatcher = messageDispatherBv;

            ClientNetWork.Init();
            opCodeTypeBv.Load(GetType().Assembly);
            messageDispatherBv.Load(GetType().Assembly);



            return base.Init();
        }
        public override bool ShutDown()
        {
            Log.Debug("NetWorkModule Shutdown");
            m_ClientNetWork.ShutDown();
            return base.ShutDown();
        }

        public override void Update()
        {
            m_ClientNetWork.Update();
        }
    }
}
