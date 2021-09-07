using GameFramework.Fsm;
using GameMain.LockStep;
using System;
using System.Collections.Generic;
using System.Text;

namespace TableRoom
{
    public class TR_InGame : FsmState<GTableRoom>
    {
        public override void OnInit(IFsm<GTableRoom> fsm)
        {
        }

        public override void OnEnter(IFsm<GTableRoom> fsm)
        {
            fsm.Owner.TableRoomState = ETableRoomState.ERoom_InGame;
            //fsm.Owner.InitServerPlayer();
        }

        public override void OnUpdate(IFsm<GTableRoom> fsm, float elapseSeconds, float realElapseSeconds)
        {
            //fsm.Owner.ServerPlayer.Update();
        }

        public override void OnLeave(IFsm<GTableRoom> fsm, bool isShutdown)
        {

        }

        public override void OnDestroy(IFsm<GTableRoom> fsm)
        {
        }
    }
}
