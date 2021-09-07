using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
using GameFramework.Event;

namespace GameMain.LockStep
{
    public class PlayerBase: ABehaviour
    {
        protected ABehaviourSet m_ABv_Set;
        protected EventManager m_Evt_Mg = new EventManager();

        public EventManager EventManager
        {
            get
            {
                return this.m_Evt_Mg;
            }
        }

        protected PlayerRecoder m_PlayerRecoder;
        public PlayerRecoder PlayerRecoder
        {
            get
            {
                return m_PlayerRecoder;
            }
        }


        protected bool m_IsStop;
        public bool IsStop
        {
            get
            {
                return m_IsStop;
            }
        }

        protected uint m_FrameCount;
        public uint FrameCount
        {
            get { return m_FrameCount; }
        }


        public override bool Init()
        {
            m_FrameCount = 0;
            m_IsStop = true;
            m_Evt_Mg.Init();
            return base.Init();
        }
        public override bool ShutDown()
        {
            m_Evt_Mg.ShutDown();
            m_ABv_Set.ShutDown();
            return base.ShutDown();
        }
        public virtual void Play()
        {
            m_IsStop = false;
        }
        public virtual void Stop()
        {
            m_IsStop = true;
        }

        public override void Update()
        {

        }
        /// <summary>
        /// 正常刷新
        /// </summary>
        /// <param name="elapseSeconds"></param>
        protected virtual void Update(float elapseSeconds)
        {

        }

        /// <summary>
        /// 一次刷新循环
        /// </summary>
        /// <param name="elapseSeconds"></param>
        protected virtual void GameFrameTurn(float elapseSeconds)
        {
            Update(elapseSeconds);
            m_FrameCount++;
            var fm = m_PlayerRecoder.GetFrameById(FrameCount);
            if (fm == null || fm.Commands.Count == 0)
            {
                return;
            }
            HandleFrame(fm);

        }

        protected virtual void HandleFrame(Frame_Data frame)
        {
        }
    }
}
