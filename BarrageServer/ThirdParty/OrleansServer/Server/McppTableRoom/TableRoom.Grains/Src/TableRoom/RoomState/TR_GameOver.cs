using GameFramework;
using GameFramework.Fsm;
using System;
using System.Collections.Generic;
using System.Text;

namespace TableRoom
{
    public class TR_GameOver : FsmState<GTableRoom>
    {
        private float m_TimeOut;
        private float m_TotalTimeOut = 3;
        public override void OnInit(IFsm<GTableRoom> fsm)
        {
        }

        public override void OnEnter(IFsm<GTableRoom> fsm)
        {

            fsm.Owner.TableRoomState =  ETableRoomState.ERoom_GameOver;
            m_TimeOut = 0;
        }

        public override void OnUpdate(IFsm<GTableRoom> fsm, float elapseSeconds, float realElapseSeconds)
        {
            /*
            fsm.Owner.ServerPlayer.Update();
            m_TimeOut += elapseSeconds;
            if(m_TimeOut>= m_TotalTimeOut)
            {
                Log.Debug("Out Time GameOverFinish");
                fsm.Owner.GameOverFinish();
                return;
            }
            fsm.Owner.CheckAllGameOver();
            */
        }
        

        public override void OnLeave(IFsm<GTableRoom> fsm, bool isShutdown)
        {

        }

        public override void OnDestroy(IFsm<GTableRoom> fsm)
        {
        }

    }
}
