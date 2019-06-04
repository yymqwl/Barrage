using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
using GameMain;
using HallGrains;

namespace GameMain.Msg
{
    [Message((ushort)HallOpCode.Login_Req)]
    public partial class Login_Req
    {

    }
    [Message((ushort)HallOpCode.SetName_Req)]
    public partial class SetName_Req
    {

    }
    [Message((ushort)HallOpCode.Say_Req)]
    public partial class Say_Req
    {

    }
    [Message((ushort)HallOpCode.Msg_Res)]
    public partial class Msg_Res
    {

    }
    [Message((ushort)HallOpCode.ChatRoomMsg_Req)]
    public partial class ChatRoomMsg_Req
    {

    }
    /*
    public partial class Chatroom_Ext
    {

    }*/

}
