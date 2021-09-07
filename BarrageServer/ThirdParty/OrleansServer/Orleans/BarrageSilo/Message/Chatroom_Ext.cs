using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
using GameMain;
using HallGrains;

namespace GameMain.Msg
{
    [Message((ushort)HallOpCode.Login_Req)]
    public partial class Login_Req :IMessage
    {

    }
    [Message((ushort)HallOpCode.Login_Res)]
    public partial class Login_Res : IMessage
    {

    }
    [Message((ushort)HallOpCode.SetName_Req)]
    public partial class SetName_Req : IMessage
    {

    }
    [Message((ushort)HallOpCode.SetName_Res)]
    public partial class SetName_Res : IMessage
    {

    }
    [Message((ushort)HallOpCode.Say_Req)]
    public partial class Say_Req : IMessage
    {

    }
    [Message((ushort)HallOpCode.Say_Res)]
    public partial class Say_Res : IMessage
    {

    }
    [Message((ushort)HallOpCode.ChatRoomMsg_Req)]
    public partial class ChatRoomMsg_Req : IMessage
    {

    }
    [Message((ushort)HallOpCode.ChatRoomMsg_Res)]
    public partial class ChatRoomMsg_Res : IMessage
    {

    }
}
