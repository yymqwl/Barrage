using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameMain;

namespace GameMain.LockStep
{
    /// <summary>
    /// 
    /// </summary>
    public class FrameRecoder : ABehaviour
    {


        protected List<Frame_Data> m_List_Frame = new List<Frame_Data>();
        protected uint m_End_FrameId;

        
        StartGame_Data m_StartGame_Data;
        public StartGame_Data StartGame_Data
        {
            get
            {
                return m_StartGame_Data;
            }
            set
            {
                m_StartGame_Data = value;
            }
        }
        public uint End_FrameId
        {
            get { return m_End_FrameId; }
        }
        public float End_FrameTime
        {
            get { return (m_End_FrameId * GameConstant.DeltaTimeF); }

        }

        protected virtual void ResetFrame(int capacity)
        {
            m_List_Frame.Clear();
            m_List_Frame.Capacity = capacity;

        }
        public List<Frame_Data> GetCurToEndFrame(uint id)
        {
            List<Frame_Data> tmp_ls_frame = new List<Frame_Data>();

            for(int i=0;i< m_List_Frame.Count;++i)
            {
                if(m_List_Frame[i].FrameId > id)
                {
                    tmp_ls_frame.Add(m_List_Frame[i]);
                }
            }
            Frame_Data edfm = new Frame_Data();
            edfm.FrameId = m_End_FrameId;
            tmp_ls_frame.Add(edfm);
            return tmp_ls_frame;
        }
        public uint GetEndFrameId()
        {
            return m_End_FrameId ;
        }

        public Frame_Data GetFrameById(uint id)
        {
             for(int i=0;i< m_List_Frame.Count;++i)
            {
                if (m_List_Frame[i].FrameId == id)
                {
                    return m_List_Frame[i];
                }
            }
            return null;
        }
        public void PushBackFrame(List<Frame_Data> ls_fm)
        {
            foreach(var fm in ls_fm)
            {
                PushBackFrame(fm);
            }
        }

        public  void PushBackFrame(Frame_Data fm)
        {
            if (m_End_FrameId < fm.FrameId)
            {
                m_End_FrameId = fm.FrameId;
                if (fm.Commands.Count > 0)
                {
                    m_List_Frame.Add(fm);

                }
            }
        }

        public override bool Init()
        {
            return base.Init();
        }

        public override bool ShutDown()
        {
            return base.ShutDown();
        }
        public static Frame_Data CreateFrame( uint id)
        {
            Frame_Data fm = new Frame_Data();
            fm.FrameId = id;
            return fm;
        }

        public virtual bool Load(StartGame_Data startGame_Data)
        {
            m_StartGame_Data = startGame_Data;
            return true;
        }
        public void LoadReplayData(ReplayData replayData)
        {
            m_List_Frame.Clear();

            m_End_FrameId = replayData.EndFrameId;
            for (int i=0;i<replayData.ListFrame.Count;++i)
            {
                Frame_Data tmpfm = replayData.ListFrame[i];
                m_List_Frame.Add(tmpfm);
            }
            m_StartGame_Data = replayData.StartGame_Data;
        }
        public ReplayData SaveToReplayData()
        {
            ReplayData  rdata = new ReplayData();
            rdata.EndFrameId = m_End_FrameId;
            foreach (var frame in  m_List_Frame)
            {
                rdata.ListFrame.Add(frame);
            }
            rdata.StartGame_Data = m_StartGame_Data;
            return rdata;

        }
    }
}
