using System;
using System.Collections.Generic;
using System.Text;

namespace GameMain.ChatRoom
{
    public enum  HallOpCode:ushort
    {
        Login_Req=1000,
        Login_Res,
        SetName_Req,
        SetName_Res,
        Say_Req,
        Say_Res,
        ChatRoomMsg_Req,
        ChatRoomMsg_Res,
        Ping_Msg,
    }
}
