using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameFramework;
using System.Collections;

namespace GameMain.LockStep
{
    public class ServerFrameCache : ABehaviour, IHandleFrame
    {
        private uint m_FrameId;
        Dictionary<uint, Frame_Data> m_Recv_Frame = new Dictionary<uint, Frame_Data>();


        public Dictionary<uint, Frame_Data> Recv_Frame
        {
            get { return m_Recv_Frame; }
        }


        public virtual void HandleFrame(Frame_Data frame)
        {
            if (m_Recv_Frame.ContainsKey(frame.FrameId))
            {
                return;
            }
            m_Recv_Frame.Add(frame.FrameId,frame);
        }

        public bool HasRecvFrame()
        {
            return m_Recv_Frame.ContainsKey(m_FrameId);
        }
        public List<Frame_Data> GetRecvFrame()
        {
            List<Frame_Data> tmp_frame = new List<Frame_Data>();
            tmp_frame.Clear();
            while (m_Recv_Frame.ContainsKey(m_FrameId))
            {

                tmp_frame.Add(m_Recv_Frame.GetValue(m_FrameId));
                m_FrameId++;
            }
            return tmp_frame;
        }

        public override bool Init()
        {
            m_FrameId = 0;
            m_Recv_Frame.Clear();

            return base.Init();
        }

    }
}
