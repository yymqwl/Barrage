using GameFramework.Fsm;
using System;
using System.Collections.Generic;
using System.Text;

namespace TableRoom
{
    public class TR_CleanUp : FsmState<GTableRoom>
    {
        public override void OnInit(IFsm<GTableRoom> fsm)
        {
        }

        public override void OnEnter(IFsm<GTableRoom> fsm)
        {
            fsm.Owner.TableRoomState = ETableRoomState.ERoom_ClearUp;
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
