using GameFramework.Fsm;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoomServer
{
    public class TR_Idle : FsmState<TableRoom>
    {
        public override void OnInit(IFsm<TableRoom> fsm)
        {
        }

        public override void OnEnter(IFsm<TableRoom> fsm)
        {
            fsm.Owner.RoomState = ERoomState.ERoom_Idle;
            fsm.Owner.IsAvailable = true;
        }

        public override void OnUpdate(IFsm<TableRoom> fsm, float elapseSeconds, float realElapseSeconds)
        {
       
        }

        public override void OnLeave(IFsm<TableRoom> fsm, bool isShutdown)
        {

        }

        public override void OnDestroy(IFsm<TableRoom> fsm)
        {
        }

    }
}
