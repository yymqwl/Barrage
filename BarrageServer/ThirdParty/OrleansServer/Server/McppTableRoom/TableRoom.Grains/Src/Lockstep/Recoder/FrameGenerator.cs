using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameFramework;

namespace GameMain.LockStep
{
    public class FrameGenerator : ABehaviour ,ISendCommand
    {



        protected uint m_FrameId;
        protected Frame_Data m_CurFrame;

        protected UpdateTime m_UpdateTime;


        protected List<Frame_Data> m_Gen_Frame = new List<Frame_Data>();
        public List<Frame_Data> Gen_Frame
        {
            get
            {
                return m_Gen_Frame;
            }
        }

        protected object m_lock_Frame = new object();
        public override bool Init()
        {
            m_FrameId = 0;
            m_UpdateTime= new UpdateTime(GameConstant.DeltaTimeF);
            m_UpdateTime.Evt_Act += GenFrame;
            m_Gen_Frame.Clear();

            m_CurFrame = FrameRecoder.CreateFrame(m_FrameId);

            return base.Init();
        }
        protected virtual void GenFrame()
        {
            m_Gen_Frame.Add(m_CurFrame);
            m_FrameId++;
            m_CurFrame = FrameRecoder.CreateFrame(m_FrameId);
        }

        public void SendCommand(Command cmd)
        {
            if (cmd == null)
            {
                Log.Error("cmd Null");
            }
            m_CurFrame.Commands.Add(cmd);
        }

        public List<Frame_Data> GetGenFrame()
        {

            List<Frame_Data> tmp_frame = m_Gen_Frame.ToList();
            m_Gen_Frame.Clear();
            return tmp_frame;
        }

        public override bool ShutDown()
        {
            return base.ShutDown();
        }

        public override void Update()
        {
            this.m_UpdateTime.Update(ClientTimer.Instance.DeltaTime);
        }
    }
}
