using System;
using System.Collections.Generic;
using System.Text;

namespace GameMain.Msg
{
    public enum GameMsgOpCode : ushort
    {
        EnterRoom_Req=100,
        EnterRoom_Res,
        ExitRoom_Req,
        ExitRoom_Res,
        Say_Req,
        Say_Res,
    }
}
