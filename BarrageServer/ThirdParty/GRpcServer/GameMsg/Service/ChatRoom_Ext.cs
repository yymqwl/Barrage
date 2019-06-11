using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
namespace GameMain.Msg
{

    [Message((ushort)GameMsgOpCode.EnterRoom_Req, (uint)ServerType.Room )]
    public partial class EnterRoom_Req : IMessage
    {

    }

    [Message((ushort)GameMsgOpCode.EnterRoom_Res, (uint)ServerType.Room)]
    public partial class EnterRoom_Res : IMessage
    {

    }
    [Message((ushort)GameMsgOpCode.ExitRoom_Req, (uint)ServerType.Room)]
    public partial class ExitRoom_Req : IMessage
    {

    }
    [Message((ushort)GameMsgOpCode.ExitRoom_Res, (uint)ServerType.Room)]
    public partial class ExitRoom_Res : IMessage
    {

    }
    [Message((ushort)GameMsgOpCode.Say_Req, (uint)ServerType.Room)]
    public partial class Say_Req : IMessage
    {

    }

    [Message((ushort)GameMsgOpCode.Say_Res, (uint)ServerType.Room)]
    public partial class Say_Res : IMessage
    {

    }

}
