using System;
using System.Collections.Generic;
using System.Text;

namespace HallGrains
{
    public enum  HallOpCode:ushort
    {
        Login_Req,
        //Login_Res,
        SetName_Req,
        //SetName_Res,
        Say_Req,
        //Say_Res,
        ChatRoomMsg_Req,
        Msg_Res,
    }
}
