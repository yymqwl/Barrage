using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameFramework;

namespace GameMain.LockStep
{
    public class ServerFrameGenerator :FrameGenerator
    {


        ObjectPool<Frame_Data> m_FramePool;
        public ObjectPool<Frame_Data> FramePool
        {
            get
            {
                return m_FramePool;
            }
        }

        public override bool Init()
        {

            m_FramePool = new ObjectPool<Frame_Data>(new FrameFactory(),GameConstant.NFrameInitCount,GameConstant.NFrameExtendCount);
            

            return base.Init();
        }

        protected override void GenFrame()
        {
            lock(m_lock_Frame)
            {
                m_Gen_Frame.Add(m_CurFrame);
                m_FrameId++;
                m_CurFrame = FramePool.Spawn();
                m_CurFrame.FrameId = m_FrameId;
            }
        }
        public override void Update()
        {
            base.Update();
        }
    }

    public class FrameFactory : ObjectFactory<Frame_Data>
    {


        public override Frame_Data CreateObj()
        {
            return FrameRecoder.CreateFrame(0);
        }

        public override void ReleaseObj(Frame_Data t)
        {

        }
    }

}
