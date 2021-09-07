using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameFramework;

namespace GameMain.LockStep
{
    /// <summary>
    /// 服务器生成
    /// </summary>
    public class ServerPlayer : PlayerBase, ISendCommand
    {

        ServerFrameGenerator m_ServerFrameGenerator;
        public ServerFrameGenerator ServerFrameGenerator
        {
            get
            {
                return m_ServerFrameGenerator;
            }
        }
        List<ISendCommand> m_List_ISendCommand = new List<ISendCommand>();

        public List<ISendCommand> List_ISendCommand
        {
            get
            {
                return m_List_ISendCommand;
            }
        }

        public void SendCommand(Command cmd)
        {
            if (m_List_ISendCommand != null)
            {
                foreach (var isc in m_List_ISendCommand)
                {
                    isc.SendCommand(cmd);
                }
            }
        }

        public override bool Init()
        {
            bool pret = base.Init();

            m_ServerFrameGenerator = new ServerFrameGenerator();
            m_PlayerRecoder = new PlayerRecoder();
            m_ABv_Set = new RootBehaviour<ServerPlayer>(this);
            m_ABv_Set.AddIBehaviour(m_ServerFrameGenerator);
            m_ABv_Set.AddIBehaviour(m_PlayerRecoder);

            m_ABv_Set.Init();


            List_ISendCommand.Add(m_ServerFrameGenerator);
            return pret;
        }
        public override void Update()
        {

            if (m_IsStop)
            {
                return;
            }

            m_ServerFrameGenerator.Update();

            if (m_ServerFrameGenerator.Gen_Frame.Count > 0)
            {
                var tmp_genfram = m_ServerFrameGenerator.GetGenFrame();
                m_PlayerRecoder.PushBackFrame(tmp_genfram);
            }
            base.Update();
        }

        public override bool ShutDown()
        {
            return base.ShutDown();
        }

    }

}