using GameFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoomServer
{
    [GameFrameworkModule]
    public class MatchingModule : GameFrameworkModule
    {
        public override int Priority => 1000;

        protected MatchingRank m_MatchingRank = new MatchingRank();
        public override bool Init()
        {
            m_MatchingRank.Init();
            return base.Init();
        }

        public int EnterMatchRank()
        {
            
            return 1;
        }
        public int ExitMatchRank()
        {

            return 1;
        }
        public override void Update()
        {
            m_MatchingRank.Update();
        }
        public override bool ShutDown()
        {
            m_MatchingRank.ShutDown();
            return base.ShutDown();
        }
    }
}
