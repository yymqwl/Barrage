using GameFramework.Fsm;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoomServer
{
    public class TR_Ready : FsmState<TableRoom>
    {
        public override void OnInit(IFsm<TableRoom> fsm)
        {
        }

        public override void OnEnter(IFsm<TableRoom> fsm)
        {
            fsm.Owner.RoomState = ERoomState.ERoom_Ready;
        }

        public override void OnUpdate(IFsm<TableRoom> fsm, float elapseSeconds, float realElapseSeconds)
        {
            fsm.Owner.CheckAllReady();
        }

        public override void OnLeave(IFsm<TableRoom> fsm, bool isShutdown)
        {

        }

        public override void OnDestroy(IFsm<TableRoom> fsm)
        {
        }
    }
}
