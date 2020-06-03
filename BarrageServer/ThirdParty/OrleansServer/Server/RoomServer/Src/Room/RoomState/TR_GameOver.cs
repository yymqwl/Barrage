using GameFramework;
using GameFramework.Fsm;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoomServer
{
    public class TR_GameOver : FsmState<TableRoom>
    {
        private float m_TimeOut;
        private float m_TotalTimeOut = 3;
        public override void OnInit(IFsm<TableRoom> fsm)
        {
        }

        public override void OnEnter(IFsm<TableRoom> fsm)
        {

            fsm.Owner.RoomState = ERoomState.ERoom_GameOver;
            m_TimeOut = 0;
        }

        public override void OnUpdate(IFsm<TableRoom> fsm, float elapseSeconds, float realElapseSeconds)
        {
            fsm.Owner.ServerPlayer.Update();
            m_TimeOut += elapseSeconds;
            if(m_TimeOut>= m_TotalTimeOut)
            {
                Log.Debug("Out Time GameOverFinish");
                fsm.Owner.GameOverFinish();
                return;
            }
            fsm.Owner.CheckAllGameOver();
        }
        

        public override void OnLeave(IFsm<TableRoom> fsm, bool isShutdown)
        {

        }

        public override void OnDestroy(IFsm<TableRoom> fsm)
        {
        }

    }
}
