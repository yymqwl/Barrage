using GameFramework;
using GameMain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ChatRoom
{
    public class SiloNetWork : GameFrameworkModule
    {
        ClientNetWork m_ClientNetWork;
        public ClientNetWork ClientNetWork { get { return m_ClientNetWork; } }
        public override bool Init()
        {
            Log.Debug("SiloNetWork Init");
            m_ClientNetWork = new ClientNetWork(NetworkProtocol.TCP);
            m_ClientNetWork.MessagePacker = new Protobuf3Packer();


            var opCodeTypeBv = new OpCodeTypeBv();
            m_ClientNetWork.AddIBehaviour(opCodeTypeBv);
            m_ClientNetWork.IOpCodeType = opCodeTypeBv;

            var messageDispatherBv = new MessageDispatherBv();
            m_ClientNetWork.AddIBehaviour(messageDispatherBv);
            m_ClientNetWork.IMessageDispatcher = messageDispatherBv;

            m_ClientNetWork.Init();


            opCodeTypeBv.Load(typeof(ChatRoom.SiloNetWork).Assembly);
            messageDispatherBv.Load(GetType().Assembly);



            return base.Init();
        }
        public override bool ShutDown()
        {
            Log.Debug("SiloNetWork ShutDown");
            m_ClientNetWork.ShutDown();
            return base.ShutDown();
        }
        public override void Update()
        {
            m_ClientNetWork.Update();
        }
    }
}