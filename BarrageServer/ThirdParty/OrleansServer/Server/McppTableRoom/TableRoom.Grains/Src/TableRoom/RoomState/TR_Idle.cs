using GameFramework.Fsm;
using System;
using System.Collections.Generic;
using System.Text;

namespace TableRoom
{
    public class TR_Idle : FsmState<GTableRoom>
    {
        public override void OnInit(IFsm<GTableRoom> fsm)
        {
        }

        public override void OnEnter(IFsm<GTableRoom> fsm)
        {
            fsm.Owner.TableRoomState  = ETableRoomState.ERoom_Idle;
        }

        public override void OnUpdate(IFsm<GTableRoom> fsm, float elapseSeconds, float realElapseSeconds)
        {
       
        }

        public override void OnLeave(IFsm<GTableRoom> fsm, bool isShutdown)
        {

        }

        public override void OnDestroy(IFsm<GTableRoom> fsm)
        {
        }

    }
}
