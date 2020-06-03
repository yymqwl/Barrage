using GameFramework.Fsm;
using GameMain.LockStep;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoomServer
{
    public class TR_InGame : FsmState<TableRoom>
    {
        public override void OnInit(IFsm<TableRoom> fsm)
        {
        }

        public override void OnEnter(IFsm<TableRoom> fsm)
        {
            fsm.Owner.RoomState = ERoomState.ERoom_InGame;
            fsm.Owner.InitServerPlayer();
        }

        public override void OnUpdate(IFsm<TableRoom> fsm, float elapseSeconds, float realElapseSeconds)
        {
            fsm.Owner.ServerPlayer.Update();
        }

        public override void OnLeave(IFsm<TableRoom> fsm, bool isShutdown)
        {

        }

        public override void OnDestroy(IFsm<TableRoom> fsm)
        {
        }
    }
}
