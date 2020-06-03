using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameFramework;

namespace GameMain.LockStep
{
    public class WebSendCommand : NetSendCommand
    {
        /*
        KcpNetWorkManager m_KcpNetWorkManager;
        public KcpNetWorkManager KcpNetWorkManager
        {
            get
            {
                if (m_KcpNetWorkManager == null)
                {
                    m_KcpNetWorkManager = NetWorkManagerComponent.Instance.KcpNetWorkManager;
                }
                return m_KcpNetWorkManager;
            }
        }*/

        //protected CObjectPool<Command_Msg> m_Command_Msg_Pool = new CObjectPool<Command_Msg>(GameConstant.DefaultPoolSize, GameConstant.DefaultPoolIncrease);
        //protected CObjectPool<GetNextFrame_Req> m_GetNextFrame_Req_Pool = new CObjectPool<GetNextFrame_Req>(GameConstant.DefaultPoolSize, GameConstant.DefaultPoolIncrease);
        /*public override void SendCommand(Command cmd)
        {
            Command_Msg command_Msg = MessagePool.Instance.Fetch<Command_Msg>();
            command_Msg.MCmd = cmd;
            KcpNetWorkManager.KcpPeer.KcpSend(MsgBase.CreateMsgBase_NoAlloc(NetMsgEnum.Command_Msg,
               command_Msg));
        }
        public virtual void GetNextFrame_Cmd(uint CurFrameId)
        {

            GetNextFrame_Req GetNextFrame_Req = MessagePool.Instance.Fetch<GetNextFrame_Req>(); //.Spawn();
            GetNextFrame_Req.MCurFrameId = CurFrameId;
            KcpNetWorkManager.KcpPeer.KcpSend(MsgBase.CreateMsgBase_NoAlloc(NetMsgEnum.GetNextFrame_Req, GetNextFrame_Req));
        }
        */
    }
}
