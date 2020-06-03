using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;

namespace RoomServer
{
    public class APlayer :ABehaviourSet
    {
        protected string m_Id;
        public virtual string Id
        {
            get
            {
                return this.m_Id;
            }
        }
        public virtual bool IsOnline
        {
            get;
        }
        protected DateTime m_LastActiveTime;
        public DateTime LastActiveTime
        {
            get
            {
                return m_LastActiveTime;
            }
            set
            {
                m_LastActiveTime = value;
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
        public override void Update()
        {
            base.Update();
        }

    }
}
